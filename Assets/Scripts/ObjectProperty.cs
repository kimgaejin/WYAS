using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperty : MonoBehaviour {

    private Vector2 size;
    protected float rangeX = 0;

    protected void Awake()
    {
        size = this.GetComponent<SpriteRenderer>().bounds.size;
    }

    public Vector2 GetSize()
    {
        return size;
    }

    public float GetRangeX()
    {
        return rangeX;
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
