using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUi : MonoBehaviour
{


    [Header("variable")]
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

    [Header("raycast info")]
    [SerializeField]
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

    // Start is called before the first frame update
    void Start()
    {
        initialScale = target.transform.localScale;
        initialRotation = target.transform.rotation;
        cam = Camera.main;
    }



    // Update is called once per frame
    void Update()
    {

        if (!anim.enabled)
        {
            anim.enabled = true;
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
        transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, 1f*Time.deltaTime);
    }

    public void BtnRotationUp() {
        float currentYrot = transform.rotation.eulerAngles.y;
        currentYrot += rotationScale;
        transform.rotation = Quaternion.Euler(0f, currentYrot, 0f);
    }
    public void BtnRotationDown() {
        float currentYrot = transform.rotation.eulerAngles.y;
        currentYrot -= rotationScale;
        transform.rotation = Quaternion.Euler(0f, currentYrot, 0f);
    }

    void FiringRayFromScreen() {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            FindDesc(hit.collider.gameObject.name);
            Debug.Log("Ray hit "+hit.collider.gameObject.name);
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
}

[System.Serializable]
struct TxtInfo{
    [Tooltip("raycast gameobject. txtNama")]
    public string alias;
    public string info;
}