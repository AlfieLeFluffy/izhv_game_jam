using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnusedNoteScript : MonoBehaviour
{
    public void UseNote()
    {
        Destroy(this.gameObject);
    }
}
