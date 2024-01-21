using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Yarn;

public class ConnectionController : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject postedNotePrefab;
    public GameObject unusedNotePrefab;

    public Transform lines;
    public Transform postedNotes;
    public Transform unusedNotes;
    public GameObject notePreview;

    public GameObject cam;
    public RectTransform canvas;
    public Transform cursor;

    private GameObject newObject;
    public bool connect;

    private string[] testNoteTexts = new string[] { "Test Text", "beep boop", "uwu", "Pedro Pascal", "idk anymore", "pain and suffering" };
    private Color[] testNoteColors = new Color[] { Color.green, Color.blue, Color.red, Color.magenta, Color.yellow, Color.cyan };
    

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

        newObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = testNoteColors[(unusedNotes.childCount - 1) % 6];
        newObject.GetComponentInChildren<TextMeshProUGUI>().text = testNoteTexts[(unusedNotes.childCount - 1) % 6];
    }

    public void PostNote(GameObject note)
    {
        newObject = Instantiate(postedNotePrefab, postedNotes);
        newObject.GetComponent<Transform>().position = sideToBoard(note.transform.position);
        //newObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = note.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color;
        newObject.GetComponentInChildren<TextMeshPro>().text = note.GetComponentInChildren<TextMeshProUGUI>().text;

        RectTransform child;

        for (int i = note.GetComponent<UnusedNoteScript>().index; i < unusedNotes.childCount; i++)
        {
            child = unusedNotes.GetChild(i).GetComponent<RectTransform>();
            child.localPosition -= new Vector3(60, 0, 0);
            child.GetComponent<UnusedNoteScript>().index--;
        }

        Destroy(note);
    }

    public void PutAside(GameObject note)
    {
        AddNewNote();
        //newObject.transform.GetChild(0).GetComponent<Material>().color = note.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color;
        newObject.GetComponentInChildren<TextMeshProUGUI>().text = note.GetComponentInChildren<TextMeshPro>().text;

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

    public void OpenPreview(string text, Color color)
    {
        notePreview.SetActive(!notePreview.activeSelf);
        notePreview.GetComponentInChildren<TextMeshProUGUI>().text = text;
        notePreview.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Image>().color = color;
    }

    public void ClosePreview()
    {
        notePreview.SetActive(false);
    }

    public void SwitchConnectVar()
    {
        connect = !connect;
    }
}
