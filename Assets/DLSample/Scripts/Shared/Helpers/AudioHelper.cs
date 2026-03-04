using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace DLSample.Shared
{
    public static class AudioHelper
    {
        public static async void PlayAudioClip(AudioClip clip, float volume = 1f, Action onComplete = default)
        {
            AudioSource source = new GameObject{
                hideFlags = HideFlags.HideAndDontSave,
            }.AddComponent<AudioSource>();

            source.clip = clip;
            source.playOnAwake = false;
            source.loop = false;
            source.volume = volume;

            source.Play();

            await UniTask.Delay((int)(1000 * clip.length));

            onComplete?.Invoke();
            GameObject.Destroy(source.gameObject);
        }
    }
}
