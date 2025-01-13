using UnityEngine;

public class EnemyHP : MonoBehaviour
{

    public int HP = 100;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    void TakeDamage()
    {
        HP -= 5;
    }


    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetMouseButtonDown(0))
        {
            TakeDamage();
        }
    }

}
