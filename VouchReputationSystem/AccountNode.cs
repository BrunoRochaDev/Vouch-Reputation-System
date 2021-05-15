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
            this.vouches = _acc.vouches;
        }
        public AccountNode(AccountChain _acc, float _weight)
        {
            this.name = _acc.name;
            this.vouches = _acc.vouches;
            this.weight = _weight;
        }

        //Weight of node determined locally
        private float _weight = 1;
        public float weight { get { return _weight; } set { _weight = Util.LimitRange(_weight, 1.5f, 0.5f); } }

        public int distanceFromObserver = 0;

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

        public Dictionary<AccountNode, bool> GetNeighbours()
        {
            Dictionary<AccountNode, bool> _result = new Dictionary<AccountNode, bool>();
            foreach (AccountChain _node in this.vouches.Keys)
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
    }
}
