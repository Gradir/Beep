using GGJ2020.Catastrophies;
using GGJ2020.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ2020
{
	public class HullStrength : MonoBehaviour
	{
		[SerializeField] private Image img;
		private CatastrophiesManager cataManager;
		void Start()
		{
			cataManager = MasterManager.Master.GetOrCreateManager<CatastrophiesManager>();
			InvokeRepeating("CheckShipHull", 5, 0.1f);
		}

		private void OnDestroy()
		{
			CancelInvoke();
		}

		private void CheckShipHull()
		{
			img.fillAmount = cataManager.hullStrength / cataManager.hullStrengthMax;
		}
	}
}
