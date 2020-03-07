using GGJ2020.Catastrophies;
using UnityEngine;

namespace GGJ2020.Tools
{
    public class ToolDistributor : MonoBehaviour
    {
        [SerializeField] private Tool toolFire;
        [SerializeField] private Tool toolSprk;
		[SerializeField] private Tool toolWatr;
		public CatastrophyType catastrophyType;
		private Tool currentTool;

		private void Start()
		{
			Signals.Get<ToolPickedUpSignal>().AddListener(CheckIfItsThisTool);
		}
		private void OnDestroy()
		{
			CancelInvoke();
			Signals.Get<ToolPickedUpSignal>().RemoveListener(CheckIfItsThisTool);
		}

		private void CheckIfItsThisTool(CatastrophyType t)
		{
			if (t == catastrophyType)
			{
				Invoke("SpawnNewTool", 5f);
			}
		}

		public void SpawnNewTool ()
		{
			switch (catastrophyType)
			{
				case CatastrophyType.Fire:
					var t = Instantiate(toolFire);
					t.transform.position = transform.position + new Vector3(0, 0.5f, 0);
					break;
				case CatastrophyType.BrokenPipe:
					var w = Instantiate(toolWatr);
					w.transform.position = transform.position + new Vector3(0, 0.5f, 0);
					break;
				case CatastrophyType.ShortCircuit:
					var s = Instantiate(toolSprk);
					s.transform.position = transform.position + new Vector3(0, 0.5f, 0);
					break;
			}

		}

        public Tool GetTool()
        {
            return currentTool;
        }
    }
}