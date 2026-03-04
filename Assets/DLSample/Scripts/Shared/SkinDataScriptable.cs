using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DLSample.Gameplay.Behaviours.Skin;

namespace DLSample.Shared
{
    [Serializable]
    public class SkinItem
    {
        [SerializeField] private string skinId = "skin.default";
        [SerializeField] private SkinBehaviourBase skinPrefab;

        public string Id => skinId;
        public SkinBehaviourBase Prefab => skinPrefab;

        public bool IsValid => skinPrefab != null;
    }

    [CreateAssetMenu(
        menuName = DLSampleConsts.Editor.CREATE_MENU_SKINDATA_MENU_NAME,
        fileName = DLSampleConsts.Editor.CREATE_MENU_SKINDATA_FILE_NAME,
        order = DLSampleConsts.Editor.CREATE_MENU_SKINDATA_ORDER)]
    public class SkinDataScriptable : ScriptableObject
    {
        [SerializeField] private SkinItem defaultSkin = new();
        [SerializeField] private List<SkinItem> skins = new();

        public SkinItem DefaultSkin => defaultSkin;

        /// <summary>
        /// 通过在ScriptableObject中设定的皮肤Id获取SkinBehaviour预制体
        /// </summary>
        /// <param name="skinId"></param>
        /// <returns></returns>
        public SkinItem GetSkin(string skinId)
        {
            if(string.IsNullOrWhiteSpace(skinId))
            {
                return defaultSkin;
            }

            SkinItem skin = skins.FirstOrDefault(s => s.Id == skinId);
            if (skin is not null) return skin;

            return defaultSkin;
        }
    }
}
