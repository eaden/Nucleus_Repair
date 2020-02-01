using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private float fTimeToLive_current = 0;
    public float FTimeToLive_current
    {
        get { return fTimeToLive_current; }
        set
        {
            if (value > fTimeToLive)
                value = fTimeToLive;
            fTimeToLive = value;
        }
    }

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
    public bool isInOrbit = false;
    private bool isActive = false;
    //private Dictionary<PixelColors, Color> Color_Dic ; // ausgelagert in GameManager

    private NucleusScript nucleus;
    private NucleusOrbitScript nucleusOrbit;
    private float nucleusOrbitRadius = 0.0f;
    // colorsettings
    // green
    private float pullRadius_greenPixel = 15f;
    private float addLiveTime_greenPixel = 10f;

    // red
    private float pullRadius_redPixel = 10f;
    private float subtractLiveTime_redPixel = 20f;

    // cyan
    private bool gaveSpeedBoost_cyanPixel = false;
    private float addSpeed_cyanPixel = 10f;

    // pink
    private Vector3 rotate_axis_pinkPixel = new Vector3(0, 0, 1);
    private float rotate_speed_pinkPixel = 1;
    private float distance_pinkPixel = 25;
    private float mass_pink = 0.0004f;

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
            HandleConnectedPixelCharacteristics();
        }
        
        if (Vector3.Distance(transform.position, nucleus.transform.position) > nucleusOrbitRadius)
            isInOrbit = false;
        
        // in Orbit or not in Orbit

        if (isInOrbit)
        {
            HandleOrbit();
        }
        else // Drag-Quick-And-Dirty
        {
            if (RB.velocity.magnitude > 0.1f)
                RB.velocity *= 0.96f;
        }

    }

    private void AdjustBaseSettings()
    {
        cPixelHue = GameManager.Color_Dic[EPixelColor];
        tPixelTimer = 0f;
        fTimeToLive_current = fTimeToLive;
        gameObject.tag = "Pixel";
        renderer.material.SetColor("_MainColor", cPixelHue);
        RB.mass = baseMass + (baseMass * massInertia * (fPixelSize - 1));
        cPixelTransparent = new Color(cPixelHue.r, cPixelHue.g, cPixelHue.b, 0);
        nucleus = GameObject.Find("Nucleus").gameObject.GetComponent<NucleusScript>();
        if (nucleus == null)
            Debug.Log("Nucleus is not called Nucleus in this scene! ABORT!");
        nucleusOrbit = GameObject.Find("NucleusOrbit").gameObject.GetComponent<NucleusOrbitScript>();
        if (nucleusOrbit == null)
            Debug.Log("NucleusOrbit is not called NucleusOrbit in this scene! ABORT!");
        nucleusOrbitRadius = nucleusOrbit.fPullRadius+1;
        distance_pinkPixel += Random.Range(-5, 6);
        if (EPixelColor == PixelColors.pink)
            RB.mass = mass_pink;
    }

    
    private void HandlePixeLive()
    {
        if (!isActive)
        {
            //ToDo: activate on LIFE!!
            //Destroy(gameObject, fTimeToLive); 
            isActive = true;
        }

        // put down here, so we are able to resert fTimeToLive
        if (isActive)
            fTimeToLive_current -= Time.fixedDeltaTime;
        if (fTimeToLive_current < 0)
            HandleDeath();

        tPixelTimer += Time.fixedDeltaTime;
        // var tTemp = (fTimeToLive - 1 - tPixelTimer) / (fTimeToLive - 1);
        var tTemp = (fTimeToLive_current - 1) / (fTimeToLive - 1);
        //Debug.Log(gameObject.name + ' ' + tTemp);

        renderer.material.SetColor("_MainColor", Color.Lerp(Color.white, cPixelHue, tTemp));
   
        // ToDo This needs rework. The graphic is done but the object is only destroyed seconds later
        float fDeadCD = 0.25f;
        if (tTemp <= fDeadCD)
        {
            //Debug.Log(gameObject.name + " " +tTemp*10);
            TriggerDeath();
        }
    }
    

    private void HandleConnectedPixelCharacteristics()
    {
        switch (EPixelColor)
        {
            case PixelColors.pink:
                break;
            case PixelColors.cyan:
                if(!gaveSpeedBoost_cyanPixel)
                {
                    nucleus.Speed += addSpeed_cyanPixel;
                    gaveSpeedBoost_cyanPixel = true;
                }
                break;
            case PixelColors.green:
                break;
            case PixelColors.blue:
                break;
            case PixelColors.red:
                break;
            case PixelColors.gold:
                break;
            case PixelColors.orange:
                break;
            default:
                break;
        }
    }
    private void HandleOrbit()
    {
        switch (EPixelColor)
        {
            case PixelColors.pink:
                // this makes it so that a certain distance radius is always kept
                if(Vector3.Distance(nucleus.transform.position, transform.position)< distance_pinkPixel)
                {
                    transform.position = (transform.position - nucleus.transform.position).normalized * distance_pinkPixel + nucleus.transform.position;
                }
                //transform.RotateAround(nucleus.transform.position, rotate_axis_pinkPixel, rotate_speed_pinkPixel * Time.fixedDeltaTime);
                break;
            case PixelColors.cyan:
                //print(Vector3.Distance(transform.position, nucleus.transform.position) + " " + gameObject.name);
                break;
            case PixelColors.green:
                break;
            case PixelColors.blue:
                break;
            case PixelColors.red:
                break;
            case PixelColors.gold:
                break;
            case PixelColors.orange:
                break;
            default:
                Debug.Log("There are Pixels without set colors. Name of pixel is " + gameObject.name);
                break;
        }
    }

    private void HandleDeath()
    {
        switch (EPixelColor)
        {
            case PixelColors.pink:
                Destroy(gameObject);
                break;
            case PixelColors.cyan:
                if(isConnected)
                    nucleus.Speed -= addSpeed_cyanPixel;
                Destroy(gameObject);
                break;
            case PixelColors.green:
                foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, pullRadius_greenPixel).Where(c => c != this && c.gameObject.tag == "Pixel"))
                {
                    collider.gameObject.GetComponent<PixelController>().FTimeToLive_current += addLiveTime_greenPixel;
                }
                Destroy(gameObject);
                break;
            case PixelColors.blue:
                Destroy(gameObject);
                break;
            case PixelColors.red:
                foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, pullRadius_redPixel).Where(c => c != this && c.gameObject.tag == "Pixel"))
                {
                    collider.gameObject.GetComponent<PixelController>().FTimeToLive_current -= subtractLiveTime_redPixel;
                }
                Destroy(gameObject);
                break;
            case PixelColors.gold:
                Destroy(gameObject);
                break;
            case PixelColors.orange:
                Destroy(gameObject);
                break;
            default:
                Debug.Log("There are Pixels without set colors. Name of pixel is " + gameObject.name);
                break;
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
