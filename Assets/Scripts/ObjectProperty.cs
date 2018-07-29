using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperty : MonoBehaviour {
    
    // reference
    protected Transform player;
    protected PlayerState pState;

    private Vector2 size;
    protected float rangeX = 0;
    protected bool mustFaced = false;
    protected bool interactingState = false;

    protected void Awake()
    {
        interactingState = false;
        size = this.GetComponent<SpriteRenderer>().bounds.size;
        player = GameObject.Find("Player").transform;
        pState = GameObject.Find("Player").GetComponent<PlayerState>();
    }

    // 범위 안인가
    
    public virtual bool GetIsInRange()
    {
        Vector3 distance = player.position - transform.position;

        if (mustFaced == true)
        {
            if (   (distance.x <= 0 && pState.GetIsFacedR() == false)
                || (distance.x >= 0 && pState.GetIsFacedR() == true)  )
            {
                return false;
            }
        }
        if (Mathf.Abs(distance.y) < pState.GetSizeY() / 2 + GetSize().y / 2)
        {
            if (Mathf.Abs(distance.x) <= GetRangeX())
            {
                return true;
            }
        }
        return false;
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
