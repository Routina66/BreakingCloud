using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Inherit from GameScreen. It manages the actions 
/// of the player in the main screen.
/// </summary>
public class MainScreen : GameScreen {
    #region Readonly fields
    #endregion

    #region Serialize fields
    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When a play object is bougth, sends the data of the bought PlayObject.")]
    public UnityEvent<PlayObjectData> OnBuyPlayObject;
    [Tooltip("When a play object is equiped, sends the data of the equiped PlayObject.")]
    public UnityEvent<PlayObjectData> OnEquipPlayObject;
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    public void BuyPlayObject(PlayObjectData playObjectData) {
        OnBuyPlayObject.Invoke(playObjectData);
    }

    public void EquipPlayObject(PlayObjectData playObjectData) {
        OnEquipPlayObject.Invoke(playObjectData);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}