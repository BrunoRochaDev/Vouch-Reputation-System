using System;
using System.Collections.Generic;
using System.Text;

namespace VouchReputationSystem.ReputationFunctions
{
    abstract class ReputationFunction
    {
        public AccountNode observerNode;
        public List<AccountNode> allNodes = new List<AccountNode>();

        public ReputationFunction(AccountNode _observerNode, List<AccountNode> _allNodes)
        {
            this.observerNode = _observerNode;
            this.allNodes = _allNodes;
        }

        public List<AccountNode> GetReputationList(List<AccountNode> _nodeList)
        {
            foreach(AccountNode _node in _nodeList)
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
