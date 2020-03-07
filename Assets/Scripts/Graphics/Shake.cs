using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SliceNDice
{
	public class Shake : MonoBehaviour
	{
		[SerializeField] private float circleRadius = 0.12f;
		[SerializeField] private float timeToTween = 0.25f;
		private Vector3 localPos;
		private void Start()
		{
			localPos = transform.localPosition;
			InvokeRepeating("RandomTween", 0.1f, timeToTween);
		}

		private void RandomTween()
		{
			//Vector3 moveTo = transform.position + Random.insideUnitCircle * circleRadius;
			var randomV2 = Random.insideUnitCircle * circleRadius;
			Vector3 moveTo = localPos + new Vector3(randomV2.x, randomV2.y, 0);
			transform.DOLocalMove(moveTo, timeToTween);
		}
	}

}