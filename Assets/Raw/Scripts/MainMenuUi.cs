using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLevel(string lvName) {
        SceneManager.LoadScene(lvName);
    }

    public void APP_Exit() {
        Application.Quit();
    }
}
