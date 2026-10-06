using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    private Classes.Enemy enemy;
    public Transform playerPosition;
    private NavMeshAgent thisAgent;

    private int timer=0;
    private readonly int maxTime = 600;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = new("zombie", 50, 20, 2, this.gameObject);
        thisAgent=GetComponent<NavMeshAgent>();
        playerPosition= GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        thisAgent.destination = playerPosition.position;
        if (timer == maxTime)
            timer = 0;
        if (timer == 0)
        {
            if (enemy.Attack())
            {
                timer++;
            }
        }
        else
            timer++;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage!!");
        enemy.TakeDamage(damage);
    }


}
