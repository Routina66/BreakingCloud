using UnityEngine;
using System.Collections;
using UnityEngine.PlayerLoop;
using System;
using CrazyGames;


/// <summary>
/// Applies a velocity to a rigidbody when the user presses the keys of
/// the x axis control in the InputManager
/// </summary>
public class MotionControl2D : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    private Rigidbody target;
    [SerializeField]
    private float velocity;
    #endregion

    #region Private fields
    private float xAxis = 0f;
    #endregion

    #region Properties
    #endregion

    #region Events
    #endregion

    #region Unity methods
    private void Update() {
        if (CrazySDK.User.SystemInfo.device.type.Equals(CrazySettingsDeviceType.desktop.ToString())) {
            xAxis = Input.GetAxis(Constants.HorizontalAxis);
        }
        else {
            if (Input.touchCount == 1) {
                if (Input.GetTouch(0).position.x >= Screen.height / 2) {
                    xAxis = .25f;
                }
                else {
                    xAxis = -.25f;
                }
            }
            else {
                xAxis = 0f;
            }
        }

        if (xAxis == 0 && target.velocity != Vector3.zero) {
            target.velocity = Vector2.zero;
        }
        else if (xAxis != 0) {
            target.velocity = Vector2.right * xAxis * velocity;
        }
    }
    #endregion

    #region Public methods
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}