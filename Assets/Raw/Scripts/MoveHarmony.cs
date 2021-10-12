using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHarmony : MonoBehaviour
{

    /*
     * offset children position harmonically based on offset and update it realtime 
     * keeping this.position relative to childrenToHarmony 
     * */
    [SerializeField]
    Transform childrenToHarmony; //target where this

    [SerializeField]
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = childrenToHarmony.transform.position + offset;
    }

}
