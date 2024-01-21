using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public ConnectionController controller;
    public GameObject cam;
    public RectTransform canvas;
    public Transform lines;

    private GameObject col;
    private Vector3 start;
    private LineRenderer l;


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
                if (col.TryGetComponent<LineRenderer>(out l))
                {
                    ChangeConnectionText(col.transform);
                }
                else
                {
                    PostedNoteDrop(col.transform);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (col.TryGetComponent<LineRenderer>(out l))
                {
                    DeleteConnection(col.transform);
                }
                else
                {
                    PostedNoteInteraction(col.transform);
                }
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


    private void ChangeConnectionText(Transform line)
    {
        print("yay");
    }

    private void DeleteConnection(Transform line)
    {
        if (controller.connect)
        { 
            Destroy(line.gameObject);
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
                if (lines.GetChild(lines.childCount - 1).GetComponent<LineController>().pointB == this.transform)
                {
                    if (lines.GetChild(lines.childCount - 1).GetComponent<LineController>().pointA == note)
                    {
                        Destroy(lines.GetChild(lines.childCount - 1).gameObject);
                        return;
                    }
                    
                    lines.GetChild(lines.childCount - 1).GetComponent<LineController>().pointB = note;
                    return;
                }
            }
            
            controller.CreateConnection(note, this.transform);
        }
        else
        {
            controller.OpenPreview(note.GetComponentInChildren<TextMeshPro>().text, Color.gray);
        }
    }
}
