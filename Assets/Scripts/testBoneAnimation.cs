using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class testBoneAnimation : MonoBehaviour {

    private UnityArmatureComponent armatureComponent;

    private void Start () {
        armatureComponent = GetComponent<UnityArmatureComponent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            armatureComponent.animation.FadeIn("walk", 0.25f);
           // armatureComponent.animation.Play("walk");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            armatureComponent.animation.FadeIn("stand", 0.25f);
            // armatureComponent.animation.Play("stand");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            armatureComponent.animation.FadeIn("jumping", 0.25f);
         //   armatureComponent.animation.Play("jumping");
        }
    }
}
