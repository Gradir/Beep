using DG.Tweening;
using UnityEngine;

namespace GGJ2020.Catastrophies
{
    public class CatastrophiesTest : MonoBehaviour
    {
        [SerializeField] private float initialDelay = 3f;

        void Start()
        {
            DOVirtual.DelayedCall(initialDelay, () =>
            {
                Signals.Get<SpawnInitialCatastrophyRequest>().Dispatch();
            });
        }
    }
}