using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractScript : MonoBehaviour
{
    //skrypty
    private GameManager gameManager;
    private ActionsManagerScript actionsManagerScript;

    //maska o nazwise Action
    private int layerMaskInteraction = 1 << 9;
    //maska o nazwie Inspect
    private int layerMaskInspection = 1 << 11;
    //maska o nazwie Items
    private int layerMaskPickup = 1 << 10;
    private RaycastHit hit;

    private void Awake()
    {
        gameManager=GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        actionsManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<ActionsManagerScript>();
    }
    private void Start()
    {
        gameManager.EnableInteract();
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("shoot");
        if (Physics.BoxCast(gameObject.transform.position, new Vector3(1, 2, 0), gameObject.transform.forward, out hit, transform.rotation, 3, layerMaskInteraction))
        {
            //akcja
            Debug.Log("action hit");
            Debug.Log(hit.collider.gameObject.tag);
            actionsManagerScript.Iteraction(hit.collider.gameObject);
        }
        else if (Physics.BoxCast(gameObject.transform.position, new Vector3(1, 2, 0), gameObject.transform.forward, out hit, transform.rotation, 3, layerMaskPickup))
        {
            //podnoszenie
            Debug.Log("pickup hit");
            Debug.Log(hit.collider.gameObject.tag);
            actionsManagerScript.Pickup(hit.collider.gameObject);
            
        }
        else if (Physics.BoxCast(gameObject.transform.position, new Vector3(1, 2, 0), gameObject.transform.forward, out hit, transform.rotation, 3, layerMaskInspection))
        {
            //inspekcja
            Debug.Log("inspection hit");
            Debug.Log(hit.collider.gameObject.tag);
            actionsManagerScript.Inspection(hit.collider.gameObject);
        }
    }
    
}
