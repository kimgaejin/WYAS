using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperty : MonoBehaviour {
    
    // reference
    protected Transform player;
    protected PlayerState pState;

    protected Vector2 size;
    protected float rangeX = 0;
    protected bool mustFaced = false;
    protected bool interactingState = false;

    protected void Awake()
    {
        interactingState = false;
        // 과거, 자신이 spriteRenderer 가졌을 때 시절
        //size = this.GetComponent<SpriteRenderer>().bounds.size;

        SetSize();
        player = GameObject.Find("Player").transform;
        pState = GameObject.Find("Player").GetComponent<PlayerState>();
    }

    // 범위 안인가
    
    public virtual bool GetIsInRange()
    {

        try
        {
            Vector3 distance = player.position - transform.position;

            if (mustFaced == true)
            {
                if ((distance.x <= 0 && pState.GetIsFacedR() == false)
                    || (distance.x >= 0 && pState.GetIsFacedR() == true))
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
        catch
        {
            return false;
            Debug.Log("GetIsInRange throw!!!!!!");
        }
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

    public virtual void SetSize()
    {
        try
        {
            size = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().bounds.size;
        }
        catch
        {
            Debug.Log(transform.name.ToString() + "의 자식의 SpriteRenderer를 ObjectProperty에서 찾지 못함.");
            size = Vector2.zero;
        }

    }
}
