using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnusedNoteScript : MonoBehaviour
{
    public ConnectionController controller;
    public int index;

    private Vector3 moveStart;


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
