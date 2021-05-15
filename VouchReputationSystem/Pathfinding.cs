using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VouchReputationSystem
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
			//This is using the A* algorithm

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
