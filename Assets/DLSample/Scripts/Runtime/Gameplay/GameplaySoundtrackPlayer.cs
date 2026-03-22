using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace DLSample.Gameplay.Stream
{
    public class GameplaySoundtrackPlayer : IStreamPlayer
    {
        private readonly AudioClip _audioClip;
        private readonly AudioSource _audioSource;

        private Playable _playable;
        private AudioPlayableOutput _output;
        private PlayableGraph _graph;

        private Tweener _fadeoutTween;

        public GameplaySoundtrackPlayer(AudioClip clip, AudioSource source)
        {
            _audioClip = clip;
            _audioSource = source;
        }
        public void Init()
        {
            _graph = PlayableGraph.Create("GameplaySoundtrackPlayableGraph");
            _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            _playable = AudioClipPlayable.Create(_graph, _audioClip, false);

            _output = AudioPlayableOutput.Create(_graph, "GameplaySoundtrackPlayableOutput", _audioSource);
            _output.SetSourcePlayable(_playable);

            _graph.Play();
            
            _playable.Pause();
            IsPlaying = false;
        }
        public void Dispose()
        {
            if (_graph.IsValid())
                _graph.Destroy();
        }


        public bool IsPlaying { get; private set; }
        public double CurrentTime
        {
            get
            {
                if (!_graph.IsValid())
                    return 0;

                return _playable.GetTime();
            }
        }

        public void Play()
        {
            if (IsPlaying) return;

            RestoreVolume();

            _playable.Play();
            IsPlaying = true;
        }
        public void Stop()
        {
            _playable.Pause();
            IsPlaying = false;
        }
        public void Seek(double timeSecond)
        {
            Stop();

            timeSecond = Mathf.Max(0f, (float)timeSecond);
            _playable.SetTime(timeSecond);
        }
        public void Fadeout()
        {
            IsPlaying = false;
            _fadeoutTween?.Kill();
            _fadeoutTween = _audioSource.DOFade(0, 3f).SetLink(_audioSource.gameObject);
        }
        private void RestoreVolume()
        {
            _fadeoutTween?.Kill();
            _fadeoutTween = _audioSource.DOFade(1, 0.3f);
        }
    }
}
