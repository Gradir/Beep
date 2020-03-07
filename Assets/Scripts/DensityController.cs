using GGJ2020.Catastrophies;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace GGJ2020
{
	public class DensityController : MonoBehaviour
	{
		public float alertOnDensity = 100f;
		public float alertOffDensity = 10;
		public Color normalColor;
		[SerializeField] private DensityVolume dv = null;
		void Start()
		{
			Signals.Get<AlertSignal>().AddListener(ReactOnAlert);
		}

		private void OnDestroy()
		{
			Signals.Get<AlertSignal>().RemoveListener(ReactOnAlert);
		}

		private void ReactOnAlert(bool on)
		{
			dv.parameters.distanceFadeEnd = on ? alertOnDensity : alertOffDensity;
			dv.parameters.albedo = on ? Color.red : normalColor;
		}
	}
}
