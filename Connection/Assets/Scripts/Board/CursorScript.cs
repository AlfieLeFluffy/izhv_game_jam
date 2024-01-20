using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public ConnectionController controller;
    public GameObject cam;
    public RectTransform canvas;
    public Transform lines;

    private GameObject col;
    private Vector3 start;


    private void Start()
    {
        col = null;
    }

    void Update()
    {
        if (col != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                start = this.transform.position;
            }
            if (Input.GetMouseButton(0))
            {
                PostedNoteMove(col.transform);
            }
            if (Input.GetMouseButtonUp(0))
            {
                PostedNoteDrop(col.transform);
            }

            else if (Input.GetMouseButtonUp(1))
            {
                PostedNoteInteraction(col.transform);
            }
        }

        this.transform.position = controller.sideToBoard(Input.mousePosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        col = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (col == collision.gameObject)
        {
            col = null;
        }
    }


    public void PostedNoteMove(Transform note)
    {
        note.position += (this.transform.position - start);
        start = this.transform.position;
    }

    public void PostedNoteDrop(Transform note)
    {
        note.position += (this.transform.position - start);

        if (Input.mousePosition.y < 70)
        {
            controller.PutAside(note.gameObject);
        }
    }

    public void PostedNoteInteraction(Transform note)
    {
        if (controller.connect)
        {
            if (lines.childCount != 0)
            {
                if (lines.GetChild(lines.childCount - 1).GetComponent<LineController>().pointA == note)
                {
                    Destroy(lines.GetChild(lines.childCount - 1).gameObject);
                    return;
                }
                else if (lines.GetChild(lines.childCount - 1).GetComponent<LineController>().pointB == this.transform)
                {
                    lines.GetChild(lines.childCount - 1).GetComponent<LineController>().pointB = note;
                    return;
                }
            }
            
            controller.CreateConnection(note, this.transform);
        }
        else
        {
            //open preview
        }
    }
}
