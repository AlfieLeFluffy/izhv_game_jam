using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnusedNoteScript : MonoBehaviour, IPointerClickHandler
{
    public ConnectionController controller;
    public int index;
    public int noteNumber;

    private Vector3 moveStart;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            controller.OpenPreview(this.GetComponentInChildren<TextMeshProUGUI>().text, this.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color);
        }
    }


    public void BeginDragNote()
    {
        moveStart = Input.mousePosition;
        controller.CamLock(true);
    }

    public void DragNote()
    {
        this.transform.position = this.transform.position + (Input.mousePosition - moveStart);
        moveStart = Input.mousePosition;
    }

    public void EndDragNote()
    {
        controller.CamLock(false);

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
