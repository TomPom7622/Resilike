using UnityEngine;
using System;

public class NoteButtonScript : MonoBehaviour
{
    /*
        Script attached to buttons in notes list that shows content of specific note
    */

    //skrypty
    private NoteScreenScript noteScreenScript;

    private int id;

    
    private void Awake()
    {
        //inicjalizacja zmiennych
        noteScreenScript = GameObject.FindGameObjectWithTag("NotesPanel").GetComponent<NoteScreenScript>();
        id = Int32.Parse(gameObject.tag.Split('_')[1]);
    }

    public void OnClick()
    {
        //sprawdza czy jest obecnie otwarta jakas notatka i jak nie, to ja otwiera
        if (!noteScreenScript.noteOpen)
            noteScreenScript.ShowNote(id);
    }
}
