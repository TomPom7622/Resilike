using UnityEngine;
using UnityEngine.UIElements;

public class AimScript : MonoBehaviour
{
    private bool aiming = false;
    private int layerBox = 1 << 7;
    private int layerLine = 1 << 3;
    private RaycastHit hitBox;
    private RaycastHit hitLine;
    private Transform destination;
    private Vector3 relativePosition;
    private Quaternion rotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiming = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(aiming)
            AimLock();
    }

    public void EnableAim()
    {
        aiming = true;
    }
    public void DisableAim() 
    {
        aiming = false; 
    }

    //prywatne
    private void AimLock()
    {
        Debug.Log("Odpalone");
        if (Physics.BoxCast(gameObject.transform.position, new Vector3(5, 10, 0), gameObject.transform.forward, out hitBox, transform.rotation, 20, layerBox))
        {
            if (Physics.Linecast(gameObject.transform.position, hitBox.collider.gameObject.transform.position, out hitLine, layerLine))
            {
                Debug.Log("Sciana");
                Debug.DrawLine(gameObject.transform.position, hitLine.collider.gameObject.transform.position);
                //nie ma lini strza³u
                aiming = false;
            }
            else
            {
                Debug.Log("cel");
                Debug.DrawLine(gameObject.transform.position, hitBox.collider.gameObject.transform.position);
                destination = hitBox.collider.transform;
                relativePosition = destination.position - transform.position;
                relativePosition.y = 0;
                rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
                transform.rotation = rotation;
                aiming = true;
            }
        }

    }
}
