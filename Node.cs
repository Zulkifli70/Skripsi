using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public List<Vector2> availableDirections { get; private set; }
    public int GridX { get; } // Kolom grid
    public int GridY { get; } // Baris grid
    public int ScoreValue { get; set; }
    public int GCost;
	public int HCost;
    public int FCost => GCost + HCost;
    public bool IsWall;
    public bool HasPellet;
	public Node Parent;
    public List<Node> Neighbors { get; } = new List<Node>();

    private void Start()
    {
        if (HasPellet)
        {
            InstantiatePellet();
        }
        
        availableDirections = new List<Vector2>();

        // We determine if the direction is available by box casting to see if
        // we hit a wall. The direction is added to list if available.
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }
    

     public Node(int gridX, int gridY)
    {
        GridX = gridX;
        GridY = gridY;
    }
 
    private void CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

        // If no collider is hit then there is no obstacle in that direction
        if (hit.collider == null) {
            availableDirections.Add(direction);
        }
    }
    void InstantiatePellet()
    {
        // Instantiate a pellet prefab at the node's position
        Instantiate(Resources.Load<GameObject>("PelletPrefab"), transform.position, Quaternion.identity);
    }
    
    public void SetNeighbors(Node[,] grid, int gridSizeX, int gridSizeY)
    {
        Neighbors.Clear();

        // Assuming a simple 4-connected grid (top, bottom, left, right)
        if (GridX > 0) Neighbors.Add(grid[GridX - 1, GridY]); // Left
        if (GridX < gridSizeX - 1) Neighbors.Add(grid[GridX + 1, GridY]); // Right
        if (GridY > 0) Neighbors.Add(grid[GridX, GridY - 1]); // Bottom
        if (GridY < gridSizeY - 1) Neighbors.Add(grid[GridX, GridY + 1]); // Top
    }

}
