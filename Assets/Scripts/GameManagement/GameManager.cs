using UnityEngine;
using UnityEngine.Events;
using CrazyGames;
using TMPro;

/// <summary>
/// Manage general game data.
///    
/// Save game state to persistent storage.
/// Ñoad game state upon startup. 
/// Laises an event when the game state loads.
/// </summary>
public class GameManager : MonoBehaviour {
    #region Readonly fileds
    private readonly string GameDatakey = "GameStatus";
    #endregion

    #region Serialize fields
    [SerializeField]
    private ObjectsManager objectsManager;
    [SerializeField]
    private ScreenManager screenManager;
    [SerializeField]
    private AdsManager adsManager;
    //[SerializeField]
    //private int rewardMultiplier = 2;
    [SerializeField]
    private TextMeshProUGUI debugText;
    #endregion

    #region Private fields
    private GameStatus gameStatus;
    #endregion

    #region Properties
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Sends the loaded data.")]
    public UnityEvent<GameStatus> OnLoadGameStatus;
    [Tooltip("When the player can multiply the reward, it sends an event")]
    public UnityEvent OnMultiplyReward;
    [Tooltip("When the player can continue playing a failed level, it sends an event")]
    public UnityEvent OnContinuePlay;
    #endregion

    /// <summary>
    /// On awake:
    ///   - It loads the last saved game status.
    ///   - It sends the game status in an event.
    ///   - It initialize de ads.
    /// </summary>
    #region Unity methods
    private void Awake() {
        string data = PlayerPrefs.GetString(GameDatakey, string.Empty);

        gameStatus = new GameStatus();

        if (string.IsNullOrEmpty(data)) {
            SaveGame();
            debugText.text += "New game\n";
        }
        else {
            JsonUtility.FromJsonOverwrite(data, gameStatus);
            debugText.text += "Old game\n";
        }

        debugText.text += "Game data loaded.\n";

        OnLoadGameStatus.Invoke(gameStatus);

        CrazySDK.Init(() => {
            adsManager.InitializeAds();
        }); 
    }

    private void OnDestroy() {
        SaveGame();
    }
    #endregion

    #region Public methods
    public void PlayCurrentLevel() {
        screenManager.PlayLevel(
            objectsManager.SelectedPlayer,
            gameStatus.currentLevel);
    }

    public void FinishLevel(LevelReport levelReport) {
        if (levelReport.levelSuccess && !gameStatus.gameEnded) {
            gameStatus.currentLevel = 
                levelReport.levelPlayed + 1;
        }

        objectsManager.SelectedPlayer = levelReport.player;
        objectsManager.GiveRewardToPlayer(levelReport.totalReward);
        
        screenManager.EndCurrentLevel();
        
        SaveGame();
    }

    public void BuyPlayObject(PlayObjectData playObjectData) {
        gameStatus.playerInventory.Add(playObjectData.Identifier);

        SaveGame();
    }

    public void EquipPlayObject(PlayObjectData playObjectData) {
        gameStatus.equipedObject = playObjectData.Identifier;

        SaveGame();
    }

    public void UpdateMoney(MoneyType moneyType, int newAmount) {
        gameStatus.playerMoney = newAmount;

        SaveGame();
    }

    public void RequestContinuePlay() {
        adsManager.OnEarnReward.AddListener(ContinuePlay);
        adsManager.ShowRewardedAd();
    }

    public void RequestMultiplyReward() {
        adsManager.OnEarnReward.AddListener(MultiplyReward);

        adsManager.ShowRewardedAd();
    }

    public void RequestPowerUp(PlayObject powerUpPrefab) {
        adsManager.OnEarnReward.AddListener( delegate {
            GetPowerUp(powerUpPrefab);
        });

        adsManager.ShowRewardedAd();
    }

    public void GoToLink(string link) {
        Application.OpenURL(link);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /*private void LoadGameStatus() {
        string data = PlayerPrefs.GetString(GameDatakey, string.Empty);

        gameStatus = new GameStatus();

        if (string.IsNullOrEmpty(data)) {
            SaveGame();
            debugText.text += "New game\n";
        }
        else {
            JsonUtility.FromJsonOverwrite(data, gameStatus);
            debugText.text += "Old game\n";
        }

        debugText.text += "Game data initialized.\n";

        OnLoadGameStatus.Invoke(gameStatus);
    }*/

    private void ContinuePlay() {
        adsManager.OnEarnReward.RemoveListener(ContinuePlay);

        OnContinuePlay.Invoke();
    }

    private void MultiplyReward() {
        adsManager.OnEarnReward.RemoveListener(MultiplyReward);

        OnMultiplyReward.Invoke();
    }

    private void GetPowerUp(PlayObject powerUpPrefab) {
        adsManager.OnEarnReward.RemoveListener(delegate {
            GetPowerUp(powerUpPrefab);
        });

        objectsManager.SendPlayObject(Instantiate(powerUpPrefab));
    }

    private void SaveGame() {
        PlayerPrefs.SetString(GameDatakey, JsonUtility.ToJson(gameStatus));
    }
    #endregion

    #region Coroutines
    #endregion
}