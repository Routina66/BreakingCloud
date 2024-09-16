using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;
using System.Collections.Generic;
using UnityEngine.Events;
using JetBrains.Annotations;

public class StartlevelInfoBox : MonoBehaviour {
	#region Serialize fields
    /*[SerializeField]
    private AudioClip clockSound;*/
	[SerializeField]
	private TextMeshProUGUI timeToStartText;
    [SerializeField]
    private TextMeshProUGUI numberLevelText;

    [Header("Screen")]
    [SerializeField]
	private Image screenIconImage;
    [SerializeField]
    private Localize screenNameText;

    [Header("Player")]
    [SerializeField]
    private Image playerIconImage;
    [SerializeField]
    private Localize playerNameText;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("Sends an alert when wait time is closed")]
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    public void ShowLevelInfo(PlayScreen playScreen) {
        screenNameText.Term = playScreen.ScreenName;
        screenIconImage.sprite = playScreen.ScreenIcon;

        playerNameText.Term = playScreen.PlayerName;
        playerIconImage.sprite = playScreen.PlayerIcon;

        numberLevelText.text = playScreen.CurrentLevel.ToString();

        gameObject.SetActive(true);
    }

    /*public void HideLevelInfo() {
        gameObject.SetActive(false); 
    }*/
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}