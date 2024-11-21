using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public static PlayerBase instance { get; private set; }

    [Header("Base Stats")]
    [SerializeField] private float health;
    
    void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Deal damage to the base
    /// </summary>
    /// <param name="damage">damage dealt</param>
    public void DamageBase(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("I have died now do something about it");
        }
    }
}
