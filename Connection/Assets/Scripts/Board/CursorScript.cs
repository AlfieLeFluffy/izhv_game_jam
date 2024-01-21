using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Yarn;

public class CursorScript : MonoBehaviour
{
    public ConnectionController controller;
    public Camera cam;
    public RectTransform canvas;
    public Transform lines;

    private GameObject col;
    private Vector3 start;
    private LineRenderer l;

    public bool dragLock;


    private void Start()
    {
        col = this.gameObject;
        dragLock = false;
    }

    void Update()
    {
        this.transform.position = controller.sideToBoard(Input.mousePosition);

        if ((cam.orthographicSize > 40
            && Input.mouseScrollDelta.y > 0)
            || (cam.orthographicSize < 100
            && Input.mouseScrollDelta.y < 0)) 
        {
            cam.orthographicSize -= Input.mouseScrollDelta.y * 3;
        }

        if (Input.GetMouseButtonDown(0))
        {
            start = this.transform.position;

            if (!col.Equals(this.gameObject))
            {
                dragLock = true;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!dragLock)
            {
                cam.gameObject.transform.position -= (this.transform.position - start) / 1.5f;
            }
            else if (!col.TryGetComponent<LineRenderer>(out l))
            {
                col.transform.position += this.transform.position - start;
            }

            start = this.transform.position;
        }

        if (!col.Equals(this.gameObject))
        {
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
                if (col.TryGetComponent<LineRenderer>(out l)
                    && controller.connect)
                {
                    Destroy(col.transform.gameObject);
                }
                else
                {
                    PostedNoteInteraction(col.transform);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!dragLock)
        {
            col = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (col.Equals(collision.gameObject))
        {
            ClearCol();
        }
    }

    public void ClearCol()
    {
        col = this.gameObject;
        dragLock = false;
    }


    private void ChangeConnectionText(Transform line)
    {
        print("yay");
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
