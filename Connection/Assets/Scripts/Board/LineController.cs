using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class LineController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    private LineRenderer line;
    private Vector3 v;
    
    
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        line.SetPosition(0, pointA.position);
        line.SetPosition(1, pointB.position);

        this.transform.position = pointA.position + (pointB.position - pointA.position) / 2;
        this.GetComponent<CapsuleCollider>().height = Vector3.Distance(pointA.position, pointB.position) / 10 - 2;

        v = pointB.position - pointA.position;
        this.transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(v, (v.x < 0) ? Vector3.up : Vector3.down));
    }
}
