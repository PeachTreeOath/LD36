using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{

    public BuildingStats stats;

    private GameObject barrelObj;
    private GameObject fireObj;
    private Sprite rubbleSpr;
    private bool isAlive = true;

    // Use this for initialization
    void Start()
    {
        barrelObj = Resources.Load<GameObject>("Prefabs/OilBarrel");
        fireObj = Resources.Load<GameObject>("Prefabs/FireParticle");
        rubbleSpr = Resources.Load<Sprite>("Images/Gas_Station_Destroy");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int dmg)
    {
        if (!isAlive)
        {
            return;
        }
        stats.curHealth -= dmg;
        if (stats.curHealth <= 0)
        {
            SpawnBarrel();
            isAlive = false;
        }
    }

    private void SpawnBarrel()
    {
        Instantiate(fireObj, transform.position, fireObj.transform.rotation);
        OilBarrel barrel = ((GameObject)Instantiate(barrelObj, transform.position, Quaternion.identity)).GetComponent<OilBarrel>();
        barrel.SetValue(stats.oilValue);
        GetComponent<SpriteRenderer>().sprite = rubbleSpr;
    }
}
