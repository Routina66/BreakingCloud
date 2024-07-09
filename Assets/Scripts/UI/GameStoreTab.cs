using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Shows all the content of a GameStore.
/// </summary>

public class GameStoreTab : MonoBehaviour {

    #region Readonly fileds
    #endregion

    #region Serialize fields
    [SerializeField]
    [Tooltip("The playObjectTogglePrefab must to has a PlayObjectInfoBox component.")]
    private Toggle playObjectTogglePrefab;
    [SerializeField]
    private ToggleGroup toggleGroup;
    [SerializeField]
	private GameStore gameStore;
    [SerializeField]
    [Tooltip("Shows the information of a PlayObject when it's selected.")]
    private PlayObjectInfoBox selectedObjectInfoBox;
    [SerializeField]
    [Tooltip("Shows the PlayObjects in the gameStore")]
    private DinamycMultipageNavigation playObjectsPage;
    //[SerializeField]
    //[Tooltip("The PlayObject types shown.")]
    //private List<PlayObjectType> playObjectTypes;
    [SerializeField]
    [Tooltip("Show the amount of the diferent MonyeType in the gameStore.")]
    private MoneyInfoBox[] moneyInfoBoxes;
    #endregion

    #region Private fields
    private Toggle selectedToggle;
    private List<PlayObjectType> selectedPlayObjectTypes;
    private Dictionary<PlayObjectType, List<Toggle>> togglesByType;
    #endregion

