using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInMap : ObjectProperty
{
    private Transform location;

    private void Start()
    {
        base.rangeX = 0.3f;
        base.mustFaced = false;

        location = transform.GetChild(0);
    }

    override public void DoInteracting()
    {

    }

    override public void IsInteracting()
    {
        
    }

    override public void StopInteracting()
    {
      
    }

}
