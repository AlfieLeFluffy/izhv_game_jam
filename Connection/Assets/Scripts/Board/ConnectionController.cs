using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionController : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject postedNotePrefab;
    public GameObject unusedNotePrefab;

    public Transform lines;
    public Transform postedNotes;
    public Transform unusedNotes;

    public GameObject cam;
    public RectTransform canvas;
    public Transform cursor;

    private GameObject newObject;
    public bool connect;
    
    
    void Start()
    {
        connect = false;
    }


    public Vector3 sideToBoard(Vector3 side)
    {
        float camHeight = 2 * cam.GetComponent<Camera>().orthographicSize;
        float camWidth = camHeight * cam.GetComponent<Camera>().aspect;
        Vector3 camPosition = cam.GetComponent<Transform>().localPosition;

        return new Vector3((side.x - canvas.position.x) * camWidth / canvas.rect.width + canvas.position.x + camPosition.x * 50,
            (side.y - canvas.position.y) * camHeight / canvas.rect.height + canvas.position.y + camPosition.z * 50, 0);
    }

    public void CreateConnection(Transform start, Transform end)
    {
        newObject = Instantiate(linePrefab, lines);

        LineController line = newObject.GetComponent<LineController>();
        line.pointA = start;
        line.pointB = end;
    }

    public void AddNewNote()
    {
        newObject = Instantiate(unusedNotePrefab, unusedNotes);
        newObject.GetComponent<RectTransform>().localPosition = new Vector3(unusedNotes.childCount * 60 - 10, 0, 0);
        newObject.GetComponent<UnusedNoteScript>().controller = this.GetComponent<ConnectionController>();
        newObject.GetComponent<UnusedNoteScript>().index = unusedNotes.childCount;
    }

    public void PostNote(GameObject note)
    {
        newObject = Instantiate(postedNotePrefab, postedNotes);
        newObject.GetComponent<Transform>().position = sideToBoard(note.transform.position);

        RectTransform[] noteArr = unusedNotes.GetComponentsInChildren<RectTransform>();

        for (int i = note.GetComponent<UnusedNoteScript>().index + 1; i < noteArr.Length; i++)
        {
            noteArr[i].localPosition -= new Vector3(60, 0, 0);
            noteArr[i].GetComponent<UnusedNoteScript>().index--;
        }

        Destroy(note);
    }

    public void PutAside(GameObject note)
    {
        newObject = Instantiate(unusedNotePrefab, unusedNotes);
        newObject.GetComponent<RectTransform>().localPosition = new Vector3(unusedNotes.childCount * 60 - 10, 0, 0);
        newObject.GetComponent<UnusedNoteScript>().controller = this.GetComponent<ConnectionController>();
        newObject.GetComponent<UnusedNoteScript>().index = unusedNotes.childCount;

        LineController line;

        for (int i = 0; i < lines.childCount; i++)
        {
            line = lines.GetChild(i).GetComponent<LineController>();
            if (line.pointA == note.transform || line.pointB == note.transform)
            {
                Destroy(lines.GetChild(i).gameObject);
            }
        }

        Destroy(note);
    }

    public void SwitchConnectVar()
    {
        connect = !connect;
    }
}
