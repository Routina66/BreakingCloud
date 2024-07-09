using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerUpTab : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    private GameStore playerInventory;
    [SerializeField]
    private MoneyInfoBox playerMoneyInfoBox;

    [Header("Big cloud options")]
	[SerializeField]
	private int bigCloudPrice = 500;
    [SerializeField]
    private Button bigCloudButton;
    [SerializeField]
    private MoneyInfoBox bigCloudMoneyInfoBox;
    [SerializeField]
    private GameObject bigCloudBoughtObject;

    [Header("Heavy ball options")]
    [SerializeField]
	private int heavyBallPrice = 1000;
    [SerializeField]
    private Button heavyBallButton;
    [SerializeField]
    private MoneyInfoBox heavyBallMoneyInfoBox;
    [SerializeField]
    private GameObject heavyBallBoughtObject;
    #endregion

    #region Private fields
    private bool 
        bigCloudBougth = false, 
        heavyBallBougth = false;
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    private void OnEnable() {
        int playerMoney = playerInventory.GetMoneyQuantity(MoneyType.GameMoney);

        playerMoneyInfoBox.ShowMoneyAmount(MoneyType.GameMoney, playerMoney);

        bigCloudButton.interactable = 
            playerMoney >= bigCloudPrice && !bigCloudBougth;
        
        heavyBallButton.interactable = 
            playerMoney >= heavyBallPrice && !heavyBallBougth;

        bigCloudMoneyInfoBox.ShowMoneyAmount(MoneyType.GameMoney, bigCloudPrice);
        heavyBallMoneyInfoBox.ShowMoneyAmount(MoneyType.GameMoney, heavyBallPrice);
    }

    public void Reset() {
        bigCloudBougth = false;
        heavyBallBougth = false;

        bigCloudBoughtObject.SetActive(false);
        heavyBallBoughtObject.SetActive(false);
    }
    #endregion

    #region Public methods
    public void BuyBigCloud() {
        bigCloudBougth = true;

        bigCloudBoughtObject.SetActive(true);
        playerMoneyInfoBox.ShowMoneyAmount(
            MoneyType.GameMoney, 
            playerInventory.SubstractMoney(MoneyType.GameMoney, bigCloudPrice));
    }

    public void BuyHeavyBall() {
        heavyBallBougth = true;

        heavyBallBoughtObject.SetActive(true);
        playerMoneyInfoBox.ShowMoneyAmount(
            MoneyType.GameMoney,
            playerInventory.SubstractMoney(MoneyType.GameMoney, heavyBallPrice));
    }

    
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}