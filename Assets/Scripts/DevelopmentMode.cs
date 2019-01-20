using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopmentMode : MonoBehaviour {

    private Transform pTrans;
    private List<Vector3> endLocation = new List<Vector3>();

    private int skipCount = 0;
    private float timeScale = 1.0f;

    private void Start()
    {
        pTrans = GameObject.Find("Player").GetComponent<Transform>();

        Transform endPoint;
        endPoint = GameObject.Find("Map").transform.FindChild("Objects").FindChild("ClearFlag").FindChild("EndPoint");

        foreach (Transform endTrans in endPoint) {
            endLocation.Add(endTrans.position);
        }
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.Plus))
            IncTimeScale();
        if (Input.GetKeyDown(KeyCode.Minus))
            DecTimeScale();
        if (Input.GetKeyDown(KeyCode.Keypad9))
            SkipStage();
    }

    private void IncTimeScale()
    {
        Time.timeScale += .1f;
    }

    private void DecTimeScale()
    {
        if (Time.timeScale == 0.0f) return;

        Time.timeScale -= .1f;
    }

    private void SkipStage()
    {
        Vector3 location;

        if (skipCount > endLocation.Count)
        {
            return;
        }

        location = endLocation[skipCount];
        skipCount++;
        pTrans.position = location;
        
    }
}
