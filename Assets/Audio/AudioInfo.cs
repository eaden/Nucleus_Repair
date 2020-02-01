using System;
using UnityEngine;

namespace Assets.Audio
{
    [Serializable]
    public class AudioInfo
    {
        public AudioClip Clip;

        [Range(0, 1)]
        public float Volume = 1;
    }
}
