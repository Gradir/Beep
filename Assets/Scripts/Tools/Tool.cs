using GGJ2020.Catastrophies;
using UnityEngine;

namespace GGJ2020.Tools
{
    public class Tool : MonoBehaviour
    {
		[SerializeField] private AudioSource aus;
        public CatastrophyType catastrophyType;

		private void Start()
		{
			Signals.Get<ToolPickedUpSignal>().AddListener(PlaySound);
		}
		private void OnDestroy()
		{
			Signals.Get<ToolPickedUpSignal>().RemoveListener(PlaySound);
		}

		private void PlaySound(CatastrophyType t)
		{
			if (enabled)
			aus.Play();
		}
    }
}