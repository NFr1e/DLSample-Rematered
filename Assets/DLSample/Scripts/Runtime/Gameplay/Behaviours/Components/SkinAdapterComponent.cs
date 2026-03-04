using UnityEngine;
using DLSample.Gameplay.Behaviours;

namespace DLSample.Gameplay.Skin
{
    public class SkinAdapterComponent : GameplayComponentBase
    {
        [SerializeField] private GameplayPlayerMove playerMove;
        [SerializeField] private PlayerDamager playerDamager;
        [SerializeField] private Transform headContainer;

        private GameplaySkinAdapter _adapter;

        protected override void OnInit()
        {
            _adapter = new GameplaySkinAdapter(playerMove, headContainer,  playerDamager);
            _adapter.Init();

            ServiceLocator.WhenServicesReady(AddAdapter, typeof(SkinChanger));
        }

        protected override void OnExit()
        {
            _adapter.Dispose();
            _adapter = null;
        }

        void AddAdapter()
        {
            ServiceLocator.Get<SkinChanger>().AddAdapter(_adapter);
        }
    }
}
