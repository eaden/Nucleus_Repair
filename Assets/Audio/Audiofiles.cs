using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Audio
{
    [CreateAssetMenu(menuName = "AudioSettings/Effects")]
    public class AudioFiles : ScriptableObject
    {
        public Dictionary<Effects, AudioClip> Clips { get; } = new Dictionary<Effects, AudioClip>();
        
        [SerializeField]
        private List<AudioClip> AudioClips = new List<AudioClip>(Enum.GetValues(typeof(Effects)).Length);

        public enum Effects
        {
            AddPixel,
        }

        public void OnEnable()
        {
            foreach (var effect in (Effects[])Enum.GetValues(typeof(Effects)))
            {
                this.Clips.Add(effect, this.AudioClips.First(c => c.name == effect.ToString()));
            }
        }
    }
}
