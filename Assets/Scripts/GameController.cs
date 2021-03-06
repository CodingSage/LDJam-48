using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(StoryGraph))]
public class GameController : MonoBehaviour
{
    private StoryGraph graph;
    private NodeBehaviour activeNode;
    
    public GameObject graphCamera;
    public SpriteRenderer sceneSpriteRenderer;
    public AudioSource audioSource;
    public GameObject menuPanel;
    public TMP_Text dialogueText;
    
    void Start()
    {
        // Load graph
        graph = GetComponent<StoryGraph>();
        graph.controller = this;
        graph.LoadGraph();

        dialogueText.text = "";

        audioSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            dialogueText.enabled = (!menuPanel.activeSelf);
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
        if (audioSource.clip == null || audioSource.clip.name != node.info.soundTrackName)
        {
            string audioPath = "Sound/" + node.info.soundTrackName;
            AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
            audioSource.clip = audioClip;
            audioSource.Play();
        }

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

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
