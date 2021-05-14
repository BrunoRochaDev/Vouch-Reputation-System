using System;
using System.Collections.Generic;
using System.Text;

namespace VouchReputationSystem
{
    class Network
    {
        //---------------
        public static int networkReach = 1;
        //---------------

        public AccountNode observerNode;
        public List<AccountNode> allNodes = new List<AccountNode>();

        //Constructor
        public Network(AccountChain _observerAcc)
        {
            this.observerNode = new AccountNode(_observerAcc);
            observerNode.PrintImmediateVouches();
            GetAllNodes();
        }

        void GetAllNodes()
        {
            //Adds the observer node to the list
            allNodes.Add(observerNode);

            //Get neighbour nodes
            List<AccountNode> neighbourNodes = new List<AccountNode>();
            foreach (AccountNode _acc in observerNode.neighbours.Keys)
            {
                AccountNode _node = new AccountNode(_acc);
                allNodes.Add(_node);
                neighbourNodes.Add(_node);
            }

            //Now add the neighbour's neighbours up until the reach. Only do this if reach > 0
            if(networkReach > 0)
            {
                Dictionary<int, List<AccountNode>> neighboursDict = new Dictionary<int, List<AccountNode>>(networkReach);
                //Adds the first layer
                neighboursDict.Add(-1, neighbourNodes);
                for (int i = 0; i < networkReach; i++)
                {
                    List<AccountNode> nextNeighbours = new List<AccountNode>();
                    foreach (AccountNode _node in neighboursDict[i-1])
                    {
                        foreach (AccountChain _neighbour in _node.neighbours.Keys)
                        {
                            AccountNode _neighbourNode = new AccountNode(_neighbour);
                            if (!nextNeighbours.Contains(_neighbourNode))
                                nextNeighbours.Add(_neighbourNode);
                        }
                    }

                    //Add the next layer if still within reach
                    if (i + 1 < networkReach)
                        neighboursDict.Add(i, nextNeighbours);
                }
            
                //Now, remove duplicates
                foreach(List<AccountNode> _nodes in neighboursDict.Values)
                {
                    foreach (AccountNode _node in _nodes)
                        if (!allNodes.Contains(_node))
                            allNodes.Add(_node);
                }
            }          
        }
    
        public void PrintAllNodes()
        {
            Console.WriteLine("NETWORK NODES\nTotal: " + allNodes.Count);

            int count = 1;
            foreach(AccountNode _node in allNodes)
            {
                Console.WriteLine(count + ". " + _node.ToString());
                count++;
            }
        }
    }
}
