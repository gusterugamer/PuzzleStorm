using System;
using System.Collections.Generic;
using GusteruStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

namespace GusteruStudio.Audio
{
    [CreateAssetMenu(menuName = "GusteruStudio/Systems/AudioSystem")]
    public class AudioSystem :ScriptableObject
    {
        [BoxGroup("References"), SerializeField] private AudioSource audioSourcePrefab;
        [BoxGroup("References"), SerializeField] private AudioMixer audioMixer;

        [NonSerialized] private Transform _audioSourcesParent;
        [NonSerialized] private Dictionary<AudioSourceData, List<AudioSource>> _audioSources = new Dictionary<AudioSourceData, List<AudioSource>>();

        public void Initialize()
        {
            _audioSourcesParent = new GameObject("AudioSourcesParent").transform;
        }

        //Create a instance of audio source and returns a reference to it in case the user wants to do any further modifications to it 
        public AudioSource PlayAudioData(AudioSourceData audioData, int element = 0)
        {
            if (!_audioSources.ContainsKey(audioData))
                _audioSources.Add(audioData, new List<AudioSource>());
            else if (_audioSources[audioData] == null)
                _audioSources[audioData] = new List<AudioSource>();

            AudioSource dataAudioSource = audioData.GetComponent<AudioSource>();

            _audioSources[audioData].RemoveAll(x => x == null);

            if (audioData.useDifferentAudioSource)
            {
                var clipData = audioData.sounds[element].clips.Random();
                var audioClip = clipData.clip;

                AudioSource newAudioSource = null;

                if (dataAudioSource != null)
                {
                    newAudioSource = Instantiate(dataAudioSource);
                    newAudioSource.clip = audioClip;
                }
                else
                {
                    newAudioSource = InstantiateAudioSource(audioClip);
                    SetMixer(newAudioSource, audioData.mixerChannel);
                    newAudioSource.volume = clipData.Volume;
                }
                _audioSources[audioData].Add(newAudioSource);
                newAudioSource.Play();
                Destroy(newAudioSource, audioClip.length);

                return newAudioSource;
            }
            else
            {
                var clipData = audioData.sounds[element].clips.Random();

                AudioSource newAudioSource = null;

                if (_audioSources[audioData].Count < 1)
                {
                    if (dataAudioSource != null)
                    {
                        newAudioSource = Instantiate(dataAudioSource);
                    }
                    else
                    {
                        newAudioSource = InstantiateAudioSource(clipData.clip);
                        newAudioSource.volume = clipData.Volume;
                    }
                    newAudioSource.clip = clipData.clip;
                    _audioSources[audioData].Add(newAudioSource);
                }
                else
                    newAudioSource = _audioSources[audioData][0];

                SetMixer(newAudioSource, audioData.mixerChannel);
                newAudioSource.Play();

                return newAudioSource;
            }
        }

        public void StopAudioData(AudioSourceData audioData)
        {
            Assert.IsTrue(audioData != null);

            if (audioData == null)
                return;
            if (!_audioSources.ContainsKey(audioData) || _audioSources[audioData] == null)
                return;

            foreach (AudioSource src in _audioSources[audioData])
                if (src != null)
                    Destroy(src.gameObject);
            _audioSources.Remove(audioData);
        }

        //Takes the game of the AudioSourceData prefab and compares to the existing instances to mute them
        public void MuteWithTag(string tag, bool isMutted)
        {
            foreach (var pair in _audioSources)
            {
                if (pair.Key.tag == tag)
                {
                    foreach (AudioSource src in pair.Value)
                    {
                        if (src != null)
                            src.mute = isMutted;
                    }
                }
            }
        }

        public void StopAll()
        {
            foreach (var audioSource in _audioSources)
            {
                foreach (AudioSource src in audioSource.Value)
                    if (src != null)
                        Destroy(src.gameObject);
            }
            _audioSources.Clear();
        }

        private AudioSource InstantiateAudioSource(AudioClip audioClip)
        {
            var newAudioSource = Instantiate(audioSourcePrefab, _audioSourcesParent);
            newAudioSource.clip = audioClip;
            return newAudioSource;
        }

        private void SetMixer(AudioSource audioSource, string mixerChannel)
        {
            if (mixerChannel.EmptyOrNull())
            {
                audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(mixerChannel)[0];
            }
        }
#if UNITY_EDITOR
        [Button]
        public void TestPlay(AudioSourceData audioData, string langKey)
        {
            audioData.Play(langKey);
        }

        [Button]
        public void TestStop(AudioSourceData audioData)
        {
            audioData.Stop();
        }
#endif
    }
}