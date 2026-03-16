using DLSample.Gameplay;
using UnityEngine;

namespace DLSample.Facility.UI
{
    public class PanelOpener_Gameplay : MonoBehaviour
    {
        [SerializeField] private string panelId = string.Empty;

        private UIElementManager _uiManager;

        private void Start()
        {
            _uiManager = GameplayEntry.Instance.ServiceLocator.Get<UIElementManager>();
        }

        public void LoadFullScreenPanel()
        {
            _uiManager?.OpenPanel(panelId);
        }
    }
}
