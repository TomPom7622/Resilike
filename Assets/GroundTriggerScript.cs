using UnityEngine;

public class GroundTriggerScript : MonoBehaviour
{
    PlayerScript player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Floor" || other.tag == "Slope")
        {
            player.isOnFloor = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Floor" || other.tag == "Slope")
        {
            player.isOnFloor = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Floor" || other.tag == "Slope")
        {
            player.isOnFloor = true;
        }
    }
}
