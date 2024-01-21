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
    private GameObject g;

    public bool dragLock;

    private string oldText;
    private bool typing;
    private TextMeshPro connectionText;


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

        if (typing)
        {
            if (Input.GetKeyUp(KeyCode.Return)
                || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                g.SetActive(false);
                typing = false;
            }
            else if (Input.GetKeyUp(KeyCode.Escape)
                || Input.GetMouseButtonUp(0)
                || Input.GetMouseButtonUp(1))
            {
                connectionText.text = oldText;
                g.SetActive(false);
                typing = false;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                connectionText.text = connectionText.text.Remove(connectionText.text.Length - 1, 1);
            }
            else if (Input.anyKeyDown)
            {
                connectionText.text += Input.inputString;
            }
        }
        else
        {
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

            if (col.Equals(this.gameObject))
            {
                if (Input.GetMouseButtonUp(1)
                    && lines.childCount != 0)
                {
                    if (lines.GetChild(lines.childCount - 1).GetComponent<LineController>().pointB == this.transform)
                    {
                        Destroy(lines.GetChild(lines.childCount - 1).gameObject);
                    }
                }
            }
            else
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
                    if (col.TryGetComponent<LineRenderer>(out l))
                    {
                        if (controller.connecting)
                        {
                            Destroy(col.transform.gameObject);
                        }
                        else
                        {
                            typing = true;
                            g = col.transform.GetChild(0).gameObject;
                            g.SetActive(true);
                            connectionText = col.transform.GetChild(1).GetComponent<TextMeshPro>();
                            oldText = connectionText.text;
                        }
                    }
                    else
                    {
                        PostedNoteInteraction(col.transform);
                    }
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
        if (controller.connecting)
        {
            if (lines.childCount != 0)
            {
                LineController lastLine = lines.GetChild(lines.childCount - 1).GetComponent<LineController>();

                if (lastLine.pointB == this.transform)
                {
                    if (lastLine.pointA == note)
                    {
                        Destroy(lines.GetChild(lines.childCount - 1).gameObject);
                        return;
                    }

                    LineController line;

                    for (int i = 0; i < lines.childCount - 1; i++)
                    {
                        line = lines.GetChild(i).GetComponent<LineController>();

                        if ((line.pointA == lastLine.pointA
                            && line.pointB == note)
                            || (line.pointA == note
                            && line.pointB == lastLine.pointA))
                        {
                            Destroy(lines.GetChild(lines.childCount - 1).gameObject);
                            return;
                        }
                    }

                    lastLine.pointB = note;
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
