using Cysharp.Threading.Tasks;

namespace DLSample.Gameplay.Stream
{
    public interface ISyncable
    {
        UniTask SyncDelay();
    }
}
