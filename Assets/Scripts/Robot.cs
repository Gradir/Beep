using GGJ2020.Catastrophies;
using GGJ2020.Tools;
using UnityEngine;

namespace GGJ2020
{
	public class ToolPickedUpSignal : ASignal<CatastrophyType> { };
	public class InteractingSignal : ASignal<bool> { };
	public class Robot : MonoBehaviour
	{
		[SerializeField] private AudioSource aus = null;
		public CatastrophyType toolHeldType = CatastrophyType.None;
		public float timeToHold = 1.2f;
		private Tool toolInfront;
		private Catastrophy cataInFront;

		private void OnPickUpTool(CatastrophyType type)
		{
			Signals.Get<ToolPickedUpSignal>().Dispatch(type);
			toolHeldType = type;
		}

		private void OnTriggerStay(Collider other)
		{
			var cata = other.GetComponent<Catastrophy>();
			var tool = other.GetComponent<Tool>();
			if (cata == null && tool == null)
			{
				cataInFront = null;
				toolInfront = null;
				Signals.Get<InteractingSignal>().Dispatch(false);
				return;
			}
			else
			{
				toolInfront = tool;
				cataInFront = cata;
				//if (cata != null && cata.catastrophyType != toolHeldType || cata.IsActive == false)
				//{
				//	cataInFront = null;
				//}
				Signals.Get<InteractingSignal>().Dispatch(true);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			var cata = other.GetComponent<Catastrophy>();
			var tool = other.GetComponent<Tool>();
			if (cata != null)
			{
				cataInFront = null;
			}
			if (tool != null)
			{
				toolInfront = null;
			}
			Signals.Get<InteractingSignal>().Dispatch(false);
		}

		private void Update()
		{
			// FixMe: Input Manager!
			if (cataInFront == null && toolInfront == null)
			{
				aus.Stop();
				Signals.Get<InteractingSignal>().Dispatch(false);
			}
			if (Input.GetButton("Fire1"))
			{
				if (toolInfront != null)
				{
					aus.Stop();
					OnPickUpTool(toolInfront.catastrophyType);
					Destroy(toolInfront.gameObject);
				}
				if (cataInFront != null)
				{
					if (cataInFront.IsActive && cataInFront.catastrophyType == toolHeldType)
					{
						cataInFront.isBeingRepaired = true;
						aus.Play();
					}
					else if (cataInFront.IsActive)
					{
						Signals.Get<InproperToolSignal>().Dispatch();
						aus.Stop();
					}
				}
			}
			if (Input.GetButtonUp("Fire1"))
			{
				aus.Stop();
				if (cataInFront != null)
				{
					cataInFront.isBeingRepaired = false;
				}
			}
		}
	}
}
