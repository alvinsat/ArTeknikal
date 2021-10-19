using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class BtnIsolateHelper : MonoBehaviour
{
    string targetGameObject;
    GameObject parentObj;
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

    public void SetTargetObj(string targetName, GameObject parentOb) {
        targetGameObject = targetName;
        parentObj = parentOb;
    }

    public void BtnIsolate() {
        GameObject a = Instantiate(parentObj);
        parentObj.SetActive(false);
        Debug.Log("seek target name "+targetGameObject+ " inside "+a.gameObject.name);
        GameObject b = a.transform.Find(targetGameObject).gameObject;
        Debug.Log("FOUND");
        b = b.GetComponent<ColliderTargetObj>().obj;
        b.transform.SetParent(null);
        sysGame.SetIsolatedObj(b);
        Destroy(a);
        b.transform.position = new Vector3(0,0,0);
        gameObject.SetActive(false);
    }
}
