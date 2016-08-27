using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

    public BuildingStats stats;

    private GameObject barrelObj;

    // Use this for initialization
    void Start () {
        barrelObj = Resources.Load<GameObject>("Prefabs/OilBarrel");
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void TakeDamage(int dmg)
    {
        stats.curHealth -= dmg;
        if(dmg <= 0)
        {
            SpawnBarrel();
        }
    }

    private void SpawnBarrel()
    {
        OilBarrel barrel = Instantiate(barrelObj).GetComponent<OilBarrel>();
        barrel.SetValue(stats.oilValue);
    }
}
