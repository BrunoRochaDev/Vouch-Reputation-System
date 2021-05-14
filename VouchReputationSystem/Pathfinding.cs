using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VouchReputationSystem
{
    class Pathfinding
    {      
		public static List<AccountNode> FindPath(AccountNode _startAcc, AccountNode _endAcc)
        {
			List<AccountNode> openSet = new List<AccountNode>();
			HashSet<AccountNode> closedSet = new HashSet<AccountNode>();
			openSet.Add(_startAcc);

			while (openSet.Count > 0)
			{
				AccountNode node = openSet[0];
				for (int i = 1; i < openSet.Count; i++)
				{
					if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
					{
						if (openSet[i].hCost < node.hCost)
							node = openSet[i];
					}
				}

				openSet.Remove(node);
				closedSet.Add(node);

				if (node == _endAcc)
				{
					return RetracePath(_startAcc, _endAcc);
				}

				foreach (AccountNode neighbour in node.neighbours.Keys)
				{
					if (closedSet.Contains(neighbour))
					{
						continue;
					}

					int newCostToNeighbour = node.gCost + 1;
					if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = 1;
						neighbour.ParentNode = node;

						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}

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
