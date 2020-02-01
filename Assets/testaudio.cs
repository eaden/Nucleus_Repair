using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Audio;
using UnityEngine;

[RequireComponent(typeof(AudioInterface))]
public class testaudio : MonoBehaviour
{
    private AudioInterface iface;

    public Transform gobject;

    // Start is called before the first frame update
    void Start()
    {
        this.iface = this.gameObject.GetComponent<AudioInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            this.iface.PlaySoundEffect(AudioFiles.Effects.AddPixel, this.gobject, 1);
        }
    }
}
