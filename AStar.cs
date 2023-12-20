using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public static List<Node> FindPath(Node startNode, Node goalNode)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == goalNode)
            {
                return ReconstructPath(currentNode);
            }

            foreach (Node neighbor in currentNode.Neighbors)
            {
                if (closedSet.Contains(neighbor) || neighbor.IsWall)
                    continue;

                int tentativeG = currentNode.GCost + 1;

                if (!openSet.Contains(neighbor) || tentativeG < neighbor.GCost)
                {
                    neighbor.Parent = currentNode;
                    neighbor.GCost = tentativeG;
                    neighbor.HCost = Heuristic(neighbor, goalNode);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null; // No path found
    }

    private static List<Node> ReconstructPath(Node current)
    {
        List<Node> path = new List<Node>();
        while (current != null)
        {
            path.Insert(0, current);
            current = current.Parent;
        }
        return path;
    }

    private static int Heuristic(Node a, Node b)
    {
        // Simple Manhattan distance heuristic
        return Mathf.Abs(a.GridX - b.GridX) + Mathf.Abs(a.GridY - b.GridY);
    }
}
