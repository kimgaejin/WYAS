using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyControl : MonoBehaviour
{
    PlayerState player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerState>();
    }

    public void OnTouchJumpPanel()
    {
        player.Jump();
    }

    public void OnTouchInteractionPanel()
    {
        player.Interact();
    }
}
