using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    //skrypty
    private EventManagerScript eventManagerScript;

    private bool succes;
    private void Awake()
    {
        eventManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManagerScript>();
        succes = true;
    }
    public void Interact(int id) 
    {
        switch(id)
        {
            case 3:
                eventManagerScript.BeginEvent("PowerBoxEvent",0);
                break;
            default:
                succes = false;
                Debug.Log("No interaction found");
                break;
        }
        if (succes)
        {
            //...
        }
    }
}
