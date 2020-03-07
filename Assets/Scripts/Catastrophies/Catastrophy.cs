using GGJ2020.Core;
using UnityEngine;

namespace GGJ2020.Catastrophies
{
	public enum CatastrophyType
	{
		None,
		Fire,
		BrokenPipe,
		ShortCircuit,
	}

	public enum DangerLevel
	{
		None,
		Low,
		Medium,
		High
	}

	public class InproperToolSignal : ASignal { }
	public class DangerSliderSignal : ASignal<float> { }
	public class ShowOrHideDangerSlider : ASignal<bool> { }
	public class Catastrophy : MonoBehaviour
	{
		public CatastrophyType catastrophyType = CatastrophyType.None;
		public DangerLevel DangerLevel { get; private set; } = DangerLevel.None;

		[SerializeField] private GameObject catastrophyPrefab;
		[SerializeField] private GameObject catastrophyPrefabLvl1;
		[SerializeField] private GameObject catastrophyPrefabLvl2;
		[SerializeField] private float timeToNextDangerState = 5f;
		[SerializeField] private float repairTime = 3f;

		private float dangerTimeCounter = 0f;
		private float repairTimeCounter = 0f;
		public bool isBeingRepaired;
		public bool IsActive { get; private set; }
		private CatastrophiesManager cataManager;

		private void Start()
		{
			var master = MasterManager.Master;
			cataManager = master.GetOrCreateManager<CatastrophiesManager>();
			InvokeRepeating("DamageShipTick", 1, 1.5f);
		}

		private void Update()
		{
			if (!IsActive || DangerLevel == DangerLevel.None)
			{
				return;
			}

			if (isBeingRepaired)
			{

				//Debug.Log(string.Format("<color=blue><b>{0}</b></color>", repairTimeCounter));

				repairTimeCounter += Time.deltaTime;
				Signals.Get<DangerSliderSignal>().Dispatch(repairTimeCounter);
				if (repairTimeCounter >= repairTime)
				{
					repairTimeCounter = 0f;
					DangerLevel -= 1;
					// to do: update viz to higher level
					ReactOnCatastrophyLevelChanged();
					Signals.Get<InteractingSignal>().Dispatch(false);

					Signals.Get<CatastrophyDangerLevelDecreased>().Dispatch(this);
				}
			}
			else if (DangerLevel != DangerLevel.High)
			{
				dangerTimeCounter += Time.deltaTime;
				if (dangerTimeCounter >= timeToNextDangerState)
				{
					dangerTimeCounter = 0f;
					DangerLevel += 1;
					ReactOnCatastrophyLevelChanged();
					// to do: update viz to lower level
					Signals.Get<CatastrophyDangerLevelIncreased>().Dispatch(this);
				}
			}
		}

		private void DamageShipTick()
		{
			cataManager.ModifyHullStrength(-GetDamageByDangerLevel());
		}

		private float GetDamageByDangerLevel()
		{
			switch (DangerLevel)
			{
				case DangerLevel.None:
					return 0;
				case DangerLevel.Low:
					return 1;
				case DangerLevel.Medium:
					return 2f;
				case DangerLevel.High:
					return 5f;
				default:
					return 0;
			}
		}

		private void ReactOnCatastrophyLevelChanged()
		{
			switch (DangerLevel)
			{
				case DangerLevel.None:
					cataManager.ModifyHullStrength(10);
					catastrophyPrefab.SetActive(false);
					catastrophyPrefabLvl1.SetActive(false);
					catastrophyPrefabLvl2.SetActive(false);
					break;
				case DangerLevel.Low:
					catastrophyPrefab.SetActive(true);
					catastrophyPrefabLvl1.SetActive(false);
					catastrophyPrefabLvl2.SetActive(false);
					break;
				case DangerLevel.Medium:
					catastrophyPrefab.SetActive(true);
					catastrophyPrefabLvl1.SetActive(true);
					catastrophyPrefabLvl2.SetActive(false);
					break;
				case DangerLevel.High:
					catastrophyPrefab.SetActive(true);
					catastrophyPrefabLvl1.SetActive(true);
					catastrophyPrefabLvl2.SetActive(true);
					break;
			}

		}

		#region Catastrophy life management
		public void BeginPlaying()
		{
			DangerLevel = DangerLevel.Low;
			IsActive = true;
			ReactOnCatastrophyLevelChanged();
		}

		public void FinishPlaying()
		{
			DangerLevel = DangerLevel.None;
			IsActive = false;
			ReactOnCatastrophyLevelChanged();
		}
		#endregion
	}
}