using System;
using System.Collections.Generic;
using System.Text;

namespace VouchReputationSystem
{
    class AccountNode : AccountChain
    {
        //Creates the node from the account
        public AccountNode(AccountChain _acc)
        {
            this.name = _acc.name;
            this.connections = _acc.connections;
        }

        //Weight of node determined locally
        private float _weight = 1;
        public float weight { get { return _weight; } set { _weight = LimitRange(_weight, 1.5f, 0.5f); } }

        public float reputation = 1;

        //
        public Dictionary<AccountNode, bool> neighbours  {   get { return GetNeighbours(); }  }

        //For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.
        public AccountNode ParentNode;

        //The distance to the start from this node.
        public int gCost;
        //The distance to the goal from this node.
        public int hCost;
        //Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.
        public int fCost { get { return gCost + hCost; } }

        #region reputation

        private Dictionary<AccountChain, float> GetReputationOfAll()
        {
            Dictionary<AccountChain, float> result = new Dictionary<AccountChain, float>();

            //Get reputation of each edge node.
            foreach (AccountChain _node in connections.Keys)
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

        public int GetAccDistance(AccountNode _other)
        {
            if (_other == this)
                return 0;

            return Pathfinding.FindPath(this, _other).Count;
        }

        #endregion

        public Dictionary<AccountNode, bool> GetNeighbours()
        {
            Dictionary<AccountNode, bool> _result = new Dictionary<AccountNode, bool>();
            foreach (AccountChain _node in this.connections.Keys)
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

                _result.Add(new AccountNode(_node), vouchFor ? true : false);
            }

            return _result;
        }

        #region print
        //Prints all the connections in string format.
        public void PrintImmediateVouches()
        {
            //If there are not connections...
            if (this.connections.Count == 0)
                Console.WriteLine(this.name + " has no vouches.");

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
            return "name: " + this.name + ", rep: " + this.reputation;
        }
        #endregion

        //Function that ensures that values are always between 0 and 1.
        static float LimitRange(float value, float inclusiveMinimum, float inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }

        //Equivalence
        public override bool Equals(object obj)
        {
            var item = obj as AccountChain;

            if (item == null)
            {
                return false;
            }

            return this.id.Equals(item.id);
        }
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }
    }
}
