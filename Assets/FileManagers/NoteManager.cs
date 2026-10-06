using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private TextAsset itemsFile;
    private Classes.NoteDatabase allNotes;
    private void Awake()
    {
        itemsFile = Resources.Load<TextAsset>("notes");
        allNotes = JsonUtility.FromJson<Classes.NoteDatabase>(itemsFile.text);
        /*foreach (Classes.Item item in allItems.items)
        {
           Debug.Log(item.Id);
        }*/
    }
    public Classes.Note getNote(int id)
    {
        return allNotes.notes[id];
    }
    public Classes.Note GetNote(string name)
    {
        //oddaje notatke po nazwie
        for (int i = 0; i < allNotes.notes.Length; i++)
            if (allNotes.notes[i].name == name)
                return allNotes.notes[i];
        return null;
    }
}
