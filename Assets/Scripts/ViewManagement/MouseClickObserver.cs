using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Each frame It sends an event if the player 
/// is using the keyboard or the mouse.
/// </summary>
public class MouseClickObserver : MonoBehaviour {
    #region Readonly fields
    private readonly int
        leftMouseButton = (int)PointerEventData.InputButton.Left,
        rightMouseButton = (int)PointerEventData.InputButton.Right;
    #endregion

    #region Serialize fields
    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    [Tooltip("When left mouse button is released, it sends the event.")]
    public UnityEvent OnLeftMouseButtonUp;
    [Tooltip("When left mouse button is released, it sends the event.")]
    public UnityEvent OnRightMouseButtonUp;
    #endregion

    #region Unity methods
    /// <summary>
    /// On update, it checks if the play has pressed
    /// or released a key or a mouse button.
    /// </summary>
    private void Update() {
        if (Input.GetMouseButtonUp(leftMouseButton)) {
            OnLeftMouseButtonUp.Invoke();
        }
        else if (Input.GetMouseButtonUp(rightMouseButton)) {
            OnRightMouseButtonUp.Invoke();
        }
    }
    #endregion

    #region Public methods
    public void AddListener(I_MouseClickListner newListener) {
        OnLeftMouseButtonUp.AddListener(newListener.OnLeftMouseButtonUp);
        OnRightMouseButtonUp.AddListener(newListener.OnRightMouseButtonUp);
    }

    public void RemoveListener(I_MouseClickListner newListener) {
        OnLeftMouseButtonUp.RemoveListener(newListener.OnLeftMouseButtonUp);
        OnRightMouseButtonUp.RemoveListener(newListener.OnRightMouseButtonUp);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}