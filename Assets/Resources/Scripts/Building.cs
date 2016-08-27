using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{

    public BuildingStats stats;

    private GameObject barrelObj;
    private Sprite rubbleSpr;

    // Use this for initialization
    void Start()
    {
        barrelObj = Resources.Load<GameObject>("Prefabs/OilBarrel");
        rubbleSpr = Resources.Load<Sprite>("Images/Gas_Station_Destroy");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int dmg)
    {
        stats.curHealth -= dmg;
        if (stats.curHealth <= 0)
        {
            SpawnBarrel();
        }
    }

    private void SpawnBarrel()
    {
        OilBarrel barrel = ((GameObject)Instantiate(barrelObj, transform.position, Quaternion.identity)).GetComponent<OilBarrel>();
        barrel.SetValue(stats.oilValue);
        GetComponent<SpriteRenderer>().sprite = rubbleSpr;
    }
}
