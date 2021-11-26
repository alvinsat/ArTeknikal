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
    bool isLoadingWithAnim;
    GameObject animLoading;

    // Start is called before the first frame update
    void Start()
    {
        if (btnReadyScene) { 
            btnReadyScene.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading)
        {
            StartCoroutine(LoadScene());
            isLoading = false;
        }

        if (isLoadingWithAnim) {
            animLoading.SetActive(true);
            StartCoroutine(LoadScene(true));
            isLoadingWithAnim = false;
        }
    }

    public void BtnSetSceneName(string alias) {
        sceneName = alias;
    }
    
    public void BtnStartLoadingScene() {
        isLoading = true;
    }

    public void BtnStarLoadingWithAnim() {
        isLoadingWithAnim = true;
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

    public void BtnSetLoadingAnimObj(GameObject o) {
        animLoading = o;
    }

    IEnumerator LoadScene(bool isHasAnimLoading) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) {
            //imgLoading.fillAmount = asyncLoad.progress;
            if (asyncLoad.progress <= 0.9f)
            {
                //asyncSync = asyncLoad;
                //btnReadyScene.SetActive(true);
                //imgLoading.gameObject.SetActive(false);
                if (!animLoading.activeInHierarchy) {
                    animLoading.SetActive(true);
                }
            }
            yield return null;
        }
    }
}
