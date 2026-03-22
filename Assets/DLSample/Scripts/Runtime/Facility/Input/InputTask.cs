using System;
using UnityEngine.InputSystem;

namespace DLSample.Facility.Input
{
    public struct InputTask : IComparable<InputTask>
    {
        public Action<InputAction.CallbackContext> Callback { get; private set; }
        public IInputLayer Layer { get; private set; }

        public InputTask(Action<InputAction.CallbackContext> callback, IInputLayer layer)
        {
            Callback = callback;
            Layer = layer;
        }

        public readonly int CompareTo(InputTask other)
        {
            return other.Layer.Priority.CompareTo(Layer.Priority);
        }
    }
}
