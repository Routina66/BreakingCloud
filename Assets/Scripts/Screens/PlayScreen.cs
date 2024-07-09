using UnityEngine;
using UnityEngine.Events;
using CrazyGames;
 
public class PlayScreen : MonoBehaviour {
    #region Serialize fields
    [Header("Data")]
    [SerializeField]
    private string screenName;
    [SerializeField]
    private Sprite screenIcon;
    [SerializeField]
    private int objectInPlaceReward = 100;
    [SerializeField]
    private int levelSuccessReward = 700;

    [Header("Configuration")]
    [SerializeField]
    private Vector3 cloudPosition;
    [SerializeField]
    private Vector3 ballPosition = Vector3.zero;
    [SerializeField]
    private StartlevelInfoBox startlevelInfoBox;
    [SerializeField]
    private HouseFloor floor;
    [SerializeField]
    private GameObject bricksHolder;
	[SerializeField]
    [Tooltip("The play screen subscribes automatically to the OnPlace event of the bricks with body.")]
	private GameBrick[] bodyBricks;

    [Header("Music")]
    [SerializeField]
    private AudioClip levelPlayMusic;
    [SerializeField]
    private AudioClip levelEndMusic;

    [Header("Effects")]
    [SerializeField]
    private ParticleSystem endLevelEffect;
    [SerializeField]
    private AudioClip bodyBrickBrokenSound;
    [SerializeField]
    private AudioClip levelSuccessSound;
    [SerializeField]
    private AudioClip levelFailedSound;
    #endregion

    #region Private fields
    private GameBrick[] bricks;
    private AudioManager audioManager;
    private LevelReport levelReport;
    private bool inPause;
    private int 
        bodyBrickCount,
        bricksBrokenReward;

    private float
        ballMass = 1,
        cloudSize = 1;

    private PlayObject 
        ball,
        cloud;
    #endregion

    #region Properties
    public bool Pause {
        set {
            inPause = value;

            if (ball != null && cloud != null) {
                if (inPause) {
                    CrazySDK.Game.GameplayStop();

                    ball.Hide();
                    cloud.Hide();
                }
                else {
                    CrazySDK.Game.GameplayStart();

                    ball.Show();
                    cloud.Show();
                }
            }
        }
    }

    public float BallMass {
        set => ballMass = value;
    }

    public float CloudSize {
        set => cloudSize = value; 
    }

    public string ScreenName {
        get => screenName;
    }

    public Sprite ScreenIcon {
        get => screenIcon;
    }

    public string BallName {
        get => ball.Data.Identifier;
    }

    public Sprite BallIcon {
        get => ball.Data.Icon;
    }

    public string CloudName {
        get => cloud.Data.Identifier;
    }

    public Sprite CloudIcon {
        get => cloud.Data.Icon;
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When the level ends it sends the report of the played level.")]
    public UnityEvent<LevelReport> OnLevelEnd;
    [Tooltip("When enter or exits in pause it sends the pause state.")]
    public UnityEvent<bool> OnPaused;
    #endregion

    #region Unity methods
    private void Awake() {
        audioManager = AudioManager.Instance;
        levelReport = new LevelReport();
        levelReport.screen = this;
        inPause = false;
        bricks = bricksHolder.GetComponentsInChildren<GameBrick>();

        foreach (var brick in bodyBricks) {
            brick.OnPlace.AddListener(delegate {
                OnBreakBodyBrick();
            });
        }

        Reset();

        gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            inPause = !inPause;
            
            OnPaused.Invoke(inPause);
        }
    }

    public void Reset() {
        bricksBrokenReward = 0;
        bodyBrickCount = bodyBricks.Length;

        levelReport.success = false;
        levelReport.totalReward = 0;
        levelReport.successReward = 0;
        levelReport.bricksBrokenReward = 0;
        levelReport.objectsInPlaceReward = 0;

        if (!bricksHolder.activeSelf) {
            bricksHolder.SetActive(true);
        }

        foreach (var brick in bricks) {
            brick.Reset();
        }
    }
    #endregion

    #region Public methods
    public void OnBreakBrick(GameBrick brick) {
        bricksBrokenReward += brick.Reward;
    }

    public void OnBreakBodyBrick() {
        bodyBrickCount--;

        audioManager.PlayEffect(bodyBrickBrokenSound);

        if (bodyBrickCount == 0) {
            EndLevel(true);
        }
    }

    public void OnBallOnFloor() {
        floor.OnObjectOnFloor.RemoveListener(OnBallOnFloor);

        EndLevel(false);
    }

    public void Play(PlayObject theBall, PlayObject theCloud) {
        floor.OnObjectOnFloor.AddListener(OnBallOnFloor);

        ball = theBall;
        cloud = theCloud;

        ball.transform.parent = transform;
        ball.transform.localPosition = ballPosition;
        ball.transform.localRotation = Quaternion.identity;
        ball.Mass = ballMass;
        
        cloud.transform.parent = transform;
        cloud.transform.localPosition = cloudPosition;
        cloud.transform.localRotation = Quaternion.identity;
        cloud.Size = cloudSize;

        Pause = true;

        CrazySDK.Game.GameplayStart();

        gameObject.SetActive(true);
        audioManager.PlayMusic(levelPlayMusic);
        startlevelInfoBox.ShowLevelInfo(this);
    }

    

    public void ResetBall() {
        ball.Velocity = Vector3.zero;
        ball.transform.position = ballPosition;
        
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    private void EndLevel(bool levelSuccess) {
        ballMass = 1f;
        cloudSize = 1f;

        levelReport.ball = ball;
        levelReport.cloud = cloud;
        levelReport.success = levelSuccess;
        levelReport.bricksBrokenReward = bricksBrokenReward;
        levelReport.successReward = levelSuccess ? levelSuccessReward : 50;
        levelReport.objectsInPlaceReward = 
            (bodyBricks.Length - bodyBrickCount) * objectInPlaceReward;

        levelReport.totalReward =
            bricksBrokenReward + levelReport.successReward 
            + levelReport.objectsInPlaceReward;

        CrazySDK.Game.GameplayStop();

        endLevelEffect.Play();
        audioManager.PlayMusic(levelEndMusic);
        cloud.gameObject.SetActive(false);
        bricksHolder.SetActive(false);

        if (levelSuccess) {
            audioManager.PlayEffect(levelSuccessSound);
            ball.gameObject.SetActive(false);
        }
        else {
            audioManager.PlayEffect(levelFailedSound);
        }
        
        OnLevelEnd.Invoke(levelReport);
    }
    #endregion

    #region Coroutines
    #endregion
}