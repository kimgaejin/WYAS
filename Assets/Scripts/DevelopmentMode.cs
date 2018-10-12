using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopmentMode : MonoBehaviour {

    private float timeScale = 1.0f;

	private void Update () {
        if (Input.GetKeyDown(KeyCode.I))
            IncTimeScale();
        if (Input.GetKeyDown(KeyCode.O))
            DecTimeScale();
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
}
