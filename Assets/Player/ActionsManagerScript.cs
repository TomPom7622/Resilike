using UnityEngine;

public class ActionsManagerScript : MonoBehaviour
{
    //skrypty

    private InventoryLogic inventoryLogic;
    private NoteScreenScript noteScreenScript;
    private TextFieldScript textFieldScript;
    private EventManagerScript eventManagerScript;
    private NotesListScript notesListScript;

    private int id;

    private void Awake()
    {
        noteScreenScript = GameObject.FindGameObjectWithTag("NotesPanel").GetComponent<NoteScreenScript>();
        inventoryLogic = GetComponent<InventoryLogic>();
        textFieldScript = GameObject.FindGameObjectWithTag("Textbox").GetComponent<TextFieldScript>();
        eventManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManagerScript>();
        notesListScript = GameObject.FindGameObjectWithTag("NotesList").GetComponent <NotesListScript>();
    }
    public void Iteraction(GameObject instance)
    {
        //triggeruje akcje
        //tutaj dodajemy akcje ++
        string objectTag = instance.tag;

        switch (objectTag)
        {
            case "PowerBox":
                eventManagerScript.BeginEvent("PowerBoxEvent"); break;
            default:
                Debug.Log("Brak nazwy idioto"); return;
        }

    }
    public void Inspection(GameObject instance)
    {
        //wyswietla inspekcje
        //tutaj dodajemy inspekcje ++

        string objectTag = instance.tag;
        id = -1;

        switch (objectTag)
        {
            case "PowerBox":
                 id = 0; break;
            case "GenericLockedDoor":
                 id = 1; break;
            case "SecurityDoor":
                 id = 2; break;
            default:
                Debug.Log("Brak nazwy idioto"); return;
        }
        textFieldScript.ShowText(id);
    }
    public void Pickup(GameObject instance)
    {
        //dodaje przedmioty
        string objectTag = instance.tag;
        if (objectTag == "Note")
        {
            switch (instance.name)
            {
                case "Electrician's note":
                    id = 0;
                    break;
            }
            notesListScript.NoteButtonEnable(id);
            noteScreenScript.ShowNote(id);
        }
        else
        {
            switch (objectTag)
            {
                case "Weapon[A]":
                    id = 0; break;
                case "Weapon[A]Ammo":
                    id = 1; break;
                case "Medkit":
                    id = 2; break;
                case "PowerboxKey":
                    id = 3; break;

            }
            inventoryLogic.AddItem(id);
        }
        Destroy(instance);

    }
}
