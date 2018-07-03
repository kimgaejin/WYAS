using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPoint : MonoBehaviour {

    public float height = 10;
    public float bottom = -100;
    public Transform rangeBox;
    private SpriteRenderer rangeSpr;
    private float rangeSize;

    private Transform player;

	void Start () {
        player = GameObject.Find("Player").transform;

        rangeSpr = rangeBox.GetComponent<SpriteRenderer>();
        Color color = rangeSpr.color;
        color.a = 0;
        rangeSpr.color = color;

        rangeSize = rangeSpr.bounds.size.x/2;
	}

    void Update()
    {

        if ((rangeBox.position.x - rangeSize <= player.position.x
            && player.position.x < rangeBox.position.x + rangeSize
            && player.position.y < rangeBox.position.y
            )
            || player.position.y < bottom)
        {
            player.position = transform.position + new Vector3(0, height, 0);
        }
    }

}
