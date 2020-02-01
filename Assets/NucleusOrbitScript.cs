using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NucleusOrbitScript : MonoBehaviour
{
    public float fPullRadius = 4.5f;
    public float fPullForce = 0.1f;
    public float fConnectForce = 1f;

    [SerializeField]
    private NucleusScript parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Pixel")
        {
            // hier im Pixel sagen, dass er innerhalb des Orbits ist
            //collision.gameObject.GetComponent<PIXELSCRIPT>().BOOLVARIABLE = true;
            //Debug.Log(collision.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pixel")
        {
            // hier im Pixel sagen, dass er innerhalb des Orbits ist
            //collision.gameObject.GetComponent<PIXELSCRIPT>().BOOLVARIABLE = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, fPullRadius).Where(c => c.gameObject.tag == "Pixel"))
        {
            //Debug.Log("attracted " + collider.name);
            // calculate direction from target to me
            Vector3 forceDirection = transform.position - collider.transform.position;

            // apply force on target towards me
            if(!collider.gameObject.GetComponent<PixelController>().isConnected)
                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * fPullForce * Time.fixedDeltaTime);
            else 
                collider.gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * fConnectForce * Time.fixedDeltaTime);
        }
    }
}
