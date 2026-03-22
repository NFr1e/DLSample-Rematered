using DLSample.Shared;

namespace DLSample.Facility.Input
{
    public struct InputLayers
    {
        public struct SystemInputLayer : IInputLayer
        {
            public readonly string Name => "System";
            public readonly int Priority => DLSampleConsts.Input.INPUT_PRIORITY_SYSTEM;
            public readonly bool BlockLowerLayers => false;
        }
        public struct UIInputLayer : IInputLayer
        {
            public readonly string Name => "UI";
            public readonly int Priority => DLSampleConsts.Input.INPUT_PRIORITY_UI;
            public readonly bool BlockLowerLayers => true;
        }
        public struct GameplayInputLayer : IInputLayer 
        {
            public readonly string Name => "Gameplay";
            public readonly int Priority => DLSampleConsts.Input.INPUT_PRIORITY_GAMEPLAY;
            public readonly bool BlockLowerLayers => true;
        }
    }
}
