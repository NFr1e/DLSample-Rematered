using DLSample.Facility.EnityFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DLSample.Gameplay.Behaviours.Skin
{
    public class HeadphoneSkinBehaviour : StretchTailSkinBehaviour
    {
        [SerializeField] private GameObject turnEffectPrefab;

        private EntityPool<ShotParticleEffect> _turnEffectPool;

        public override void OnApply()
        {
            base.OnApply();

            _turnEffectPool = new(turnEffectPrefab, 10, _effectsContainer);
            _turnEffectPool.Prewarm(5);
        }

        public override void OnDetach()
        {
            _turnEffectPool?.Dispose();

            base.OnDetach();
        }

        public override void OnPlayerTurn(PlayerMovingArgs arg)
        {
            base.OnPlayerTurn(arg);

            PlayShotParticle(_turnEffectPool, arg.Position, Quaternion.identity, 1);
        }
    }
}
