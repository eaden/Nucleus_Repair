using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Audio
{
    [CreateAssetMenu(menuName = "AudioSettings/Effects")]
    public class AudioEffects : ScriptableObject
    {
        public enum Effects
        {
            AddPixel
        }

        [SerializeField]
        private List<AudioClip> AudioClips = new List<AudioClip>(Enum.GetValues(typeof(Effects)).Length);

        public Dictionary<Effects, AudioClip> Clips { get; } = new Dictionary<Effects, AudioClip>();

        public void OnEnable()
        {
            foreach (var effect in (Effects[]) Enum.GetValues(typeof(Effects)))
            {
                this.Clips.Add(effect, this.AudioClips.First(c => c.name == effect.ToString()));
            }
        }
    }
}