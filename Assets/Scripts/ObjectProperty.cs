using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperty : MonoBehaviour {
    
    // reference
    protected Transform player;
    protected PlayerState pState;

    protected Vector2 size;
    protected float rangeX = 0;
    protected bool interactingState = false;

    protected void Awake()
    {
        interactingState = false;
        size = this.GetComponent<SpriteRenderer>().bounds.size;
        player = GameObject.Find("Player").transform;
        pState = GameObject.Find("Player").GetComponent<PlayerState>();
    }

    public Vector2 GetSize()
    {
        return size;
    }

    public float GetRangeX()
    {
        return rangeX;
    }

    public bool GetInterctingState()
    {
        return interactingState;
    }

    public virtual void DoInteracting()
    {

    }

    public virtual void IsInteracting()
    {

    }

    public virtual void StopInteracting()
    {

    }

}
