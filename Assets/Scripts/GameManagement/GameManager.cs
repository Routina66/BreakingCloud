using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using CrazyGames;
using TMPro;

/// <summary>
/// It controls the game.
/// </summary>
public class GameManager : MonoBehaviour {
    #region Readonly fileds
    private readonly string GameDatakey = "GameData";
    #endregion

    #region Serialize fields
    [SerializeField]
    private TextMeshProUGUI debugText;
    [SerializeField]
    private int breakBrickReward;
    [SerializeField]
    private int screenCount;
    [SerializeField]
    private GameStore playerInventory;
    [SerializeField]
    private GameStore gameInventory;
    #endregion

    #region Private fields
    private GameData gameData;
    #endregion

    #region Properties
    public int CurrentScreen {
        get => gameData.level;
        set => gameData.level = value;
    }

    public int CurrentMoney {
        get => gameData.money;
        set => gameData.money = value;
    }

    public GameStore PlayerInventory {
        get => playerInventory;
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Sends the loaded data.")]
    public UnityEvent<GameData> OnGameDataLoaded;
    [Tooltip("An addblock is detected.")]
    public UnityEvent AddblockDetected;
    #endregion

    #region Unity methods
    private void Awake() {
        CrazySDK.Ad.HasAdblock((adblockActive) => {
            if (adblockActive) {
                AddblockDetected.Invoke();
            }

            //Debug.Log("adblock: " + adblockActive.ToString());
            debugText.text += "adblock: " + adblockActive + "\n";
        });
        
        //Debug.Log("Loading game data.");
        debugText.text += "Loading game data.\n" ;
        playerInventory.Set();
        gameInventory.Set();

        LoadGameData();
        debugText.text += "Game data loaded.\n";
    }

    private void OnDestroy() {
        SaveGame();
    }
    #endregion

    #region Public methods
    public void OnLevelEnd(EndLevelInfoBox infobox) {
        var levelReport = infobox.LevelReport;

        if (levelReport.success && !gameData.GameEnded) {
            CurrentScreen++;

            if (CurrentScreen >= screenCount ) {
                gameData.GameEnded = true;
                CrazySDK.Game.HappyTime();
                Debug.Log("HappyTime");
            }
        }

        levelReport.screen.gameObject.SetActive(false);

        playerInventory.AddPlayObject(levelReport.ball);
        playerInventory.AddPlayObject(levelReport.cloud);
        playerInventory.AddMoney(MoneyType.GameMoney, levelReport.totalReward);
        
        //CurrentMoney = playerInventory.GetMoneyQuantity(MoneyType.GameMoney);
        //gameInventory.SetMoney(MoneyType.GameMoney, gameData.money);

        SaveGame();
    }

    public void OnBuyObject(PlayObjectData playObjectData) {

        switch (playObjectData.Type) {
            case PlayObjectType.Ball:
                gameData.balls.Add(playObjectData.Identifier);
                break;

            case PlayObjectType.Cloud:
                gameData.clouds.Add(playObjectData.Identifier);
                break;
        }

        SaveGame();
    }

    public void OnEquipPlayObject(PlayObjectData playObjectData) {
        switch (playObjectData.Type) {
            case PlayObjectType.Ball:
                gameData.equipedBall = playObjectData.Identifier;
                break;

            case PlayObjectType.Cloud:
                gameData.equipedCloud = playObjectData.Identifier;
                break;
        }

        SaveGame();
    }

    public void OnChangeMoney(MoneyType moneyType, int newAmount) {
        CurrentMoney = newAmount;

        SaveGame();
    }

    public void ShowAd(EndLevelInfoBox infobox) {
        CrazySDK.Ad.RequestAd(
            CrazyAdType.Rewarded,
            null,
            null, 
            () => DuplicateReward(infobox));
    }

    public void GoToLink(string link) {
        Application.OpenURL(link);
    }
    #endregion
    #region Protected methods
    #endregion

    #region Private methods
    private void LoadGameData() {
        string data = PlayerPrefs.GetString(GameDatakey, string.Empty);

        gameData = new GameData();

        if (string.IsNullOrEmpty(data)) {
            SaveGame();
            //Debug.Log("New game");
            debugText.text += "New game\n";
        }
        else {
            JsonUtility.FromJsonOverwrite(data, gameData);
            //Debug.Log("Old game");
            debugText.text += "Old game\n";
        }

        //Debug.Log("Game data loaded");"

        debugText.text += "Game data initialized.\n";

        LoadPlayObjects(gameData.balls);
        LoadPlayObjects(gameData.clouds);

        //Debug.Log("Play objects loaded");
        debugText.text += "Play objects loaded.\nLoading money\n";

        playerInventory.SetMoney(MoneyType.GameMoney, 0);
        debugText.text += "Player money " + playerInventory.GetMoneyQuantity(MoneyType.GameMoney);
        playerInventory.AddMoney(MoneyType.GameMoney, gameData.money);
        debugText.text += "\nPlayer money " + playerInventory.GetMoneyQuantity(MoneyType.GameMoney);

        //Debug.Log("Money loaded");

        debugText.text += "\nMoney loaded.\n";

        OnGameDataLoaded.Invoke(gameData);
    }

    private void LoadPlayObjects(List<string> playObjetIds) {
        PlayObjectData playObjectData;
        if (gameInventory == null) {
            debugText.text += "Game inventory null.\n";
        }

        if (playerInventory == null) {
            debugText.text += "Player inventory null.\n";
        }

        if (playerInventory != null && gameInventory != null) {

            foreach (var id in playObjetIds) {
                playObjectData = gameInventory.GetObjectData(id);

                if (playObjectData == null) {
                    //Debug.Log(id + " is null");
                    debugText.text += id + " is null.\n";
                }
                else {
                    playerInventory.AddPlayObject(
                        gameInventory.GetPlayObject(playObjectData));

                    //Debug.Log(id + " added to player inventory.");
                    debugText.text += id + " added to player inventory.\n";
                }
            }
        }
    }

    private void SaveGame() {
        PlayerPrefs.SetString(GameDatakey, JsonUtility.ToJson(gameData));
    }

    private void DuplicateReward(EndLevelInfoBox infobox) {
        var report = infobox.LevelReport;

        report.successReward *= 2;
        report.bricksBrokenReward *= 2;
        report.objectsInPlaceReward *= 2;
        report.totalReward *= 2;

        infobox.ShowInfo(report);
    }

    /*private void DebugData(string state) {
        Debug.Log(state);
        Debug.Log("- Money " + gameData.money);
        Debug.Log("- Balls:");

        foreach (var ball in gameData.balls) {
            Debug.Log("    " + ball);
        }

        Debug.Log("- Clouds:");

        foreach (var cloud in gameData.clouds) {
            Debug.Log("    " + cloud);
        }

        Debug.Log("Equiped ball: " + gameData.equipedBall);
        Debug.Log("Equiped cloud: " + gameData.equipedCloud);
    }*/
    #endregion

    #region Coroutines
    #endregion
}