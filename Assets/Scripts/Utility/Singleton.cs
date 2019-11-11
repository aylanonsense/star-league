using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Singleton<T> : BaseMonoBehaviour where T : BaseMonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString() + " (Singleton)";
                        DontDestroyOnLoad(singleton);
                    }
                }

                return instance;
            }
        }
    }
}