using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUi : MonoBehaviour
{
    [Header("variable")]
    [SerializeField]
    float timeWaitRay;
    float timeWaitRayTemp;
    [Header("Target scale")]
    Vector3 initialScale;
    Quaternion initialRotation;
    [SerializeField]
    float scaleFactor;
    [SerializeField]
    float maxZoomIn;
    [SerializeField]
    float rotationScale;
    bool isGamePaused;
    bool isFound = false;
    [SerializeField]
    float btnIsolateSize;
    [SerializeField]
    GameObject btnIsolate;
    [SerializeField]
    Transform uiCanvas;
    GameObject tempBtnIsolate;
    bool isAlreadyTracking;

    [Header("raycast info")]
    [SerializeField]
    [Tooltip("raycast gameobject. txtNama = gameObject.name.collider")]
    TxtInfo[] infos;

    [Header("Komponen")]
    [SerializeField]
    Transform target;
    [SerializeField]
    Animator anim;
    RaycastHit hit;
    Ray ray;
    Camera cam;
    [SerializeField]
    GameObject panelKetView;
    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    TMPro.TextMeshProUGUI txtDesc;
    [SerializeField]
    GameObject arObjTemplate;
    [SerializeField]
    GameObject arObj;
    GameObject isolatedObj;
    readonly string btnIsolateName = "btnIsolate";
    
    // Start is called before the first frame update
    void Start()
    {
        timeWaitRayTemp = timeWaitRay;
        initialScale = target.transform.localScale;
        initialRotation = target.transform.rotation;
        cam = Camera.main;
    }

    public void SetIsolatedObj(GameObject o) {
        isolatedObj = o;
    }

    public void WhenTracking() {
        if (!isAlreadyTracking)
        {
            // TODO resume last
            if (isolatedObj)
            {
                isolatedObj.SetActive(true);
            }
            else {
                arObj.SetActive(true);
            }
            isAlreadyTracking = true;
        }
    }

    public void LostTracking() {
        if (isolatedObj)
        {
            isolatedObj.SetActive(false);
            GameObject t;
            t = GameObject.Find(btnIsolateName);
            if (t) {
                t.SetActive(false);
            }
        }
        else {
            arObj.SetActive(false);
        }
        isAlreadyTracking = false;
    }

    public void BtnDissolveIsolate() {
        GameObject t;
        t = GameObject.Find(btnIsolateName);
        if (t) {
            t.SetActive(false);
        }

        if (isolatedObj)
        {
            Destroy(isolatedObj);
        }
        else {
            if (isAlreadyTracking)
            {
                arObj.SetActive(true);
            }
        }
        anim.speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {

        if (!anim.enabled)
        {
            anim.enabled = true;
            anim.speed = 1f;
        }
        if (!isGamePaused)
        {
            FiringRayFromScreen();
        }
        if (!isGamePaused) {
            if (Input.GetButtonUp("Cancel")) {
                pausePanel.SetActive(true);
                isGamePaused = true;
            }
        }
    }


    public void BtnPauseGame()
    {
        pausePanel.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;
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

    public void BtnResumeGame() {
        isGamePaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BtnZoomOutMax() {
        // Force zooming out
        // TODO anim zooming out
        target.transform.localScale = initialScale;
    }

    public void BtnToggleAnim() {
        if (anim.enabled)
        {
            anim.enabled = false;
        }
    }

    public void BtnToggleAnimSpeed() {
        if (anim.speed == 1f)
        {
            anim.speed = 0f;
        }
        else {
            anim.speed = 1f;
        }
    }

    public void BtnRestartRotation() {
        target.transform.rotation = initialRotation;
    }

    public void BtnRotationUp() {
        float currentYrot = target.transform.rotation.eulerAngles.y;
        currentYrot += rotationScale;
        target.transform.rotation = Quaternion.Euler(0f, currentYrot, 0f);
    }
    public void BtnRotationDown() {
        float currentYrot = target.transform.rotation.eulerAngles.y;
        currentYrot -= rotationScale;
        target.transform.rotation = Quaternion.Euler(0f, currentYrot, 0f);
    }

    void FiringRayFromScreen() {
        //if(Event)
        if (!isFound)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    FindDesc(hit.collider.gameObject.name, out isFound);
                }

                if (!isFound) {
                    if (tempBtnIsolate != null) {
                        tempBtnIsolate.SetActive(false);
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                if (timeWaitRay > 0)
                {
                    timeWaitRay -= 1f * Time.deltaTime * Time.timeScale;
                    if (timeWaitRay < 0f)
                    {
                        Handheld.Vibrate();
                        ray = cam.ScreenPointToRay(Input.mousePosition);
                        
                        if (Physics.Raycast(ray, out hit))
                        {
                            FindDesc(hit.collider.gameObject.name);
                            //Debug.Log("Ray hit " + hit.collider.gameObject.name);
                            if (tempBtnIsolate == null)
                            {
                                tempBtnIsolate = Instantiate(btnIsolate);
                                tempBtnIsolate.name = btnIsolateName;
                                tempBtnIsolate.transform.SetParent(uiCanvas);
                                Vector3 mPos = tempBtnIsolate.transform.position = cam.WorldToScreenPoint(hit.point);
                                mPos.z = 0f;
                                tempBtnIsolate.transform.position = mPos;
                                tempBtnIsolate.transform.localScale = new Vector3(1f,1f,1f);
                                tempBtnIsolate.GetComponent<BtnIsolateHelper>().SetTargetObj(hit.collider.gameObject.name, arObjTemplate);
                            }
                            else {
                                tempBtnIsolate.SetActive(true);
                                Vector3 mPos = tempBtnIsolate.transform.position = cam.WorldToScreenPoint(hit.point);
                                mPos.z = 0f;
                                tempBtnIsolate.transform.position = mPos;
                                tempBtnIsolate.transform.localScale = new Vector3(1f, 1f, 1f);
                                tempBtnIsolate.GetComponent<BtnIsolateHelper>().SetTargetObj(hit.collider.gameObject.name, arObjTemplate);
                            }
                            anim.speed = 0f;
                            //TODO set match rotation
                            //set active only single
                            //move to center pose
                            //TODO edge aware
                           
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {
            timeWaitRay = timeWaitRayTemp;
            isFound = false;
        }
    }

    void FindDesc(string alias) {
        int i = 0;
        while (i < infos.Length) {
            if (infos[i].alias == alias) {
                if (!panelKetView.activeInHierarchy) {
                    panelKetView.SetActive(true);
                }
                txtDesc.SetText(infos[i].info);
            }
            i++;
        }
    }

    public void ForceResumeAnim() {
        anim.speed = 1f;
    }

    void FindDesc(string alias, out bool isFound)
    {
        int i = 0;
        while (i < infos.Length)
        {
            if (infos[i].alias == alias)
            {
                isFound = true;
                Debug.Log(alias + " is found");

                return;
            }
            i++;
        }
        Debug.Log(alias+" is not found");
        isFound = false;
    }
}

[System.Serializable]
struct TxtInfo{

    public string alias;
    public string info;
}