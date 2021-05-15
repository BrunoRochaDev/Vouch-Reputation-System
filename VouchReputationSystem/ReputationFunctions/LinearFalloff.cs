using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VouchReputationSystem.ReputationFunctions
{
    class LinearFalloff : ReputationFunction
    {
        public LinearFalloff(AccountNode _observerNode, List<AccountNode> _allNodes) : base(_observerNode, _allNodes)
        {
            //Constructor
        }

        public override float Function(AccountNode _node)
        {
            //List<AccountNode> relevantNeighbours = observerNode.neighbours.Keys.Intersect(_node.neighbours.Keys).ToList();
            return 1;
        }
    }
}
