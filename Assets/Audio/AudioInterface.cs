using System.Collections.Generic;
using Assets.Audio;
using UnityEngine;
using UnityEngine.Audio;

public class AudioInterface : MonoBehaviour
{
    public AudioFiles _effects;

    private double _nextEventTime;
    private readonly Dictionary<AudioSource, double> _sources = new Dictionary<AudioSource, double>();

    public float Bpm = 140.0f;
    public int NumBeatsPerSegment = 1;

    private double TimeBetweenEvents => 60.0f / this.Bpm * this.NumBeatsPerSegment;

    private void Start()
    {
        this._nextEventTime = AudioSettings.dspTime + 2.0f;
    }

    private void Update()
    {
        var time = AudioSettings.dspTime;

        if (time + 0.1f > this._nextEventTime)
        {
            this._nextEventTime += this.TimeBetweenEvents;
        }
    }

    /// <summary>
    ///     Plays a sound effect. Optional at a destination. Optional with a delay in beats on background music
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="destination"></param>
    /// <param name="delay">instant if zero, otherwise at next full ticks</param>
    /// <returns>Returns the time until the sound is played</returns>
    public double PlaySoundEffect(AudioFiles.Effects effect, Transform destination = null, byte delay = 0)
    {
        AudioSource source = null;
        foreach (var s in this._sources)
        {
            if (s.Value < AudioSettings.dspTime)
                source = s.Key;
        }

        if (source == null)
        {
            source = new GameObject("Sound Effect").AddComponent<AudioSource>();
            this._sources.Add(source, double.MaxValue);
        }

        var clip = this._effects.Clips[effect];
        source.clip = clip;

        if (destination != null)
        {
            source.transform.parent = destination;
            source.spatialBlend = 1;
        }
        else
        {
            source.spatialBlend = 0;
        }

        if (delay != 0)
        {
            delay--;
            var timeUntilPlayed = this._nextEventTime - AudioSettings.dspTime + delay * this.TimeBetweenEvents;
            var schedule = timeUntilPlayed + AudioSettings.dspTime;
            source.PlayScheduled(schedule);

            this._sources[source] = schedule + source.clip.length;

            return timeUntilPlayed;
        }

        source.Play();
        this._sources[source] = source.clip.length + AudioSettings.dspTime;

        return 0;
    }
}