using UnityEngine;
using TMPro;
using I2.Loc;
 
/// <summary>
/// It Shows a message for some time, then it hides the message.
/// </summary>
public class FloatMessage : MonoBehaviour {
    #region Readonly fileds
    #endregion

    #region Serialize fields
    [SerializeField]
    private GUIAnimFREE GuiAnim;
    [SerializeField]
    private TextMeshProUGUI message;
    [SerializeField]
    [Tooltip("Show time in seconds")]
    private float showTime = 3.0f;
    [SerializeField]
    [Tooltip("After closeTime seconds the game object will be disabled")]
    private float closeTime = 2.0f;

    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    /// <summary>
    /// It shows theMessage. The message will start to be closed after showTime seconds.
    /// </summary>
    /// <param name="theMessage">Text to show.</param>
    public void ShowMessage(string theMessage) {
        message.text = LocalizationManager.GetTranslation(theMessage);

        gameObject.SetActive(true);
        GuiAnim.PlayInAnims();

        Invoke(nameof(Hide), showTime);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// It plays out animations. The message will be hiden after closeTime seconds.
    /// </summary>
    private void Hide() {
        GuiAnim.PlayOutAnims();

        Invoke(nameof(Close), closeTime);
    }

    /// <summary>
    /// It deletes message and disables gameObject.
    /// </summary>
    private void Close() {
        message.text = string.Empty;

        gameObject.SetActive(false);
    }
    #endregion

    #region Coroutines
    #endregion
}