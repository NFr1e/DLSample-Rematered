using Cysharp.Threading.Tasks;
using DLSample.Shared.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DLSample.Facility.UI
{
    public class UIElementManager
    {
        private readonly Stack<UIElementData<Panel>> _fullscreenPanelsStack = new();
        private readonly Dictionary<UIElementData<Panel>, Panel> _fullscreenPanesCache = new();
        private UIElementData<Panel> _currentFullscreenPanelData = new();

        private readonly Dictionary<string, Panel> _persistentPanelCache = new();

        private UIPanelsDataScriptable _panelsConfig;

        private Camera _uiCamera;
        private Transform _fullscreenPanelsContainer;
        private Transform _persistPanelsContainer;

        public void SetupConfigs(UIPanelsDataScriptable panelsConfig)
        {
            _panelsConfig = panelsConfig;
        }
        public void SetupCamera(Camera camera)
        {
            _uiCamera = camera;
        }
        public void SetupContainers(Transform fullscreenPanelsContainer, Transform persistPanelsContainer)
        {
            _fullscreenPanelsContainer = fullscreenPanelsContainer;
            _persistPanelsContainer = persistPanelsContainer;
        }

        #region Panel

        #region OpenPanel
        public async UniTask<Panel> OpenPanel(string id)
        {
            if(_panelsConfig.GetPanel(id, out var panelItem))
            {
                var panelComp = panelItem.Item;

                return panelComp.Type switch
                {
                    PanelType.Persistent => await OpenPersistPanel(panelItem),
                    PanelType.FullScreen => await OpenFullScreenPanel(panelItem),
                    _ => null
                };
            }

            return null;
        }
        private async UniTask<Panel> OpenFullScreenPanel(UIElementData<Panel> panelData)
        {
            if (EqualityComparer<UIElementData<Panel>>.Default.Equals(_currentFullscreenPanelData,panelData) && _fullscreenPanesCache.ContainsKey(panelData))
                return _fullscreenPanesCache[panelData];

            if (!string.IsNullOrEmpty(_currentFullscreenPanelData.ItemId) &&
                _fullscreenPanesCache.TryGetValue(_currentFullscreenPanelData, out var currentPanel))
            {
                await UniTask.SwitchToMainThread();
                currentPanel.Pause();
            }

            Panel panel;

            if (_fullscreenPanesCache.TryGetValue(_currentFullscreenPanelData, out var cached))
            {
                panel = cached;
                await UniTask.SwitchToMainThread();
                panel.Resume();
            }
            else
            {
                await UniTask.SwitchToMainThread();
                var go = GameObject.Instantiate(panelData.Item, _fullscreenPanelsContainer);
                panel = go.GetComponent<Panel>();
                _fullscreenPanesCache[panelData] = panel;
                panel.Load();
            }

            _currentFullscreenPanelData = panelData;
            _fullscreenPanelsStack.Push(panelData);

            panel.SetCamera(_uiCamera);

            return panel;
        }

        private async UniTask<Panel> OpenPersistPanel(UIElementData<Panel> panelData)
        {
            if (_persistentPanelCache.TryGetValue(panelData.ItemId, out var existing))
            {
                return existing;
            }

            await UniTask.SwitchToMainThread();
            var panel = GameObject.Instantiate(panelData.Item, _persistPanelsContainer).GetComponent<Panel>();

            _persistentPanelCache[panelData.ItemId] = panel;
            panel.Load();

            panel.SetCamera(_uiCamera);

            return panel;
        }
        #endregion

        #region ClosePanel
        public async UniTask CloseCurrentFullScreenPanel()
        {
            if (string.IsNullOrEmpty(_currentFullscreenPanelData.ItemId) || _fullscreenPanelsStack.Count == 0)
                return;

            var closingPanel = _fullscreenPanelsStack.Pop();
            _currentFullscreenPanelData = default;

            if (_fullscreenPanesCache.TryGetValue(closingPanel, out var closingPage))
            {
                await UniTask.SwitchToMainThread();
                closingPage.Unload();
                _fullscreenPanesCache.Remove(closingPanel);
            }

            if (_fullscreenPanelsStack.Count > 0)
            {
                var prevPanel = _fullscreenPanelsStack.Peek();
                _currentFullscreenPanelData = prevPanel;
                if (_fullscreenPanesCache.TryGetValue(prevPanel, out var prevPage))
                {
                    await UniTask.SwitchToMainThread();
                    prevPage.Resume();
                }
            }
        }
        public async UniTask CloseAllFullscreenPanel()
        {
            while(_fullscreenPanelsStack.Count > 0)
            {
                await CloseCurrentFullScreenPanel();
            }
        }
        public async UniTask ClosePersistentPanel(string panelId)
        {
            if (!_persistentPanelCache.TryGetValue(panelId, out var page)) return;

            await UniTask.SwitchToMainThread();
            page.Unload();
            _persistentPanelCache.Remove(panelId);
        }
        #endregion

        #endregion
    }
}
