using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GusteruStudio.Audio
{
    public class AudioSourceData : MonoBehaviour
    {
        [BoxGroup("References"), SerializeField] private AudioSystem audioSystem;

        [BoxGroup("Sounds")] public Sounds[] sounds;
        [BoxGroup("Sounds")] public string mixerChannel;

        [BoxGroup("Options")] public bool useDifferentAudioSource;

        public AudioSource Play()
        {
            return audioSystem.PlayAudioData(this);
        }

        public AudioSource Play(string langKey)
        {
            return audioSystem.PlayAudioData(this, GetSoundsIndex(langKey));
        }

        public void Stop()
        {
            audioSystem.StopAudioData(this);
        }

        private int GetSoundsIndex(string langKey)
        {
            for (var index = 0; index < sounds.Length; index++)
            {
                if (sounds[index].langKey == langKey)
                    return index;
            }

            Debug.Log($"The sound with lang key {langKey} does not exist");
            return 0;
        }
    }

    [Serializable]
    public class Sounds
    {
        public string langKey = "default";
        public ClipData[] clips;
    }


    [Serializable]
    public class ClipData
    {
        public AudioClip clip;
        [MinMaxSlider(0, 1, true), SerializeField] private Vector2 volumeRange;
        public float Volume => Random.Range(volumeRange.x, volumeRange.y);
    }
}