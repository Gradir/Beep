using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VolumeSettings : MonoBehaviour
{
	private DepthOfField dofComponent;
	private Fog fogComponent;

	void Start()
	{
		Volume volume = gameObject.GetComponent<Volume>();
		DepthOfField tmp;
		Fog tmpFog;
		if (volume.profile.TryGet<DepthOfField>(out tmp))
		{
			dofComponent = tmp;
		}
		if (volume.profile.TryGet<Fog>(out tmpFog))
		{
			fogComponent = tmpFog;
		}
		fogComponent.active = Settings.hq;
		dofComponent.active = Settings.hq;
	}
}
