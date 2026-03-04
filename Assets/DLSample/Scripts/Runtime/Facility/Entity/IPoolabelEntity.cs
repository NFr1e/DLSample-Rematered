using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DLSample.Facility.EnityFramework
{
    public interface IPoolabelEntity : IEntity
    {
        void OnEnpool();
        void OnDepool();
    }
}
