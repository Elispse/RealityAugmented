using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float health;

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(" I have " + health + " health left");
        if (health <= 0)
        {
            death();
        }
    }

    public void death()
    {
		Destroy(gameObject);
	}
}
