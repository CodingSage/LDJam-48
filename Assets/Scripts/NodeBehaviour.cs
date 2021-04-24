using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBehaviour : MonoBehaviour
{
    public NodeInfo info;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        
    }

    public void makeVisible()
    {
        gameObject.SetActive(true);
    }
}
