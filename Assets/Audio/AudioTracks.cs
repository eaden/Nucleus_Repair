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
            NucleusBackground,
        }

        [SerializeField]
        private List<AudioInfo> AudioClips = new List<AudioInfo>(Enum.GetValues(typeof(Tracks)).Length);

        public Dictionary<Tracks, AudioInfo> Clips { get; } = new Dictionary<Tracks, AudioInfo>();

        public void OnEnable()
        {
            foreach (var effect in (Tracks[])Enum.GetValues(typeof(Tracks)))
            {
                this.Clips.Add(effect, this.AudioClips.First(c => c.Clip.name == effect.ToString()));
            }
        }
    }
}