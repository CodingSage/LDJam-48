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

    public void UpdateScene(NodeInfo info)
    {
        // update scene

        // update soundtrack
    }
}
