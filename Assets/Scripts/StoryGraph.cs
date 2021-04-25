using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoryGraph : MonoBehaviour
{
    private NodeBehaviour currentNode;
    [HideInInspector] public GameController controller;

    public GameObject nodePrefab;
    public TextAsset nodeInfosText;
    public TextAsset nodeConnectionsText;

    private Dictionary<int, GameObject> nodeMap = new Dictionary<int, GameObject>();
    private Dictionary<int, List<int>> nodeConnections = new Dictionary<int, List<int>>();

    public void LoadGraph()
    {
        NodeInfos nodeInfos = JsonUtility.FromJson<NodeInfos>(nodeInfosText.text);
        foreach (NodeInfo nodeInfo in nodeInfos.nodes)
        {
            GameObject spawnedNode = Instantiate(nodePrefab);
            NodeBehaviour node = spawnedNode.GetComponent<NodeBehaviour>();
            node.info = nodeInfo;
            node.graph = this;
            nodeMap.Add(nodeInfo.id, spawnedNode);
        }

        NodeConnections connectionsInfo = JsonUtility.FromJson<NodeConnections>(nodeConnectionsText.text);
        foreach (NodeConnection connectionInfo in connectionsInfo.connections) 
        {
            if (!nodeConnections.ContainsKey(connectionInfo.source)) 
            {
                nodeConnections.Add(connectionInfo.source, new List<int>());
            }
            nodeConnections[connectionInfo.source].Add(connectionInfo.destination);
        }
    }

    public NodeBehaviour GetCurrentNode()
    {
        return currentNode;
    }

    public void UpdateState(NodeBehaviour updatedNode) 
    {
        if (updatedNode == currentNode) 
        {
            return;
        }

        updatedNode.MarkActive();
        updatedNode.visited = true;
        foreach (int nodeId in nodeConnections[updatedNode.info.id]) 
        {
            nodeMap[nodeId].GetComponent<NodeBehaviour>().MarkActive();
        }
        currentNode = updatedNode;
        controller.UpdateScene(currentNode.info);
    }
}
