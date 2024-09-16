using UnityEngine;

public class ScreenManager : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    private float changeSceneDelay = 2f;
    [SerializeField]
    private GameScreen mainScreen;
	[SerializeField]
	private PlayScreen[] playScreens;
    #endregion

    #region Private fields
    //private PlayObject selectedPlayer;
	private PlayScreen selectedPlayScreen;
	#endregion

	#region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    public void Awake() {
        selectedPlayScreen = playScreens[0];
    }
    #endregion

    #region Public methods
    public void PlayLevel(PlayObject player, int level) {
        mainScreen.Show(false);

        selectedPlayScreen.Show(true);
        //selectedPlayScreen.ResetPlayScreen();
        selectedPlayScreen.Play(player, level);

        /*if (selectedPlayScreen.Pause) {
            selectedPlayScreen.Play(level);
        }
        else {
            selectedPlayScreen.ResetPlayScreen();
            selectedPlayScreen.Play(player, level);
        }*/
    }

    public void EndCurrentLevel() {
        Invoke(nameof(ShowMainScreen), changeSceneDelay);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    private void ShowMainScreen() {
        selectedPlayScreen.Show(false);
        mainScreen.Show(true);
    }
    #endregion

    #region Coroutines
    #endregion
}