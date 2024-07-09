using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// A GameObject witch can be bought, sold and stored in a GameStore.
/// </summary>
public class PlayObject : MonoBehaviour
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
	private AudioManager audioManager;
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
	[Header("Events")]
	[Tooltip("Sends itself.")]
	public UnityEvent<PlayObject> OnClick;
    #endregion

    #region Unity methods
    private void Awake() {
		audioManager = AudioManager.Instance;
    }

    private void OnCollisionEnter(Collision collision) {
        PlaySound(data.Sound);

        if (collisionEffect != null) {
			collisionEffect.transform.position = collision.transform.position;
			collisionEffect.Play();
		}
    }
    #endregion

    #region Public methods
    public void Show() {
        gameObject.SetActive(true);
		PlaySound(data.Sound);
    }

	public void Hide() {
		gameObject.SetActive(false);
		PlaySound(data.Sound);
	}

	public void ResetTransform() {
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}
	#endregion

	#region Private methods
	private void PlaySound(AudioClip sound) {
		if (sound != null) {
			audioManager.PlayEffect(sound, transform.position);
		}
	}

    /*private void Enable() {
		gameObject.SetActive(true);
		
    }*/
    #endregion
}
