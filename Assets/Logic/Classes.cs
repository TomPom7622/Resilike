using TMPro;
using UnityEngine;

public class Classes : MonoBehaviour
{
    public class Player
    {
        //klasa gracza

        //maksymalne zdrowie gracza
        protected int maxHP;
        //obecne zdrowie gracza
        protected int currentHP;
        //
        protected bool player;

        //wskaznik zycia
        private HealthBarScript healthBar;
        //skrypt gracza
        private PlayerScript playerScript;

        //okreslanie dostepu
        public int CurrentHP
        {
            get { return currentHP; }
            set { currentHP = value; }
        }
        public int MaxHP
        {
            get { return maxHP; }
        }

        //konstruktory
        public Player(int currentHP)
        {
            //uniwersalny
            maxHP = 100;
            this.currentHP = currentHP;
            healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBarScript>();
            player = true;
        }
        public Player()
        {
            //gracz na poczatku
            maxHP = 100;
            currentHP = maxHP;
            healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBarScript>();
            player = true;
        }

        public void TakeDamage(int damage)
        {
            //otrzymanie obrazen
            Debug.Log("TakenDamage!!");
            currentHP -= damage;
            //zaktualizowanie wyswietlacza
            if (player)
                UpdateHealthBar();
            //czy smierc
            if (currentHP <= 0)
                Death();
        }
        protected virtual void Death()
        {
            //smierc
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            playerScript.PlayerDeath();
            Debug.Log("KONIEC GRY");
            //tutaj kod konca gry ...
        }
        public void Heal(int hp)
        {
            //leczenie
            int tmp = currentHP + hp;
            if (tmp > maxHP)
                currentHP = maxHP;
            else
                currentHP = tmp;
            if (player)
                UpdateHealthBar();
            return;
        }
        private void UpdateHealthBar()
        {
            //przekazanie obecnej wartosci zdrowia do wyswietlacza
            healthBar.UpdateHealth(CurrentHP);
        }
    }
    public class Enemy : Player
    {
        //klasa przeciwnika

        //rodzaj
        protected string type;
        //obrazenia jakie zadaje
        protected int maxDamage;
        //zasieg
        protected int range;
        //obiekt przeciwnika
        private GameObject enemy;

        //okreslenia dostepu
        public int MaxDamage
        {
            get { return maxDamage; }
        }
        public int Range
        {
            get { return range; }
        }
        public string Type
        {
            get { return type; }
        }

