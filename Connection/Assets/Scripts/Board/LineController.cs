using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private LineRenderer lr;
    
    
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lr.SetPosition(0, pointA.position);
        lr.SetPosition(1, pointB.position);
    }
}
