using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockWorldPos : MonoBehaviour
{
    Vector3 myWorldPos;
    // Start is called before the first frame update
    void Start()
    {
        myWorldPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = myWorldPos;
    }
}
