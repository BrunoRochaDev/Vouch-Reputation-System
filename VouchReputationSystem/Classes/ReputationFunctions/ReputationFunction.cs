using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VouchReputationSystem.Classes.ReputationFunctions
{
    abstract class ReputationFunction
    {
        public Network network;
        public AccountNode observerNode;
        public List<AccountNode> allNodes = new List<AccountNode>();

        public ReputationFunction(Network _network)
        {
            this.network = _network;
            this.observerNode = _network.observerNode;
            this.allNodes = _network.allNodes;
        }

        public List<AccountNode> GetReputationList(List<AccountNode> _nodeList)
        {
            foreach (AccountNode _node in _nodeList)
                _node.reputation = Function(_node);

            return _nodeList;
        }

        public AccountNode GetReputationOfNode(AccountNode _node)
        {
            _node.reputation = Function(_node);
            return _node;
        }

        public abstract float Function(AccountNode _node);
    }
}
