using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PixelType
{
    basic,
    speed,
    inertia,
    sinister
}

public class PixelController : MonoBehaviour
{
    //Tag pixel

    [SerializeField]
    private PixelType EPixelType = PixelType.basic;

    [SerializeField]
    private float fTimeToLive = 20f;

    [SerializeField]
    private Color cPixelHue = Color.white;

    [SerializeField] [Range(0.25f,1.5f)]
    private float fPixelSize = 1f;

    private float tPixelTimer;
    SpriteRenderer sr;
    public bool isConnected = false;
    private bool isActive = false;

    private void Awake()
    {
        
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = cPixelHue;
        gameObject.transform.localScale *= new Vector2(fPixelSize, fPixelSize);

        // placeholder
        // Set some properties depending in PixelType if neccesary, before spawn        
    }

    // Start is called before the first frame update
    void Start()
    {
        tPixelTimer = 0f;
        gameObject.tag = "Pixel";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isConnected)
        {
            HandlePixeLive();
        }
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

        sr.color = Color.Lerp(Color.white, cPixelHue, tTemp);

        float fDeadCD = 0.2f;
        if (tTemp <= fDeadCD)
        {
            Debug.Log(gameObject.name + " " +tTemp);
            gameObject.transform.localScale = Vector2.Lerp(Vector2.zero,transform.localScale, tTemp*10);
        }
    }

    public void Rotate(Transform center, Vector3 axis, float rotationSpeed, float radius, float radiusSpeed)
    {
        transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
        var desiredPosition = (transform.position - center.position).normalized * radius + center.position;
        transform.position = Vector2.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }
}
