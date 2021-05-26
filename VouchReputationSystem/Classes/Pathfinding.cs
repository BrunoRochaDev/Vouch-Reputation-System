using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VouchReputationSystem.Classes
{
	class Pathfinding
	{
		public static int GetNodeDistance(AccountNode _node, AccountNode _observer)
		{
			if (_node == _observer)
				return 0;

			return Pathfinding.FindPath(_node, _observer).Count;
		}

		public static List<AccountNode> FindPath(AccountNode _startNode, AccountNode _endNode)
        {
            return PathfindAlgorithim(_startNode, _endNode, 0);
        }

		public static bool HasVouchPath(AccountNode _startNode, AccountNode _endNode, bool _polarity)
        {
			//Go through pathfinding
			return PathfindAlgorithim(_startNode, _endNode, _polarity ? 1 : -1 ).Count > 0;
		}

        private static List<AccountNode> PathfindAlgorithim(AccountNode _startNode, AccountNode _endNode, int _obstacle)
        {
			//This is using the A* algorithm

			/*
			We can find a path looking for any connection between node or exclusively from one type of connection, that is, positive or negative.
			If _obstacle is zero, then every node is a possible path.
			If _obstacle is positive, then only positive relations are valid.
			If _obstacle is negative, then only negative relations are valid.
			 */

			//The set of nodes to be evaluated
			List<AccountNode> openSet = new List<AccountNode>();
            //The set of nodes to already evaluated
            HashSet<AccountNode> closedSet = new HashSet<AccountNode>();

            //At first, the open set consists only of the start currentNode
            openSet.Add(_startNode);

            //Loops until there are no more sets to evaluate
            while (openSet.Count > 0)
			{
				//Find the node in OPEN with the lowest f cost
				AccountNode currentNode = openSet[0];
				for (int i = 1; i < openSet.Count; i++)
				{
					if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
					{
						if (openSet[i].hCost < currentNode.hCost)
							currentNode = openSet[i];
					}
				}

				//Swap current node from OPEN to CLOSED
				openSet.Remove(currentNode);
				closedSet.Add(currentNode);

				//If the current node is the end node, then we have a path
				if (currentNode.Equals(_endNode))
					return RetracePath(_startNode, currentNode);

				//Loop through each neighbour node from the current node looking for valid neighbours to add to the OPEN list.
				foreach (AccountNode neighbour in currentNode.neighbours.Keys)
				{
					//If the neighbour node was already evaluated, then we dont need to considerer it further.
					if (closedSet.Contains(neighbour))
						continue;

					//Now, for obstacle checking. If _obstacle is zero, then we are not looking for obstacles
					if (_obstacle != 0)
                    {
						//If _obstacle is positive, then we are looking only for positive connections
						if (_obstacle >= 1)
                        {
							//The connection is negative. This node is not suitiable
							if (!neighbour.neighbours[currentNode])
								continue;
                        }
						//If _obstacle is negative, then we are looking only for negative connections
						else
						{
							//The connection is positive. This node is not suitiable
							if (neighbour.neighbours[currentNode])
								continue;
						}
                    }

					//If the new path to neighhbour is shorter OR neighbor is not in OPEN
					int newCostToNeighbour = currentNode.gCost + 1;
					if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						//Set fCost (gCost + hCost)
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = 1;
						//Set the parent node
						neighbour.ParentNode = currentNode;

						//If the OPEN set not already contaisn neighbour, then add it.
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
			//If it made it this far, then all the nodes were eveluated but still couldnt reach the target node. Meaning that there is no possible path.
			Console.WriteLine("Error: Could not find path from " + _startNode.name + " to " + _endNode.name);
			return new List<AccountNode>();
		}

		static List<AccountNode> RetracePath(AccountNode _start, AccountNode _end)
		{
			List<AccountNode> path = new List<AccountNode>();
			AccountNode currentNode = _end;

			while (currentNode != _start)
			{
				path.Add(currentNode);
				currentNode = currentNode.ParentNode;
			}
			path.Reverse();

			return path;
		}
	}
}
