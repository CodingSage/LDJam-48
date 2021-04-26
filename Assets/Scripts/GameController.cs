using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(StoryGraph))]
public class GameController : MonoBehaviour
{
    private StoryGraph graph;
    private NodeBehaviour activeNode;
    
    public GameObject graphCamera;
    public SpriteRenderer sceneSpriteRenderer;
    public GameObject menuPanel;
    public TMP_Text dialogueText;
    
    void Start()
    {
        // Load graph
        graph = GetComponent<StoryGraph>();
        graph.controller = this;
        graph.LoadGraph();

        dialogueText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
    }

    public void UpdateScene(GameObject nodeObj)
    {
        NodeBehaviour node = nodeObj.GetComponent<NodeBehaviour>();

        // update scene
        string scenePath = "Images/" + node.info.sceneName;
        Sprite sceneSprite = Resources.Load<Sprite>(scenePath);
        sceneSpriteRenderer.sprite = sceneSprite;

        // update soundtrack

        // update dialogue
        StartCoroutine(DisplayText(node.info.dialogues, 1f));

        // update graph camera
        Vector3 nodePos = nodeObj.transform.position;
        Vector3 cameraPos = graphCamera.transform.position;
        graphCamera.transform.position = new Vector3(nodePos.x, nodePos.y, cameraPos.z);
    }

    IEnumerator DisplayText(string[] dialogues, float delay)
    {
        string allLines = "";
        foreach (string line in dialogues)
        {
            allLines += line + "\n\n";
            dialogueText.text = allLines;
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }
}
