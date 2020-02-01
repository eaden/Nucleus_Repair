using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PixelType
{
    basic,
    speed,
    orbit,
    sinister,
    gold
}

public class PixelController : MonoBehaviour
{
    //Tag pixel

    [SerializeField]
    private PixelType EPixelType = PixelType.basic;

    [SerializeField]
    private float fTimeToLive = 20f;

    [SerializeField]
    private PixelColors EPixelColor = PixelColors.green;
    private Color cPixelHue;
    private Color cPixelTransparent;

    [SerializeField] [Range(0.25f,1.5f)]
    private float fPixelSize = 1f;

    [SerializeField]
    private float massInertia = 3.3f;

    private float baseMass = 0.0001f;

    private float fDissolveAmount = 0;
    private bool isDissolving;

    private float tPixelTimer;
    Renderer renderer;
    Rigidbody2D RB;
    public bool isConnected = false;
    private bool isActive = false;
    //private Dictionary<PixelColors, Color> Color_Dic ; // ausgelagert in GameManager

    private void Awake()
    {
        
        renderer = gameObject.GetComponent<Renderer>();
        RB = gameObject.GetComponent<Rigidbody2D>();
        gameObject.transform.localScale *= new Vector2(fPixelSize/2, fPixelSize/2);

        // placeholder
        // Set some properties depending in PixelType if neccesary, before spawn        
    }

    // Start is called before the first frame update
    void Start()
    {
        //CreateColorDic(); // ausgelagert in GameManager
        AdjustBaseSettings();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isConnected)
        {
            HandlePixeLive();
        }
    }

    private void AdjustBaseSettings()
    {
        cPixelHue = GameManager.Color_Dic[EPixelColor];
        tPixelTimer = 0f;
        gameObject.tag = "Pixel";
        renderer.material.SetColor("_MainColor", cPixelHue);
        RB.mass = baseMass + (baseMass * massInertia * (fPixelSize - 1));
        cPixelTransparent = new Color(cPixelHue.r, cPixelHue.g, cPixelHue.b, 0);
    }

    
    private void HandlePixeLive()
    {
        if (!isActive)
        {
            //ToDo: activate on LIFE!!
            Destroy(gameObject, fTimeToLive); 
            isActive = true;
        }
        tPixelTimer += Time.fixedDeltaTime;
        var tTemp = (fTimeToLive - 1 - tPixelTimer) / (fTimeToLive - 1);
        //Debug.Log(gameObject.name + ' ' + tTemp);

        renderer.material.SetColor("_MainColor", Color.Lerp(Color.white, cPixelHue, tTemp));
   

        float fDeadCD = 0.25f;
        if (tTemp <= fDeadCD)
        {
            //Debug.Log(gameObject.name + " " +tTemp*10);
            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        gameObject.transform.localScale = Vector2.Lerp(Vector2.zero, transform.localScale, 2);
        fDissolveAmount = Mathf.Clamp01(fDissolveAmount + Time.deltaTime);
        renderer.material.SetFloat("_DissolveAmount", fDissolveAmount);
    }

    public void Rotate(Transform center, Vector3 axis, float rotationSpeed, float radius, float radiusSpeed)
    {
        transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
        var desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        transform.position = Vector2.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }

    public PixelColors getColor()
    {
        return EPixelColor;
    }

    public float getTTL()
    {
        return fTimeToLive;
    }

    public PixelType getType()
    {
        return EPixelType;
    }


    /*
         private void CreateColorDic()
    {
        Color_Dic = new Dictionary<PixelColors, Color>();
        Color_Dic.Add(PixelColors.blue, Color.blue);
        Color_Dic.Add(PixelColors.cyan, Color.cyan);
        Color_Dic.Add(PixelColors.green, Color.green);
        Color_Dic.Add(PixelColors.pink, Color.magenta);
        Color_Dic.Add(PixelColors.red, Color.red);
        Color_Dic.Add(PixelColors.gold, new Color(0.95f, 0.75f, 0));
    }
     */

    public void AdjustSettings2(PixelColors pc, float size)
    {
        Debug.Log("vorher " +EPixelColor);
        cPixelHue = GameManager.Color_Dic[pc];
        EPixelColor = pc;
        Debug.Log("nacher" +EPixelColor);
        tPixelTimer = 0f;
        //gameObject.tag = "Pixel";
        renderer.material.SetColor("_MainColor", cPixelHue);
        //renderer.material.SetColor("_MainColor", cPixelHue);
        RB.mass = baseMass + (baseMass * massInertia * (size - 1));
        cPixelTransparent = new Color(cPixelHue.r, cPixelHue.g, cPixelHue.b, 0);
    }

}
