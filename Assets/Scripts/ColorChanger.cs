using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem PSystem;

    private NucleusScript NScript;

    //TODO: change color of the lightning

    // Start is called before the first frame update
    void Start()
    {
        NScript = gameObject.GetComponentInParent<NucleusScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
