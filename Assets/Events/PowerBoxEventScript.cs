using UnityEngine;
using UnityEngine.Playables;

public class PowerBoxEventScript : MonoBehaviour
{
    //Skrypt od eventu z bezpiecznikami

    //skrypty
    //kamery
    private CameraLogic cameraLogic;
    //player
    private PlayerScript playerScript;
    //wyswietlanie tekstu
    private TextFieldScript textFieldScript;
    //game manager
    private GameManager gameManager;
    //kontroler animacji
    private TimelineControllerScript timelineController;

    //animacja wejscia
    private PlayableDirector playableDirector;
    //animacja wejscia przy otwartym
    private PlayableDirector playableDirectorOpen;
    //animacja opcji "tak"
    private PlayableDirector playableDirectorContinue;
    //animacja opcji "nie"
    private PlayableDirector playableDirectorAbandon;
    //power box object
    private GameObject powerBox;
    //tekst boxa
    private string text;
    //czy juz otwarte
    private bool open;
    //textFieldScript.ShowText(eventsTexts[id]);
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        textFieldScript = GameObject.FindGameObjectWithTag("Textbox").GetComponent<TextFieldScript>();  
        timelineController = GameObject.FindGameObjectWithTag("CutsceneManager").GetComponent<TimelineControllerScript>();  
        cameraLogic = GameObject.FindGameObjectWithTag("GameController").GetComponent<CameraLogic>();
        powerBox = GameObject.FindGameObjectWithTag("PowerBox");
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        open = false;
    }
    private void Start()
    {
        //jesli chcesz dodac, trzeba dodac w Unity kolejny element listy timelineController
        // nie zapomniec o dodaniu warunkow stopped i played

        playableDirector = timelineController.GiveDirector(0);
        playableDirectorContinue = timelineController.GiveDirector(1);
        playableDirectorAbandon = timelineController.GiveDirector(2);
        playableDirectorOpen= timelineController.GiveDirector(3);
        playableDirectorOpen.stopped += OnPowerBoxEnd;
        playableDirector.stopped += OnPowerBoxEnd;
        playableDirectorAbandon.stopped += OnPowerBoxAbandon;
        playableDirectorContinue.stopped += EventEnd;
        playableDirector.played += CutsceneStart;
        playableDirectorOpen.played += CutsceneStart;
        playableDirectorContinue.played += CutsceneStart;
        playableDirectorAbandon.played += CutsceneStart;
    }
    public void StartEvent(string text ="")
    {
        //rozpoczyna event
        Debug.Log("event start");
        this.text = text;
        cameraLogic.CurrentCameraOnOrOf();
        PlayerInputOff();
        if (!open)
        { 
            playableDirector.Play();
            open = true;
            powerBox.layer = 9;
        }
        else
            playableDirectorOpen.Play();
    }
    public void ContinueEvent()
    {
        //kontroluje co sie dzieje gdy gracz odpowie tak
        Debug.Log("Continuing event");
        playableDirectorContinue.Play();
        Vector3 val = playerScript.GetCoordinates();
        val.x += 124;
        playerScript.SetCoordinates(val);
    }
    public void AbandonEvent()
    {
        //co sie dzieje gdy gracz odpowie nie
        Debug.Log("Abandoning event");
        playableDirectorAbandon.Play();
    }

    //prywatne
    private void EventShowText()
    {
        //pokazuje tekst uystwaiony w event start
        textFieldScript.ShowText(text, true);
        Debug.Log("koniec");
    }
    private void OnPowerBoxEnd(PlayableDirector director)
    {
        //wlacza tekst
        EventShowText();
    }
    private void OnPowerBoxAbandon(PlayableDirector director)
    {
        //odwraca zmiany w inpucie gracza
        PlayerInputOn();
    }
    private void CutsceneStart(PlayableDirector director)
    {
        //kontroluje inputy gracza gdy zaczyna sie cutscenka
        PlayerInputOff();
    }
    private void PlayerInputOff()
    {
        gameManager.DisableInteract();
        gameManager.InventoryOptionDisable();
        gameManager.InventoryMovementDisable();
        gameManager.DisableMovement();
        gameManager.DisableInteract();
    }
    private void PlayerInputOn() 
    {
        cameraLogic.CurrentCameraOnOrOf();
        gameManager.EnableInteract();
        textFieldScript.EventEnd();
        gameManager.EventEndController();
    }
    private void EventEnd(PlayableDirector director)
    {
        cameraLogic.cameraChange("8");
        cameraLogic.CurrentCameraOnOrOf();
        PlayerInputOn();
    }
}
