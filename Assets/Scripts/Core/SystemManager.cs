namespace GGJ2020.Core
{
		public abstract class SystemManager
	{
		public virtual void OnCreateManager()
		{
			UnityEngine.Debug.Log($"SystemManager of type {this.GetType()} has been created.");
		}

		public virtual void OnDestroyManager()
		{
			UnityEngine.Debug.Log($"SystemManager of type {this.GetType()} has been destroyed.");
		}

		public virtual void Update(float deltaTime)
		{
		}
	}
}