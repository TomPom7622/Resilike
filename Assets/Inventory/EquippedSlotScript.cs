using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquippedSlotScript : MonoBehaviour
{
    //zajmuje sie logika slotu wyposazenia w ekwipunku

    //skrypt gracza
    private PlayerScript PlayerScript;

    //wyswietlana ikona
    public Image Icon;
    //tablica ikon
    public Sprite[] Sprites;
    //wyswietlana quant ammunicji
    private TMP_Text AmmoCount;

    //obecnie wyekwipowana bron
    public Classes.Item Bron;

    //quant amunicji
    public int ammo;
    //id obecnego przedmiotu
    public int currentItemNumber;

    private void Awake()
    {
        //pobieranie danych
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        Icon = GameObject.FindGameObjectWithTag("EquippedSlotIcon").GetComponent<Image>();
        AmmoCount = GetComponentInChildren<TMP_Text>();

        //ustawianie parametrow poczatkowych
        Icon.enabled = false;
        AmmoCount.enabled = false;
        currentItemNumber = -1;
    }

    
    public void ChangeEquipped(Classes.Item item)
    {
        //zmiana wyekwipowanego przedmiotu
        Bron = item;
        Debug.Log("Changing");
        int itemNumber = item.Id;
        int itemAmmo = item.Quant;
        if (currentItemNumber != itemNumber) 
        { 
            //zmiana ekwipunku jezeli jest inny
            currentItemNumber = itemNumber;
            ammo = itemAmmo;
            Icon.sprite = Sprites[itemNumber];
            Icon.enabled = true;
            AmmoCount.text = ammo.ToString();
            AmmoCount.enabled = true;
            PlayerScript.Equip(item);
            Debug.Log("Changed");
        }
        else 
        {
            //unequip jezeli sa te same
            AmmoCount.enabled = false;
            Icon.enabled = false;
            currentItemNumber = -1;
        }
    }
    public void DecreseAmmo(int amount)
    {
        //zmniejszanie amunicji o wartosc
        ammo-=amount;
        AmmoCount.text = ammo.ToString();
    }
    public void RefreshEquipped(Classes.Item item)
    {
        //odswiezenie slotu
        Debug.Log("Robi sie");
        Bron = item;
        ammo = Bron.Quant;
        AmmoCount.text = ammo.ToString();
    }
}
