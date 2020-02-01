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

    public bool b = false;
    public bool g = false;

    // Start is called before the first frame update
    void Start()
    {
        this.iface = this.gameObject.GetComponent<AudioInterface>();
        this.iface.AddTrack(AudioTracks.Tracks.HigherDrums);
        this.iface.AddTrack(AudioTracks.Tracks.LowerDrums);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            this.iface.PlaySoundEffect(AudioEffects.Effects.AddPixel, this.gobject, 1, false, AudioInterface.BeatType.Full);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            this.iface.PlaySoundEffect(AudioEffects.Effects.AddPixel, null, 1, true, AudioInterface.BeatType.Quarter);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (this.b)
            {
                this.iface.RemoveTrack(AudioTracks.Tracks.Bass);
            }
            else
            {
                this.iface.AddTrack(AudioTracks.Tracks.Bass);
            }

            this.b = !this.b;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (this.g)
            {
                this.iface.RemoveTrack(AudioTracks.Tracks.Guitar);
            }
            else
            {
                this.iface.AddTrack(AudioTracks.Tracks.Guitar);
            }

            this.g = !this.g;
        }


    }
}
