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

    [SerializeField] [Range(0.5f,2f)]
    private float fPixelSize = 1f;

    private float tPixelTimer;
    SpriteRenderer sr;

    private void Awake()
    {
        gameObject.tag = "Pixel";
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = cPixelHue;
        gameObject.transform.localScale = new Vector2(fPixelSize, fPixelSize);

        // placeholder
        // Set some properties depending in PixelType if neccesary, before spawn        
    }

    // Start is called before the first frame update
    void Start()
    {
        tPixelTimer = 0f;
        Destroy(gameObject, fTimeToLive);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tPixelTimer += Time.deltaTime;
        var tTemp = (fTimeToLive-1 - tPixelTimer) / (fTimeToLive-1);
        Debug.Log(gameObject.name + ' ' + tTemp);

        sr.color = Color.Lerp(Color.white, cPixelHue, tTemp);

        if (fTimeToLive - tPixelTimer <= 2f)
        {
            gameObject.transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, (fTimeToLive - tPixelTimer / 2));
        }
    }
}
