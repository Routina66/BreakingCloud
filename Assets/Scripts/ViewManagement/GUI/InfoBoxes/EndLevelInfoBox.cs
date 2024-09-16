using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndLevelInfoBox : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    private TextMeshProUGUI labelSuccess;
    [SerializeField]
    private TextMeshProUGUI labelFailed;
    [SerializeField]
	private Button viewVideoButton;
    //[SerializeField]
	//private MoneyInfoBox briksBrokenReward;
    [SerializeField]
    private MoneyInfoBox moneyCollected;
    [SerializeField]
	private MoneyInfoBox levelEndReward;
	[SerializeField]
	private MoneyInfoBox totalReward;
	#endregion

	#region Private fields
	private LevelReport levelReport;
	#endregion

	#region Properties
	public LevelReport LevelReport {
		get => levelReport;
	}
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("When it shows the end level information, sends an event.")]
    //public UnityEvent OnShowEndLevelInfo;
    #endregion

    #region Unity methods
    /*private void OnEnable() {
        OnShowEndLevelInfo.Invoke();
    }*/
    #endregion

    #region Public methods
    public void ShowInfo(LevelReport report) {
		levelReport = report;

		if (!gameObject.activeSelf) {
            viewVideoButton.interactable = true;

            gameObject.SetActive(true);
        }

        if (report.levelSuccess) {
            labelSuccess.enabled = true;
            labelFailed.enabled = false;
        }
        else {
            labelSuccess.enabled = false;
            labelFailed.enabled = true;
        }

		moneyCollected.ShowMoneyAmount(
			MoneyType.GameMoney, levelReport.moneyCollected);

        levelEndReward.ShowMoneyAmount(
            MoneyType.GameMoney, levelReport.levelEndReward);

        totalReward.ShowMoneyAmount(
            MoneyType.GameMoney, levelReport.totalReward);
    }

    public void ClearInfo() {
        //briksBrokenReward.ShowMoneyAmount(MoneyType.GameMoney, 0);
		moneyCollected.ShowMoneyAmount(MoneyType.GameMoney, 0);
        levelEndReward.ShowMoneyAmount(MoneyType.GameMoney, 0);
		totalReward.ShowMoneyAmount(MoneyType.GameMoney, 0);
	}
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
	#endregion

	#region Coroutines
	#endregion
}