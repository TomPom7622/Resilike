using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Classes;
public class NoteScreenScript : MonoBehaviour
{
    /*
        Script that contolls the NoteScreen
        Includes showing notes and cycling trough them
    */

    //skrypty
    private GameManager gameManager;
    private InventoryNavigationLogicScript inventoryNavigationLogicScript;
    private NoteManager noteManager;

    [SerializeField]
    private Button[] noteButtons;
    private Button noteButton;
    private TMP_Text noteName;
    private TMP_Text noteText;
    private TMP_Text notePrompt;
    private Note note;
    private string[] textWindows;
    private int i;
    private Color baseColour;

    //flaga czy notatka jest otwarta
    public bool noteOpen;
    private void Awake()
    {
        inventoryNavigationLogicScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<InventoryNavigationLogicScript>();
        noteManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<NoteManager>();
        noteName = GameObject.FindGameObjectWithTag("NoteName").GetComponent<TMP_Text>();
        noteText = GameObject.FindGameObjectWithTag("NoteText").GetComponent<TMP_Text>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponentInParent<GameManager>();
        notePrompt = GameObject.FindGameObjectWithTag("NoteTextPrompt").GetComponent<TMP_Text>();
        noteOpen = false;
    }

    public void ShowNote(int id)
    {
        Debug.Log("NotesButton_" + id.ToString());
        i = 0;
        noteButton = noteButtons[id];
        var colour = noteButton.colors;
        baseColour = colour.normalColor;
        colour.normalColor = colour.selectedColor;
        noteButton.colors = colour;
        noteOpen = true;
        note = noteManager.getNote(id);
        noteName.text = note.Name;
        textWindows = note.text.Split("**");
        noteText.text = textWindows[0];
        notePrompt.text = "Next";
        inventoryNavigationLogicScript.NotesOpen();
        gameManager.InventoryOptionDisable();
        gameManager.DisableInteract();
        gameManager.DisableMovement();
        gameManager.EnableNoteInteract();
    }
    public void OnNoteInteract(InputAction.CallbackContext context)
    {   
        CycleNote();
    }

    //prywatne
    private void CycleNote()
    {
        i++;
        if(i<textWindows.Length)
        {
            Debug.Log("Cycle");
            if (i != textWindows.Length - 1)
                notePrompt.text = "Next";
            else
                notePrompt.text = "Close";
            noteText.text = textWindows[i];
        }
        else
        {
            gameManager.DisableNoteInteract();
            inventoryNavigationLogicScript.TurnOffEverything();
            gameObject.SetActive(false);
            noteName.text = "";
            noteText.text = "";
            notePrompt.text = "";
            gameManager.InventoryOptionEnable();
            gameManager.EnableInteract();
            gameManager.EnableMovement();
            if (noteButton != null)
            {
                var colour = noteButton.colors;
                colour.normalColor = baseColour;
                noteButton.colors = colour;
            }
            noteOpen = false;
        }
    }

}
