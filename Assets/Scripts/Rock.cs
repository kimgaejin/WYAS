﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : ObjectProperty
{
    private Rigidbody2D rigid;

    private Vector3 distant;
    private Vector3 heightDifference;

    // 700거리 기준 ㄷㄷ;
    private float throwingForce = 16.0f;

    private int bounceCount = 0;

    private void Start()
    {
        base.rangeX = 1.1f;
        base.mustFaced = false;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "PLAYER" ) {

            bounceCount++;
            if (bounceCount > 3)
            {
                bounceCount = 0;
                rigid.velocity = Vector2.zero;
            }
        }
       
    }

    override public void DoInteracting()
    {
        heightDifference = new Vector3(0, pState.GetSizeY() / 2.0f + GetSize().y / 2.0f, 0);

        //if (pState.GetIsReversed() == false) transform.position = player.position + heightDifference;
        // else transform.position = player.position - heightDifference;
        transform.position = player.position + heightDifference;

        rigid.isKinematic = true;
        bounceCount = 0;

        interactingState = true;
    }

    override public void IsInteracting()
    {
        transform.position = player.position + heightDifference;
        //if (pState.GetIsReversed() == false) transform.position = player.position + heightDifference;
        //else transform.position = player.position - heightDifference;
    }

    override public void StopInteracting()
    {
        rigid.isKinematic = false;
        Vector3 arrow;
        if (pState.GetIsFacedR() == true) arrow = new Vector3(1, 0.25f, 0);
        else arrow = arrow = new Vector3(-1, 0.25f, 0);


        rigid.AddForce(throwingForce * arrow, ForceMode2D.Impulse);
    }
}
