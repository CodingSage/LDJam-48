using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StoryGraph))]
public class GameController : MonoBehaviour
{
    private StoryGraph graph;
    private NodeBehaviour activeNode;
    
    public GameObject graphCamera;
    public GameObject sceneImage;
    
    void Start()
    {
        // Load graph
        graph = GetComponent<StoryGraph>();
        graph.controller = this;
        graph.LoadGraph();
    }

    public void UpdateScene(GameObject nodeObj)
    {
        // update scene

        // update soundtrack

        // update graph camera
        //Vector cameraPos = graphCamera.transform.position;
        Vector3 nodePos = nodeObj.transform.position;
        Vector3 cameraPos = graphCamera.transform.position;
        graphCamera.transform.position = new Vector3(nodePos.x, nodePos.y, cameraPos.z);
    }
}
