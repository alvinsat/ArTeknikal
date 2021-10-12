using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUi : MonoBehaviour
{
    [SerializeField]
    Transform target;
    
    [Header("Target scale")]
    Vector3 initialScale;
    [SerializeField]
    float scaleFactor;
    [SerializeField]
    float maxZoomIn;

    // Start is called before the first frame update
    void Start()
    {
        initialScale = target.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnZoomIn() {
        if (target.transform.localScale.x < maxZoomIn) {
            Vector3 newScale = target.localScale;
            newScale.x += scaleFactor;
            newScale.y += scaleFactor;
            newScale.z += scaleFactor;
            target.transform.localScale = newScale;
        }
    }

    public void BtnZoomOut() {
        if (target.transform.localScale.x > initialScale.x) {
            Vector3 newScale = target.localScale;
            newScale.x -= scaleFactor;
            newScale.y -= scaleFactor;
            newScale.z -= scaleFactor;
            target.transform.localScale = newScale;
        }
    }

    public void BtnZoomOutMax() {
        // Force zooming out
        // TODO anim zooming out
        target.transform.localScale = initialScale;
    }

}
