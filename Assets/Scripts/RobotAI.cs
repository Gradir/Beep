using UnityEngine;
using UnityEngine.AI;

namespace GGJ2020
{
	public class RobotAI : MonoBehaviour
	{
		[SerializeField] private NavMeshAgent agent = null;
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<Robot>())
			{
				transform.LookAt(other.transform.position);
			}
		}

		private void SetNewTarget()
		{
			Vector3 randomPoint = transform.position + Random.insideUnitSphere * 5;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 10, NavMesh.AllAreas))
			{
				agent.destination = hit.position;
			}
			Invoke("SetNewTarget", Random.Range(5, 10));
		}

		void Start()
		{
			Invoke("SetNewTarget", Random.Range(5, 10));
		}
	}
}
