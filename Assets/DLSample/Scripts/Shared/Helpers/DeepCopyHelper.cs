using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DLSample.Shared
{
    public class DeepCopyHelper
    {
        public static T Clone<T>(T original)
        {
            string json = JsonUtility.ToJson(original);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
