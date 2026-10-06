using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryNavigationLogicScript : MonoBehaviour
{
    private GameManager gameManager;

    private GameObject inventoryScreen;
    private GameObject equipementPanel;
    private GameObject notesPanel;
    
    private GameObject equipementObject;
    private GameObject notesObject;
    private GameObject mapObject;
    private GameObject optionsObject;

    private Button equipementButton;
    private Button notesButton;
    private Button mapButton;
    private Button optionsButton;
    [SerializeField]
    private GameObject[] notesButtons;

    private GameObject currentActiveTab;

    private GameObject inventorySlotFirst;

    private ColorBlock colorBlock;
    private Color normalColor;
    private Color highlightColor;

    [SerializeField]
    private int inventoryState;
    private void Awake()
    {
        inventoryScreen = GameObject.FindGameObjectWithTag("InventoryScreen");
        equipementPanel = GameObject.FindGameObjectWithTag("EquipementPanel");
        notesPanel = GameObject.FindGameObjectWithTag("NotesPanel");
        gameManager = GetComponent<GameManager>();

        equipementObject = GameObject.FindGameObjectWithTag("InventoryButton");
        mapObject = GameObject.FindGameObjectWithTag("MapButton");
        notesObject = GameObject.FindGameObjectWithTag("NotesButton");
        optionsObject = GameObject.FindGameObjectWithTag("OptionsButton");

        equipementButton = equipementObject.GetComponent<Button>();
        mapButton = mapObject.GetComponent<Button>();
        notesButton = notesObject.GetComponent<Button>();
        optionsButton = optionsObject.GetComponent<Button>();
        
        inventorySlotFirst = GameObject.Find("InventorySlot 1");

        inventoryState = 0;
    }
    private void Start()
    {
        normalColor = equipementButton.colors.normalColor;
        colorBlock = equipementButton.colors;
        TurnOffEverything();
    }
    public void EquipementOpen()
    {
        inventoryState = 1;
        currentActiveTab = equipementObject;
        inventoryScreen.SetActive(true);
        FakeHighlight(equipementButton);
        equipementPanel.SetActive(true);
        notesPanel.SetActive(false);
        gameManager.ManagerSetObject(inventorySlotFirst);

        StartingComands();
    }
    public void NotesOpen(bool by_button=false)
    {
        //otwiera zakladke z notatkami
        inventoryState = 3;
        currentActiveTab = notesObject;
        inventoryScreen.SetActive(true);
        FakeHighlight(notesButton);
        notesPanel.SetActive(true);
        equipementPanel.SetActive(false);

        if (by_button)
        {
            gameManager.ManagerSetObject(notesButtons[0]);
            Debug.Log(notesButtons[0].name);
        }

        StartingComands();
    }
    public int InventoryState()
    {
        //zwraca status ekwipunku
        /*
            0 -> wylaczony
            1 -> przedmioty
            2 -> mapa
            3 -> notatki
            4 -> opcje
        */
        return inventoryState;
    }

    //callback actions
    public void InputNotesOpen(InputAction.CallbackContext context)
    {
        if (!notesPanel.activeSelf)
        {
            TurnOffEverything();
            NotesOpen(true);
        }
        else
        {
            inventoryState = 0;
            gameManager.Disable();
            TurnOffEverything();
        }
    }
    public void InputCancel(InputAction.CallbackContext context)
    {
        if ((notesPanel.activeSelf || equipementPanel.activeSelf) && currentActiveTab != null)
        {
            GoToTabs();
        }
        else
        {
            inventoryState = 0;
            gameManager.Disable();
            TurnOffEverything();
        }
    }

    //prywatne
    private void GoToTabs()
    {
        //funkcja wracajaca do zakladek
        gameManager.ManagerSetObject(currentActiveTab);
        currentActiveTab = null;
        RevertAll();
    }
    public void TurnOffEverything()
    {
        //wylacza ekipunek i przywraca wszystko do wartosci domyslnych
        inventoryScreen.SetActive(false);
        notesPanel.SetActive(false);
        equipementPanel.SetActive(false);
        RevertAll();
        currentActiveTab = null;
        inventoryState = 0;
        gameManager.Disable();
    }
    private void StartingComands()
    {
        //odpala sie przy wlaczeniu ekipunku i kontroluje mozliwosci poruszania sie
        gameManager.InventoryMovementEnable();
        gameManager.DisableMovement();
        gameManager.DisableInteract();
    }
    private void FakeHighlight(Button button)
    {
        //sztucznie zaznacza przycisk
        highlightColor = button.colors.highlightedColor;
        colorBlock.normalColor = highlightColor;
        button.colors = colorBlock;
    }
    private void FakeRevert(Button button)
    {
        //usuwa zanznaczenie przycisku
        colorBlock.normalColor = normalColor;
        button.colors = colorBlock;
    }
    private void RevertAll()
    {
        //usuwa sztuczne zaznaczenie wszystkich elementow ekwipunku
        FakeRevert(equipementButton);
        FakeRevert(mapButton);
        FakeRevert(notesButton);
        FakeRevert(optionsButton);
    }
}
