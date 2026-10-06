using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    //uzywanie przedmiotu

    //skrypt gracza
    private PlayerScript playerScript;

    //id przedmiotu, ktorego mozna w tej chwili uzyc
    private int keyItemId = -1;

    private void Awake()
    {
        //pobranie danych
        playerScript = GetComponent<PlayerScript>();
    }
    public bool UseCurrentItem(Classes.Item item)
    {
        //proba uzycia przedmiotu
        if (item.Type == "healing")
        { 
            //leczenie
            playerScript.player.Heal(20);
            Debug.Log("Healed");
            return true;
        }
        else if(item.Type == "key" && keyItemId==item.Id)
        {
            //przedmiot kluczowy
            GameObject.FindGameObjectWithTag("Interactable").GetComponent<InteractableScript>().Interact(item.Id);
            return true;
        }
        //jesli sie nie udalo uzyc ...
        return false;
    }
    public void ChangeItemId(int itemId)
    {
        //zmiana id przedmiotu mozliwego do uzycia
        Debug.Log("Id change to: " + itemId.ToString());
        keyItemId = itemId;
    }
}
