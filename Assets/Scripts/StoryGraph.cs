using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoryGraph : MonoBehaviour
{
    private GameObject currentNode;
    [HideInInspector] public GameController controller;

    public GameObject nodePrefab;
    public LineRenderer lineRenderer;
    public TextAsset nodeInfosText;
    public TextAsset nodeConnectionsText;

    public float nodeDistanceX = 10f;
    public float nodeDistanceY = 5f;

    private Dictionary<int, GameObject> nodeMap = new Dictionary<int, GameObject>();
    private Dictionary<int, List<int>> nodeConnections = new Dictionary<int, List<int>>();

    public void LoadGraph()
    {
        NodeInfos nodeInfos = JsonUtility.FromJson<NodeInfos>(nodeInfosText.text);
        foreach (NodeInfo nodeInfo in nodeInfos.nodes)
        {
            GameObject spawnedNode = Instantiate(nodePrefab);
            spawnedNode.SetActive(false);
            NodeBehaviour node = spawnedNode.GetComponent<NodeBehaviour>();
            node.info = nodeInfo;
            node.graph = this;
            nodeMap.Add(nodeInfo.id, spawnedNode);
        }

        HashSet<int> allNodes = new HashSet<int>();
        HashSet<int> destinationNodes = new HashSet<int>();
        NodeConnections connectionsInfo = JsonUtility.FromJson<NodeConnections>(nodeConnectionsText.text);
        foreach (NodeConnection connectionInfo in connectionsInfo.connections) 
        {
            if (!nodeConnections.ContainsKey(connectionInfo.source)) 
            {
                nodeConnections.Add(connectionInfo.source, new List<int>());
            }
            nodeConnections[connectionInfo.source].Add(connectionInfo.destination);
            
            allNodes.Add(connectionInfo.source);
            allNodes.Add(connectionInfo.destination);

            destinationNodes.Add(connectionInfo.destination);
        }

         allNodes.ExceptWith(destinationNodes);
        if (allNodes.Count != 1)
        {
            string error = "";
            foreach (int nd in allNodes)
            {
                error += nd + ", ";
            }
            throw new MissingComponentException("Incorrect graph configuration, incorrect source node count : " + allNodes.Count + " - " + error);
        }

        int sourceId = new List<int>(allNodes)[0];
        GameObject sourceNode = nodeMap[sourceId];
        UpdateState(sourceNode);
    }

    public NodeBehaviour GetCurrentNode()
    {
        return currentNode.GetComponent<NodeBehaviour>();
    }

    public void UpdateState(GameObject updatedNode) 
    {
        if (updatedNode == currentNode) 
        {
            return;
        }

        if (currentNode != null)
        {
            NodeBehaviour currentInfo = currentNode.GetComponent<NodeBehaviour>();
            List<int> currentOptionIds = nodeConnections.ContainsKey(currentInfo.info.id) ? 
                nodeConnections[currentInfo.info.id] : new List<int>();
            foreach (int ndId in currentOptionIds)
            {
                nodeMap[ndId].SetActive(false);
            }
        }

        controller.UpdateScene(updatedNode);
        NodeBehaviour updatedNodeBehaviour = updatedNode.GetComponent<NodeBehaviour>();
        updatedNode.SetActive(true);
        updatedNodeBehaviour.visited = true;
        Vector3 curPos = updatedNode.transform.position;
        
        List<int> nodeIds = nodeConnections.ContainsKey(updatedNodeBehaviour.info.id) ?
            nodeConnections[updatedNodeBehaviour.info.id] : new List<int>();
        float[] nodePosY = GetNodePositionsY(curPos.y, nodeIds.Count);
        
        for (int i = 0; i < nodeIds.Count; i++)
        {
            int nodeId = nodeIds[i];
            GameObject childObject = nodeMap[nodeId];
            childObject.SetActive(true);
            Vector3 childPos = new Vector3(curPos.x  + nodeDistanceX, nodePosY[i], curPos.z);
            childObject.transform.position = childPos;
        }
        AddLine(updatedNode.transform.position);
        currentNode = updatedNode;
    }

    private void AddLine(Vector3 destination)
    {
        Vector3[] allPos = new Vector3[lineRenderer.positionCount + 1];
        lineRenderer.GetPositions(allPos);
        allPos[lineRenderer.positionCount] = destination;
        lineRenderer.positionCount = allPos.Length;
        lineRenderer.SetPositions(allPos);
    }

    private float[] GetNodePositionsY(float yPos, int totalPoints)
    {
        float[] res = new float[totalPoints];
        for (int i = 0; i < totalPoints/2; i++) 
        {
            res[i] = (totalPoints/2 - i) * nodeDistanceY + yPos;
            res[totalPoints-i-1] = yPos - (totalPoints/2 - i) * nodeDistanceY;
        }
        if (totalPoints % 2 != 0) 
        {
            res[totalPoints/2] = yPos;
        }
        
        return res;
    }
}
