using System;
using System.Collections.Generic;
using System.Text;

namespace VouchReputationSystem
{
    class AccountChain
    {
        public string name;
        public int nom;
        public float weight;

        //Dictionary with every node it has a connection with. True is vouch for, False is vouch against.
        public Dictionary<AccountChain, bool> edges = new Dictionary<AccountChain, bool>();

        private Dictionary<AccountChain, bool> _neighbours = new Dictionary<AccountChain, bool>();
        public Dictionary<AccountChain, bool> neighbours
        {
            get
            {
                GetNeighbours();
                return _neighbours;
            }
            set
            {
                _neighbours = value;
            }
        }

        //Constructors
        public AccountChain()
        {
            //Just here because of inheritance
        }
        public AccountChain(string _name)
        {
            this.name = _name;
            this.weight = 1;
        }
        //Now with weight
        public AccountChain(string _name, float _weight)
        {
            this.name = _name;
            this.weight = LimitRange(_weight, 0.5f, 1.5f);
        }

        #region pathfinding

        public AccountChain ParentNode;//For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.

        public int gCost;//The distance to the start from this node.
        public int hCost;//The distance to the goal from this node.

        public int fCost { get { return gCost + hCost; } }//Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.

        #endregion

        #region vouch

        //Function that adds a node to the edge dictionary.
        public void Vouch(AccountChain _node, bool _polarity)
        {
            //Cannot add itself as a vouch
            if (_node.Equals(this))
            {
                Console.WriteLine("Error: Cannot vouch for itself.");
                return;
            }

            //If given node is already vouched for
            if (this.edges.ContainsKey(_node))
            {
                //If the current vouch polarity is already whats being vouched, then remove it
                if (edges[_node].Equals(_polarity))
                    edges.Remove(_node);
                //If not, then update it
                else
                    this.edges[_node] = _polarity;
            }
            else
                //If not, vouch for it.
                this.edges.Add(_node, _polarity);

            //Update neighbours list
            GetNeighbours();
        }
        //Simplified function only for vouching for
        public void VouchFor(AccountChain _node)
        {
            this.Vouch(_node, true);
        }
        //Simplified function only for vouching against
        public void VouchAgainst(AccountChain _node)
        {
            this.Vouch(_node, false);
        }

        public bool DoesVouchFor(AccountChain _otherNode)
        {
            //For now, people can only vouch for someone if they are being vouched for in return.

            //Gets the vouch state from this node
            bool thisPolarity;
            if (this.edges.ContainsKey(_otherNode))
                thisPolarity = this.edges[_otherNode];
            else
                thisPolarity = false;

            //Gets the vouch state from the other node
            bool otherPolarity;
            if (_otherNode.edges.ContainsKey(this))
                otherPolarity = _otherNode.edges[this];
            else
                otherPolarity = false;


            return (thisPolarity && otherPolarity);
        }

        public bool DoesVouchAgainst(AccountChain _otherNode)
        {
            //Gets the vouch state from this node
            bool thisPolarity = true;
            if (this.edges.ContainsKey(_otherNode))
                thisPolarity = this.edges[_otherNode];

            //Gets the vouch state from the other node
            bool otherPolarity = true;
            if (_otherNode.edges.ContainsKey(this))
                otherPolarity = _otherNode.edges[this];

            return !(thisPolarity && otherPolarity);
        }

        public Dictionary<AccountChain, bool> GetNeighbours()
        {
            Dictionary<AccountChain, bool> _result = new Dictionary<AccountChain, bool>();
            foreach (AccountChain _node in edges.Keys)
            {
                bool vouchFor = DoesVouchFor(_node);
                bool vouchAgainst = DoesVouchAgainst(_node);

                //If both are true, then something is wrong...
                if (vouchFor && vouchAgainst)
                {
                    Console.WriteLine("Error! A node cant vouch for and against some other at the same time.");
                    continue;
                }

                //If both are true, then something is wrong...
                if (!vouchFor && !vouchAgainst)
                    continue;

                _result.Add(_node, vouchFor ? true : false);
            }

            neighbours = _result;
            return _result;
        }

        #endregion

        #region string-functions

        public override string ToString()
        {
            return "name: " + this.name;
        }

        //Prints all the edges in string format.
        public string PrintImmediateVouches()
        {
            //If there are not edges...
            if (this.edges.Count == 0)
                return this.name + " has no vouches.";

            string _result = "";
            //Get vouch relations of each edge node.
            foreach (AccountChain _node in neighbours.Keys)
            {
                bool polarity = neighbours[_node];

                //Get result in string format
                if (polarity)
                    _result += "(" + this.name +") <=> (" + _node.name + ")\n";
                else
                    _result += "(" + this.name + ") =x= (" + _node.name + ")\n";
            }

            //If there are not relations...
            if (string.IsNullOrWhiteSpace(_result))
                return this.name + " has no vouches.";

            return this.name + " vouch relations:\n" + _result;
        }

        //Prints all the reputations in string format.
        public string PrintReputations()
        {
            Dictionary<AccountChain, float> reputationDict = this.GetReputationOfAll();
            //If there are not edges...
            if (reputationDict.Count == 0)
                return this.name + " has no vouches.";

            string _result = "";
            //Get vouch relations of each edge node.
            foreach (AccountChain _node in reputationDict.Keys)
            {
                _result += _node.name + ": " + reputationDict[_node]+"\n";
            }

            return this.name + "'s reputation list:\n" + _result;
        }

        #endregion

        #region reputation

        private Dictionary<AccountChain, float> GetReputationOfAll()
        {
            Dictionary<AccountChain, float> result = new Dictionary<AccountChain, float>();

            //Get reputation of each edge node.
            foreach (AccountChain _node in edges.Keys)
            {
                result.Add(_node, GetReputationOfNode(_node));
            }

            return result;
        }

        private float GetReputationOfNode(AccountChain _node)
        {
            //Loop through each node to determine all the neighbors

            return 1;
        }

        private float LinearReputation(Dictionary<AccountChain, int> _nodes)
        {
            return 1;
        }

        public int GetAccDistance(AccountChain _other)
        {
            if (_other == this)
                return 0;

            return Pathfinding.FindPath(this, _other).Count;
        }

        #endregion

        //Function that ensures that values are always between 0 and 1.
        static float LimitRange(float value, float inclusiveMinimum, float inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }
    }
}
