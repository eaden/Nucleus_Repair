using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class NucleusScript : MonoBehaviour
{
    Rigidbody2D rigid;

    [SerializeField]
    private NucleusStats Stats;

    private float maxSpeed = 150;
    [SerializeField]
    private float speed = 5;
    public float Speed
    {
        get { return speed; }
        set
        {
            if (value > maxSpeed)
                value = maxSpeed;
            speed = value;
        }
    }

    [SerializeField]
    private float fRotationSpeed = 300f;
    [SerializeField]
    private float fConnectionRadius = 1.2f;
    [SerializeField]
    private float fRadiusSpeed = 1f;

    [SerializeField]
    private GameObject GoldenPixel;

    public bool isKeyDown = false;

    private void Awake()
    {
        GameManager._Initiate();
        GameManager.CreateColorDic();
        Stats = ScriptableObject.CreateInstance<NucleusStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        Spawn(7);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.velocity = new Vector2();
        if (Input.GetKey(KeyCode.UpArrow))
            rigid.velocity = new Vector2(rigid.velocity.x, speed);
        if (Input.GetKey(KeyCode.DownArrow))
            rigid.velocity = new Vector2(rigid.velocity.x, -speed);
        if (Input.GetKey(KeyCode.LeftArrow))
            rigid.velocity = new Vector2(-speed, rigid.velocity.y);
        if (Input.GetKey(KeyCode.RightArrow))
            rigid.velocity = new Vector2(speed, rigid.velocity.y);

        if (Input.GetKey(KeyCode.B))
        {
            isKeyDown = true;
            foreach (GameObject pixel in GameObject.FindGameObjectsWithTag("Pixel").Where(c => c.gameObject.GetComponent<PixelController>().isConnected))
            {
                pixel.GetComponent<PixelController>().Rotate(transform, new Vector3(0, 0, 1), fRotationSpeed, fConnectionRadius, fRadiusSpeed);
            }
        }

        if (Input.GetKey(KeyCode.M))
        {
            isKeyDown = true;
            foreach (GameObject pixel in GameObject.FindGameObjectsWithTag("Pixel").Where(c => c.gameObject.GetComponent<PixelController>().isConnected))
            {
                pixel.GetComponent<PixelController>().Rotate(transform, new Vector3(0, 0, 1), -fRotationSpeed, fConnectionRadius, fRadiusSpeed);
            }
        }

        if (Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.B))
            isKeyDown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pixel" && !collision.gameObject.GetComponent<PixelController>().isConnected)
        {
            collision.gameObject.GetComponent<PixelController>().isConnected = true;

            UpdateStatistics(collision);
        }
    }

    private void UpdateStatistics(Collision2D collision)
    {
        PixelColors cc = collision.gameObject.GetComponent<PixelController>().getColor();
        //Debug.Log(cc);
        //Debug.Log(Stats);
        Stats.Colors.Add(cc);
        Stats._stats.Add(new Statistic { EPixColor = cc, iCount = 1, fTTL = collision.gameObject.GetComponent<PixelController>().getTTL(), ftimeStamp = Time.time, });
        if (collision.gameObject.GetComponent<PixelController>().getType() == PixelType.speed)
            Stats.fXtraSpeed++;

        List<string> output = new List<string>();
        foreach (var e in Stats._stats)
        {
            output.Add(e.EPixColor + "$"
                   + e.iCount + "$"
                   + e.fTTL + "$"
                   + e.ftimeStamp + ";")
                    ;
        }

        GameManager.WriteData(output, output.Count);
    }

    private void Spawn(int number)
    {
        for (int i = 1; i <= number; i++)
        {
            GameObject go = Instantiate(GoldenPixel);
            go.transform.position = new Vector2(transform.position.x + i / 2, transform.position.y + i/2);
            Vector3 forceDirection = transform.position - go.transform.position;
            go.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * 50f * Time.fixedDeltaTime);
        }
    }
}