    #region Properties
    public GameStore GameStore {
        get => gameStore;
        set {
            ClearGameStoreData();

            gameStore = value;

            ShowGameStoreData();
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Sends the playObjectData of seleceted PlayObject or null if there isn`t selected PlayObject")]
    public UnityEvent<PlayObjectData> OnPlayObjectSelected;
    #endregion

    #region Unity methods
    /*private void Set() {
        var playObjectTypes = Enum.GetValues(typeof(PlayObjectType)).Cast<PlayObjectType>();

        selectedToggle = null;
        selectedPlayObjectTypes = new List<PlayObjectType>(playObjectTypes.Count());
        togglesByType = new Dictionary<PlayObjectType, List<Toggle>>();

        foreach (var type in playObjectTypes) {
            togglesByType.Add(type, new List<Toggle>());
        }

        ShowGameStoreData();
    }*/
    #endregion

    #region Public methods
    /// <summary>
    /// Loads the data of the store and show them.
    /// </summary>
    public void LoadStore() {
        var playObjectTypes = Enum.GetValues(typeof(PlayObjectType)).Cast<PlayObjectType>();

        selectedToggle = null;
        selectedPlayObjectTypes = new List<PlayObjectType>(playObjectTypes.Count());
        togglesByType = new Dictionary<PlayObjectType, List<Toggle>>();

        foreach (var type in playObjectTypes) {
            togglesByType.Add(type, new List<Toggle>());
        }

        ShowGameStoreData();
    }

    /// <summary>
    /// When a toggle is selected, shows the information
    /// of the PlayObject attached to the togle.
    /// </summary>
    /// <param name="toggle">The selected toggle.</param>
    public void OnShopToggleSelect(Toggle toggle) {
        if (toggle.isOn) {
            var data = toggle.GetComponent<PlayObjectInfoBox>().Data;

            selectedToggle = toggle;

            selectedObjectInfoBox.ShowInfo(data);
            OnPlayObjectSelected.Invoke(data);
        }
        else {
            selectedToggle = null;

            selectedObjectInfoBox.ClearInfo();
            OnPlayObjectSelected.Invoke(null);
        }

    }

    public void OnAddPlayObjectToStore(PlayObjectData playObjectData) {
        ShowPlayObjectData(playObjectData);
    }

    public void OnRemovePlayObjectFromStore(PlayObjectData playObjectData) {
        if (selectedToggle != null) {
            playObjectsPage.RemoveToggleFromCurrentPage(selectedToggle);
            togglesByType.GetValueOrDefault(playObjectData.Type).Remove(selectedToggle);
        }
    }

    public void OnChangeGameStoreMoney(MoneyType moneyType, int amount) {
        var moneyInfoBox = Array.Find(moneyInfoBoxes, box => box.MoneyType == moneyType);

        if (moneyInfoBox != null) {
            moneyInfoBox.ShowMoneyAmount(moneyType, amount);
        }
    }


    public void OnTypeToggleChangeValue(PlayObjectTypeToggle typeToggle) {
        if (togglesByType.TryGetValue(typeToggle.playOjectType, out var toggles)) {
            if (selectedToggle != null) {
                selectedToggle.isOn = false;
            }

            foreach (var toggle in toggles) {
                if (typeToggle.IsOn) {
                    selectedPlayObjectTypes.Add(typeToggle.playOjectType);
                    toggle.gameObject.SetActive(true);
                }
                else {
                    selectedPlayObjectTypes.Remove(typeToggle.playOjectType);
                    toggle.gameObject.SetActive(false);
                }
                
            }

            playObjectsPage.SetCurrentPageSize();
        }
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// Clears all playObjectData of the GameStoreTab.
    /// </summary>
    private void ClearGameStoreData() {
        //gameStore.OnPlayObjectAdded.RemoveListener(OnAddPlayObjectToStore);
        //gameStore.OnPlayObjectRemoved.RemoveListener(OnRemovePlayObjectFromStore);
        gameStore.OnMoneyChanged.RemoveListener(OnChangeGameStoreMoney);

        if (selectedToggle != null) {
            selectedToggle.isOn = false;
        }

        playObjectsPage.Clear();
        
        foreach (var playObjectType in togglesByType.Keys) {
            togglesByType.GetValueOrDefault(playObjectType).Clear();
        }
        
        foreach (var moneyInfo in moneyInfoBoxes) {
            moneyInfo.ShowMoneyAmount(moneyInfo.MoneyType, 0);
        }
    }

    /// <summary>
    /// Shows a playObjectData in the playObjectsPage.
    /// </summary>
    /// <param name="playObjectData">The playObjectData.</param>
    private void ShowPlayObjectData(PlayObjectData playObjectData) {
        var storeToggle = Instantiate(playObjectTogglePrefab);
        var infoBox = storeToggle.GetComponent<PlayObjectInfoBox>();

        storeToggle.group = toggleGroup;
        storeToggle.name = playObjectData.Identifier;

        infoBox.ShowInfo(playObjectData);

        togglesByType.GetValueOrDefault(playObjectData.Type).Add(storeToggle);
        toggleGroup.RegisterToggle(storeToggle);

        playObjectsPage.AddToggle(storeToggle);
        
        storeToggle.gameObject.
            SetActive(selectedPlayObjectTypes.Contains(playObjectData.Type));
    }

    /// <summary>
    /// Shows the playObjectData of the content in the gameStore.
    /// </summary>
    private void ShowGameStoreData() {
        //gameStore.OnPlayObjectAdded.AddListener(OnAddPlayObjectToStore);
        //gameStore.OnPlayObjectRemoved.AddListener(OnRemovePlayObjectFromStore);
        gameStore.OnMoneyChanged.AddListener(OnChangeGameStoreMoney);

        ShowGameStoreObjects();
        ShowGameStoreMoney();
    }

    /// <summary>
    /// Add the toggles with playObjectData of the PlayObject from 
    /// the gameStore to the playObjectsPage.
    /// </summary>
    private void ShowGameStoreObjects() {
        foreach (var data in gameStore.PlayObjectsData) {
            ShowPlayObjectData(data);
        }
    }

    /// <summary>
    /// Shows the playObjectData of the diferent moneys in a store.
    /// </summary>
    private void ShowGameStoreMoney() {
        MoneyType moneyType;

        foreach (var moneyInfo in moneyInfoBoxes) {
            moneyType = moneyInfo.MoneyType;

            moneyInfo.ShowMoneyAmount(moneyType, gameStore.GetMoneyQuantity(moneyType));
        }
    }
    #endregion

    #region Coroutines
    #endregion
}