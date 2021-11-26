using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnToggleObj : MonoBehaviour
{
    [SerializeField]
    GameObject[] objs;
    int iX;

    void Start()
    {
        objs[1].SetActive(false);
    }
    public void BtnToggle() {
        objs[iX].SetActive(false);
        if (iX < objs.Length-1)
        {
            iX++;
        }
        else {
            iX = 0;
        }
        objs[iX].SetActive(true);
    }
}
