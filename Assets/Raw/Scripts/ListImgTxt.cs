using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ListImgTxt : MonoBehaviour
{
    [System.Serializable]
    struct TooltipsCollection {
        public Sprite img;
        public string tipInfo;
    };
    [SerializeField]
    TooltipsCollection[] toolTipLoading;
    int indexNow;
    [SerializeField]
    UnityEngine.UI.Image imgTarget;
    [SerializeField]
    TMPro.TextMeshProUGUI txtTarget;
    // Start is called before the first frame update
    void Start()
    {
        imgTarget.sprite = toolTipLoading[indexNow].img;
        txtTarget.SetText(toolTipLoading[indexNow].tipInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnNext() {
        imgTarget.sprite = toolTipLoading[indexNow].img;
        txtTarget.SetText(toolTipLoading[indexNow].tipInfo);
        if (indexNow < toolTipLoading.Length - 1)
        {
            indexNow++;
        }
        else {
            indexNow = 0;
        }
    }
}
