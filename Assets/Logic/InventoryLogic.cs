using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryLogic : MonoBehaviour
{
    //skrypt ekwipunku

    //skrypty

    //kontroller gry
    private GameManager Manager;
    //skrypt slotu w ekipunku
    private InventorySlot InventorySlotsScripts;
    //informacje o wszystkich przedmiotach
    private ItemManager ItemManager;
    //skrypt przycisku "wyposaz"
    public EquipScript EquipScript;
    //skrypt przycisku "sprawdz"
    public InspectScript InspectScript;
    //skrypt przycisku "polacz"
    public CombineScript CombineScript;
    //slot z wyposazona bronia
    public EquippedSlotScript equippedSlotScript;

    //lista wszystkich slotow
    private GameObject[] InventorySlots;
    //pomocnicza lista wszystkich slotow
    private GameObject[] Tmp;
    //quant danego przedmiotu wyswietlana w ekipunku
    private TMP_Text Numerek;
    //panel wyswietlajacy informacje
    private GameObject inspectionPanel;
    //tekst wyswietlany na panelu
    private TMP_Text inspectText;

    //event system
    private EventSystem EventSystem;
    //menu kontekstowe
    private Canvas ContextMenu;
    //przyisk "wyposaz"
    public GameObject Equip;
    //przycisk "sprawdz"
    public GameObject Inspect;
    //przycisk "polacz"
    public GameObject Combine;
    //tekst na przycisku "wyposaz"
    public TMP_Text EquipText;

    //slot w ekwipunku
    public InventorySlot ItemInventorySlot;
    //drugi slot do laczenia
    public InventorySlot CombineItemInventorySlot;
    //przedmioty do laczenia
    public Classes.Item combineItem1;
    public Classes.Item combineItem2;

    //flagi
    public bool contextMenuOpen;
    private bool combineMode;
    //numery slotow
    private int lastSlotNumber;
    private int secondSlotNumber;

    //stale, ktore dotycza danej postaci
    public static int inventorySzie = 8;

    //deklaracja ekwipunku
    Classes.Ekwipunek ekwipunek = new();

    private void Awake()
    {
        //czarny rytulal znajdujacy obiekty kafelkow w ekwipunku i sortujacy je
        Tmp = GameObject.FindGameObjectsWithTag("InventorySlot");
        InventorySlots = GameObject.FindGameObjectsWithTag("InventorySlot");
        //sortowanie slotow
        for(int i = 0; i < Tmp.Length; i++)
        {
            InventorySlots[int.Parse(Tmp[i].name[13..]) - 1] = Tmp[i];
        }
        Tmp = null;
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            ekwipunek.Sloty[i]= InventorySlots[i].GetComponent<InventorySlot>();
        }
        
        //pobieranie danych
        Manager = GetComponent<GameManager>();
        ItemManager = GetComponent<ItemManager>();
        EventSystem = FindAnyObjectByType<EventSystem>();
        ContextMenu = GameObject.FindGameObjectWithTag("ContextMenu").GetComponent<Canvas>();
        equippedSlotScript= GameObject.FindGameObjectWithTag("EquippedSlot").GetComponent<EquippedSlotScript>();

        Equip = GameObject.FindGameObjectWithTag("Equip/Use");
        Inspect = GameObject.FindGameObjectWithTag("Inspect");
        Combine = GameObject.FindGameObjectWithTag("Combine");

        EquipScript = Equip.GetComponent<EquipScript>();
        InspectScript = Inspect.GetComponent<InspectScript>();
        CombineScript = Combine.GetComponent<CombineScript>();

        EquipText=Equip.GetComponentInChildren<TMP_Text>();
        Numerek = InventorySlots[0].GetComponentInChildren<TMP_Text>();

        inspectionPanel = GameObject.FindGameObjectWithTag("InspectionPanel");
        inspectText = inspectionPanel.GetComponentInChildren<TMP_Text>();
    }
    private void Start()
    {
        //przypisywanie poczatkowych wartosci
        ContextMenu.enabled = false;
        Equip.SetActive(false);
        Inspect.SetActive(false);
        Combine.SetActive(false);
        contextMenuOpen = false;
        combineMode = false;
        inspectText.text = "";
    }


    public void AddItem(int id)
    {
        Debug.Log(id);
        Classes.Item item = ItemManager.getItem(id);
        //funkcja dodajaca przedmiots
        if (item.Stackable)
        {
            //rozpoczynamy stackowanie
            Debug.Log("Stacking");
            //Debug.Log(item.Id);
            item = Stacking(item);
            if (item == null)
                return;
        }
        for (int i = 0; i < inventorySzie; i++)
        {
            if (ekwipunek.Przedmioty[i] == null)
            {
                //znalezienie pustego miejsca
                ekwipunek.Przedmioty[i] = item;
                Debug.Log("Dodano");
                Debug.Log(ekwipunek.Przedmioty[i].Name);
                InventorySlotsScripts = ekwipunek.Sloty[i];
                InventorySlotsScripts.AddItemInv(item);
                if(item.Stackable || item.Equipable)
                {
                    //wysiwetlanie quanti przedmiotu
                    Numerek = InventorySlots[i].GetComponentInChildren<TMP_Text>();
                    Numerek.text = ekwipunek.Przedmioty[i].Quant.ToString();
                    Numerek.enabled = true;
                }
                return;
            }
        }
        //pelny ekwipunek
        Debug.Log("Pelny ekwipunek");
    }

    public void EnableContextMenu(Classes.Item item, int numer)
    {
        //wyswietla menu kontekstowe danego przedmiotiu
        Debug.Log("menu kontekstowe");
        if(combineMode)
        {
            //kombinacja przedmiotów
            secondSlotNumber = numer;     
            Debug.Log("Kombinacja");
            if(combineItem1 != null) 
            {
                //ustawienie parametrow drugiego przedmiotu i slotu
                combineItem2 = item;
                CombineItemInventorySlot = GameObject.Find("InventorySlot " + secondSlotNumber.ToString()).GetComponent<InventorySlot>();
                for (int i = 0; i < combineItem2.Combinable.Length; i++) 
                {
                    //przeszukanie listy laczonych przedmiotow
                    if (combineItem1.Id == combineItem2.Combinable[i])
                    {
                        Debug.Log("Zaczynamy jazde");
                        if(secondSlotNumber>lastSlotNumber)
                            CombineItems(combineItem2, combineItem1);
                        else
                            CombineItems(combineItem1, combineItem2);
                        OnItemCancel();
                        combineMode = false;
                        return;
                    }
                }
                Debug.Log("Nie da sie polaczyc");
                combineMode = false;
            }
            return;
        }
        else 
        { 
            //wlaczanie menu kontekstowego
            lastSlotNumber = numer;
            if (item.Equipable)
                EquipText.text = "Equip";
            else
                EquipText.text = "Use";
            ContextMenu.enabled = true;
            Equip.SetActive(true);
            Inspect.SetActive(true);
            Combine.SetActive(true);
            EquipScript.item = item;
            InspectScript.item = item;
            CombineScript.item = item;
            //dodanie zaznaczenia przycisku "Equip"
            EventSystem.SetSelectedGameObject(Equip);
            contextMenuOpen = true;
        }
    }

    public void OnItemCancel(bool ifevent=false)
    {
        //wylaczanie menu kontekstowego lub ekwipunku
        Debug.Log("Cancelinggg");
        if(contextMenuOpen)
        {
            //wylaczanie menu kontekstowego
            Debug.Log("zamykanie");
            ContextMenu.enabled = false;
            Equip.SetActive(false);
            Inspect.SetActive(false);
            Combine.SetActive(false);
            EquipScript.item = null;
            InspectScript.item= null;
            CombineScript.item = null;
            contextMenuOpen = false;
            //wrocenie koloru slotu i zaznaczenie go
            ItemInventorySlot = GameObject.Find("InventorySlot "+ lastSlotNumber.ToString()).GetComponent<InventorySlot>();
            ItemInventorySlot.Reverse();
            EventSystem.SetSelectedGameObject(GameObject.Find("InventorySlot " + lastSlotNumber.ToString()));
        }
        else
        {
            //wylacza ekwipunek
            Manager.Disable(ifevent);
        }
    }
    public void BeginCombine(Classes.Item item)
    {
        //rozpoczyna laczenie przedmiotow
        Debug.Log("First inventory slot: " + lastSlotNumber);
        combineMode = true;
        //zaznaczenie innego slotu
        if (lastSlotNumber % 2 == 0)
            secondSlotNumber = lastSlotNumber - 1;
        else
            secondSlotNumber= lastSlotNumber + 1;
        Debug.Log("Second inventory slot: " + secondSlotNumber);
        
        EventSystem.SetSelectedGameObject(GameObject.Find("InventorySlot " + secondSlotNumber.ToString()));
        combineItem1 = item;
    }
    public void AmmoDecreseOnFire(Classes.Item weapon, int oIle)
    {
        //zmniejszenie amunicji w ekiwpunku
        weapon.Quant -= oIle;
        ekwipunek.ZmienParametry(weapon);
        ekwipunek.InventoryRefresh();
        equippedSlotScript.DecreseAmmo(oIle);
    }
    
    public void DeleteItem(Classes.Item item)
    {
        //usuwanie przedmiotu z ekwipunku
        ekwipunek.DeleteItem(item);
        Debug.Log("USUWANIE");
    }

    public void OnItemHighlit(GameObject slot)
    {
        //wyswietla nazwe przedmiotu w polu inspekcji
        if (slot.GetComponent<InventorySlot>().GetItem() != null)
            inspectText.text = slot.GetComponent<InventorySlot>().GetItem().Name;
        else
            inspectText.text = "";
    }
    public void InventoryItemRefresh(Classes.Item item)
    {
        //zmienia parametry przedmiotu
        for (int i = 0; i < ekwipunek.Przedmioty.Length; i++)
            if (ekwipunek.Przedmioty[i].Id==item.Id)
            { 
                ekwipunek.Przedmioty[i] = item;
                break;
            } 
    }

    //prywatne
    private Classes.Item Stacking(Classes.Item item)
    {
        //funkcja stackujaca itemy
        //Debug.Log(item.Name);
        int i = 0;
        int q, qn, lim;

        //szukanie itemu
        while (i < inventorySzie)
        {
            if (ekwipunek.Przedmioty[i] != null && ekwipunek.Przedmioty[i].Id == item.Id && ekwipunek.Przedmioty[i].Quant < ekwipunek.Przedmioty[i].MaxQuant)
            {
                //quant stara
                q = ekwipunek.Przedmioty[i].Quant;
                //quant dodawana
                qn = item.Quant;
                //maksymalna quant
                lim = item.MaxQuant;
                q += qn;
                Debug.Log(q);
                Numerek = InventorySlots[i].GetComponentInChildren<TMP_Text>();
                if (q > lim)
                {
                    //maksynmalna quant przedmiotu
                    ekwipunek.Przedmioty[i].Quant = lim;
                    //reszta do obsadzenia
                    qn = q - lim;
                    item.Quant = qn;
                    Debug.Log(lim);
                    Numerek.text = lim.ToString();
                }
                else
                {
                    //polaczenie przedmiotow
                    ekwipunek.Przedmioty[i].Quant = q;
                    Debug.Log(ekwipunek.Przedmioty[i].Quant);
                    Numerek.text = ekwipunek.Przedmioty[i].Quant.ToString();
                    return null;
                }
            }
            i++;
        }
        //jesli nie da sie zestackowac funkcja oddaje przedmiot do dodania
        return item;
    }

    private void CombineItems(Classes.Item item1, Classes.Item item2)
    {
        //laczy przemioty
        //przedmiot 1 jest wyzej w ekwipunku niz przedmiot 2
        Debug.Log("plujac");
        Classes.Item bron = null;
        Classes.Item ammo = null;
        bool przeladowanie = false;
        if (item1.Type == "key" && item2.Type == "key")
        {
            //dwa przemioty kluczowe ...
            return;
        }
        else if (item1.Id == item2.Id && item1.Stackable)
        {
            //stackowanie
            Debug.Log("Mamy to");
            int q = item1.Quant, qn = item2.Quant, lim = item1.MaxQuant;
            q += qn;
            if (q > lim)
            {
                //przeniesienie czesci do przedmiotu 1
                qn = q - lim;
                q = lim;
                item1.Quant = qn;
                item2.Quant = q;
            }
            else
            {
                //przeniesienie calosci
                item1.Quant = q;
                for (int i = 0; i < ekwipunek.Przedmioty.Length; i++)
                {
                    //usuniecie przedmiotu 2
                    if (ekwipunek.Przedmioty[i] == item2)
                    {
                        ekwipunek.Przedmioty[i] = null;
                        InventorySlots[i].GetComponent<InventorySlot>().RemoveItem();
                    }
                }
            }
        }
        else if (item1.Type == "gun" && item2.Type == "ammo")
        {
            //przygotowanie przeladowanie 1
            Debug.Log("sytuacja1");
            przeladowanie = true;
            bron = item1;
            ammo = item2;
        }
        else if (item1.Type == "ammo" && item2.Type == "gun")
        {
            //przygotowanie przeladowanie 2
            Debug.Log("sytuacja2");
            przeladowanie = true;
            bron = item2;
            ammo = item1;
        }
        if (przeladowanie)
        {
            //przeladowanie
            int q = bron.Quant, qn = ammo.Quant, lim = bron.MaxQuant, r;
            if (q != lim)
            {
                Debug.Log("q:" + q);
                Debug.Log("qn:" + qn);
                //ile zostanie po przeladownaiu
                r = qn - (bron.MaxQuant - bron.Quant);
                Debug.Log("r:" + r);
                q += qn;
                if (q > lim)
                {
                    q = lim;
                }
                bron.Quant = q;
                if (r <= 0)
                {
                    //trzeba znalezc i usunac amunicje
                    for (int i = 0; i < ekwipunek.Przedmioty.Length; i++)
                    {
                        if (ekwipunek.Przedmioty[i] == ammo)
                        {
                            ekwipunek.Przedmioty[i] = null;
                            InventorySlots[i].GetComponent<InventorySlot>().RemoveItem();
                        }
                    }
                }
                else
                {
                    ammo.Quant = r;
                }
                for (int i = 0; i < ekwipunek.Przedmioty.Length; i++)
                {
                    //zmiana parametrow
                    if (ekwipunek.Przedmioty[i] == ammo)
                        ekwipunek.Przedmioty[i] = ammo;
                    else if (ekwipunek.Przedmioty[i] == bron)
                        ekwipunek.Przedmioty[i] = bron;
                }
            }
            equippedSlotScript.RefreshEquipped(bron);
        }
        //koniec laczenia
        Debug.Log(lastSlotNumber);
        Debug.Log(secondSlotNumber);
        ekwipunek.InventoryRefresh();
        CombineItemInventorySlot.GetComponent<InventorySlot>().Reverse();
    }
}
