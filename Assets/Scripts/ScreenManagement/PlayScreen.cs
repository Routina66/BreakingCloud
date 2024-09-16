//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayScreen : GameScreen {
    #region Readonly fields
    //When the player uses a power-up it will mark bombsCount * powerUpMultiplier bombs.
    private readonly float powerUpMultiplier = .05f;
    #endregion

    #region Serialize fields
    [Header("Controls")]
    [SerializeField]
    private MouseClickObserver mouseClickObserver;

    [Header("Rewards")]
    [SerializeField]
    private int levelSuccessReward = 1500;
    [SerializeField]
    private int levelFailedReward = 200;

    [Header("Cameras")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Vector3 playerCameraPosition;
    [SerializeField]
    private Vector3 playerCameraRotation;

    [Header("Configuration")]
    [SerializeField]
    [Tooltip("The stage of the game-.")]
    private Transform stage;
    [SerializeField]
    [Tooltip("Maximun number of times that the player can continue playing after to fail the level.")]
    private int continues = 1;
    [SerializeField]
    [Tooltip("Tiles by side on the first level")]
    private int initialTilesSide;
    [SerializeField]
    [Tooltip("Density of bombPrefab on the firs level.")]
    [Range(0.1f, .8f)]
    private float initialBombsDensity;
    [SerializeField]
    [Tooltip("Every few levels a tile is added to the sides of the country.")]
    private int adTileLevels;
    [SerializeField]
    [Tooltip("The size of any tile. All tiles must be the same size.")]
    private float tileSize = 1f;
    [SerializeField]
    [Tooltip("Time, in seconds, to put all the tiles on the country.")]
    private float loadTime = 5f;
    [SerializeField]
    private Vector3 initialPlayerPosition;
    [SerializeField]
    private Vector3 initialTilePosition;
    [SerializeField]
    [Tooltip("Effects of bombPrefab explosions.")]
    private Bomb bombPrefab;
    [Tooltip("Each play, it will select one or more random themes.")]
    [SerializeField]
    private TileTheme[] tileThemes;

    [Header("GUI options")]
    [SerializeField]
    private Window continueWindow;
    [SerializeField]
    private Window powerUpShopWindow;
    [SerializeField]
    private StartlevelInfoBox startlevelInfoBox;
    [SerializeField]
    private GameObject playLevelInfoBox;
    [SerializeField]
    private EndLevelInfoBox endlevelInfoBox;
    [SerializeField]
    private Button rewardedVideoButton;
    [SerializeField]
    private TextMeshProUGUI bombsCountText;


    [Header("Music")]
    [SerializeField]
    private AudioClip levelPlayMusic;
    [SerializeField]
    private AudioClip levelEndMusic;

    [Header("Effects")]
    [SerializeField]
    private ParticleSystem endLevelEffect;
    [SerializeField]
    private AudioClip levelSuccessSound;
    [SerializeField]
    private AudioClip levelFailedSound;
    #endregion

    #region Private fields
    private LevelReport levelReport;
    private bool inPause;
    private PlayObject player;
    private Tile[,] country;
    private int 
        currentLevel,
        usedContinues,
        tilesBySide,
        //bombsCount,
        bombsMarkCount;
    #endregion

    #region Properties
    public int BombsCount {
        get => int.Parse(bombsCountText.text);
        set => bombsCountText.text = value.ToString();
    }

    public bool Pause {
        get => inPause;
        set {
            inPause = value;

            player.Show(!inPause);

            playerCamera.gameObject.SetActive(!inPause);
            mainCamera.gameObject.SetActive(inPause);
        }
    }
    
    public string PlayerName {
        get => player.Data.Identifier;
    }

    public Sprite PlayerIcon {
        get => player.Data.Icon;
    }

    public int CurrentLevel {
        get => currentLevel;
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When a tile is exposed, it sends the tile.")]
    public UnityEvent<Tile> OnExposeTile;
    [Tooltip("When a tile is marked or dismarked, it sends the tile.")]
    public UnityEvent<Tile> OnMarkTile;
    [Tooltip("When a bomb explodes, it sends the bomb.")]
    public UnityEvent<Bomb> OnExplodeBomb;
    [Tooltip("When enter or exits in pause, it sends the pause state.")]
    public UnityEvent<bool> OnPause;
    [Tooltip("When the player wants to multiply the reward, it sends an event.")]
    public UnityEvent<int> OnRequestMultiplyReward;
    [Tooltip("When the player wants to use a power up, it sends de power up data.")]
    public UnityEvent<PlayObjectData> OnRequestPowerUp;
    [Tooltip("When the player wants to view a video to gets a power up, it sends an event.")]
    public UnityEvent OnRequestPowerUpVideo;
    [Tooltip("When the player wants to view a video to continue a failed game, it sends an event.")]
    public UnityEvent OnRequestContinuePlay;
    [Tooltip("When it shows the end level information, sends an event.")]
    public UnityEvent OnShowEndLevelInfo;
    [Tooltip("When the player wants to get the the reward, it sends the report of the played level.")]
    public UnityEvent<LevelReport> OnEndLevel;
    
    #endregion

    #region Unity methods
    private void Awake() {
        levelReport = new LevelReport {
            screen = this,
            levelPlayed = currentLevel
        };

        mainCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
    }

    /*private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            inPause = !inPause;
            
            OnPause.Invoke(inPause);
        }
    }*/


    #endregion

    #region Public methods
    public void Play(PlayObject thePlayer, int level) {
        float bombsDensity =
            Mathf.Min(1, initialBombsDensity + (level % adTileLevels / 100));

        tilesBySide = initialTilesSide + Mathf.FloorToInt(level / adTileLevels);
        BombsCount = Mathf.FloorToInt(tilesBySide * tilesBySide * bombsDensity);
        country = new Tile[tilesBySide, tilesBySide];
        player = thePlayer;

        player.transform.parent = stage;
        player.transform.localPosition = initialPlayerPosition;
        player.transform.localRotation = Quaternion.identity;

        levelReport.player = player;
        levelReport.levelPlayed = currentLevel;

        playerCamera.transform.parent = player.transform;
        playerCamera.transform.localPosition = playerCameraPosition;
        playerCamera.transform.localEulerAngles = playerCameraRotation;

        Pause = true;

        powerUpShopWindow.Open();

        StartCoroutine(StartPlay());
    }

    /*public void Play() {
        StartCoroutine(StartPlay());

        Debug.Log($"Play started. Tiles by size: {tilesBySide}, bombPrefab {BombsCount}");
    }*/

    public void EndPlay(bool levelSuccess) {
        Pause = false;

        playerCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        playerCamera.transform.parent = transform;

        levelReport.levelSuccess = levelSuccess;
        levelReport.levelEndReward = levelSuccess ?
            levelSuccessReward : levelFailedReward;

        levelReport.totalReward =
            levelReport.levelEndReward + levelReport.moneyCollected;

        if (levelSuccess) {
            currentLevel++;
        }

        if (endLevelEffect != null) {
            endLevelEffect.Play();
        }

        endlevelInfoBox.ShowInfo(levelReport);

        OnShowEndLevelInfo.Invoke();
    }

    /*public void EndPlay() {
        bool levelSuccess = levelReport.levelSuccess;

        Pause = false;

        levelReport.levelEndReward = levelSuccess ?
            levelEndReward : levelFailedReward;

        levelReport.totalReward =
            levelReport.levelEndReward;

        if (levelSuccess) {
            currentLevel++;
        }

        if (endLevelEffect != null) {
            endLevelEffect.Play();
        }

        endlevelInfoBox.ShowInfo(levelReport);

        OnShowEndLevelInfo.Invoke();
    }*/

    public void RequestMultiplyReward(int multiplier) {
        rewardedVideoButton.interactable = false;

        OnRequestMultiplyReward.Invoke(multiplier);
    }

    public void MultiplyReward(int multiplier) {
        levelReport.levelEndReward *= multiplier;
        levelReport.totalReward *= multiplier;

        endlevelInfoBox.ShowInfo(levelReport);
    }

    public void GetReward() {
        StartCoroutine(ResetPlayScreen());

        //gameObject.SetActive(false);
        OnEndLevel.Invoke(levelReport);
    }

    public void RequestContinuePlay() {
        OnRequestContinuePlay.Invoke();
    }

    public void RequestPowerUp(PlayObjectData powerUpData) {
        OnRequestPowerUp.Invoke(powerUpData);
    }

    public void ReequestPowerUpVideo() {
        OnRequestPowerUpVideo.Invoke();
    }

    public void UsePowerUp(PlayObject powerUp) {
        bombsMarkCount = Mathf.CeilToInt(BombsCount * powerUpMultiplier);

        Destroy(powerUp.gameObject);

        Pause = false;
    }

    public void ExposeTile(Tile tile) {
        int coordX = (int)tile.Coords.x;
        int coordY = (int)tile.Coords.y;
        int bombsAround = 0;
        bool yDivisibleTwo = coordY % 2 == 0;
        List<Tile> tilesAround = new List<Tile>();

        levelReport.moneyCollected += tile.Reward;
        Debug.Log($"Exposing tile: {coordX}, {coordY}");

        //Add coordY tiles
        if (coordX > 0) {
            tilesAround.Add(country[coordX - 1, coordY]);
        }

        if (coordX < tilesBySide - 1) {
            tilesAround.Add(country[coordX + 1, coordY]);
        }

        //Add coordY + 1 tiles 
        if (coordY < tilesBySide - 1) {
            tilesAround.Add(country[coordX, coordY + 1]);

            if (coordX > 0 && yDivisibleTwo) {
                tilesAround.Add(country[coordX - 1, coordY + 1]);
            }

            if (coordX < tilesBySide - 1 && !yDivisibleTwo) {
                tilesAround.Add(country[coordX + 1, coordY + 1]);
            }
        }

        //Add coordY - 1 tiles
        if (coordY > 0) {
            tilesAround.Add(country[coordX, coordY - 1]);

            if (coordX > 0 && yDivisibleTwo) {
                tilesAround.Add(country[coordX - 1, coordY - 1]);
            }

            if (coordX < tilesBySide - 1 && !yDivisibleTwo) {
                tilesAround.Add(country[coordX + 1, coordY - 1]);
            }
        }

        Debug.Log("Tiles around: ");

        foreach (Tile aTile in tilesAround) {
            Debug.Log($"    {aTile.Coords}, has bomb: {aTile.HasBomb}");
            if (aTile.HasBomb) {
                bombsAround++;
            }
        }

        tile.BombAround = bombsAround;

        if (bombsAround == 0) {
            foreach (Tile aTile in tilesAround) {
                if (!aTile.IsMarked && !aTile.IsExposed) {
                    aTile.Expose();
                }
            }
        }

        OnExposeTile.Invoke(tile);
    }

    public void MarkTile(Tile tile) {
        if (tile.IsMarked) {
            BombsCount--;

            if (BombsCount == 0) {
                EndPlay(true);
            }
        }
        else {
            BombsCount++;
        }

        OnMarkTile.Invoke(tile);
    }

    public void ExplodeBomb(Bomb bomb) {
        //bomb.OnExplode.RemoveAllListeners();

        Pause = true;
        BombsCount--;

        if (usedContinues < continues) {
            usedContinues++;

            continueWindow.Open();
        }
        else {
            EndPlay(false);
        }

        OnExplodeBomb.Invoke(bomb);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods


    #endregion

    #region Coroutines
    private IEnumerator<WaitForSeconds> StartPlay() {
        Tile currentTile;
        float deltaLoadTime = loadTime * Time.deltaTime;
        Tile[] tilePrefabs = 
            tileThemes[Random.Range(0, tileThemes.Length)].TilePrefabs;
        Vector2 bombCoords = new Vector2(
            Random.Range(0, tilesBySide), Random.Range(0, tilesBySide));
        Vector3 currentTilePosition = new Vector3(
            initialTilePosition.x,
            initialTilePosition.y,
            initialTilePosition.z);

        while (inPause) {
            yield return null;// new WaitForSeconds(1f);
        }

        mainCamera.gameObject.SetActive(true);
        player.Show(false);

        //playerCamera.gameObject.SetActive(false);

        powerUpShopWindow.Close();
        startlevelInfoBox.ShowLevelInfo(this);
        
        for (int y = 0; y < tilesBySide; y++) {
            currentTilePosition.x = (float)y % 2 / 2;

            for (int  x = 0; x < tilesBySide; x++) {
                currentTile = Instantiate(
                    tilePrefabs[Random.Range(0, tilePrefabs.Length)], stage);

                mouseClickObserver.AddListener(currentTile);

                currentTile.OnExpose.AddListener(ExposeTile);
                currentTile.OnMark.AddListener(MarkTile);
                
                country[x, y] = currentTile;

                currentTile.transform.position = currentTilePosition;
                currentTile.Coords = new Vector2(x, y);

                yield return new WaitForSeconds(deltaLoadTime);

                currentTilePosition.x += tileSize;
            }

            currentTilePosition.z += tileSize;
        }

        for (int i = 0; i < BombsCount; i++) {
            currentTile = country[(int)bombCoords.x, (int)bombCoords.y];

            while (currentTile.HasBomb || bombCoords == Vector2.zero) {
                bombCoords.x = Random.Range(0, tilesBySide); 
                bombCoords.y = Random.Range(0, tilesBySide);

                currentTile = country[(int)bombCoords.x, (int)bombCoords.y];
            }

            currentTile.Bomb = Instantiate(bombPrefab, currentTile.transform);

            currentTile.Bomb.OnExplode.AddListener(ExplodeBomb);

            if (bombsMarkCount > 0) {
                bombsMarkCount--;

                currentTile.Mark();
            }
        }

        player.Show(true);

        playerCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);

        startlevelInfoBox.gameObject.SetActive(false);
        playLevelInfoBox.SetActive(true);
    }

    private IEnumerator ResetPlayScreen() {
        yield return null;

        inPause = false;
        usedContinues = 0;
        bombsMarkCount = 0;
        
        bombsCountText.text = string.Empty;

        if (!rewardedVideoButton.interactable) {
            rewardedVideoButton.interactable = true;
        }

        if (country != null) {
            for (int i = 0; i < tilesBySide; i++) {
                for (int j = 0; j < tilesBySide; j++) {
                    country[i, j].OnExpose.RemoveAllListeners();
                    country[i, j].OnMark.RemoveAllListeners();

                    if (country[i, j].HasBomb) {
                        country[i, j].Bomb.OnExplode.RemoveAllListeners();
                    }

                    mouseClickObserver.RemoveListener(country[i, j]);

                    DestroyImmediate(country[i, j].gameObject);

                    yield return null;
                }
            }
        }

        levelReport.levelSuccess = false;
        levelReport.totalReward = 0;
        levelReport.moneyCollected = 0;
        levelReport.levelEndReward = 0;

        playLevelInfoBox.SetActive(false);

        Debug.Log("Play screen reseted.");
    }
    #endregion
}