using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VouchReputationSystem.Classes.ReputationFunctions
{
    class GeometricFalloff : ReputationFunction
    {
        public GeometricFalloff(Network _network) : base(_network)
        {
            //Constructor
        }

        public override string GetFunction(AccountNode _node)
        {
            //Sets the self reputation value to the network's default.
            float selfRep = network.defaultNodeReputation;
            //If the observer node vouches for this node, then set it to 0 or 1 accordingly.
            if (observerNode.vouches.ContainsKey(_node))
                selfRep = observerNode.vouches[_node] ? 1 : 0;

            string numerator = selfRep + "+";
            //The first term of the denominator should be it's w
            string denominator = "1+";

            foreach (AccountNode _neighbour in _node.neighbours.Keys)
            {
                if (_neighbour.Equals(observerNode))
                    continue;

                //If the neighbour has no path for or against to the observer, then dont take it into consideration.
                if (!_neighbour.hasVouchForPath && !_neighbour.hasVouchAgainstPath)
                    continue;

                float term = (_node.neighbours[_neighbour] ? 1 : 0);

                //Invert it if arawahawhawanbz
                if(!_neighbour.hasVouchForPath || !_neighbour.hasVouchForPath)
                {
                    if (term == 1 && _neighbour.hasVouchForPath && !_neighbour.hasVouchForPath)
                        term = 0;
                    if (term == 0 && !_neighbour.hasVouchForPath && _neighbour.hasVouchForPath)
                        term = 1;
                }

                //The distance weight is calculated as 1/2^n, where n is the node's distance to the observer.
                float distanceWeight = (float)(1 / Math.Pow(2, _neighbour.distanceFromObserver));

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
