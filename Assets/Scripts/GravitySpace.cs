using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySpace : MonoBehaviour {

    float gravityPower = 9.8f;
    Vector3 arrow;
    Transform player;
    PlayerState pState;
    Vector2 point1;
    Vector2 point2;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        pState = player.GetComponent<PlayerState>();
        arrow = Vector3.down;

        SpriteRenderer spr = GetComponent<SpriteRenderer>();

        point1 = new Vector2(transform.position.x - spr.bounds.size.x / 2, transform.position.y + spr.bounds.size.y / 2);
        point1 = new Vector2(transform.position.x + spr.bounds.size.x / 2, transform.position.y - spr.bounds.size.y / 2);

        SetArrow();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        Rigidbody2D rigid = null;
        rigid = col.GetComponent<Rigidbody2D>();
        if (rigid != null)
        {
            float weightGravity = rigid.mass;
            //Debug.Log("wei" + weightGravity); 
            //rigid.AddForce(arrow * gravityPower * weightGravity * 2, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        float colGrav = col.GetComponent<Rigidbody2D>().gravityScale;
        if (col.tag == "PLAYER" && arrow == Vector3.up)
        {
            pState.MakeIsReversed(true);
        }
        Debug.Log("enter!" + col.transform.position + "grav:"+colGrav);
        if (colGrav >= 0) col.GetComponent<Rigidbody2D>().gravityScale *= -1;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        float colGrav = col.GetComponent<Rigidbody2D>().gravityScale;
        if (col.tag == "PLAYER" && arrow == Vector3.up)
        {
            pState.MakeIsReversed(false);
        }

        Debug.Log("exit" + col.transform.position);
        if (colGrav <= 0) col.GetComponent<Rigidbody2D>().gravityScale *= -1;
    }

    private void SetArrow()
    {
        Vector3 quat;
        quat = GetComponent<Transform>().rotation.eulerAngles;
        float zValue = quat.z % 360;

        Debug.Log(quat);
        // Quaternion (0,0,0,1) , 360 -> (0,0,0,-1)
        if (zValue <= 45.0f || zValue >= 315.0f)
        {
            arrow = Vector3.down;
        }
        // Quaternion (0,0,0.7,0.7), 450 -> (0,0,-0.7,-0.7)
        else if (zValue <= 135.0f)
        {
            arrow = Vector3.left;
        }
        // Quaternion (0,0,1,0)
        else if (zValue <= 225.0f)
        {
            arrow = Vector3.up;
        }
        // Quaternion (0,0,0.7,-0.7)
        else
        {
            arrow = Vector3.right;
        }
    }
}
