using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{
    public void OpenLevel(string lvName) {
        SceneManager.LoadScene(lvName);
    }

    public void APP_Exit() {
        Application.Quit();
    }

    public void APP_LoadUrl(string surl) {
        Application.OpenURL(surl);
    }
}
