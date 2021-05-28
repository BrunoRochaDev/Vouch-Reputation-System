using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VouchReputationSystem.Classes.Diagram
{
    class Diagram
    {
        private const double ATTRACTION_CONSTANT = 0.1;     // spring constant
        private const double REPULSION_CONSTANT = 10000;    // charge constant

        private const double DEFAULT_DAMPING = 0.5;
        private const int DEFAULT_SPRING_LENGTH = 10; //before was 100
        private const int DEFAULT_MAX_ITERATIONS = 500;

        private List<AccountNode> nodes;
        private AccountNode observer;
        
        public Diagram(List<AccountNode> _nodes, AccountNode _observer)
        {
            nodes = _nodes;
            observer = _observer;
            Arrange();
        }

        // Runs the force-directed layout algorithm on this Diagram, using the default parameters.
        public void Arrange()
        {
            Arrange(DEFAULT_DAMPING, DEFAULT_SPRING_LENGTH, DEFAULT_MAX_ITERATIONS, true);
        }

        // Runs the force-directed layout algorithm on this Diagram, offering the option of a random or deterministic layout.
        public void Arrange(bool deterministic)
        {
            Arrange(DEFAULT_DAMPING, DEFAULT_SPRING_LENGTH, DEFAULT_MAX_ITERATIONS, deterministic);
        }

        // Runs the force-directed layout algorithm on this Diagram, using the specified parameters.
        public void Arrange(double damping, int springLength, int maxIterations, bool deterministic)
        {
            // random starting positions can be made deterministic by seeding System.Random with a constant
            Random rnd = deterministic ? new Random(0) : new Random();

            // copy nodes into an array of metadata and randomise initial coordinates for each node
            NodeLayoutInfo[] layout = new NodeLayoutInfo[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                layout[i] = new NodeLayoutInfo(nodes[i], new Vector(), Point.Empty);
                layout[i].Node.Location = new Point(rnd.Next(-50, 50), rnd.Next(-50, 50));
            }

            int stopCount = 0;
            int iterations = 0;

            while (true)
            {
                double totalDisplacement = 0;

                for (int i = 0; i < layout.Length; i++)
                {
                    NodeLayoutInfo current = layout[i];

                    // express the node's current position as a vector, relative to the origin
                    Vector currentPosition = new Vector(CalcDistance(Point.Empty, current.Node.Location), GetBearingAngle(Point.Empty, current.Node.Location));
                    Vector netForce = new Vector(0, 0);

                    // determine repulsion between nodes
                    foreach (AccountNode other in nodes)
                    {
                        if (other != current.Node) netForce += CalcRepulsionForce(current.Node, other);
                    }

                    // determine attraction caused by neighbours
                    foreach (AccountNode child in current.Node.neighbours.Keys)
                    {
                        netForce += CalcAttractionForce(current.Node, child, springLength);
                    }
                    foreach (AccountNode parent in nodes)
                    {
                        if (parent.neighbours.ContainsKey(current.Node)) netForce += CalcAttractionForce(current.Node, parent, springLength);
                    }

                    // apply net force to node velocity
                    current.Velocity = (current.Velocity + netForce) * damping;

                    // apply velocity to node position
                    current.NextPosition = (currentPosition + current.Velocity).ToPoint();
                }

                // move nodes to resultant positions (and calculate total displacement)
                for (int i = 0; i < layout.Length; i++)
                {
                    NodeLayoutInfo current = layout[i];

                    totalDisplacement += CalcDistance(current.Node.Location, current.NextPosition);
                    current.Node.Location = current.NextPosition;
                }

                iterations++;
                if (totalDisplacement < 10) stopCount++;
                if (stopCount > 15) break;
                if (iterations > maxIterations) break;
            }
        }

        // Calculates the attraction force between two connected nodes, using the specified spring length.
        private Vector CalcAttractionForce(AccountNode x, AccountNode y, double springLength)
        {
            int proximity = Math.Max(CalcDistance(x.Location, y.Location), 1);

            // Hooke's Law: F = -kx
            double force = ATTRACTION_CONSTANT * Math.Max(proximity - springLength, 0);
            double angle = GetBearingAngle(x.Location, y.Location);

            return new Vector(force, angle);
        }

        // Calculates the distance between two points.
        public static int CalcDistance(Point a, Point b)
        {
            double xDist = (a.X - b.X);
            double yDist = (a.Y - b.Y);
            return (int)Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
        }


        // Calculates the repulsion force between any two nodes in the diagram space.
        private Vector CalcRepulsionForce(AccountNode x, AccountNode y)
        {
            int proximity = Math.Max(CalcDistance(x.Location, y.Location), 1);

            // Coulomb's Law: F = k(Qq/r^2)
            double force = -(REPULSION_CONSTANT / Math.Pow(proximity, 2));
            double angle = GetBearingAngle(x.Location, y.Location);

            return new Vector(force, angle);
        }

        // Calculates the bearing angle from one point to another.
        private double GetBearingAngle(Point start, Point end)
        {
            Point half = new Point(start.X + ((end.X - start.X) / 2), start.Y + ((end.Y - start.Y) / 2));

            double diffX = (double)(half.X - start.X);
            double diffY = (double)(half.Y - start.Y);

            if (diffX == 0) diffX = 0.001;
            if (diffY == 0) diffY = 0.001;

            double angle;
            if (Math.Abs(diffX) > Math.Abs(diffY))
            {
                angle = Math.Tanh(diffY / diffX) * (180.0 / Math.PI);
                if (((diffX < 0) && (diffY > 0)) || ((diffX < 0) && (diffY < 0))) angle += 180;
            }
            else
            {
                angle = Math.Tanh(diffX / diffY) * (180.0 / Math.PI);
                if (((diffY < 0) && (diffX > 0)) || ((diffY < 0) && (diffX < 0))) angle += 180;
                angle = (180 - (angle + 90));
            }

            return angle;
        }

        // Determines the logical bounds of the diagram. This is used to center and scale the diagram when drawing.
        private Rectangle GetDiagramBounds()
        {
            int minX = Int32.MaxValue, minY = Int32.MaxValue;
            int maxX = Int32.MinValue, maxY = Int32.MinValue;
            foreach (AccountNode node in nodes)
            {
                if (node.X < minX) minX = node.X;
                if (node.X > maxX) maxX = node.X;
                if (node.Y < minY) minY = node.Y;
                if (node.Y > maxY) maxY = node.Y;
            }

            return Rectangle.FromLTRB((int)(minX * 1.5f), (int)(minY * 1.5f), (int)(maxX * 1.5f), (int)(maxY * 1.5f));

            //return Rectangle.FromLTRB(minX, minY, maxX, maxY);
        }

        // Draws the diagram using GDI+, centering and scaling within the specified bounds.
        public void Draw(Graphics graphics, Rectangle bounds)
        {
            Point center = new Point(bounds.X + (bounds.Width / 2), bounds.Y + (bounds.Height / 2));

            // determine the scaling factor
            Rectangle logicalBounds = GetDiagramBounds();
            double scale = 1;
            if (logicalBounds.Width > logicalBounds.Height)
            {
                if (logicalBounds.Width != 0) scale = (double)Math.Min(bounds.Width, bounds.Height) / (double)logicalBounds.Width;
            }
            else
            {
                if (logicalBounds.Height != 0) scale = (double)Math.Min(bounds.Width, bounds.Height) / (double)logicalBounds.Height;
            }

            // draw all of the connectors first
            foreach (AccountNode node in nodes)
            {
                Point source = ScalePoint(node.Location, scale);

                // connectors
                foreach (AccountNode other in node.neighbours.Keys)
                {
                    Point destination = ScalePoint(other.Location, scale);
                    node.DrawConnector(graphics, center + (Size)source, center + (Size)destination, node.HasVouchForConnection(other));
                }
            }

            // then draw all of the nodes
            foreach (AccountNode node in nodes)
            {
                Point destination = ScalePoint(node.Location, scale);

                Size nodeSize = node.Size;
                Rectangle nodeBounds = new Rectangle(center.X + destination.X - (nodeSize.Width / 2), center.Y + destination.Y - (nodeSize.Height / 2), nodeSize.Width, nodeSize.Height);
                node.DrawNode(graphics, nodeBounds, observer.Equals(node));
            }
        }

        private Point ScalePoint(Point point, double scale)
        {
            return new Point((int)((double)point.X * scale), (int)((double)point.Y * scale));
        }

        // Private inner class used to track the node's position and velocity during simulation.
        private class NodeLayoutInfo
        {

            public AccountNode Node;           // reference to the node in the simulation
            public Vector Velocity;     // the node's current velocity, expressed in vector form
            public Point NextPosition;  // the node's position after the next iteration

            /// Initialises a new instance of the Diagram.NodeLayoutInfo class, using the specified parameters.
            public NodeLayoutInfo(AccountNode node, Vector velocity, Point nextPosition)
            {
                Node = node;
                Velocity = velocity;
                NextPosition = nextPosition;
            }
        }
    }
}
