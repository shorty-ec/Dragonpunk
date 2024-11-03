using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private bool _currentlyMovingElevator;
    private bool _currentlyInElevator;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator animator;
    [SerializeField] float forgivenessMargin = 0.1f;
    float lastPositionX;
    bool isWalking;
    [SerializeField] TextMeshProUGUI textMesh;

    void Start()
    {
        currentState = PlayerState.NodePending;
        lastPositionX = transform.position.x;
    }

    void Update()
    {
        if (_currentlyMovingElevator) return; //DO NOTHING IF CURRENTLY IN MOVING ELEVATOR
        // Detect input directly to update speed more responsively
        float speed = Input.GetAxisRaw("Horizontal");
        // Flip sprite based on movement direction
        if (speed < 0)
        {
            sprite.flipX = false;  // Moving left
        }
        else if (speed > 0)
        {
            sprite.flipX = true; // Moving right
        }

        if (isWalking)
        {
            animator.SetFloat("Speed", Mathf.Abs(1f));
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(0f));
        }



        // Update last position to reflect current frame
        lastPositionX = transform.position.x;

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
                HandleInput();
                break;
            case PlayerState.Movement:
                MoveToTargetNode();
                //if we have a nodeCollidedWith
                if (currentlyCollidedNode)
                    HandleInput();
                break;
        }



        
    }

    // Method to enter a node's trigger and switch to Pending State
    public void EnterNode(Node node)
    {
        currentNode = node;

        if (!_currentlyInElevator)
            currentState = PlayerState.NodePending;
        else
            currentState = PlayerState.Elevator;

        currentlyCollidedNode = node;

        Dictionary<KeyCode, Node> connections = currentlyCollidedNode.GetConnectionsDictionary();
        string inputAction = "Movement Options";
        foreach (var entry in connections)
        {
            KeyCode key = entry.Key;
            inputAction += key.ToString()+" ";
        }
        _currentlyMovingElevator = false;
        textMesh.SetText(inputAction);
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
            //Debug.Log("Entered Node Open State");
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
                //Debug.Log("Attempt" + key.ToString());
                holdDuration += Time.deltaTime;

                if (holdDuration >= holdTime)
                {
                    if (entry.Value.isLocked())
                        continue;
                    //Debug.Log("success" + key.ToString());
                    //We found our next node

                    // if we are in elevator state and we are going to an elevator node
                    if (currentState == PlayerState.Elevator && currentlyCollidedNode.nodeConnections.Find(x => x.connectedNode == entry.Value).isElevatorNode)
                    {
                        MoveWithElevator(entry.Value);
                    }
                    else
                    {
                        SetTargetNode(entry.Value, key);
                    }
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
        //Debug.Log("Entered Movement State");
    }
    private void MoveWithElevator(Node target_node)
    {
        targetNode = target_node;
        //currentKey = key;
        oppositeKey = GetOppositeKey(currentKey);
        previousNode = currentNode;
        holdDuration = 0f;

        _elevator.Move(target_node.transform.position);
        //also disable movement temporarily while the elevator moves
        _currentlyMovingElevator = true;
    }
    public void SetElevator(Elevator elevator)
    {
        _currentlyInElevator = true;
        _elevator = elevator;
        currentState = PlayerState.Elevator;
    }

    public void ExitElevator()
    {
        _currentlyInElevator = false;
    }


    private void MoveToTargetNode()
    {
        if (Input.GetKey(currentKey))
        {
            isWalking = true;
            Vector3 directionToTarget = (targetNode.transform.position - transform.position).normalized;
            transform.position += directionToTarget * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.1f)
            {
                EnterNode(targetNode);  // Enter the new node
            }
        }
        else if (Input.GetKey(oppositeKey) && previousNode != null)
        {
            isWalking = true;
            Vector3 directionToPrevious = (previousNode.transform.position - transform.position).normalized;
            transform.position += directionToPrevious * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, previousNode.transform.position) < 0.1f)
            {
                EnterNode(previousNode);  // Enter the previous node
            }
        }
        else
        {
            isWalking = false;
        }
    }

    private KeyCode GetOppositeKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.W: return KeyCode.S;
            case KeyCode.S: return KeyCode.W;
            case KeyCode.A: return KeyCode.D;
            case KeyCode.D: return KeyCode.A;
            /*3D
            case KeyCode.W: return KeyCode.X;
            case KeyCode.X: return KeyCode.W;
            case KeyCode.A: return KeyCode.D;
            case KeyCode.D: return KeyCode.A;
            case KeyCode.E: return KeyCode.Z;
            case KeyCode.Z: return KeyCode.E;
            case KeyCode.Q: return KeyCode.C;
            case KeyCode.C: return KeyCode.Q;*/
            default: return key;  // If no opposite, return the same key
        }
    }
}