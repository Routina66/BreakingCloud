using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionScreen : MonoBehaviour {
	#region Serialize fields
	[SerializeField]
	private GameManager gameManager;
    /*[SerializeField]
    private PlayObject defautlSelectedBall;
    [SerializeField]
    private PlayObject defautlSelectedCloud;*/
    [SerializeField]
	private Window pauseWindow;
	[SerializeField]
	private LockButton[] playScreenButtons;
	#endregion

	#region Private fields
	private PlayScreen selectedScreen;
    private GameStore playerInventory;
    private PlayObjectData
		selectedBallData,
		selectedCloudData;
	#endregion

	#region Properties
	public PlayScreen SelectedPlayScreen {
		set => selectedScreen = value;
	}

	public float CloudSize {
		set => selectedScreen.CloudSize = value;
	}

	public float BallMass {
		set => selectedScreen.BallMass = value;
	}
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /*private void Set() {
		selectedBallData = defautlSelectedBall.Data;
		selectedCloudData = defautlSelectedCloud.Data;
		playerInventory = gameManager.PlayerInventory;
    }*/

    private void OnEnable() {
		int currentScreen = gameManager.CurrentScreen;

		for (int i = 0; i < playScreenButtons.Length; i++) {
			playScreenButtons[i].locked = i >= currentScreen;
		}
    }
	#endregion

	#region Public methods
	public void OnSelectPlayObject(PlayObjectData playObjectData) {
		switch (playObjectData.Type) {
			case PlayObjectType.Ball:
                selectedBallData = playObjectData;
				break;
			case PlayObjectType.Cloud:
                selectedCloudData = playObjectData;
                break;
        }
	}

	public void LoadGameData(GameData gameData) {
        playerInventory = gameManager.PlayerInventory;

        selectedBallData = playerInventory.GetObjectData(gameData.equipedBall);
        selectedCloudData = playerInventory.GetObjectData(gameData.equipedCloud);
    }

    public void PlaySelectPlayScreen() {
        selectedScreen.Play(
            playerInventory.GetPlayObject(selectedBallData),
            playerInventory.GetPlayObject(selectedCloudData));

        gameObject.SetActive(false);
    }

	public void ResetSelectedPlayScreen() {
		selectedScreen.Reset();
	}

	public void PauseSelectedPlayScreen(bool pause) {
		selectedScreen.Pause = pause;

		if (pause) {
			pauseWindow.Open();
		}
		else {
			pauseWindow.Close();
		}
	}

    public void ResetCurrentBall() {
		selectedScreen.ResetBall();
    }

    public void EndLevel() {
		selectedScreen.OnBallOnFloor();
		pauseWindow.Close();
	}
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
    #endregion

    #region Coroutines
    #endregion
}