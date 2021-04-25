using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviour : MonoBehaviour
{
    public NodeInfo info;
    [HideInInspector] public bool visited = false;
    [HideInInspector] public StoryGraph graph;

    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        graph.UpdateState(this);
    }

    public void MarkActive()
    {
        gameObject.SetActive(true);
    }
}
