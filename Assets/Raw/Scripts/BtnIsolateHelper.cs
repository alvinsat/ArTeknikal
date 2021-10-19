using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class BtnIsolateHelper : MonoBehaviour
{
    string targetGameObject;
    GameObject parentObj;
    GameObject mmain;
    GameUi sysGame;
    // Start is called before the first frame update
    void Start()
    {
        sysGame = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameUi>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetObj(string targetName, GameObject parentOb, GameObject main) {
        targetGameObject = targetName;
        parentObj = parentOb;
        mmain = main;
    }

    public void BtnIsolate() {
        // Getting another instance with modified structure ARObject
        GameObject a = Instantiate(parentObj);
        parentObj.SetActive(false);
        a.transform.position = mmain.transform.position;
        a.transform.rotation = mmain.transform.rotation;
        a.transform.localScale = mmain.transform.localScale;
        // Trying to find the isolate ARObj and isolate it
        GameObject b = a.transform.Find(targetGameObject).gameObject;
        b = b.GetComponent<ColliderTargetObj>().obj;
        b.transform.SetParent(null);
        sysGame.SetIsolatedObj(b);
        // Sets the position center of ARWorld
        b.transform.position = new Vector3(0, 0, 0);

        // clearing the instance
        Destroy(a);
        // Hiding the main ARObj
        mmain.SetActive(false);
        // Turning this btn off
        gameObject.SetActive(false);
    }
}
