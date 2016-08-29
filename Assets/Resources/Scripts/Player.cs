using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private OilManager oilManager;
    public static Player instance;
	GameObject healthUI;
	RectTransform healthRect;
	float fullHealthSize;
	public float maxHealth;
	float curHealth;
	float healTimer;
	float healTime = 2f;
	public float healRate;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {



		curHealth = maxHealth;
		healthUI = GameObject.Find("HealthPanel") as GameObject;
		healthRect = healthUI.GetComponent<RectTransform>();
		fullHealthSize = healthRect.sizeDelta.x;
		healTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		if(Time.time - healTimer > healTime)
		{
			healTimer = Time.time;
			curHealth += healRate;
			if(curHealth > maxHealth)
			{
				curHealth = maxHealth;
			}
		}
		healthRect.sizeDelta = new Vector2(fullHealthSize * (curHealth/maxHealth), healthRect.sizeDelta.y);
    }

    public void AddOil(int count)
    {
        OilManager.instance.ChangeOilAmount(count);
    }

	public void TakeDamage(int damage)
	{
        BackgroundManager.instance.PlaySound(2);
        curHealth -= damage;
		if(curHealth <= 0)
		{
			Application.LoadLevel("Lose");
		}
	}
}
