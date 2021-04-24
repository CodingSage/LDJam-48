using UnityEngine;
using UnityEditor;

public class StoryGraph
{
    private NodeBehaviour activeNode;

    public TextAsset nodeInfosText;
    public TextAsset nodeConnectionsText;



    public void LoadGraph()
    {
        NodeInfos nodeInfos = JsonUtility.FromJson<NodeInfos>(nodeInfosText.text);
        /*for (NodeInfo node in nodeInfos.nodes)
        {

        }*/

    }
}
