using System;
using System.Collections.Generic;
using System.Drawing;

//Vouch Reputation System by Bruno Rocha Moura
//https://github.com/BrunoRochaDev/

namespace VouchReputationSystem.Classes
{
    //The representation of an account in a personal network.
    //It extends from the account class, but also holds information pertinent to network and pathfinding logic.
    //It also has methods for drawing the node on the screen.

    public class AccountNode : Account
    {
        //Creates the node from the account
        public AccountNode(Account _acc)
        {
            this.name = _acc.name;
            this.vouches = _acc.vouches;
        }

        //Creates the node from the account with a custom weight
        public AccountNode(Account _acc, float _weight)
        {
            this.name = _acc.name;
            this.vouches = _acc.vouches;
            this.weight = _weight;
        }

        //Dictionary containing all the different neighbours nodes (key) and it's relation polarity (value).
        public Dictionary<AccountNode, bool> neighbours = new Dictionary<AccountNode, bool>();

        //Weight of node, determined locally
        private float _weight = 1;
        public float weight { get { return _weight; } set { _weight = Util.LimitRange(_weight, 1.5f, 0.5f); } }

        //Distance of this node to the observer node
        public int distanceFromObserver = 0;

        //If the node can reach the observer travelling exclusively through positive or negative connections.
        public bool hasVouchForPath = false;
        public bool hasVouchAgainstPath = false;

        //Reputation of the node, but be between 0 and 1.
        public double reputation = 1;

        //For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.
        public AccountNode ParentNode;

        #region PathfindingVariables
        //The distance to the start from this node.
        public int gCost;
        //The distance to the goal from this node.
        public int hCost;
        //Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.
        public int fCost { get { return gCost + hCost; } }
        #endregion

        #region DrawingVariables
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

        //The different brushes and pens that will be used to draw the node.
        private SolidBrush nodeFill = new SolidBrush(Color.FromArgb(82, 137, 181));
        private Pen nodeStroke = new Pen(Color.FromArgb(237, 242, 243)) { Width = 3 };
        private SolidBrush nodeFont = new SolidBrush(Color.FromArgb(237, 242, 243)); 
        private Pen greePen = new Pen(Color.FromArgb(42, 157, 143)) { Width = 2 };
        private Pen redPen = new Pen(Color.FromArgb(231, 111, 81)) { Width = 2 };

        #endregion

        //This method returns the validity of a voach. This is for networking reasons.
        public bool isVouchValid(Account _acc)
        {
            bool vouchFor = this.HasVouchForConnection(_acc);
            bool vouchAgainst = this.HasVouchAgainstConnection(_acc);

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

        #region Drawing
        //Draws itself on node the screen.
        public void DrawNode(Graphics graphics, Rectangle bounds, bool isObserver)
        {
            //Draws the node ellipse
            graphics.FillEllipse(nodeFill, bounds);
            //Adds a white stroke if the given node is the observer.
            if (isObserver)
                graphics.DrawEllipse(nodeStroke, bounds);

            //Creates a reference to the node's text, which always start with it's name.
            string nodeText = this.name;
            //If the given node is not the observer, then add it's reputation to the string as well.
            if (!isObserver)
            {
                double truncatedRep = Math.Truncate(this.reputation * 100) / 100;
                nodeText = this.name + "\n" + truncatedRep;
                nodeText = nodeText.Replace(',', '.');
            }

            //Draws the node text.
            StringFormat _StringFormat = new System.Drawing.StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            graphics.DrawString(nodeText, new System.Drawing.Font("Arial", 12), nodeFont, bounds, _StringFormat);
        }

        //Draws a connector between the given points with the appopriate color, depending on the given polarity.
        public virtual void DrawConnector(Graphics graphics, Point from, Point to, bool polarity)
        {
            graphics.DrawLine(polarity ? greePen : redPen, from, to);
        }
        #endregion

        //Custom size for the node, dependent on the node's reputation. It's maximum diameter is of 64 pixels but can be as low as 56.
        public Size Size
        {
            get
            {
                return new Size((int)(64 + ((this.reputation - 1) * 16)), (int)(64 + ((this.reputation - 1) * 16)));
            }
        }

        //Overrides the ToString method so that it returns the node's name, it's reputation and it's distance from the observer node.
        public override string ToString()
        {
            return "name: " + this.name + ", rep: " + this.reputation + ", distance: " + this.distanceFromObserver;
        }
    }
}