        //konstruktory
        public Enemy(string type, int maxHP, int currentHP, int maxDamage, int range, GameObject enemy)
        {
            //uniwersalny
            this.type = type;
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.maxDamage = maxDamage;
            this.range = range;
            this.enemy = enemy;
            player = false;
        }
        public Enemy(string type, int maxHP, int maxDamage, int range, GameObject enemy)
        {
            //w pelni zdrowia
            this.type = type;
            this.maxHP = maxHP;
            this.currentHP = maxHP;
            this.maxDamage = maxDamage;
            this.range = range;
            this.enemy = enemy;
            player = false;
        }
        protected override void Death()
        {
            //smierc przeciwnika
            Destroy(enemy);
        }
        public bool Attack()
        {
            //atak
            RaycastHit hit;
            if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit, range, 1 << 8))
            {
                //trafiony
                Debug.DrawRay(enemy.transform.position, enemy.transform.forward * range, Color.yellow);
                hit.collider.gameObject.GetComponent<PlayerScript>().player.TakeDamage(maxDamage);
                return true;
            }
            Debug.DrawRay(enemy.transform.position, enemy.transform.forward * range, Color.red);
            return false;
        }

    }
    [System.Serializable]
    public class Item
    {
        //unikatowy numer id
        public int id;
        //name
        public string name;
        //desc
        public string desc;
        //quant sztuk
        public int quant;
        //maksymalna quant sztuk w jednym stacku
        public int maxQuant;
        //czy bron
        public bool equipable;
        //czy sie stackuje
        public bool stackable;
        //z jakimi itemami da sie polaczyæ
        public int[] combinable;
        //typ
        public string type;
        //obrazenia
        public int maxDamage = 0;

        //okreslenie dostepu
        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        public int Quant
        {
            get { return quant; }
            set
            {
                if (value < maxQuant)
                    quant = value;
                else
                    quant = maxQuant;
            }
        }

        public int MaxQuant
        {
            get { return maxQuant; }
        }

        public bool Equipable
        {
            get { return equipable; }
        }

        public bool Stackable
        {
            get { return stackable; }
        }
        public int[] Combinable
        {
            get { return combinable; }
        }
        public string Type
        {
            get { return type; }
        }
        public int MaxDamage
        {
            get { return maxDamage; }
        }

        //konstruktory
        public Item(int id, string name, string desc, int quant, int maxQuant, bool equipable, bool stackable, int[] Combinable, string type)
        {
            //pelen konstruktor
            this.id = id;
            this.name = name;
            this.desc = desc;
            this.quant = quant;
            this.maxQuant = maxQuant;
            this.equipable = equipable;
            this.stackable = stackable;
            combinable = Combinable;
            this.type = type;
        }
        public Item(int id, string name, string desc, int quant, int maxQuant, bool equipable, bool stackable, string type)
        {
            //pelen konstruktor bez laczenia
            this.id = id;
            this.name = name;
            this.desc = desc;
            this.quant = quant;
            this.maxQuant = maxQuant;
            this.equipable = equipable;
            this.stackable = stackable;
            combinable = null;
            this.type = type;
        }

        public Item(int id, string name, string desc, int quant, int maxQuant, int[] Combinable)
        {
            //konstruktor amunicji
            this.id = id;
            this.name = name;
            this.desc = desc;
            this.quant = quant;
            this.maxQuant = maxQuant;
            equipable = false;
            stackable = true;
            combinable = Combinable;
            type = "ammo";
        }
        public Item(int id, string name, string desc, int quant, int maxQuant, int[] combinable, int maxDamage)
        {
            //konstruktor broni
            this.id = id;
            this.name = name;
            this.desc = desc;
            this.quant = quant;
            this.maxQuant = maxQuant;
            equipable = true;
            stackable = false;
            this.combinable = combinable;
            type = "gun";
            this.maxDamage = maxDamage;
        }
        public Item(int id, string name, string desc, int[] Combinable)
        {
            //key constructor
            this.id = id;
            this.name = name;
            this.desc = desc;
            quant = 1;
            maxQuant = 1;
            equipable = false;
            stackable = false;
            combinable = Combinable;
            type = "key";
        }
    }
    [System.Serializable]
    public class ItemDatabase
    {
        //przechowuje wszystkie przedmioty
        public Item[] items;
    }
    [System.Serializable]
    public class Note
    {
        //klasa notatki

        //id
        public int id;
        //nazwa
        public string name;
        //surowy tekst
        public string text;

        //modyfikatory dostepu
        public int Id
        {
            get { return id; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }
    [System.Serializable]
    public class NoteDatabase
    {
        //przecowuje informacje o wszystkich notatkach
        public Note[] notes;
    }
    public class Ekwipunek
    {
        //ekwipunek gracza

        //tworzenie tablicy predmiotow i adresow slotow
        private Item[] przedmioty = new Item[8] { null, null, null, null, null, null, null, null };
        //przechowuje notatki w ekwipunku gracza
        private Note[] notes = new Note[10] { null, null, null, null, null, null, null, null,null,null };
        [SerializeField]
        private InventorySlot[] sloty = new InventorySlot[8];
        //zmienna pomocnicza do wyswietlanej quanti przedmiotu
        private TMP_Text Numerek;

        //okreslenie dostepu
        public InventorySlot[] Sloty
        {
            get { return sloty; }
            set { sloty = value; }
        }
        public Item[] Przedmioty
        {
            get { return przedmioty; }
        }

        
        public void DodajPrzemiot(Item item)
        {
            //funkcja dodaj¹ca przedmiot
            Debug.Log("Dodawanie");
            for (int i = 0; i < przedmioty.Length; i++)
                if (przedmioty[i] == null)
                {
                    przedmioty[i] = item;
                    Debug.Log(przedmioty[i].Name);
                    break;
                }
        }

        public void ZmienParametry(Item item)
        {
            //zamiana parametrow danego przemiotu w ekwipunku
            for (int i = 0; i < przedmioty.Length; i++)
                if (przedmioty[i] != null && przedmioty[i].Id == item.Id)
                {
                    przedmioty[i] = item;
                    break;
                }
        }

        public void InventoryRefresh()
        {
            //odswierzenie calego ekwipunku
            for (int i = 0; i < przedmioty.Length; i++)
            {
                if (sloty[i] != null && przedmioty[i] != null)
                {
                    Numerek = sloty[i].GetComponentInChildren<TMP_Text>();
                    Numerek.text = przedmioty[i].Quant.ToString();
                }
            }
        }
        public void AddNote(Note note)
        {
            //dodaje notatke
            notes[note.Id] = note;
        }
        public Note GetNote(int id)
        {  
            //oddaje notatke po id
            return notes[id]; 
        }
        public Note GetNote(string name)
        {
            //oddaje notatke po nazwie
            for(int i = 0;i<notes.Length;i++)
                if (notes[i].name == name) 
                    return notes[i];
            return null;
        }
        public void DeleteItem(Item item)
        {
            //usuwanie przedmiotu
            for(int i = 0;i < przedmioty.Length;i++)
            {
                if (przedmioty[i]==item)
                {
                    przedmioty[i] = null;
                    sloty[i].RemoveItem();
                    return;
                }
            }
        }
    }
}
