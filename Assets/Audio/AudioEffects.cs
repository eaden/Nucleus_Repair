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
            AddPixel,
            Shot,
            Spray,
        }

        [SerializeField]
        private List<AudioInfo> AudioClips = new List<AudioInfo>(Enum.GetValues(typeof(Effects)).Length);

        public Dictionary<Effects, AudioInfo> Clips { get; } = new Dictionary<Effects, AudioInfo>();

        public void OnEnable()
        {
            foreach (var effect in (Effects[]) Enum.GetValues(typeof(Effects)))
            {
                this.Clips.Add(effect, this.AudioClips.First(c => c.Clip.name == effect.ToString()));
            }
        }
    }
}