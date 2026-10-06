using UnityEngine;
using UnityEngine.UI;

public class NotesListScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] noteButtons;
    public void NoteButtonEnable(int id) 
    {
        noteButtons[id].SetActive(true);
    }
}
