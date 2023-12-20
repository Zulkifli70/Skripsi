using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    public AnimatedSprite deathSequence;
    public SpriteRenderer spriteRenderer { get; private set; }
    public new Collider2D collider { get; private set; }
    public Movement movement { get; private set; }
    public float speed = 5f;
    private List<Node> path;
    private int currentPathIndex = 0;
    private Node nearestPelletNode;

    void Update()
    {
        HandleInput();
        Move();
    }

    void HandleInput()
    {
        // ... existing input handling ...

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Use A* to find the path to the nearest pellet
            Node startNode = FindPacmanStartNode();
            Node goalNode = FindNearestPelletNode(startNode);

            path = AStar.FindPath(startNode, goalNode);

            currentPathIndex = 0;
        }
    }

    void Move()
    {
        if (path != null && currentPathIndex < path.Count)
        {
            transform.position = Vector2.MoveTowards(transform.position, path[currentPathIndex].transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, path[currentPathIndex].transform.position) < 0.01f)
            {
                // Check for pellet at the current position
                CollectPellet(path[currentPathIndex]);

                currentPathIndex++;
            }
        }
    }


    Node FindPacmanStartNode()
    {
        // Convert Pac-Man's position to a node in your grid
        // This may involve some calculations based on the grid size, node spacing, etc.
        // For simplicity, this example assumes a simple conversion
        return GetNodeAtPosition(transform.position);
    }

    Node FindNearestPelletNode(Node startNode)
    {
        // Find the nearest pellet node using A* or other methods
        // You may need to modify this based on the repository's grid structure

        // ... existing code to find the nearest pellet node ...

        return nearestPelletNode;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        movement = GetComponent<Movement>();
    }


    void CollectPellet(Node node)
    {
        // Handle pellet collection logic
        if (node.HasPellet)
        {
            node.HasPellet = false;
            Destroy(node.gameObject); // Assuming the pellet is a child of the node
            GameManager.Instance.UpdateScore(node.GetComponentInChildren<Pellet>().scoreValue);
        }
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        deathSequence.enabled = false;
        deathSequence.spriteRenderer.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.spriteRenderer.enabled = true;
        deathSequence.Restart();
    }

    Node GetNodeAtPosition(Vector3 position)
    {
        // Convert position to a node in your grid
        // This is a placeholder and may need modification based on the grid structure
        foreach (var node in GameObject.FindObjectsOfType<Node>())
        {
            if (Vector2.Distance(position, node.transform.position) < 0.1f)
            {
                return node;
            }
        }

        return null;
    }
}


