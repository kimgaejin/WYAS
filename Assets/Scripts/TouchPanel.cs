using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler
{
    KeyControl KeyCont;

    private void Start()
    {
        KeyCont = GameObject.Find("Managers").GetComponent<KeyControl>();
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        KeyCont.OnTouchInteractionPanel();
    }
}
