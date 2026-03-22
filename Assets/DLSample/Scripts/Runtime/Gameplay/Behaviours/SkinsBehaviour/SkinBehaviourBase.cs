using UnityEngine;

namespace DLSample.Gameplay.Behaviours.Skin
{
    public abstract class SkinBehaviourBase : MonoBehaviour
    {
        protected Transform _headContainer;

        public abstract void OnApply();
        public abstract void OnDetach();

        public virtual void OnReset()
        {
            Debug.Log($"{gameObject.name} : I'm Resetting!");
        }

        public virtual void OnStartMove(PlayerMovingArgs arg)
        {

        }

        public virtual void OnStopMove(PlayerMovingArgs arg)
        {

        }

        public virtual void OnPlayerMoving(PlayerMovingArgs arg)
        {

        }

        public virtual void OnPlayerLand(PlayerMovingArgs arg)
        {

        }

        public virtual void OnPlayerTurn(PlayerMovingArgs arg)
        {

        }

        public virtual void OnPlayerDie(PlayerEventsParams.PlayerDieArg arg)
        {

        }

        public void SetHeadContainer(Transform headContainer) => _headContainer = headContainer;
    }
}
