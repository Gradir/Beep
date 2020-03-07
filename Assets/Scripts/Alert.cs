using GGJ2020.Catastrophies;
using UnityEngine;

namespace GGJ2020
{
	public class Alert : MonoBehaviour
	{
		[SerializeField] private AudioSource aus;
		[SerializeField] private GameObject objectToTurnOn;
		[SerializeField] private GameObject objectToTurnOff;
		void Start()
		{
			Signals.Get<AlertSignal>().AddListener(ReactOnAlertChange);
		}

		private void OnDestroy()
		{
			Signals.Get<AlertSignal>().RemoveListener(ReactOnAlertChange);
		}

		private void ReactOnAlertChange(bool alertIsOn)
		{
			if (aus == null)
			{
				return;
			}
			if (alertIsOn)
			{
				aus.Play();
			}
			else
			{
				aus.Stop();
			}
			objectToTurnOn.SetActive(alertIsOn);
			objectToTurnOff.SetActive(!alertIsOn);
		}
	}
}
