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
    GameObject objTemplateTxt;
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
        InitScrollBarText();
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
            // if the button is not visible or never created in a few frame
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(1))
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    // okay am I find the correct data ?
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
                                tempBtnIsolate.transform.localScale = new Vector3(1f, 1f, 1f);
                                BtnIsolateHelper btn = tempBtnIsolate.GetComponent<BtnIsolateHelper>();
                                btn.SetTargetObj(hit.collider.gameObject.name, arObjTemplate, arObj);
                                btn.SetTrackedPos(hit.point);
                            }
                            else
                            {
                                tempBtnIsolate.SetActive(true);
                                Vector3 mPos = tempBtnIsolate.transform.position = cam.WorldToScreenPoint(hit.point);
                                mPos.z = 0f;
                                tempBtnIsolate.transform.position = mPos;
                                tempBtnIsolate.transform.localScale = new Vector3(1f, 1f, 1f);
                                BtnIsolateHelper btn = tempBtnIsolate.GetComponent<BtnIsolateHelper>();
                                btn.SetTargetObj(hit.collider.gameObject.name, arObjTemplate, arObj);
                                btn.SetTrackedPos(hit.point);
                            }
                            anim.speed = 0f;
                            //TODO edge aware
                        }
                        else {
                            if (tempBtnIsolate) {
                                tempBtnIsolate.SetActive(false);
                            }
                            anim.speed = 1f;
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

    /// <summary>
    /// Find if description exists then draw it if it is
    /// </summary>
    /// <param name="alias"> gameObject name = List<Desc> </param>
    void FindDesc(string alias) {
        int i = 0;
        while (i < infos.Length) {
            if (infos[i].alias == alias) {
                if (!panelKetView.activeInHierarchy) {
                    panelKetView.SetActive(true);
                }
                //txtDesc.SetText(infos[i].info);
                AddTextToScrollBar(infos[i].info);
            }
            i++;
        }
    }

    void AddTextToScrollBar(string slx) {
        // Prepare the Obj
        int mxq = txtDescList.Count;// this var is reuable that acts as a counter
        int i = 0;
        if (mxq > 0) {
            i = 0;
            while (i < mxq) {
                Destroy(txtDescList[i]);// clearing data before this
                i++;
            }
            txtDescList.Clear();
        }
        // split the string into parts where is <= maxCharInScrollBar
        GameObject o;
        // determine how many loop
        i = 0;
        int calc = slx.Length;
        mxq = 0;
        while (calc > 0) {
            calc -= maxCharInScrollBar;
            mxq++;
        }
        //
        i = 0;
        //int txtLeftLength = slx.Length;// decrease this each loop step based on slx.length
        int nowAt = 0;// increse this every loop step as a resume starting position of a text
        while (i < mxq) {
            o = Instantiate(objTemplateTxt, objTemplateTxt.transform.parent);
            if (slx.Length > maxCharInScrollBar)
            {
                if (i != mxq - 1)
                {
                    o.GetComponent<TMPro.TextMeshProUGUI>().SetText(slx.Substring(nowAt, maxCharInScrollBar));
                    txtDescList.Add(o);
                    o.SetActive(true);
                    nowAt += maxCharInScrollBar;

                }
                else if (i == mxq - 1)
                {
                    o.GetComponent<TMPro.TextMeshProUGUI>().SetText(slx.Substring(nowAt, slx.Length % maxCharInScrollBar));
                    txtDescList.Add(o);
                    o.SetActive(true);
                    nowAt += maxCharInScrollBar;

                }
            }
            else {
                o.GetComponent<TMPro.TextMeshProUGUI>().SetText(slx);
                txtDescList.Add(o);
                o.SetActive(true);
                //nowAt += maxCharInScrollBar;
            }

            i++;
        }
    }

    int maxCharInScrollBar;
    List<GameObject> txtDescList;
    /// <summary>
    /// Call to init the scroll bar text
    /// </summary>
    void InitScrollBarText() {
        // prepare the list that can be use as text info placeholder
        txtDescList = new List<GameObject>();
        // Find maximum length that the design accept
        maxCharInScrollBar = objTemplateTxt.GetComponent<TMPro.TextMeshProUGUI>().text.Length;
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