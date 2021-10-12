using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTarget : MonoBehaviour
{
    LineRenderer renderer;
    Transform[] childrens;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<LineRenderer>();
        renderer.positionCount = childrens.Length;
        int i = 0;
        while (i < childrens.Length) {
            
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
