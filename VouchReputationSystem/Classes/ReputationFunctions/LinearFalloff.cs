using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VouchReputationSystem.Classes.ReputationFunctions
{
    class LinearFalloff : ReputationFunction
    {
        public LinearFalloff(Network _network) : base(_network)
        {
            //Constructor
        }

        public override float Function(AccountNode _node)
        {
            //Sets the self reputation value to the network's default.
            float selfRep = network.defaultNodeRep;
            //If the observer node vouches for this node, then set it to 0 or 1 accordingly.
            if (observerNode.vouches.ContainsKey(_node))
                selfRep = observerNode.vouches[_node] ? 1 : 0;


            return selfRep;
        }
    }
}
