using UnityEngine;

namespace DLSample.Shared
{
    public static class LayerHelper
    {
        public static bool IsLayer(GameObject go, LayerMask mask)
        {
            return (mask.value & (1 << go.layer)) != 0;
        }
    }
}
