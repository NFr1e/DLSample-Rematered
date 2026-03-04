using UnityEngine;
using DLSample.Facility.Events;

namespace DLSample.App
{
    public static class AppEntry
    {
        public static EventBus EventBus { get; private set; }
        public static GameInput GameInput { get; private set; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Enter()
        {
            EventBus = new EventBus();
            GameInput = new GameInput();
        }
    }
}
