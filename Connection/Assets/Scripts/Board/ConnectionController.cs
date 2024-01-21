using TMPro;
using UnityEngine;

public class ConnectionController : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject postedNotePrefab;
    public GameObject unusedNotePrefab;
    public NoteTable noteTable;

    public Transform lines;
    public Transform postedNotes;
    public Transform unusedNotes;
    public GameObject notePreview;

    public GameObject cam;
    public RectTransform canvas;
    public CursorScript cursor;

    private GameObject newObject;
    public bool connecting;
    

    void Start()
    {
        connecting = false;
    }


    public void CamLock(bool value)
    {
        cursor.dragLock = value;
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

    public void AddNoteToSide(int noteNumber)
    {
        newObject = Instantiate(unusedNotePrefab, unusedNotes);
        newObject.GetComponent<RectTransform>().localPosition = new Vector3(unusedNotes.childCount * 60 - 10, 0, 0);
        newObject.GetComponent<UnusedNoteScript>().controller = this.GetComponent<ConnectionController>();
        newObject.GetComponent<UnusedNoteScript>().index = unusedNotes.childCount;
        newObject.GetComponent<UnusedNoteScript>().noteNumber = noteNumber;

        newObject.GetComponentInChildren<TextMeshProUGUI>().text = noteTable.titles[noteNumber];
        newObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = noteTable.colors[noteNumber];
    }

    public void PutAside(GameObject note)
    {
        AddNoteToSide(note.GetComponent<UnusedNoteScript>().noteNumber);

        LineController line;

        for (int i = 0; i < lines.childCount; i++)
        {
            line = lines.GetChild(i).GetComponent<LineController>();
            if (line.pointA == note.transform || line.pointB == note.transform)
            {
                Destroy(lines.GetChild(i).gameObject);
            }
        }

        cursor.ClearCol();
        Destroy(note);
    }

    public void PostNote(GameObject note)
    {
        newObject = Instantiate(postedNotePrefab, postedNotes);
        newObject.GetComponent<Transform>().position = sideToBoard(note.transform.position);
        newObject.GetComponentInChildren<TextMeshPro>().text = noteTable.titles[note.GetComponent<UnusedNoteScript>().noteNumber];
        newObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = noteTable.materials[note.GetComponent<UnusedNoteScript>().noteNumber];

        newObject.GetComponent<PostedNoteScript>().noteNumber = note.GetComponent<UnusedNoteScript>().noteNumber;

        RectTransform child;

        for (int i = note.GetComponent<UnusedNoteScript>().index; i < unusedNotes.childCount; i++)
        {
            child = unusedNotes.GetChild(i).GetComponent<RectTransform>();
            child.localPosition -= new Vector3(60, 0, 0);
            child.GetComponent<UnusedNoteScript>().index--;
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
        connecting = !connecting;
    }
}
