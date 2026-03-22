using UnityEngine;
using DLSample.Facility.Events;
using DLSample.Facility.UI;
using DLSample.Facility.Input;
using DLSample.Facility.SceneManage;

namespace DLSample.App
{
    public class AppEntry
    {
        public static EventBus EventBus { get; private set; }
        public static AsyncEventBus AsyncEventBus { get; private set; }
        public static GameInput GameInput { get; private set; }
        public static InputManager InputManager { get; private set; }
        public static UIElementManager UIManager { get; private set; }
        public static ScenesManager SceneManager { get; private set; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Enter()
        {
            EventBus = new EventBus();
            AsyncEventBus = new AsyncEventBus();
            GameInput = new GameInput();
            InputManager = new InputManager();
            UIManager = new UIElementManager();
            SceneManager = new ScenesManager();

            RegisterEvents();
            Initialize();
        }

        private static void RegisterEvents()
        {
            Application.quitting -= OnAppQuit;
            Application.quitting += OnAppQuit;
        }
        private static void Initialize()
        {
            GameInput.App.Enable();
            UIManager.Init();
        }

        private static void OnAppQuit()
        {
            GameInput.App.Disable();

            EventBus.Dispose();
            AsyncEventBus.Dispose();
            GameInput.Dispose();
            InputManager.Dispose();
            UIManager.Dispose();

            EventBus = null;
            AsyncEventBus = null;
            GameInput = null;
            InputManager = null;
            UIManager = null;
            SceneManager = null;

            Application.quitting -= OnAppQuit;
        }
    }
}
