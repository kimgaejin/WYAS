using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : ObjectProperty {

    private Rigidbody2D pRigid;

    private float ladderSpeed = 1.0f;
    private float maxY;
    private float minY;
    private bool canUse;

    WaitForSeconds wait005 = new WaitForSeconds(0.05f);

    private void Start()
    {
        base.rangeX = 0.3f;
        base.mustFaced = true;
        pRigid = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        canUse = true;
        maxY = transform.position.y + GetSize().y/2 + pState.GetSizeY()/2;
        minY = transform.position.y - GetSize().y / 2;
    }

    override public void DoInteracting()
    {
        if (canUse == false) return;

        canUse = false;
        pState.makeMove(false);
        pState.makeJump(false);
        pRigid.isKinematic = true;
        interactingState = true;

        pState.makeResetSpeed();
        pState.makeIsJumping(false);
        StartCoroutine("MoveToLadder");
    }

    override public void IsInteracting()
    {
        
        if (player.position.y > maxY || player.position.y < minY)
        {
            StopInteracting();
        }
        if (Input.GetKeyDown(KeyCode.J))
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
        canUse = true;
        pState.makeMove(true);
        pState.makeJump(true);
        pRigid.isKinematic = false;
        interactingState = false;
        
        StopCoroutine("MoveToLadder");
        player.transform.Translate(new Vector3(transform.position.x - player.transform.position.x,0,0));
        // StartCoroutine("CoolTime");
    }

    IEnumerator MoveToLadder()
    {
        Vector3 dis = transform.position - player.transform.position;
        Vector3 speed = new Vector3(dis.x, 0, 0) * (1.0f/20);
        while (true)
        {
            for (int i = 0; i < 20; i++)
            {
                player.transform.Translate(speed);
                yield return wait005;
            }
            break;
        }
    }
    
}
