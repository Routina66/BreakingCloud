using UnityEngine;
using UnityEngine.UI;
using I2.Loc;

/// <summary>
/// An animated window, with a title and a message.
/// </summary>
public class Window : MonoBehaviour {
	#region Serialize fields
	[SerializeField]
	private Localize title;
	[SerializeField]
    private Localize message;

	[Header("Sound options")]
	[SerializeField]
	private AudioClip openSound;
	[SerializeField]
	private AudioClip closeSound;
	[SerializeField]
	private AudioClip clickSound;


    [Header("Animation options")]
    [SerializeField]
    private float openTime = 2.0f;
    [SerializeField]
    private float openDelay = .0f;
    [SerializeField]
    private float closeTime = 2.0f;
    [SerializeField]
    private float closeDelay = .0f;
    [SerializeField]
    private GUIAnimFREE[] animatedObjects;
	#endregion

	#region Private fileds
	private AudioManager audioManager;
    #endregion

    #region Properties
    public string Title {
		set => title.Term = value;
	}

	public string Message {
		set => message.Term = value;
	}
    #endregion

    #region Events
    #endregion

    #region Unity methods
    private void Awake() {
		audioManager = AudioManager.Instance;

		foreach (var button in GetComponentsInChildren<Button>(true)) {
			button.onClick.AddListener(delegate {
				PlaySound(clickSound);
			});
		}

		foreach (var toggle in GetComponentsInChildren<Toggle>(true)) {
			toggle.onValueChanged.AddListener(delegate {
				PlaySound(clickSound);
			});
		}

		foreach(var anim in animatedObjects) {
			anim.m_MoveIn.Time = openTime;
			anim.m_MoveIn.Delay = openDelay;

			anim.m_MoveOut.Time = closeTime - .2f;
			anim.m_MoveOut.Delay = closeDelay;

			anim.m_FadeIn.Time = openTime;
			anim.m_FadeIn.Delay = openDelay;

			anim.m_FadeOut.Time = closeTime - .2f;
			anim.m_FadeOut.Delay = closeDelay;
		}
    }
    #endregion

    #region Public methods
    public void Open() {
		gameObject.SetActive(true);
		PlaySound(openSound);

		foreach (var anim in animatedObjects) {
			if (anim.gameObject.activeInHierarchy) {
				anim.PlayInAnims();
			}
		}
	}

	public void Close() {
		PlaySound(closeSound);

		foreach (var anim in animatedObjects) {
			if (anim.gameObject.activeInHierarchy) {
				anim.PlayOutAnims();
			}
		}

		Invoke(nameof(Hide), closeTime + closeDelay);
	}
	#endregion

	#region Private methods
	private void Hide() {
		gameObject.SetActive(false);
	}

	private void PlaySound(AudioClip sound) {
		if (audioManager == null) {
			audioManager = AudioManager.Instance;
		}

		if (sound != null) {
			audioManager.PlayEffect(sound);
		}
	}
	#endregion
}
