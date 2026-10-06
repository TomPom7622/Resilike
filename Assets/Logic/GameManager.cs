using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //zajmuje sie wlaczeniem i wylaczaniem ekwipunku
    //ustala reguly gry

    //skrypty
    //skrypt gracza
    private PlayerScript Player;
    //skrypt ekwipunku
    private InventoryLogic InventoryLogic;
    //skrypt wysiwtlacza zycia
    private HealthBarScript healthBarScript;
    //skrypt inspekcji
    private InspectScript inspectScript;
    //skrypt pola tekstowego
    private TextFieldScript textFieldScript;
    //ekran notatek
    private NoteScreenScript noteScreenScript;
    //nawigacja po ekwipunku
    private InventoryNavigationLogicScript inventoryNavigationScript;


    //wyswietlacz ekwipunku
    private GameObject InventoryScreen;
    //akcje gracza
    private DefaultPlayerActions PlayerActions;
    //event system
    private EventSystem EventSystem;
    //akcje inspekcji gracza
    private PlayerInteractScript playerInteractScript;
    //pierwszy przycisk po wlaczeniu ekwipunku
    private GameObject PrimaryButton;
    
    //zmienne przechowujace dane akcje
    public InputAction aimAction;
    public InputAction fireAction;
    public InputAction runAction;
    public InputAction moveAction;
    public InputAction tankAction;
    public InputAction inventoryAction;
    public InputAction mapAction;
    public InputAction notesAction;
    public InputAction optionsAction;
    public InputAction cancelAction;

    //flagi
    [SerializeField]
    private bool inventoryOptions;
    [SerializeField]
    private bool inventory;
    [SerializeField]
    private bool inventoryMovement;
    [SerializeField]
    private bool inspectInteract;
    [SerializeField]
    private bool interact;
    [SerializeField]
    private bool textInteract;
    [SerializeField]
    private bool playerMovement;

    //ustala poczatkowe reguly gry
    private void Awake()
    {
        //pobieranie danych
        PlayerActions = new DefaultPlayerActions();
        playerInteractScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteractScript>();
        EventSystem = FindAnyObjectByType<EventSystem>();
        PrimaryButton = GameObject.Find("InventorySlot 1");
        InventoryScreen = GameObject.FindGameObjectWithTag("InventoryScreen");
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        InventoryLogic=GetComponent<InventoryLogic>();
        inventoryNavigationScript = GetComponent <InventoryNavigationLogicScript>();
        healthBarScript=GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBarScript>();
        inspectScript = GameObject.FindGameObjectWithTag("Inspect").GetComponent<InspectScript>();
        textFieldScript = GameObject.FindGameObjectWithTag("Textbox").GetComponent<TextFieldScript>();
        noteScreenScript = GameObject.FindGameObjectWithTag("NotesPanel").GetComponentInParent<NoteScreenScript>();
    }

    private void Start()
    {
        //przypisywanie wartosci
        EventSystem.sendNavigationEvents = false;
        InventoryScreen.SetActive(false);
    }

    private void OnEnable()
    {
        cancelAction = PlayerActions.Player.Cancel;
        cancelAction.performed += inventoryNavigationScript.InputCancel;

        fireAction = PlayerActions.Player.Fire;
        fireAction.performed += Player.OnFire;

        inventoryAction = PlayerActions.Player.Inventory;
        inventoryAction.performed += OpenInv;

        notesAction = PlayerActions.Player.Notes;
        notesAction.performed += inventoryNavigationScript.InputNotesOpen;
        
        mapAction = PlayerActions.Player.Map;

        optionsAction = PlayerActions.Player.Options;

        moveAction = PlayerActions.Player.Move;

        tankAction = PlayerActions.Player.TankControls;

        aimAction.performed += Player.Aim;
        aimAction = PlayerActions.Player.Aim;

        runAction = PlayerActions.Player.Run;

        InventoryOptionEnable();
    }
    private void OnDisable()
    {
        InventoryOptionDisable();
    }

    public void ManagerSetObject(GameObject obj)
    {
        //ustawia wskaznik na dany obiekt
        EventSystem.SetSelectedGameObject(obj);
    }
    public void InventoryOptionEnable()
    {
        //wlacza mozliwosc wyswietlenia ekipunku
        inventoryAction.Enable();
        notesAction.Enable();
        inventoryOptions = true;
    }
    public void InventoryOptionDisable()
    {
        //wlacza mozliwosc wyswietlenia ekipunku
        inventoryAction.Disable();
        notesAction.Disable();
        inventoryOptions = false;
    }

    public void Disable(bool ifevent = false)
    {
        //wylacza ekwipunek
        if (InventoryScreen.activeSelf)
        {
            Debug.Log("wy³¹czam");
            Time.timeScale = 1;
            inventoryNavigationScript.TurnOffEverything();
            InventoryMovementDisable();
            if (!ifevent)
            {
                Player.InputOnOrOf();
                EnableMovement();
                EnableInteract();
                InventoryOptionEnable();
                healthBarScript.TurnHealthBarOnOrOff();
            }
        }
        inventory = false;
    }

    public void InventoryMovementEnable(bool wait = false)
    {
        //wlacza opcje poruszania sie po ekipunku
        if (wait)
            StartCoroutine(Coroutine());
        else
            EventSystem.sendNavigationEvents = true;
        inventoryMovement = true;
        cancelAction.Enable();

    }
    public void InventoryMovementDisable()
    {
        //wylacza opcje poruszania sie po ekwipunku

        EventSystem.sendNavigationEvents = false;
        inventoryMovement = false;
        cancelAction.Disable();
    }

    public void EnableInspectInteract()
    {
        //wlacza zmiane okna inspekcji i wylacza poruszanie sie po ekwipunku
        PlayerActions.UI.Proceed.performed += inspectScript.OnInspectInteract;
        PlayerActions.UI.Proceed.Enable();
        PlayerActions.UI.Submit.Disable();
        InventoryMovementDisable();
        inspectInteract = true;
    }
    public void DisableInspectInteract()
    {
        //wylacza zmiane okna inspekcji i wlacza poruszanie sie po ekwipunku
        PlayerActions.UI.Proceed.performed -= inspectScript.OnInspectInteract;
        PlayerActions.UI.Proceed.Disable();
        PlayerActions.UI.Submit.Enable();
        InventoryMovementEnable(true);
        inspectInteract = false;
    }
    public void EnableInteract()
    {
        //wlacza interakcje z otoczeniem
        Debug.Log("EnableInteract");
        PlayerActions.Player.Interact.performed += playerInteractScript.OnInteract;
        PlayerActions.Player.Interact.Enable();
        interact = true;
    }
    public void DisableInteract()
    {
        //wylacza interakcje z otoczniem
        Debug.Log("DisableInteract");
        PlayerActions.Player.Interact.performed -= playerInteractScript.OnInteract;
        PlayerActions.Player.Interact.Disable();
        interact = false; ;
    }
    public void EnableTextInteract()
    {
        //wlacza interakcje z polem tekstowym
        PlayerActions.Player.Interact.performed += textFieldScript.OnTextInteract;
        PlayerActions.Player.Interact.Enable();
        textInteract = true;
    }
    public void DisableTextInteract()
    {
        //wylacza interakcje z polem tekstowym
        PlayerActions.Player.Interact.performed -= textFieldScript.OnTextInteract;
        PlayerActions.Player.Interact.Disable();
        textInteract = false;
    }
    public void EnableMovement()
    {
        //wlacza poruszanie siê gracza
        moveAction.Enable();
        tankAction.Enable();
        runAction.Enable();
        fireAction.Enable();
        aimAction.Enable();
        playerMovement = true;
    }
    public void DisableMovement()
    {
        //wylacza poruszanie siê gracza
        moveAction.Disable();
        tankAction.Disable();
        runAction.Disable();
        fireAction.Disable();
        aimAction.Disable();
        playerMovement = false;
    }
    public void EnableNoteInteract()
    {
        PlayerActions.Player.Interact.performed += noteScreenScript.OnNoteInteract;
        PlayerActions.Player.Interact.Enable();
    }
    public void DisableNoteInteract()
    {
        PlayerActions.Player.Interact.performed -= noteScreenScript.OnNoteInteract;
        PlayerActions.Player.Interact.Disable();
    }
    public void EventEndController()
    {
        healthBarScript.TurnHealthBarOn();
    }
    IEnumerator Coroutine()
    {
        yield return new WaitForSecondsRealtime(1);
        EventSystem.sendNavigationEvents = true;
    }

    //prywatne

    private void OpenInv(InputAction.CallbackContext context)
    {
        //wlacza i wylacza ekwipunek
        if (inventoryNavigationScript.InventoryState()==1)
        {
            //jesli wlaczony
            if(!InventoryLogic.contextMenuOpen)
            {
                //jesli menu kontekstowe wylaczone
                //wylacza ekwipunek
                Disable(); 
            }
            else
            {
                //menu kontekstowe wlaczone ...
            }
        }
        else
        {
            //jesli wylaczony
            //wlacza ekwipunek
            inventoryNavigationScript.TurnOffEverything();
            Enable();
        }
    }
    
    private void Enable()
    {
        //wlacza ekipunek
        if (!InventoryScreen.activeSelf)
        {
            Time.timeScale = 0;
            InventoryMovementEnable();
            InventoryLogic.OnItemHighlit(PrimaryButton);
            ManagerSetObject(PrimaryButton);
            InventoryScreen.SetActive(true);
            Player.InputOnOrOf();
            healthBarScript.TurnHealthBarOnOrOff();
            DisableInteract();
            inventoryNavigationScript.EquipementOpen();
            inventory = true;
        }
    }

    
}

