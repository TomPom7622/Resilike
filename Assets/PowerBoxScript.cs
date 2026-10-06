using UnityEngine;

public class PowerBoxScript : MonoBehaviour
{
    private UseItem useItem;
    private void Awake()
    {
        useItem = GameObject.FindGameObjectWithTag("Player").GetComponent<UseItem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("in collider");
            useItem.ChangeItemId(3);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("out of collider");
            useItem.ChangeItemId(-1);
        }
    }
}
