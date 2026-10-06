using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectScript : MonoBehaviour
{
    //kontroluje zachowanie ekwipunku podczas inspekcji

    //skrypty
    //game manager do wylaczania ruchu w ekwipunku
    private GameManager gameManager;
    //logika ekwipunku
    private InventoryLogic inventoryLogic;


    //tekst panelu inspekcji
    private TMP_Text inspectionText;

    //akcje gracza
    //przedmiot inspekcji
    public Classes.Item item;
    //tablica z tekstem inspekcji i nowa nazwa
    private string[] tmp;
    //tablica z tekstem podzielonym na okna
    private string[] inspectText;
    //nazwa przedmiotu
    private string itemName;
    //licznik okien
    private int i = 0;

    private void Awake()
    {
        //pobieranie danych
        inventoryLogic = GameObject.FindGameObjectWithTag("GameController").GetComponent<InventoryLogic>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        inspectionText = GameObject.FindGameObjectWithTag("InspectionPanel").GetComponentInChildren<TMP_Text>();
    }
    public void OnInspect()
    {
        if (item != null)
        {
            //co sie dzieje po kliknieciu "check"
            Debug.Log(item);
            tmp = item.Desc.Split("||");
            itemName = item.Name;
            if (tmp.Length > 1)
            { 
                itemName = tmp[1];
                item.Name = itemName;
                inventoryLogic.InventoryItemRefresh(item);
            }
            inventoryLogic.OnItemCancel();
            inspectText = tmp[0].Split("**");
            inspectionText.text = inspectText[0];
            gameManager.EnableInspectInteract();
        }
        else
            Debug.Log(":(");
    }
    public void OnInspectInteract(InputAction.CallbackContext context)
    {
        //zmienia okno inspekcji
        i++;
        if (i<inspectText.Length)
        {
            inspectionText.text = inspectText[i];
        }
        else
        {
            i = 0;
            inspectionText.text = itemName;
            gameManager.DisableInspectInteract();
        }
    }
}
