using UnityEngine;

namespace DLSample.Shared
{
    [RequireComponent(typeof(Renderer))]
    public class RuntimeInvisible : MonoBehaviour
    {
        private Renderer m_Renderer;

        private void Awake()
        {
            m_Renderer = GetComponent<Renderer>();
        }

        private void Start () 
        {
            m_Renderer.enabled = false;
        }
    }
}
