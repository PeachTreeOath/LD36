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
        GameObject parent = GameObject.Find("Background");
        Vector2 size = bgPrefab.GetComponent<SpriteRenderer>().bounds.size;

        float startX = (size.x * colNum * -.5f) + size.x/2;
        float startY = (size.y * rowNum *.5f) - size.y/2;
        float currX = startX;
        float currY = startY;

        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                GameObject bg = ((GameObject)Instantiate(bgPrefab, Vector2.zero, Quaternion.identity));
                bg.transform.position = new Vector2(currX, currY);
                bg.transform.SetParent(parent.transform);
                currX += size.x;
            }
            currY -= size.y;
            currX = startX;
        }
    }
}
