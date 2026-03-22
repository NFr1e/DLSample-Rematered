using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace DLSample.Facility.Input
{
    public class InputTaskPool
    {
        private readonly List<InputTask> _tasks = new();

        private bool _isSorted = false;

        public void AddTask(InputTask task)
        {
            if (_tasks.Contains(task)) return;

            _tasks.Add(task);
            _isSorted = false;
        }

        public bool RemoveTask(InputTask task)
        {
            if (_tasks.Remove(task))
            {
                _isSorted = false;
                return true;
            }
            return false;
        }

        public void OnInputed(InputAction.CallbackContext ctx)
        {
            if (!_isSorted)
            {
                Sort();
            }

            foreach (var task in _tasks)
            {
                if (!ctx.started && !ctx.performed) continue; 

                task.Callback?.Invoke(ctx);

                if (task.Layer.BlockLowerLayers)
                {
                    break;
                }
            }
        }

        private void Sort()
        {
            _tasks.Sort();
            _isSorted = true;
        }

        public void Clear()
        {
            _tasks.Clear();
            _isSorted = false;
        }

        public bool IsEmpty() => _tasks.Count == 0;
    }
}
