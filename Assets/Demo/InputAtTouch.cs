using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAtTouch : MonoBehaviour
{
    // time touch hold
    [SerializeField]
    float timeTap;
    float timeTapTemp;

    //Teks to show.. this includes a parent as gameobjct
    [SerializeField]
    TMPro.TextMeshProUGUI txtTeks;

    GameObject txtObjParent;

    Ray ray;// raycast
    RaycastHit hit;// raycasthit
    Camera cam;// main camera 

    bool isFound;// sets true if ray detects an object, don't forget to sets this to false on text exit maybe..

    // Start is called before the first frame update
    void Start()
    {
        // hardcoded gameobject structure
        // parent->txt
        txtObjParent = txtTeks.transform.parent.gameObject;
        timeTapTemp = timeTap;
        cam = Camera.main;
    }

    public void BtnSetFocus(bool isFound) {
        this.isFound = isFound;
    }

    // Update is called once per frame
    void Update()
    {
        FiringRayFromScreen();
    }

    /// <summary>
    /// This is the ray input.. or so to say. tap hold
    /// </summary>
    void FiringRayFromScreen()
    {
        if (!isFound)
        {
            // this is call when we never tap on anything
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    // This is when hit something.. in this case.. anything
                    isFound = true;
                }

            }
        }
        else
        {
            // this is when touch at something and keep hold it
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                if (timeTap > 0)
                {
                    timeTap -= 1f * Time.deltaTime * Time.timeScale;
                    if (timeTap < 0f)
                    {
                        Handheld.Vibrate();
                        ray = cam.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray, out hit))
                        {
                            FindDesc("Make new text");
                        }
                    }
                }
            }

            // this when you attempt to tap at somewhere else.. reset the tap hold counter
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                timeTap = timeTapTemp;
                isFound = false;
            }
        }
    }

    /// <summary>
    /// Draws Text and enable text obj
    /// </summary>
    /// <param name="objTxt"> this is the text that will be shown </param>
    void FindDesc(string objTxt) {
        // This should be part when drawing a text
        // so, let's do so
        txtTeks.SetText(objTxt);
        txtObjParent.SetActive(true);
    }
}
