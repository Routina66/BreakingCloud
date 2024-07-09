using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// In the shop the ball can sell and buy their PlayObjects. 
/// </summary>
public class Shop : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    [Tooltip("If true, the destination can only sell a PlayObject if the origin has got enough money. Otherwise, the destination can allways sell PlayObjects.")]
    private bool chargeToShop;
    [SerializeField]
    [Tooltip("It will play when a transaction is success.")]
    private Animator shopAnimator;
    [SerializeField]
    [Tooltip("Shows the information of a PlayOject when it has being bought or sold.")]
    private PlayObjectInfoBox transactionObjectInfobox;
    [SerializeField]
    [Tooltip("Shows the price of a PlayOvject witch has being bought or sold.")]
    private TextMeshProUGUI transactionMoneyAmount;
    [SerializeField]
    [Tooltip("Shows the icon of the money type selected in a puchase or a sale.")]
    private Image transactionMoneyIcon;
    [SerializeField]
    [Tooltip("It will show when a transaction is failed.")]
    private Window transactionErrorWindow;
    


    [Header("Stores")]
    [SerializeField]
    private GameStoreTab shopStoreTab;
    [SerializeField]
    private GameStoreTab playerStoreTab;
    [SerializeField]
    [Tooltip("Must hava a MoneyInfoBox component.")]
    private Button[] buyButttons;
    [SerializeField]
    [Tooltip("Must have a MoneyInfoBox component.")]
    private Button[] sellButtons;
    #endregion

    #region Private fields
    private PlayObjectData 
        selectedData,
        equipedCloud,
        equipedBall;
    private MoneyType selectedMoneyType;
    private GameStore
        playerStore,
        shopStore;
    #endregion

    #region Properties
    public GameStore ShopStore {
        set {
            shopStore = value;
            shopStoreTab.GameStore = value;
        }
    }

    public GameStore PlayerStore {
        set {
            playerStore = value;
            playerStoreTab.GameStore = value;
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Sends the data of the equiped PlayObject.")]
    public UnityEvent<PlayObjectData> OnPlayObjectEquiped;
    [Tooltip("Sends the data of the bought PlayObject.")]
    public UnityEvent<PlayObjectData> OnPlayObjectBought;
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameData"></param>
    public void LoadGameData(GameData gameData) {
        shopStore = shopStoreTab.GameStore;
        playerStore = playerStoreTab.GameStore;

        equipedBall = playerStore.GetObjectData(gameData.equipedBall);
        equipedCloud = playerStore.GetObjectData(gameData.equipedCloud);
        
        playerStoreTab.LoadStore();
        shopStoreTab.LoadStore();
    }

    /// <summary>
    /// When a PlayObject is selected its shows the
    /// prices of the selected PlayObject.
    /// </summary>
    /// 
    /// <param name="playObjectData">The data of selectred PlayObject.</param>
    public void OnSelectShopPlayObject(PlayObjectData playObjectData) {
        SelectPlayObjectData(playObjectData, buyButttons, playerStore);
        //ShowSelectedDataPrices(buyButttons, playerStore);
    }

    /// <summary>
    /// When a PlayObject is selected its shows the
    /// prices of the selected PlayObject.
    /// </summary>
    /// 
    /// <param name="playObjectData">The data of selectred PlayObject.</param>
    public void OnSelectPlayerPlayObject(PlayObjectData playObjectData) {
        SelectPlayObjectData(playObjectData, sellButtons, chargeToShop ? shopStore : null);
        //ShowSelectedDataPrices(sellButtons, chargeToShop ? shopStore : null);
    }

    /// <summary>
    /// When a buy button is pressed it shows its data in the
    /// TransactionInfoBox to confirm the transaction.
    /// </summary>
    /// 
    /// <param name="moneyInfoBox">The pressed MoneyButton.</param>
    public void OnPressMoneyButton(MoneyInfoBox moneyInfoBox) {
        if (selectedData != null) {
            selectedMoneyType = moneyInfoBox.MoneyType;

            transactionObjectInfobox.gameObject.SetActive(true);
            transactionObjectInfobox.ShowInfo(selectedData);

            transactionMoneyIcon.sprite = moneyInfoBox.MoneyIcon;
            transactionMoneyAmount.text = 
                selectedData.GetPrice(selectedMoneyType).ToString();
        }
    }

    /// <summary>
    /// The ball buys the selected PlayObject to the shop.
    /// </summary>
    public void OnBuySelectedObject() {
        if (ExchangeSelectedObject(shopStore, playerStore)) {
            ChargeSelectedObject(chargeToShop ? shopStore : null, playerStore);

            OnPlayObjectBought.Invoke(selectedData);
        }
    }

    /// <summary>
    /// The ball sells the selected PlayObject from the shop.
    /// </summary>
    public void OnEquipSelectedObject() {
        switch (selectedData.Type) {
            case PlayObjectType.Cloud:
                equipedCloud = selectedData;
                break;
            case PlayObjectType.Ball:
                equipedBall = selectedData;
                break;
        }

        OnPlayObjectEquiped.Invoke(selectedData);
        /*if (ExchangeSelectedObject(playerStore, shopStore)) {
            ChargeSelectedObject(playerStore, chargeToShop? shopStore : null);
        }*/
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// Selects the data of a PlayObject.
    /// </summary>
    /// 
    /// <param name="playObjectData">The data of the PlayObject.</param>
    private void SelectPlayObjectData(PlayObjectData playObjectData, Button[] buttons, GameStore gameStore) {
        MoneyInfoBox moneyInfoBox;
        MoneyType moneyType;
        int price;

        selectedData = playObjectData;

        foreach (var button in buttons) {
            if (selectedData != null && IsEquiped(selectedData)) {
                if (button.gameObject.activeSelf) {
                    button.gameObject.SetActive(false);
                }
            }
            else {
                moneyInfoBox = button.GetComponentInChildren<MoneyInfoBox>(true);
                moneyType = moneyInfoBox.MoneyType;
                price = selectedData == null ? -1 : selectedData.GetPrice(moneyType);

                if (price < 0) {
                    price = 0;
                    button.interactable = false;
                }
                else {
                    button.interactable =
                            gameStore == null || gameStore.GetMoneyQuantity(moneyType) >= price;
                }

                if (!button.gameObject.activeSelf) {
                    button.gameObject.SetActive(true);
                }

                moneyInfoBox.ShowMoneyAmount(moneyType, price);
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="store">The store.</param>
    private bool IsEquiped(PlayObjectData playObjectData) {
        return 
            (equipedCloud != null && equipedCloud.Identifier.Equals(playObjectData.Identifier)) 
            || (equipedBall != null && equipedBall.Identifier.Equals(playObjectData.Identifier));
    }

    /// <summary>
    /// Buy the selected PlayObject from the origin
    /// to the destination.
    /// </summary>
    /// <param name="origin">The origin.</param>
    /// <param name="destination">The destination.</param>
    private bool ExchangeSelectedObject(GameStore origin, GameStore destination) {
        bool exchangeOk = false;

        if (selectedData != null) {
            var selectedObject = origin.GetPlayObject(selectedData);

            if (selectedObject != null) {
                exchangeOk = true;

                destination.AddPlayObject(selectedObject);

                if (shopAnimator.runtimeAnimatorController != null) {
                    shopAnimator.Play("TransactioOkAnimation");
                }
            }
        }

        if (!exchangeOk) {
            transactionErrorWindow.Open();
        }

        Invoke(nameof(HideTransactionInfoBox), 2.5f);

        return exchangeOk;
    }

    /// <summary>
    /// Charges the selected obect from the customer to the seller.
    /// </summary>
    /// <param name="seller">If null, the money will not be added.</param>
    /// <param name="customer">If null, the money will not be charged.</param>    
    private void ChargeSelectedObject(GameStore seller, GameStore customer) {
        int price = selectedData.GetPrice(selectedMoneyType);

        if (customer != null) {
            customer.SubstractMoney(selectedMoneyType, price);
        }

        if (seller != null) {
            seller.AddMoney(selectedMoneyType, price);
        }
    }

    private void HideTransactionInfoBox() {
        transactionObjectInfobox.ClearInfo();
        transactionObjectInfobox.gameObject.SetActive(false);
    }
    #endregion

    #region Coroutines
    #endregion
}