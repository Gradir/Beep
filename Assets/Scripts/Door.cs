using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace GGJ2020
{
	public class Door : MonoBehaviour
	{
		[SerializeField] private AudioSource aus = null;
		[SerializeField] private Transform leftDoor = null;
		[SerializeField] private Transform rightDoor = null;
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<CharacterController>())
			{

				Debug.Log(string.Format("<color=green><b>{0}</b></color>", "OPENING"));
				leftDoor.DOLocalMoveX(-1, 1).SetEase(Ease.OutBounce);
				rightDoor.DOLocalMoveX(1, 1).SetEase(Ease.OutBounce);
				PlaySound();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.GetComponent<CharacterController>())
			{
				leftDoor.DOLocalMoveX(0, 1).SetEase(Ease.OutBounce);
				rightDoor.DOLocalMoveX(0, 1).SetEase(Ease.OutBounce);
				PlaySound();
			}
		}

		private void PlaySound()
		{
			aus.Play();
		}
	}
}
