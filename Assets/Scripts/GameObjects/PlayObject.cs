using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// A GameObject witch can be bought, sold and stored in a GameStore.
/// </summary>
public class PlayObject : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
	#region Serialize fields
	[SerializeField]
	private PlayObjectData data;
	[SerializeField]
	private Rigidbody rigidBody;
	[SerializeField]
	private ParticleSystem collisionEffect;


    /*[Header("Sounds")]
    [SerializeField]
    private AudioClip enableSound;
	[SerializeField]
	private AudioClip disableSound;
    [SerializeField]
	private AudioClip collisionSound;*/
	#endregion

	#region Private fields
    #endregion

    #region Properties
    /*public string Identifier {
		get => data.Identifier;
	}*/

	public PlayObjectData Data {
		get => data;
	}

	public float Size {
		set => transform.localScale = new Vector3(value, value, value);
	}

	public float Mass {
		set => rigidBody.mass = value;
	}

	public Vector3 Velocity {
		set => rigidBody.velocity = value;
	}
	#endregion

	#region Events
	/*[Header("Events")]
	[Tooltip("When the pointer down sends itself.")]
	public UnityEvent<PlayObject> PointerGoneDown;
	[Tooltip("When the pointer up sends itself.")]
	public UnityEvent<PlayObject> PointerGoneUp;*/
    #endregion

    #region Unity methods
    private void OnCollisionEnter(Collision collision) {
        if (collisionEffect != null) {
			collisionEffect.transform.position = collision.transform.position;
			collisionEffect.Play();
		}
    }

    private void OnCollisionExit(Collision collision) {
        
    }
    #endregion

    #region Public methods
    /*public void OnPointerDown(PointerEventData eventData) {
		PointerGoneDown.Invoke(this);
    }

    public void OnPointerUp(PointerEventData eventData) {
		PointerGoneUp.Invoke(this);
    }*/

    public void Show(bool show) {
        gameObject.SetActive(show);
    }

	public void Hide() {
		gameObject.SetActive(false);
	}

	public void ResetTransform() {
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}
    #endregion

    #region Private methods
    #endregion
}
