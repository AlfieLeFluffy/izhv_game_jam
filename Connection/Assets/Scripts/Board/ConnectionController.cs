using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConnectionController : MonoBehaviour
{
    public Transform[] points;

    public GameObject linePrefab;
    private GameObject newLine;
    
    
    void Start()
    {
        newLine = Instantiate(linePrefab);
        LineController lc = newLine.GetComponent<LineController>();
        lc.pointA = points[0];
        lc.pointB = points[1];
    }
}
