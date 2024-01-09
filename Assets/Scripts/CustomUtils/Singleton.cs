using System;
using UnityEngine;

namespace DarkHavoc.CustomUtils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        [Obsolete("Only for this class")]
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
            SingletonAwake();
        }

        protected virtual void SingletonAwake()
        {
        }
    }
}