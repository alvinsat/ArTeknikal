using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadingHelper : MonoBehaviour
{
    string sceneName;
    [SerializeField]
    [Tooltip("sliced img type radial 360")]
    Image imgLoading;
    [SerializeField]
    GameObject btnReadyScene;
    AsyncOperation asyncSync;
    bool isLoading;

    // Start is called before the first frame update
    void Start()
    {
        btnReadyScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading)
        {
            StartCoroutine(LoadScene());
            isLoading = false;
        }
    }

    public void BtnSetSceneName(string alias) {
        sceneName = alias;
    }
    
    
    public void BtnStartLoadingScene() {
        isLoading = true;
    }


    public void BtnReadyGotoScene() {
        asyncSync.allowSceneActivation = true;
    }

    IEnumerator LoadScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone) {
            imgLoading.fillAmount = asyncLoad.progress;
            if (asyncLoad.progress >= 0.9f)
            {
                asyncSync = asyncLoad;
                btnReadyScene.SetActive(true);
                imgLoading.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}
