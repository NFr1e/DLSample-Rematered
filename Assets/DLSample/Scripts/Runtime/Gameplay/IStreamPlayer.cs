namespace DLSample.Gameplay.Stream
{
    public interface IStreamPlayer
    {
        void Play();
        void Stop();
        void Seek(double time);
        bool IsPlaying { get; }
        double CurrentTime { get; }
    }
}
