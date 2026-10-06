using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //skrypt slotu w ekwipunku

    //skrypt ekwipunku
    private InventoryLogic inventoryLogic;

    [SerializeField]
    //zawiera listê ikon
    public Sprite[] sprites;
    //ikona slotu
    public Image icon;
    //przycisk slotu
    private Button Button;
    

    //przedmiot slotu
    private Classes.Item item;

    //domyslny kolor slotu
    private Color baseColour;

    private void Awake()
    {
        //pobieranie danych
        inventoryLogic = GameObject.FindGameObjectWithTag("GameController").GetComponent<InventoryLogic>();
        Button=GetComponent<Button>();
        //ustawienie domyslnego koloru
        baseColour = Button.colors.normalColor;
    }

    
    public void AddItemInv(Classes.Item itemAdd)
    {
        //funkcja wizualnego dodawania itemow
        //Debug.Log(itemAdd.Id);
        item = itemAdd;
        icon.sprite = sprites[itemAdd.Id];
        icon.enabled = true;
    }
    public void RemoveItem()
    {
        //usuwanie przedmiotu
        item=null;
        icon.sprite=null;
        icon.enabled = false;
    }
    
    public void OnItemClick()
    {
        //klikniecie przedmiotu
        Debug.Log("Clicked");
        if (item != null)
        {
            Debug.Log("item != null");
            //stale zaznaczenie obecnego slotu
            var colour = Button.colors;
            colour.normalColor = colour.selectedColor;
            Button.colors = colour;
            //wlaczenie menu kontekstowego
            inventoryLogic.EnableContextMenu(item, Int32.Parse(gameObject.name.Substring(gameObject.name.Length-2)));
            Debug.Log(gameObject.name.Substring(gameObject.name.Length - 2));
        }
    }

    public void Reverse()
    {
        //powrot do normalnego koloru
        Debug.Log("Reverse");
        var colour = Button.colors;
        colour.normalColor = baseColour;
        Button.colors = colour;
    }
    public Classes.Item GetItem()
    { return item; }

}
