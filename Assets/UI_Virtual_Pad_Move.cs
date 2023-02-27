using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Virtual_Pad_Move : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private StarterAssetsInputs _input;

    public void OnPointerDown(PointerEventData eventData)
    {
        _input.move = Vector2.up;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _input.move = Vector2.zero;
    }
}
