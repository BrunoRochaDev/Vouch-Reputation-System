using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VouchReputationSystem
{
    class Pathfinding
    {      
		public static List<AccountChain> FindPath(AccountChain _startAcc, AccountChain _endAcc)
        {
			List<AccountChain> openSet = new List<AccountChain>();
			HashSet<AccountChain> closedSet = new HashSet<AccountChain>();
			openSet.Add(_startAcc);

			while (openSet.Count > 0)
			{
				AccountChain node = openSet[0];
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

				foreach (AccountChain neighbour in node.neighbours.Keys)
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

			return new List<AccountChain>();
		}

		static List<AccountChain> RetracePath(AccountChain _start, AccountChain _end)
		{
			List<AccountChain> path = new List<AccountChain>();
			AccountChain currentNode = _end;

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
