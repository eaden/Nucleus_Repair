using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Audio
{
    [CreateAssetMenu(menuName = "AudioSettings/Tracks")]
    public class AudioTracks : ScriptableObject
    {
        public enum Tracks
        {
            Bass,
            Guitar,
            HigherDrums,
            LowerDrums,
        }

        [SerializeField]
        private List<AudioClip> AudioClips = new List<AudioClip>(Enum.GetValues(typeof(Tracks)).Length);

        public Dictionary<Tracks, AudioClip> Clips { get; } = new Dictionary<Tracks, AudioClip>();

        public void OnEnable()
        {
            foreach (var effect in (Tracks[])Enum.GetValues(typeof(Tracks)))
            {
                this.Clips.Add(effect, this.AudioClips.First(c => c.name == effect.ToString()));
            }
        }
    }
}