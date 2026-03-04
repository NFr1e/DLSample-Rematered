using UnityEngine;
using DLSample.Facility.EnityFramework;

namespace DLSample.Gameplay.Behaviours.Skin
{
    public class ShotParticleEffect : MonoBehaviour, IPoolabelEntity
    {
        [SerializeField] private ParticleSystem particle;

        public ParticleSystem Particle => particle;

        public void OnEnpool()
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
        }
        public void OnDepool()
        {
            particle.gameObject.SetActive(true);
        }
        public void SetVisiblity(bool visible)
        {

        }
        public bool IsVaild { get; set; }
    }
}
