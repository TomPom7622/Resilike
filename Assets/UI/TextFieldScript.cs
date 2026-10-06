using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TextFieldScript : MonoBehaviour
{
    //skrypty
    private GameManager gameManager;
    private InteractPromptManagerScript interactPromptManagerScript;

    private EventSystem eventSystem;
    private GameObject healthBar;
    private TMP_Text text;
    private GameObject textBox;
    private GameObject optionBox;
    private GameObject yesButton;
    //private Button noButton;
    //private Button thirdButton;
    private string[] tmp;
    private string newText;
    private string[] textWindows;
    private string[] options;
    private int i;
    private bool pickingOptions;
    private bool ifEvent = false;
    private void Awake()
    {
        //pobieranie danych
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        yesButton = GameObject.FindGameObjectWithTag("ButtonYes");
        eventSystem=FindAnyObjectByType<EventSystem>();
        //noButton=GameObject.FindGameObjectWithTag("ButtonNo").GetComponent<Button>();
        //thirdButton=GameObject.FindGameObjectWithTag("ButtonThird").GetComponent<Button>();
        optionBox = GameObject.FindGameObjectWithTag("DialogueOptionsMenu");
        interactPromptManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<InteractPromptManagerScript>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        textBox = GameObject.FindGameObjectWithTag("Textbox");
        text = textBox.GetComponentInChildren<TMP_Text>();
        pickingOptions = false;
    }
    private void Start()
    {
        //przypisywanie zmiennych
        textBox.SetActive(false);
        optionBox.SetActive(false);
        i = 0;
    }
    public void ShowText(string boxText, bool ifevent=false)
    {
        ifEvent = ifevent;
        tmp = boxText.Split("{}");
        textWindows = tmp[0].Split("**");
        if (tmp.Length > 1)
        {
            options = tmp[1].Split("**");
        }
        text.text = textWindows[0];
        gameManager.InventoryOptionDisable();
        gameManager.DisableInteract();
        gameManager.DisableMovement();
        healthBar.SetActive(false);
        gameManager.EnableTextInteract();
        Time.timeScale = 0;
        textBox.SetActive(true);
    }

    public void ShowText(int id,bool ifevent= false)
    {
        ifEvent = ifevent;
        Debug.Log(id);
        newText = interactPromptManagerScript.GetPromptText(id); 
        tmp = newText.Split("{}");
        textWindows = tmp[0].Split("**");
        if (tmp.Length > 1)
        {
            options = tmp[1].Split("**");
        }
        text.text = textWindows[0];
        gameManager.InventoryOptionDisable();
        gameManager.DisableInteract();
        gameManager.DisableMovement();
        healthBar.SetActive(false);
        gameManager.EnableTextInteract();
        Time.timeScale = 0;
        textBox.SetActive(true);
    }
    public void OnTextInteract(InputAction.CallbackContext context)
    {
        Cycling();
    }
    public void MoveOn()
    {
        pickingOptions = false;
        Cycling();
    }
    public void EventEnd()
    {
        ifEvent = false;
        gameManager.InventoryOptionEnable();
        gameManager.EnableInteract();
        gameManager.EnableMovement();
        healthBar.SetActive(true);
    }

    //prywatne
    private void Cycling()
    {
        if (!pickingOptions)
        {
            i++;
            if (i < textWindows.Length)
            {
                if (textWindows[i] == "[option]")
                {
                    eventSystem.SetSelectedGameObject(yesButton);
                    optionBox.SetActive(true);
                    pickingOptions = true;
                    gameManager.InventoryMovementEnable(true);

                }
                else
                    text.text = textWindows[i];
            }
            else
            {
                Debug.Log("Wyjscie");
                i = 0;
                textBox.SetActive(false);
                optionBox.SetActive(false);
                gameManager.DisableTextInteract();
                if(!ifEvent)
                {
                    gameManager.InventoryOptionEnable();
                    gameManager.EnableInteract();
                    gameManager.EnableMovement();
                    healthBar.SetActive(true);
                }
                Time.timeScale = 1;
            }
        }
    }
}
