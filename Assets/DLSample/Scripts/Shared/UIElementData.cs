using DLSample.Facility.UI;
using System;
using UnityEngine;

namespace DLSample.Shared.UI
{
    [Serializable]
    public struct UIElementData<T> where T : UIElement
    {
        public readonly T Item
        {
            get
            {
                return _item;
            }
        }

        public readonly string ItemId
        {
            get
            {
                return _itemId;
            }
        }

        [SerializeField] string _itemId;
        [SerializeField] T _item;
    }
}
