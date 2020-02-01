
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PixelSpawner : MonoBehaviour
{
    [SerializeField]
    private int iSpawnFrequency = 1;

    [SerializeField][Range(0.1f, 1f)]
    private float fSpawnProbability = 1f;

    [SerializeField]
    private int iAnfangsSpawn = 10;

    [SerializeField]
    PixelColors col1;
    [SerializeField]
    PixelColors col2;
    [SerializeField]
    PixelColors col3;

    [SerializeField]
    float weight1;
    [SerializeField]
    float weight2;
    [SerializeField]
    float weight3;

    [SerializeField]
    private RectTransform Area;

    [SerializeField]
    private GameObject PixelTemplate;

    float minx,maxx,miny,maxy;

    private bool hasSpawned = false;


    // Start is called before the first frame update
    void Start()
    {
        minx = Area.position.x;
        miny = Area.position.y;
        maxx = Area.position.x + Area.rect.width;
        maxy = Area.position.y + Area.rect.height;

        for (int i = 1; i < iAnfangsSpawn; i++)
        {
            ChoosePixel();
        }

        //Debug.Log(minx + " " + maxx + " " + miny + " " + maxy);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((int)Time.time % (iSpawnFrequency+1) == iSpawnFrequency)
        {
            if (!hasSpawned && Random.Range(0.0f, 1.0f) <= fSpawnProbability)
            {
                ChoosePixel();                
            }
                
            hasSpawned = true;
        }
        else
            hasSpawned = false;
    }

    public void ChoosePixel()
    {
        
        UnityEngine.Random.InitState(UnityEngine.Random.Range(0, 317));
        var r = UnityEngine.Random.Range(0.0f, 1.0f);
        float ges = weight1 + weight2 + weight3;

        if (r <=weight1 / ges)
            SpawnPixel(col1);
        else if (r <= (weight1 + weight2) / ges)
            SpawnPixel(col2);
        else if (r > (weight1 + weight2) / ges)
            SpawnPixel(col3);
        else
            Debug.Log("Niete");
    }

    private void SpawnPixel(PixelColors pc)
    {
        var x = UnityEngine.Random.Range(minx, maxx);
        var y = UnityEngine.Random.Range(miny, maxy);

        GameObject GO = Instantiate(PixelTemplate);
        PixelController pctrl = GO.GetComponent<PixelController>();

        GO.GetComponent<PixelController>().AdjustSettings2(pc, 1);
        GO.transform.position = new Vector2(x, y);

    }
}
