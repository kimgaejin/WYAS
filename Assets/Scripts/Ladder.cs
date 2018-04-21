using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : ObjectProperty {

    private Rigidbody2D pRigid;

    private float ladderSpeed = 1.0f;
    private float maxY;
    private float minY;

    private void Start()
    {
        base.rangeX = 1.1f;
        pRigid = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        maxY = transform.position.y + GetSize().y/2 + pState.GetSizeY()/2;
        minY = transform.position.y - GetSize().y / 2;
        Debug.Log("max" + maxY + "minY" + minY);
    }

    override public void DoInteracting()
    {
        pState.makeMove(false);
        pState.makeJump(false);
        pRigid.isKinematic = true;
        interactingState = true;
        Debug.Log("사다리 실행");
    }

    override public void IsInteracting()
    {
        if (player.position.y > maxY || player.position.y < minY)
        {
            StopInteracting();
        }

        if (Input.GetKey(KeyCode.W))
            pRigid.transform.Translate(Vector2.up * ladderSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.S))
            pRigid.transform.Translate(Vector2.down * ladderSpeed * Time.deltaTime);
    }

    override public void StopInteracting()
    {
        pState.makeMove(true);
        pState.makeJump(true);
        pRigid.isKinematic = false;
        interactingState = false;
        Debug.Log("사다리 거부");
    }
}
