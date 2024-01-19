using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionController : MonoBehaviour
{
    public Transform[] points;
    public GameObject linePrefab;
    public GameObject postedNotePrefab;
    public GameObject unusedNotePrefab;

    public Transform lines;
    public Transform postedNotes;
    public Transform unusedNotes;

    private GameObject newObject;
    
    
    void Start()
    {

        CreateConnection();
    }


    public void CreateConnection()
    {
        newObject = Instantiate(linePrefab, lines);

        LineController lc = newObject.GetComponent<LineController>();
        lc.pointA = points[0];
        lc.pointB = points[1];
    }

    public void AddNewNote()
    {
        newObject = Instantiate(unusedNotePrefab, unusedNotes);
        newObject.GetComponent<RectTransform>().localPosition = new Vector3(unusedNotes.childCount * 60 - 10, 0, 0);
    }
}
