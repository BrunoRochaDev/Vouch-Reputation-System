using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VouchReputationSystem.Classes
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

        public Dictionary<AccountNode, bool> neighbours = new Dictionary<AccountNode, bool>();

        //Weight of node determined locally
        private float _weight = 1;
        public float weight { get { return _weight; } set { _weight = Util.LimitRange(_weight, 1.5f, 0.5f); } }

        public int distanceFromObserver = 0;

        public float reputation = 1;

        //For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.
        public AccountNode ParentNode;

        //The distance to the start from this node.
        public int gCost;
        //The distance to the goal from this node.
        public int hCost;
        //Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.
        public int fCost { get { return gCost + hCost; } }

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
    }
}
