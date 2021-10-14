using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cableSmall : MonoBehaviour
{
    LineRenderer renderiy;
    [SerializeField]
    Transform[] posLine;
    // Start is called before the first frame update
    void Start()
    {
        renderiy = GetComponent<LineRenderer>();
        renderiy.positionCount = posLine.Length;
    }

    // Update is called once per frame
    void Update()
    {
        renderiy.SetPosition(0, posLine[0].position);
        renderiy.SetPosition(1, posLine[1].position);
    }
}
