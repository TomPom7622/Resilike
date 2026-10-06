using UnityEngine;

public class EventManagerScript : MonoBehaviour
{

    //skrypty
    private TextFieldScript textFieldScript;

    //eventy
    private PowerBoxEventScript powerBoxEventScript;

    private TextAsset eventTextFile;
    private string currentEventTag;
    private int option;
    private string[] eventsTexts;

    
    private void Awake()
    {
        eventTextFile = Resources.Load<TextAsset>("eventTexts");
        eventsTexts = eventTextFile.text.Split("||");
        textFieldScript = GameObject.FindGameObjectWithTag("Textbox").GetComponent<TextFieldScript>();
    }
    public void BeginEvent(string eventName,int id=+-1)
    {
        currentEventTag = eventName;
        switch (eventName)
        {
            case "PowerBoxEvent":
                Debug.Log("event found");
                powerBoxEventScript=GameObject.FindGameObjectWithTag(eventName).GetComponent<PowerBoxEventScript>();
                Debug.Log(eventsTexts[0]);
                powerBoxEventScript.StartEvent(eventsTexts[0]);
                break;
        }
    }
    public void YesOption()
    {
        option = 0;
        OptionHandeler();
    }
    public void NoOption()
    {
        Debug.Log("noOption");
        option = 1;
        OptionHandeler();
    }
    public void ThirdOption()
    {
        option = 2;
        OptionHandeler();
    }
    

    //prywatne
    private void IterateTroughOptions(string currentEventTag, int option)
    {
        switch (currentEventTag)
        {
            case "PowerBoxEvent":
                switch(option)
                {
                    case 0:
                        powerBoxEventScript.ContinueEvent();
                        break;
                    case 1:
                        Debug.Log("No");
                        powerBoxEventScript.AbandonEvent();
                        break;
                }
                option = -1;
                break;
        }
    }
    private void OptionHandeler()
    {
        IterateTroughOptions(currentEventTag, option);
        textFieldScript.MoveOn();
    }
}
