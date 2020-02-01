using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Assets.Audio
{
    public class AudioInterface : MonoBehaviour
    {
        public AudioEffects _effects;
        public AudioTracks _tracks;

        private double _nextEventTime;
        private readonly Dictionary<AudioSource, double> _sources = new Dictionary<AudioSource, double>();
        private readonly Dictionary<AudioTracks.Tracks, AudioSource> _usedTracks = new Dictionary<AudioTracks.Tracks, AudioSource>();

        public float Bpm = 140.0f;

        private int _bar = 0;
        private int _beatInBar = 0;
        private double TimeBetweenEvents => (60.0f / this.Bpm) / 4d;

        private void Start()
        {
            this._nextEventTime = AudioSettings.dspTime + this.TimeBetweenEvents * 2;
        }

        private void Update()
        {
            var time = AudioSettings.dspTime;

            if (time + 0.01f > this._nextEventTime)
            {
                this._nextEventTime += this.TimeBetweenEvents;
                this._beatInBar = (this._beatInBar + 1) % (int)BeatType.Full;
                if (this._beatInBar == 0)
                {
                    this._bar = (this._bar + 1) % 4;
                }
            }
        }

        public double AddTrack(AudioTracks.Tracks track)
        {
            if (this._usedTracks.ContainsKey(track))
                return 0;

            var source = GetAudioSource();

            this._usedTracks.Add(track, source);

            source.clip = this._tracks.Clips[track];
            source.loop = true;
            source.transform.parent = null;
            source.spatialBlend = 0;

            var bars = 4 - (this._bar + 1);
            var delay = GetTimeUntilBeat(1, false, BeatType.Full);
            var totalDelay = bars * this.TimeBetweenEvents * (double) BeatType.Full + delay;
            var schedule = AudioSettings.dspTime + totalDelay;

            source.PlayScheduled(schedule);
            FreeAudioSource(source, double.MaxValue);

            return totalDelay;
        }

        public double RemoveTrack(AudioTracks.Tracks track)
        {
            if (!this._usedTracks.ContainsKey(track))
                return 0;

            var source = this._usedTracks[track];
            var next = GetTimeUntilBeat(1, false, BeatType.Full);
            FreeAudioSource(source, next + AudioSettings.dspTime);
            this._usedTracks.Remove(track);

            return next;
        }

        private AudioSource GetAudioSource()
        {
            AudioSource source = null;
            foreach (var s in this._sources)
            {
                if (s.Value < AudioSettings.dspTime)
                    source = s.Key;
            }

            if (source != null)
                return source;

            source = new GameObject("Sound").AddComponent<AudioSource>();
            this._sources.Add(source, double.MaxValue);

            return source;
        }

        private void FreeAudioSource(AudioSource source, double totalTime = 0)
        {
            this._sources[source] = totalTime;
            source.SetScheduledEndTime(totalTime);
        }

        /// <summary>
        ///     Plays a sound effect. Optional at a destination. Optional with a delay in beats on background music
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="destination"></param>
        /// <param name="delay">instant if zero, otherwise at next full ticks</param>
        /// <param name="next">if true, it counts next beats, otherwise it counts beats from the next new bar on</param>
        /// <param name="type">the type of beat in which the delay is given</param>
        /// <returns>Returns the time until the sound is played</returns>
        public double PlaySoundEffect(AudioEffects.Effects effect, Transform destination = null, byte delay = 0, bool next = true, BeatType type = BeatType.Quarter)
        {
            var source = GetAudioSource();

            var clip = this._effects.Clips[effect];
            source.clip = clip;
            source.loop = false;

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
                var timeUntilPlayed = GetTimeUntilBeat(delay, next, type);
                var schedule = timeUntilPlayed + AudioSettings.dspTime;
                source.PlayScheduled(schedule);

                FreeAudioSource(source, schedule + source.clip.length);

                return timeUntilPlayed;
            }

            source.Play();
            FreeAudioSource(source, source.clip.length + AudioSettings.dspTime);

            return 0;
        }

        private double GetTimeUntilBeat(byte delay, bool next, BeatType type)
        {
            if (delay == 0)
                return 0;

            var time = AudioSettings.dspTime;
            var nextTick = this._nextEventTime - time;
            if (next)
            {
                var rawDelay = (delay - 1) * this.TimeBetweenEvents * (int) type + nextTick;
                return rawDelay;
            }

            var fullTickDelay = delay * this.TimeBetweenEvents * (int)type - this.TimeBetweenEvents;
            var totalRawDelay = nextTick + fullTickDelay;
            var result = ((int)BeatType.Full - (this._beatInBar + 1)) * this.TimeBetweenEvents + totalRawDelay;
            var fullBar = this.TimeBetweenEvents * (int)BeatType.Full;
            return result > fullBar ? result - fullBar : result;
        }

        public enum BeatType
        {
            Sixteenth = 1,
            Eighth = 2,
            Quarter = 4,
            Half = 8,
            Full = 16,
        }
    }


}