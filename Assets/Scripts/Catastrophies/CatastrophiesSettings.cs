using UnityEngine;

namespace GGJ2020.Catastrophies
{
    [CreateAssetMenu(
        menuName = "ScriptableObjects/Settings/Catastrophies settings",
        fileName = "CatastrophiesSettings")]
    public class CatastrophiesSettings : ScriptableObject
    {
        public float minTimeDelay = 5f;
        public float maxTimeDelay = 8f;

		public float hullStrengthMax = 800;
		public float maxPercentForAlert = 0.3f;
	}
}