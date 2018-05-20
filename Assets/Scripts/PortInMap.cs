using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortInMap : ObjectProperty
{
    private Transform linkLocation;

    private void Start()
    {
        base.rangeX = 0.3f;
        base.mustFaced = false;

        FindLinkedPortal();
    }

    override public void DoInteracting()
    {
        pState.transform.position = linkLocation.position;
        StopInteracting();
    }

    override public void IsInteracting()
    {

    }

    override public void StopInteracting()
    {

    }

    void FindLinkedPortal()
    {
        linkLocation = transform.parent.transform.GetChild(0);
        if (linkLocation == this.transform) linkLocation = transform.parent.transform.GetChild(1);
    }
}
