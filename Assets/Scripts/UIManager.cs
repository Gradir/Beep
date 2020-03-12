using GGJ2020.Catastrophies;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGJ2020
{
	public class WinGameSignal : ASignal { }
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private DensityVolume densityVolume = null;
		[SerializeField] private CookieCloudAnimation cookieCloudAnimation = null;

		[SerializeField] private AudioSource aus = null;
		[SerializeField] private GameObject winScreen = null;
		[SerializeField] private GameObject loseScreen = null;
		[SerializeField] private GameObject holdToInteractObj = null;
		[SerializeField] private GameObject properToolObj = null;
		[SerializeField] private Image repairSlider = null;
		[SerializeField] private TextMeshProUGUI timeLeft = null;
		[SerializeField] private Image toolHeldIcon = null;
		[SerializeField] private Sprite fireSprite = null;
		[SerializeField] private Sprite sparkSprite = null;
		[SerializeField] private Sprite steamSprite = null;
		[SerializeField] private Color fireColor;
		[SerializeField] private Color steamColor;
		[SerializeField] private Color electroColor;
		public float timeLeftFloat = 240f;

		private void Start()
		{
			if (Settings.hq)
			{
				var pipelineAsset = Instantiate(GraphicsSettings.renderPipelineAsset) as HDRenderPipelineAsset;
				var set = pipelineAsset.currentPlatformRenderPipelineSettings;
				set.hdShadowInitParams.maxShadowRequests = 64;

				set.supportVolumetrics = false;
				
				GraphicsSettings.renderPipelineAsset = pipelineAsset;
				densityVolume.enabled = true;
				cookieCloudAnimation.enabled = true;
			}
			else
			{
				var pipelineAsset = Instantiate(GraphicsSettings.renderPipelineAsset) as HDRenderPipelineAsset;
				var set = pipelineAsset.currentPlatformRenderPipelineSettings;
				set.hdShadowInitParams.maxShadowRequests = 12;
				GraphicsSettings.renderPipelineAsset = pipelineAsset;
				densityVolume.enabled = false;
				cookieCloudAnimation.enabled = false;
			}
			//GraphicsSettings.renderPipelineAsset = Settings.hq? hqAsset : lqAsset;
			Signals.Get<InteractingSignal>().AddListener(HoldToInteractShowOrHide);
			Signals.Get<DangerSliderSignal>().AddListener(ReactOnDangerSlider);
			Signals.Get<InproperToolSignal>().AddListener(ProperToolShow);
			Signals.Get<ToolPickedUpSignal>().AddListener(OnPickUpTool);
			Signals.Get<WinGameSignal>().AddListener(WinScreenShow);
			Signals.Get<GameOverSignal>().AddListener(LoseGameShow);
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = 1;
		}

		private void OnDestroy()
		{
			Signals.Get<InteractingSignal>().RemoveListener(HoldToInteractShowOrHide);
			Signals.Get<DangerSliderSignal>().RemoveListener(ReactOnDangerSlider);
			Signals.Get<InproperToolSignal>().RemoveListener(ProperToolShow);
			Signals.Get<ToolPickedUpSignal>().RemoveListener(OnPickUpTool);
			Signals.Get<WinGameSignal>().RemoveListener(WinScreenShow);
			Signals.Get<GameOverSignal>().RemoveListener(LoseGameShow);
		}

		private void ReactOnDangerSlider(float value)
		{
			repairSlider.fillAmount = value / 3f;
			repairSlider.color = Color.Lerp(Color.blue, Color.green, repairSlider.fillAmount);
		}

		private void OnPickUpTool(CatastrophyType type)
		{
			switch (type)
			{
				case CatastrophyType.Fire:
					toolHeldIcon.sprite = fireSprite;
					toolHeldIcon.color = fireColor;
					break;
				case CatastrophyType.BrokenPipe:
					toolHeldIcon.sprite = steamSprite;
					toolHeldIcon.color = steamColor;
					break;
				case CatastrophyType.ShortCircuit:
					toolHeldIcon.sprite = sparkSprite;
					toolHeldIcon.color = electroColor;
					break;
			}
		}

		private void HoldToInteractShowOrHide(bool show)
		{
			holdToInteractObj.SetActive(show);
			repairSlider.enabled = show;
		}

		public void Restart()
		{
			SceneManager.LoadScene(0);
		}

		private void Update()
		{
			if (loseScreen.activeSelf || winScreen.activeSelf)
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
					Restart();
				}
			}
		}

		private void ProperToolShow()
		{
			Invoke("ProperToolHide", 2.5f);
			aus.Play();
			properToolObj.SetActive(true);
		}

		private void ProperToolHide()
		{
			properToolObj.SetActive(false);
		}

		private void FixedUpdate()
		{
			if (timeLeftFloat <= 0)
			{
				Signals.Get<WinGameSignal>().Dispatch();
				return;
			}
			timeLeftFloat -= Time.deltaTime;

			timeLeft.text = timeLeftFloat.ToString("#") + "s";
		}

		private void WinScreenShow()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Time.timeScale = 0;
			winScreen.SetActive(true);
		}

		private void LoseGameShow()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Time.timeScale = 0;
			loseScreen.SetActive(true);
		}
	}
}