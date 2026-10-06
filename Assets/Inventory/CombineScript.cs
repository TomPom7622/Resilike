using UnityEngine;

public class CombineScript : MonoBehaviour
{
    //rozpoczynanie laczenia

    //skrypty
    //przedmiot do laczenia
    public Classes.Item item;
    //skrypt ekwipunku
    public InventoryLogic InventoryLogic;

    public void Awake()
    {
        InventoryLogic = GameObject.FindGameObjectWithTag("GameController").GetComponent<InventoryLogic>();
    }
    public void OnClick()
    {
        //wyslanie przedmiotu do skryptu ekwipunku
        if(item != null) 
        {
            InventoryLogic.BeginCombine(item);
        }
    }
}
