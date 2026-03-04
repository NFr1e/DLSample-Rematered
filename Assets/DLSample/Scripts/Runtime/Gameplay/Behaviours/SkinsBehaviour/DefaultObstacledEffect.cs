using DLSample.Facility.EnityFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DLSample.Gameplay.Behaviours.Skin
{
    public class DefaultObstacledEffect : MonoBehaviour, IPoolabelEntity
    {
        [SerializeField] private List<Rigidbody> clips = new();

        public bool IsVaild { get; }

        public void OnEnpool()
        {
            ResetClips();
            gameObject.SetActive(false);
        }
        public void OnDepool()
        {
            gameObject.SetActive(true);
        }
        public void SetVisiblity(bool visible) { }

        private void ResetClips()
        {
            foreach(var item in clips)
            {
                item.isKinematic = true;
                item.useGravity = false;
                item.transform.localPosition = Vector3.zero;
            }
        }
        public void BoomClips()
        {
            foreach (var item in clips)
            {
                item.isKinematic = false;
                item.useGravity = true;
            }
        }
    }
}
