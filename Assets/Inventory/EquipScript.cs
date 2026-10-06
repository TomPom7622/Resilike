using UnityEngine;

public class EquipScript : MonoBehaviour
{
    //skrypt przycisku ekwipowania albo uzywania

    //skrypty
    //slot tego co mamy w rece
    private EquippedSlotScript EquippedSlotScript;
    //skrypt ekwipunku
    private InventoryLogic inventoryLogic;
    //skrypt uzywania przedmiotu
    private UseItem useItem;

    //przedmiot
    public Classes.Item item;

    private void Awake()
    {
        //pobieranie danych
        EquippedSlotScript= GameObject.FindGameObjectWithTag("EquippedSlot").GetComponent<EquippedSlotScript>();
        inventoryLogic = GameObject.FindGameObjectWithTag("GameController").GetComponent<InventoryLogic>();
        useItem = GameObject.FindGameObjectWithTag("Player").GetComponent<UseItem>();
    }

    public void OnClick()
    {
        //klikniecie w przycisk Equip / Use
        Debug.Log("Click");
        if (item != null)
        {
            //mamy przedmiot
            Debug.Log("Check");
            if (item.Equipable)
            {
                //mozna go wyposazyc
                Debug.Log("Function");
                EquippedSlotScript.ChangeEquipped(item);
                inventoryLogic.OnItemCancel();
            }
            else
            {
                //nie mozna go wyposazyc, za to nalezy go uzyc
                if(useItem.UseCurrentItem(item))
                {
                    inventoryLogic.DeleteItem(item);
                    inventoryLogic.OnItemCancel(true);
                    Debug.Log("Udalo sie uzyc");
                    //jesli udalo sie uzyc ...
                }
                else
                {
                    Debug.Log("Nie udalo sie uzyc");
                    //jesli nie udalo sie uzyc ...
                }
                inventoryLogic.OnItemCancel(true);
                
            }
            
        }
    } 
}
