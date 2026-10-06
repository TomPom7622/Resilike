using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    //skrypty
    private EquippedSlotScript equippedSlotScript;

    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private GameObject hitMarker;
    
    private int maskInt = (1 << 3) | (1<<7);
    public int damage;
    private void Awake()
    {
        equippedSlotScript =GameObject.FindGameObjectWithTag("EquippedSlot").GetComponent<EquippedSlotScript>();
    }
    public bool OnFire()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit,Mathf.Infinity,maskInt))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            Instantiate(hitMarker,hit.point,transform.rotation);
            damage=equippedSlotScript.Bron.MaxDamage;
            if(hit.collider.gameObject.layer==7)
                hit.collider.gameObject.GetComponent<enemyAI>().TakeDamage(damage);
            Debug.Log("Did Hit");
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1000, Color.white);
            Debug.Log("Did not Hit");
            return false;
        }
    }
}
