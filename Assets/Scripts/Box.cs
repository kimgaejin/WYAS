using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : ObjectProperty {

    // reference
    private Transform player;
    private PlayerState pState;

    // setting
    private float horizonSpeedSave;
    private float decelration;

    private void Awake()
    {
        base.Awake();
        base.rangeX = 1.1f;
    }

    public void DoInteracting()
    {
        pState.makeJump(false);
        pState.makeMove(true);

        decelration = 0.5f;
        horizonSpeedSave = pState.GetHorizonSpeed();
    }

    public void IsInteracting()
    {
        if (pState.GetIsJumping() == true)
        {
            StopInteracting();
        }
        else
        {
            pState.makeHorizonspeed(horizonSpeedSave * decelration);
        }
    }

    public void StopInteracting()
    {
        pState.makeJump(true);
        pState.makeMove(true);
        pState.makeHorizonspeed(horizonSpeedSave);
    }
}
