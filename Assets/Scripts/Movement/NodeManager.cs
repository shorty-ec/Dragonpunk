using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public enum PlayerState { NodePending, NodeOpen, Movement, Elevator }

    public float moveSpeed = 5f;   // Speed of movement between nodes
    public float holdTime = 0.5f;  // Time required to hold a key for movement
    public float smoothDampTime = 0.1f; // Time for smoothing speed

    public PlayerState currentState = PlayerState.NodePending;
    private Node currentNode;         // Node the player is currently in
    private Node targetNode;          // Node the player is moving towards
    private Node previousNode;        // The last node the player was at
    private Node currentlyCollidedNode; //Node that you are colliding with
    private KeyCode currentKey;       // The key pressed to move towards target node
    private KeyCode oppositeKey;      // The opposite key for going back to the previous node
    private float holdDuration = 0f;  // How long the player has been holding a key
    private Elevator _elevator;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator animator;
    [SerializeField] float forgivenessMargin = 0.1f;
    float lastPositionX;

    void Start()
    {
        currentState = PlayerState.NodePending;
        lastPositionX = transform.position.x;
    }

    void Update()
    {
        // Detect input directly to update speed more responsively
        float speed = Input.GetAxisRaw("Horizontal") * moveSpeed;

        // Update the 'Speed' parameter in the animator to make it more responsive
        animator.SetFloat("Speed", Mathf.Abs(speed));
        // Handle state logic for node movement
        switch (currentState)
        {
            case PlayerState.NodePending:
                MoveToCenterOfNode();
                break;
            case PlayerState.NodeOpen:
                HandleInput();
                break;
            case PlayerState.Elevator:
                MoveWithElevator();
                break;
            case PlayerState.Movement:
                MoveToTargetNode();
                //if we have a nodeCollidedWith
                if (currentlyCollidedNode)
                    HandleInput();
                break;
        }



        // Flip sprite based on movement direction
        if (speed < 0)
        {
            sprite.flipX = false;  // Moving left
        }
        else if (speed > 0)
        {
            sprite.flipX = true; // Moving right
        }

        // Update last position to reflect current frame
        lastPositionX = transform.position.x;
    }

    // Method to enter a node's trigger and switch to Pending State
    public void EnterNode(Node node)
    {
        currentNode = node;
        currentState = PlayerState.NodePending;

        currentlyCollidedNode = node;

        // Ensure we only reset previousNode to null when it's the initial node
        //if (previousNode == null)
        //{
            previousNode = currentNode;
        //}
    }

    public void ExitNode(Node node)
    {
        if (node == currentlyCollidedNode)
        {
            currentlyCollidedNode = null;
        }
    }

    private void MoveToCenterOfNode()
    {
        if (currentNode == null) return;

        transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, moveSpeed * Time.deltaTime);

        // If we're at the center, switch to NodeOpen state
        if (Vector3.Distance(transform.position, currentNode.transform.position) < 0.1f)
        {
            currentState = PlayerState.NodeOpen;
            Debug.Log("Entered Node Open State");
        }
    }

    private void HandleInput()
    {
        Dictionary<KeyCode, Node> connections = currentlyCollidedNode.GetConnectionsDictionary();
        foreach (var entry in connections)
        {
            KeyCode key = entry.Key;
            if (Input.GetKey(key))
            {
                Debug.Log("Attempt" + key.ToString());
                holdDuration += Time.deltaTime;

                if (holdDuration >= holdTime)
                {
                    Debug.Log("success" + key.ToString());
                    SetTargetNode(entry.Value, key);
                    return;
                }
            }
            else if (Input.GetKeyUp(key))
            {
                holdDuration = 0f;  // Reset hold time if key is released
            }
        }
    }

    private void SetTargetNode(Node node, KeyCode key)
    {
        if (node == null) return;

        targetNode = node;
        currentKey = key;

        oppositeKey = GetOppositeKey(currentKey);

        previousNode = currentNode;

        currentState = PlayerState.Movement;
        holdDuration = 0f;
        Debug.Log("Entered Movement State");
    }
    private void MoveWithElevator()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            _elevator.Move(currentlyCollidedNode.GetElevatorNodes()[0].transform.position);
        }*/

        //Grab possible elevator nodes, decide which ones should be which inputs
        //If that input is pressed, move the elevator to that node with _elevator.Move()
        //also disable movement temporarily while the elevator moves
    }
    public void SetElevator(Elevator elevator)
    {
        _elevator = elevator;
        currentState = PlayerState.Elevator;
    }


    private void MoveToTargetNode()
    {
        if (Input.GetKey(currentKey))
        {
            Vector3 directionToTarget = (targetNode.transform.position - transform.position).normalized;
            transform.position += directionToTarget * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.1f)
            {
                EnterNode(targetNode);  // Enter the new node
            }
        }
        else if (Input.GetKey(oppositeKey) && previousNode != null)
        {
            Vector3 directionToPrevious = (previousNode.transform.position - transform.position).normalized;
            transform.position += directionToPrevious * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, previousNode.transform.position) < 0.1f)
            {
                EnterNode(previousNode);  // Enter the previous node
            }
        }
    }

    private KeyCode GetOppositeKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W: return KeyCode.X;
            case KeyCode.X: return KeyCode.W;
            case KeyCode.A: return KeyCode.D;
            case KeyCode.D: return KeyCode.A;
            case KeyCode.E: return KeyCode.Z;
            case KeyCode.Z: return KeyCode.E;
            case KeyCode.Q: return KeyCode.C;
            case KeyCode.C: return KeyCode.Q;
            default: return key;  // If no opposite, return the same key
        }
    }
}