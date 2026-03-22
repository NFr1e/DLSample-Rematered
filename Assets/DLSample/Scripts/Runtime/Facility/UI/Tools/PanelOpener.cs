using DLSample.App;
using UnityEngine;

namespace DLSample.Facility.UI
{
    public class PanelOpener : MonoBehaviour
    {
        [SerializeField] private string panelId = string.Empty;

        public async void OpenPanel()
        {
             var panel = await AppEntry.UIManager.OpenPanel(panelId);

            Debug.Log($"OpenPanel {panel.name}");
        }
    }
}
