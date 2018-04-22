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
        base.rangeX = 0.3f;
        base.mustFaced = false;
        pRigid = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        maxY = transform.position.y + GetSize().y/2 + pState.GetSizeY()/2;
        minY = transform.position.y - GetSize().y / 2;
    }

    override public void DoInteracting()
    {
        pState.makeMove(false);
        pState.makeJump(false);
        pRigid.isKinematic = true;
        interactingState = true;

        pState.makeResetSpeed();
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
        pState.makeMove(true);
        pState.makeJump(true);
        pRigid.isKinematic = false;
        interactingState = false;
        StopCoroutine("MoveToLadder");
        StartCoroutine("CoolTime");
    }

    IEnumerator MoveToLadder()
    {
        WaitForSeconds wait005 = new WaitForSeconds(0.05f);

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

    IEnumerator CoolTime()
    {
        while (true)
        {
            rangeX = 0;
            yield return new WaitForSeconds(1.0f);
            rangeX = 0.3f;
            break;
        }
    }
}
