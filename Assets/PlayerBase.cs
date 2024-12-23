using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    public static PlayerBase instance { get; private set; }

    [Header("Base Stats")]
    [SerializeField] private float health;
    private float usedHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Transform healthbarTransform;
    
    void Start()
    {
        instance = this;
        usedHealth = health;
    }

	private void Update()
	{
        healthbarTransform.LookAt(Camera.main.transform.position);
	}

	/// <summary>
	/// Deal damage to the base
	/// </summary>
	/// <param name="damage">damage dealt</param>
	public void DamageBase(float damage)
    {
		usedHealth -= damage;
        if (usedHealth <= 0)
        {
            GameManager.instance.LoseGame();
        }

        healthSlider.value = usedHealth / health; 
    }
}
