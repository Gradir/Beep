using DG.Tweening;
using GGJ2020.Core;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020.Catastrophies
{
	public class GameOverSignal : ASignal { }
	public class AlertSignal : ASignal<bool> { }
	public class SpawnInitialCatastrophyRequest : ASignal { }
	public class CatastrophyDangerLevelIncreased : ASignal<Catastrophy> { }
	public class CatastrophyDangerLevelDecreased : ASignal<Catastrophy> { }

	public class CatastrophiesManager : SystemManager
	{
		private List<Catastrophy> catastrophies;
		private Dictionary<CatastrophyType, List<Catastrophy>> activeCatastrophies;
		private CatastrophiesSettings catSettings;

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			ModifyHullStrength(deltaTime);
		}

		#region Core
		public override void OnCreateManager()
		{
			base.OnCreateManager();
			catSettings = Resources.Load<CatastrophiesSettings>("Catastrophies/CatastrophiesSettings");
			hullStrengthMax = catSettings.hullStrengthMax;
			hullStrength = hullStrengthMax;

			catastrophies = new List<Catastrophy>(GameObject.FindObjectsOfType<Catastrophy>());
			activeCatastrophies = new Dictionary<CatastrophyType, List<Catastrophy>>()
			{
				{ CatastrophyType.BrokenPipe, new List<Catastrophy>() },
				{ CatastrophyType.Fire, new List<Catastrophy>() },
				{ CatastrophyType.ShortCircuit, new List<Catastrophy>() },
			};

			UnityEngine.Debug.Log($"Found {catastrophies.Count} catastrophies on scene");

			Signals.Get<SpawnInitialCatastrophyRequest>().AddListener(SpawnRandomCatastrophy);
		}

		public override void OnDestroyManager()
		{
			base.OnDestroyManager();

			Signals.Get<SpawnInitialCatastrophyRequest>().RemoveListener(SpawnRandomCatastrophy);
		}
		#endregion

		#region Catastrophies management
		private void StartCounting()
		{
			var delay = GetRandomDelayTime();
			DOVirtual.DelayedCall(delay, () =>
			{
				SpawnRandomCatastrophy();
			});
		}


		private void SpawnRandomCatastrophy()
		{
			if (catastrophies.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, catastrophies.Count);
				var catastrophy = catastrophies[index];
				catastrophies.RemoveAt(index);

				if (activeCatastrophies.TryGetValue(catastrophy.catastrophyType, out List<Catastrophy> list))
				{
					catastrophy.BeginPlaying();
					list.Add(catastrophy);
				}

				StartCounting();
				AudioClip clip = Resources.Load("Sounds/KATASTROFA") as AudioClip;
				AudioSource.PlayClipAtPoint(clip, catastrophy.transform.position);
			}
		}
		#endregion

		#region Utils
		private float GetRandomDelayTime()
		{
			return UnityEngine.Random.Range(catSettings.minTimeDelay, catSettings.maxTimeDelay);
		}
		#endregion


		public float hullStrengthMax = 800;
		public float hullStrength = 800;

		private bool isInAlertState = false;
		public void ModifyHullStrength(float value)
		{

			hullStrength += value;
			if (hullStrength <= 0)
			{
				Signals.Get<GameOverSignal>().Dispatch();
				Debug.Log(string.Format("<color=blue><b>{0}</b></color>", "GAME OVER"));
				return;
			}
			bool isBelow = CheckIfIsBelowTreshold();
			if (hullStrength >= hullStrengthMax)
			{
				hullStrength = hullStrengthMax;
			}
			if (isInAlertState)
			{
				if (isBelow == false)
				{
					isInAlertState = false;
					Signals.Get<AlertSignal>().Dispatch(false);
				}
			}
			else
			{
				if (isBelow == true)
				{
					isInAlertState = true;
					Signals.Get<AlertSignal>().Dispatch(true);
				}
			}
		}

		private bool CheckIfIsBelowTreshold()
		{
			return hullStrength < hullStrengthMax * catSettings.maxPercentForAlert;
		}
	}
}