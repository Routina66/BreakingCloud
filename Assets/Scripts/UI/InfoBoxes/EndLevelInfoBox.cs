using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class EndLevelInfoBox : MonoBehaviour {
	#region Serialize fields
	[SerializeField]
	private MoneyInfoBox briksBrokenReward;
    [SerializeField]
    private MoneyInfoBox objectsInPlaceReward;
    [SerializeField]
	private MoneyInfoBox levelSuccesReward;
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
	//[Tooltip("")]
	#endregion

	#region Unity methods
	#endregion

	#region Public methods
	public void ShowInfo(LevelReport report) {
		levelReport = report;

        briksBrokenReward.ShowMoneyAmount(
            MoneyType.GameMoney, levelReport.bricksBrokenReward);

        objectsInPlaceReward.ShowMoneyAmount(
            MoneyType.GameMoney, levelReport.objectsInPlaceReward);

        levelSuccesReward.ShowMoneyAmount(
            MoneyType.GameMoney, levelReport.successReward);

        totalReward.ShowMoneyAmount(
            MoneyType.GameMoney, levelReport.totalReward);
    }

    public void ClearInfo() {
		briksBrokenReward.ShowMoneyAmount(MoneyType.GameMoney, 0);
		objectsInPlaceReward.ShowMoneyAmount(MoneyType.GameMoney, 0);
        levelSuccesReward.ShowMoneyAmount(MoneyType.GameMoney, 0);
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