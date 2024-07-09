using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;
using System.Collections.Generic;
using UnityEngine.Events;
using JetBrains.Annotations;

public class StartlevelInfoBox : MonoBehaviour {
	#region Serialize fields
	[SerializeField]
	private float waitTime = 3f;
    [SerializeField]
    private AudioClip clockSound;
	[SerializeField]
	private TextMeshProUGUI timeToStartText;

    [Header("City")]
    [SerializeField]
	private Image cityIconImage;
    [SerializeField]
    private Localize cityNameText;

    [Header("Ball")]
    [SerializeField]
    private Image ballIconImage;
    [SerializeField]
    private Localize ballNameText;

    [Header("Cloud")]
    [SerializeField]
    private Image cloudIconImage;
    [SerializeField]
    private Localize cloudNameText;

    #endregion

    #region Private fields
    private AudioManager audioManager;
    private float timeToStart;
    #endregion

    #region Properties
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Sends an alert when wait time is closed")]
    public UnityEvent OnLevelStarted;
    #endregion

    #region Unity methods
    private void Awake() {
        audioManager = AudioManager.Instance;
    }
    #endregion

    #region Public methods
    public void ShowLevelInfo(PlayScreen playScreen) {
        timeToStart = waitTime;
        timeToStartText.text = waitTime.ToString();

        cityNameText.Term = playScreen.ScreenName;
        cityIconImage.sprite = playScreen.ScreenIcon;

        ballNameText.Term = playScreen.BallName;
        ballIconImage.sprite = playScreen.BallIcon;

        cloudNameText.Term = playScreen.CloudName;
        cloudIconImage.sprite = playScreen.CloudIcon;

        StartCoroutine(StartLevel());
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    private IEnumerator<WaitForSeconds> StartLevel() {
        timeToStart = waitTime;

        while (timeToStart > 0) {
            yield return new WaitForSeconds(1f);

            timeToStart--;
            timeToStartText.text = timeToStart.ToString();

            audioManager.PlayEffect(clockSound);
        }

        OnLevelStarted.Invoke();
    }
    #endregion
}