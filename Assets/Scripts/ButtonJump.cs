using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonJump : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        print("button down...");
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.Charging(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("button up...");
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.Charging(false);
        }
    }
}
