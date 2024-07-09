using UnityEngine;
using UnityEngine.Events;
 

/// <summary>
/// The floor of the house.
/// </summary>
public class HouseFloor : MonoBehaviour {
    #region Readonly fileds
    #endregion

    #region Serialize fields
    /*[SerializeField]
    private GameObject floor;*/
    #endregion

    #region Private fields
    //private bool isSolid = false;
	#endregion

	#region Properties
	/*public bool IsSolid {
        set {
            isSolid = value;

            floor.SetActive(value);
        }
	}*/
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Invoke on collision enter")]
    public UnityEvent OnObjectOnFloor;

    #endregion

    #region Unity methods
    private void OnCollisionEnter(Collision collision) {
        OnObjectOnFloor.Invoke();
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