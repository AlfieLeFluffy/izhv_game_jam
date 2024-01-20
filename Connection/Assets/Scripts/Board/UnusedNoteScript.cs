using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnusedNoteScript : MonoBehaviour, IPointerClickHandler
{
    public ConnectionController controller;
    public int index;

    private Vector3 moveStart;



    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            controller.SwitchPreviewActive();
        }
    }


    public void GrabNote()
    {
        moveStart = Input.mousePosition;
    }

    public void DragNote()
    {
        this.transform.position = this.transform.position + (Input.mousePosition - moveStart);
        moveStart = Input.mousePosition;
    }

    public void DropNote()
    {
        if (Input.mousePosition.y > 70)
        {
            controller.PostNote(this.gameObject);
        }
        else
        {
            this.transform.localPosition = new Vector3(index * 60 - 10, 0, 0);
        }
    }
}
