using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{

    public int rowNum;
    public int colNum;
    // Use this for initialization
    void Start()
    {
        CreateBG();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateBG()
    {
        GameObject bgPrefab = Resources.Load<GameObject>("Prefabs/BGTile");
        Vector2 size = bgPrefab.GetComponent<SpriteRenderer>().bounds.size;

        float startX = 2 / 2 * -1.5f;
        float startY = 2 / 2 * -1.5f;
        float currX = startX;
        float currY = startY;
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                GameObject bg = ((GameObject)Instantiate(bgPrefab, Vector2.zero, Quaternion.identity));
            }
        }
    }
}
