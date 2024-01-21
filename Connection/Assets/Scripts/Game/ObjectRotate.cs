using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class ObjectRotate : MonoBehaviour
{
    public float speed = 2f;

    public void RotateTo(GameObject target)
    {
        //find the vector pointing from our position to the target
        var _direction = (target.transform.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        var _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * speed);
        transform.rotation = new Quaternion(0,_lookRotation.y,0,_lookRotation.w);
    }
}
