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
    Transform initialIsolatedObjData;
    Vector3 globalScaleIsolated;
    Vector3 initialRotationIsolated;
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
        initialIsolatedObjData = o.transform;
        globalScaleIsolated = o.transform.lossyScale;
        initialRotationIsolated = o.transform.eulerAngles;
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

        if (isolatedObj !=null)
        {
            Destroy(isolatedObj);
        }

        if (isAlreadyTracking)
        {
            arObj.SetActive(true);
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
        if (tempBtnIsolate) {
            tempBtnIsolate.SetActive(false);
        }
        Time.timeScale = 0f;
    }

    public void BtnResumeGame() {
        isGamePaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BtnZoomIn()
    {
        if (isolatedObj == null)
        {
            if (target.transform.localScale.x < maxZoomIn)
            {
                Vector3 newScale = target.localScale;
                newScale.x += scaleFactor;
                newScale.y += scaleFactor;
                newScale.z += scaleFactor;
                target.transform.localScale = newScale;
            }
        }
        else {
            Vector3 newScale = isolatedObj.transform.localScale;
            newScale.x += scaleFactor*3;
            newScale.y += scaleFactor*3;
            newScale.z += scaleFactor*3;
            isolatedObj.transform.localScale = newScale;
        }
    }

    public void BtnZoomOut()
    {
        if (isolatedObj == null)
        {
            if (target.transform.localScale.x > initialScale.x)
            {
                Vector3 newScale = target.localScale;
                newScale.x -= scaleFactor;
                newScale.y -= scaleFactor;
                newScale.z -= scaleFactor;
                target.transform.localScale = newScale;
            }
        }
        else
        {
            Vector3 newScale = isolatedObj.transform.localScale;
            newScale.x -= scaleFactor*3;
            newScale.y -= scaleFactor*3;
            newScale.z -= scaleFactor*3;
            isolatedObj.transform.localScale = newScale;
        }
    }

    public void BtnZoomOutMax() {
        if (isolatedObj == null)
        {
            target.transform.localScale = initialScale;
        }
        else {
            isolatedObj.transform.localScale = globalScaleIsolated;
        }
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
        if (isolatedObj == null)
        {
            target.transform.rotation = initialRotation;
        }
        else {
            isolatedObj.transform.rotation = Quaternion.Euler(initialRotationIsolated);
        }
    }

    public void BtnRotationUp() {
        if (isolatedObj == null)
        {
            float currentYrot = target.transform.rotation.eulerAngles.y;
            currentYrot += rotationScale;
            target.transform.rotation = Quaternion.Euler(0f, currentYrot, 0f);
        }
        else {
            float currentYrot = isolatedObj.transform.rotation.eulerAngles.y;
            currentYrot += rotationScale;
            isolatedObj.transform.rotation = Quaternion.Euler(isolatedObj.transform.rotation.eulerAngles.x, currentYrot, isolatedObj.transform.rotation.eulerAngles.z);
        }
    }
    public void BtnRotationDown() {
        if (isolatedObj == null)
        {
            float currentYrot = target.transform.rotation.eulerAngles.y;
            currentYrot -= rotationScale;
            target.transform.rotation = Quaternion.Euler(0f, currentYrot, 0f);
        }
        else {
            float currentYrot = isolatedObj.transform.rotation.eulerAngles.y;
            currentYrot -= rotationScale;
            isolatedObj.transform.rotation = Quaternion.Euler(isolatedObj.transform.rotation.eulerAngles.x, currentYrot, isolatedObj.transform.rotation.eulerAngles.z);
        }
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
                                tempBtnIsolate.GetComponent<BtnIsolateHelper>().SetTargetObj(hit.collider.gameObject.name, arObjTemplate, arObj);
                            }
                            else {
                                tempBtnIsolate.SetActive(true);
                                Vector3 mPos = tempBtnIsolate.transform.position = cam.WorldToScreenPoint(hit.point);
                                mPos.z = 0f;
                                tempBtnIsolate.transform.position = mPos;
                                tempBtnIsolate.transform.localScale = new Vector3(1f, 1f, 1f);
                                tempBtnIsolate.GetComponent<BtnIsolateHelper>().SetTargetObj(hit.collider.gameObject.name, arObjTemplate, arObj);
                            }
                            anim.speed = 0f;
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