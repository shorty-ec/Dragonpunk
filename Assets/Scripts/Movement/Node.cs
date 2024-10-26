using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class Node : MonoBehaviour
{
    [Serializable]
    public class NodeConnection
    {
        public KeyCode key;  // The key used to go to the connected node
        public Node connectedNode;  // The node to move towards
        public bool isElevatorNode;
    }
    // List of connections from this node to other nodes
    public List<NodeConnection> nodeConnections = new List<NodeConnection>();

    // Trigger when the player enters the node
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NodeManager nodeManager = other.GetComponent<NodeManager>();
            if (nodeManager != null)
            {
                nodeManager.EnterNode(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NodeManager nodeManager = other.GetComponent<NodeManager>();
            if (nodeManager != null)
            {
                nodeManager.ExitNode(this);
            }
        }
    }

    // Convert the list of connections to a dictionary for easy access in code
    public Dictionary<KeyCode, Node> GetConnectionsDictionary()
    {
        Dictionary<KeyCode, Node> connections = new Dictionary<KeyCode, Node>();
        foreach (NodeConnection connection in nodeConnections)
        {
            if (!connections.ContainsKey(connection.key))
            {
                connections.Add(connection.key, connection.connectedNode);
            }
        }
        return connections;
    }

    public List<Node> GetElevatorNodes()
    {
        var elevatorConnections = nodeConnections.Where(x => x.isElevatorNode);
        List<Node> elevatorNodes = new();
        foreach(var conn in elevatorConnections)
        {
            elevatorNodes.Add(conn.connectedNode);
        }
        return elevatorNodes;
    }

    // Optionally draw gizmos to visualize connections between nodes in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (NodeConnection connection in nodeConnections)
        {
            if (connection.connectedNode != null)
            {
                Gizmos.DrawLine(transform.position, connection.connectedNode.transform.position);
            }
        }
    }
}

