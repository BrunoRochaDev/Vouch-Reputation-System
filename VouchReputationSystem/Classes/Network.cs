using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VouchReputationSystem.Classes.ReputationFunctions;

namespace VouchReputationSystem.Classes
{
    public class Network
    {
        //Paremeters for the network.
        #region NetworkParameters
        //How many nodes the network scope extends beyond the observer node's immediate neighbours.
        public int networkReach = 2;

        //The default reputation a node is given if it's not a neighbour to the observer node.
        float _defaultNodeReputation = 0.5f;
        public float defaultNodeReputation { get { return _defaultNodeReputation; } set { _defaultNodeReputation = Util.LimitRange(_defaultNodeReputation, 1f, 0f); } }
        #endregion

        //Reference to the observer node
        public AccountNode observerNode;
        //List of all the nodes in the network.
        public List<AccountNode> allNodes = new List<AccountNode>();

        //Returns the account in the network from a node it contains.
        public AccountNode GetNodeWithAccount(Account _acc)
        {
            return allNodes.Find(o => o.name == _acc.name);
        }

        //Constructor
        public Network(Account _observerAcc)
        {
            //Creates the observer node from the given account.
            this.observerNode = new AccountNode(_observerAcc);

            //Get the rest of the nodes in the oberserver node's personal network.
            GetAllNodes();

            //Gives each node it's reputation score.
            SetUpReputation();
        }

        //This method rebuilds the network. Used for when the network parameters are tweaked.
        public void RefreshNetwork()
        {
            //Clears previous data.
            foreach (AccountNode _acc in allNodes)
                _acc.neighbours.Clear();
            allNodes.Clear();

            //Get the rest of the nodes in the oberserver node's personal network.
            GetAllNodes();

            //Gives each node it's reputation score.
            SetUpReputation();
        }

        //This method creates the network AND sets up the neighbour relations.
        private void GetAllNodes()
        {
            //Adds the observer node to the list
            allNodes.Add(observerNode);

            //Get neighbour nodes
            List<AccountNode> neighbourNodes = new List<AccountNode>();
            foreach (Account _acc in observerNode.vouches.Keys)
            {
                if (!observerNode.isVouchValid(_acc))
                    continue;

                AccountNode _node = GetNodeWithAccount(_acc);
                if (_node == null)
                {
                    _node = new AccountNode(_acc);
                    allNodes.Add(_node);
                }

                neighbourNodes.Add(_node);

                //Add the neighbours
                bool polarity = observerNode.vouches[_acc];
                observerNode.neighbours.Add(_node, polarity);
                //Now add to the other node if its a vouch against
                if (!polarity)
                {
                    if (_node.neighbours.ContainsKey(observerNode))
                        _node.neighbours[observerNode] = false;
                    else
                        _node.neighbours.Add(observerNode, false);
                }
            }

            //Now add the neighbour's neighbours up until the reach. Only do this if reach > 0
            if (networkReach > 0)
            {
                //The set of nodes to be evaluated, already with the neighbourNodes
                List<AccountNode> openSet = new List<AccountNode>(neighbourNodes);

                for (int i = 0; i < networkReach; i++)
                {
                    //Validate all the current nodes in openSet
                    foreach (AccountNode currentNode in openSet.ToArray())
                    {
                        //Swap current node from OPEN to CLOSED
                        openSet.Remove(currentNode);

                        //Now look through the current node's neighbours
                        foreach (Account _acc in currentNode.vouches.Keys)
                        {
                            //If the vouch relation is not valid, then skip it.
                            if (!currentNode.isVouchValid(_acc))
                                continue;

                            //Get a reference for the account node. If it doesn't exist, create it.
                            AccountNode _node = GetNodeWithAccount(_acc);
                            if (_node == null)
                            {
                                _node = new AccountNode(_acc);
                                allNodes.Add(_node);
                            }

                            //Add the neighbour to the open set to be avaluated next.
                            openSet.Add(_node);

                            //Setup the neighbours, first to the current node.
                            bool polarity = currentNode.vouches[_acc];
                            if (!currentNode.neighbours.ContainsKey(_node))
                                currentNode.neighbours.Add(_node, polarity);
                            //Now add to the other node if its a vouch against
                            //This isnt quite right. Fix this later
                            if (_node.neighbours.ContainsKey(currentNode))
                                _node.neighbours[currentNode] = polarity;
                            else
                                _node.neighbours.Add(currentNode, polarity);
                        }
                    }
                }
            }
        }

        private void SetUpReputation()
        {
            //First of all, setup all the nodes distances
            foreach (AccountNode _node in allNodes)
            {
                _node.distanceFromObserver = Pathfinding.GetNodeDistance(_node, observerNode);
                _node.hasVouchForPath = Pathfinding.HasVouchPath(_node, observerNode,true);
                _node.hasVouchAgainstPath = Pathfinding.HasVouchPath(_node, observerNode, false);
            }

            //Creates the reputation function
            ReputationFunction reputationFunction = new GeometricFalloff(this);

            //Gets the updated list with the reputation setup.
            allNodes = reputationFunction.GetReputationList(allNodes);
        }

        public void PrintAllNodes()
        {
            Console.WriteLine("NETWORK NODES\nTotal: " + allNodes.Count);

            int count = 1;
            foreach (AccountNode _node in allNodes)
            {
                Console.WriteLine(count + ". " + _node.ToString());
                count++;
            }
        }
    }
}
