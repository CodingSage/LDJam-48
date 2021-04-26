using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviour : MonoBehaviour
{
    public NodeInfo info;
    [HideInInspector] public bool visited = false;
    [HideInInspector] public StoryGraph graph;

    private void OnMouseDown()
    {
        if (visited) return;
        graph.UpdateState(gameObject);
    }

    private void OnMouseEnter()
    {
        Tooltip.Show(info.dialogues[0]);
    }

    private void OnMouseExit()
    {
        Tooltip.Hide();
    }
}
