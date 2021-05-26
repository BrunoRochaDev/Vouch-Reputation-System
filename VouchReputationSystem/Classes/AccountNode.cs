using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VouchReputationSystem.Classes
{
    public class AccountNode : AccountChain
    {
        //Creates the node from the account
        public AccountNode(AccountChain _acc)
        {
            this.name = _acc.name;
            this.vouches = _acc.vouches;

            SetupGraphics();
        }
        //Creates the node from the account with a custom weight multiplier
        public AccountNode(AccountChain _acc, float _weight)
        {
            this.name = _acc.name;
            this.vouches = _acc.vouches;
            this.weight = _weight;

            SetupGraphics();
        }

        //Setup the references to the brushes and pens that will be used later on.
        void SetupGraphics()
        {
            mFont = new SolidBrush(Color.FromArgb(237, 242, 243));
            mStroke = new Pen(Color.FromArgb(237, 242, 243)) {  Width = 3 };
            mFill = new SolidBrush(Color.FromArgb(82, 137, 181));
        }
        
        //Dictionary containing all the different neighbours nodes (key) and it's relation polarity (value)
        public Dictionary<AccountNode, bool> neighbours = new Dictionary<AccountNode, bool>();

        //Weight of node determined locally
        private float _weight = 1;
        public float weight { get { return _weight; } set { _weight = Util.LimitRange(_weight, 1.5f, 0.5f); } }

        //Distance of this node to the observer node
        public int distanceFromObserver = 0;

        //If the node can reach the observer travelling exclusively through positive or negative connections.
        public bool hasVouchForPath = false;
        public bool hasVouchAgainstPath = false;


        public double reputation = 1;

        //For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.
        public AccountNode ParentNode;

        //The distance to the start from this node.
        public int gCost;
        //The distance to the goal from this node.
        public int hCost;
        //Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.
        public int fCost { get { return gCost + hCost; } }

        #region drawing
        // Gets or sets the position of the node.
        Point _location;			// node position, relative to the origin
        public Point Location
        {
            get { return _location; }
            set { _location = value; }
        }

        // Gets or sets the X coordinate of the node, relative to the origin.
        public int X
        {
            get { return _location.X; }
            set { _location.X = value; }
        }
        // Gets or sets the Y coordinate of the node, relative to the origin.
        public int Y
        {
            get { return _location.Y; }
            set { _location.Y = value; }
        }

        private SolidBrush mFill;
        private Pen mStroke;
        private SolidBrush mFont;
        //-----------------

        #endregion

        //This method returns the validity of a voach. This is for networking reasons.
        public bool isVouchValid(AccountChain _acc)
        {
            bool vouchFor = this.DoesVouchFor(_acc);
            bool vouchAgainst = this.DoesVouchAgainst(_acc);

            //If both are true, then something is wrong...
            if (vouchFor && vouchAgainst)
            {
                Console.WriteLine("Error! A node cant vouch for and against some other at the same time.");
                return false;
            }

            //If both are true, then something is wrong...
            if (!vouchFor && !vouchAgainst)
            {
                Console.WriteLine("Error! A node cant vouch for and against some other at the same time.");
                return false;
            }

            return true;
        }

        #region print
        //Prints all the vouches in string format.
        public void PrintImmediateVouches()
        {
            //If there are not relations...
            if (neighbours.Keys.Count == 0)
            {
                Console.WriteLine(this.name + " has no vouches.");
                return;
            }

            string _result = "";
            //Get vouch relations of each edge node.
            foreach (AccountNode _node in neighbours.Keys)
            {
                bool polarity = neighbours[_node];

                //Get result in string format
                if (polarity)
                    _result += "(" + this.name + ") <=> (" + _node.name + ")\n";
                else
                    _result += "(" + this.name + ") =x= (" + _node.name + ")\n";
            }

            Console.WriteLine(this.name + " vouch relations:\n" + _result);
        }
        //Print object
        public override string ToString()
        {
            return "name: " + this.name + ", rep: " + this.reputation + ", distance: " + this.distanceFromObserver;
        }
        #endregion

        //Equivalence
        public override bool Equals(object obj)
        {
            var item = obj as AccountChain;

            if (item == null)
            {
                return false;
            }

            return this.name.Equals(item.name);
        }
        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }

        //-----------
        public virtual void DrawConnector(Graphics graphics, Point from, Point to, AccountNode other, bool polarity)
        {
            Pen color = new Pen(polarity ? Color.FromArgb(42, 157, 143) : Color.FromArgb(231, 111, 81)) {Width = 2 };
            graphics.DrawLine(color, from, to);
        }

        public void DrawNode(Graphics graphics, Rectangle bounds, bool isObserver)
        {
            //Draw node
            graphics.FillEllipse(mFill, bounds);
            if (isObserver)
                graphics.DrawEllipse(mStroke, bounds);

            StringFormat _StringFormat = new System.Drawing.StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            string nodeText = this.name;
            if (!isObserver)
            {
                double truncatedRep = Math.Truncate(this.reputation * 100) / 100;
                nodeText = this.name + "\n" + truncatedRep;
                nodeText = nodeText.Replace(',', '.');
            }

            graphics.DrawString(nodeText, new System.Drawing.Font("Arial", 12), mFont, bounds, _StringFormat);
        }

        public Size Size
        {
            get
            {
                return new Size((int)(64 + ((this.reputation - 1) * 16)), (int)(64 + ((this.reputation - 1) * 16)));
                //return new Size(64, 64);
            }
        }
    }
}
