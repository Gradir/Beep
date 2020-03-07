using System.Collections.Generic;
using UnityEngine;


namespace GGJ2020.Core
{
    public class MasterManager : MonoBehaviour
    {
        private static MasterManager instance;
        public static MasterManager Master => instance;


        #region Core
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }

        void Update()
        {
            var deltaTime = Time.deltaTime;
            foreach (var manager in managers.Values)
            {
                manager.Update(deltaTime);
            }
        }

        private void OnDestroy()
        {
            // release all resources and memory:
            foreach (var manager in managers.Values)
            {
                manager.OnDestroyManager();
            }
            managers.Clear();
        }
        #endregion

        #region SystemManager objects management
        private Dictionary<System.Type, SystemManager> managers = new Dictionary<System.Type, SystemManager>();

        private T CreateManagerInternal<T>() where T : SystemManager
        {
            var type = typeof(T);

            var manager = (SystemManager)System.Activator.CreateInstance(type);
            manager.OnCreateManager();
            managers.Add(type, manager);

            return (T)manager;
        }

        private void DestroyManagerInternal<T>() where T : SystemManager
        {
            System.Type type = typeof(T);
            if (managers.ContainsKey(type))
            {
                var manager = managers[type];
                manager.OnDestroyManager();
                managers.Remove(type);
            }
        }

        public T GetOrCreateManager<T>() where T : SystemManager
        {
            System.Type type = typeof(T);

            if (managers.ContainsKey(type))
            {
                return (T)managers[type];
            }
            else
            {
                var manager = CreateManagerInternal<T>();
                return manager;
            }
        }

        public T GetExistingObject<T>() where T : SystemManager
        {
            System.Type type = typeof(T);
            if (managers.ContainsKey(type))
            {
                return (T)managers[type];
            }
            else
            {
                return null;
            }
        }

        public void DestroyManager<T>() where T : SystemManager
        {
            DestroyManagerInternal<T>();
        }
        #endregion
    }
}