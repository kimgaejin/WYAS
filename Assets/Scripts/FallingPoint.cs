using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPoint : MonoBehaviour {

    private Transform fallingPoint;

    private void Start()
    {
        fallingPoint = transform.parent.parent.transform.FindChild("fallingPoint").transform;
        Color fallingPointColor;
        fallingPointColor = fallingPoint.gameObject.GetComponent<SpriteRenderer>().color;
        fallingPointColor.a = 0;
        fallingPoint.gameObject.GetComponent<SpriteRenderer>().color = fallingPointColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("ON");
        if (collision.tag == "PLAYER")
        {
            collision.transform.position = fallingPoint.position;
        }
        else
        {
            if (collision.tag == "BOX"
                || collision.tag == "ROCK")
            Destroy(collision);
        }
    } 

}
