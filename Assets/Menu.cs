using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Menu : MonoBehaviour
{
	[SerializeField] private AudioSource aus = null;
	[SerializeField] private CanvasGroup cg = null;
	[SerializeField] private AudioClip click = null;
	private void Start()
	{
		gameObject.SetActive(true);
	}

	public bool hq;
	public void LaunchInHQ()
	{
		Settings.hq = true;
		Fade();
	}
	public void LaunchInLQ()
	{
		Settings.hq = false;
		Fade();
	}

	private void Fade()
	{
		aus.Play();
		cg.DOFade(0, 3f);
		DOVirtual.DelayedCall(3f, () => SwitchScenes());
	}

	private void SwitchScenes()
	{
		gameObject.SetActive(false);
		SceneManager.LoadScene(1);
	}

	public void Click()
	{
		aus.PlayOneShot(click);
	}
}
