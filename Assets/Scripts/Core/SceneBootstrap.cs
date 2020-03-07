using GGJ2020.Catastrophies;
using UnityEngine;

namespace GGJ2020.Core
{
    public class SceneBootstrap : MonoBehaviour
    {
        void Awake()
        {
            var master = MasterManager.Master;

            master.GetOrCreateManager<CatastrophiesManager>();
        }
    }
}