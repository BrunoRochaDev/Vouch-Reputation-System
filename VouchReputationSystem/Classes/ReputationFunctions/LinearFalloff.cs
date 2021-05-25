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

        public override string GetFunction(AccountNode _node)
        {
            //Sets the self reputation value to the network's default.
            float selfRep = network.defaultNodeRep;
            //If the observer node vouches for this node, then set it to 0 or 1 accordingly.
            if (observerNode.vouches.ContainsKey(_node))
                selfRep = observerNode.vouches[_node] ? 1 : 0;

            string numerator = selfRep + "+";
            string denominator = "1+";

            foreach (AccountNode _neighbour in _node.neighbours.Keys)
            {
                if (_neighbour.Equals(observerNode))
                    continue;

                int term = (_node.neighbours[_neighbour] ? 1 : 0);

                float distanceWeight = ((float)network.networkReach - (float)_node.distanceFromObserver) / (float)network.networkReach;

                //Adds a minimum for the distance weight. Placeholder.
                if (distanceWeight <= 0)
                    distanceWeight = 0.1f;

                string weight = _neighbour.name + "*" + distanceWeight;

                if (term > 0 && distanceWeight > 0)
                    numerator += weight + "+";
                denominator += weight + "+";
            }
            numerator = numerator.TrimEnd('+');
            denominator = denominator.TrimEnd('+');

            string formula = "(" + numerator + ")/(" + denominator + ")";

            return formula +"-"+ _node.name;
        }
    }
}
