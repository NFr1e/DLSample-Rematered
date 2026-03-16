using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DLSample.Facility.UI;

namespace DLSample.Shared.UI
{
    [CreateAssetMenu(
        menuName = DLSampleConsts.Editor.CREATE_MENU_PANELS_MENU_NAME,
        fileName = DLSampleConsts.Editor.CREATE_MENU_PANELS_FILE_NAME,
        order = DLSampleConsts.Editor.CREATE_MENU_PANELS_ORDER)]
    public class UIPanelsDataScriptable : ScriptableObject
    {
        [SerializeField] private List<UIElementData<Panel>> panelsData;

        public bool GetPanel(string id, out UIElementData<Panel> item) 
        {
            item = panelsData.FirstOrDefault(x => x.ItemId == id);

            if (item.Item == null || string.IsNullOrEmpty(item.ItemId))
                return false;

            return true;
        }
    }
}
