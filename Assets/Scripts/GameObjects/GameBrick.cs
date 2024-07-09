using UnityEngine;
using UnityEngine.Events;
 
/// <summary>
/// When it is destroyed, it puts its body and its place and disables himself. 
/// </summary>
public class GameBrick : MonoBehaviour {
	#region Serialize fields
	[SerializeField]
	[Tooltip("When it is destroyed it will put the body in the place.")]
	private Transform place;
	[SerializeField]
	[Tooltip("The body is the first child in hierarchy. When it is destroyed it will put the body in the place.")]
	private Transform bodyHolder;
    [SerializeField]
    [Tooltip("The brick gives a reward for each broken icon.")]
    private int brokenIconReward = 100;
    [SerializeField]
    [Tooltip("It starts showing a random icon. On collision enter it will show the previous icon. When it is showing the first icon and enter on collision, it will be destroyed")]
    private GameObject[] icons;

    [Header("Effects")]
    [SerializeField]
    private ParticleSystem changeColorEfect;
    [SerializeField]
    private ParticleSystem destroyEffect;
    [SerializeField]
    private AudioClip changeColorSound;
    [SerializeField]
    private AudioClip breakSound;
    #endregion

    #region Private fields
    private GameObject body;
    private AudioManager audioManager;
    private int reward;
    private int currentIconIndex;
    #endregion

    #region Properties
    public int Reward {
        get => reward;
    }

    private Transform BodyParent {
        set {
            body.transform.parent = value;

            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            body.transform.localScale = Vector3.one;
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Invoke OnPlace when it is put in its place.")]
    public UnityEvent<GameBrick> OnPlace;
    #endregion

    #region Unity methods
    private void Start() {
        body = bodyHolder.childCount > 0 ? bodyHolder.GetChild(0).gameObject : null;
        audioManager = AudioManager.Instance;

        Reset();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision != null) {
            icons[currentIconIndex].SetActive(false);

            if (currentIconIndex > 0) {
                currentIconIndex--;

                changeColorEfect.Play();
                audioManager.PlayEffect(changeColorSound);
                icons[currentIconIndex].SetActive(true);
            }
            else {
                destroyEffect.Play();
                audioManager.PlayEffect(breakSound);

                if (body != null) {
                    BodyParent = place;
                }

                OnPlace.Invoke(this);
            }
        }
    }

    public void Reset() {
        currentIconIndex = Random.Range(0, icons.Length);
        reward = (currentIconIndex + 1) * brokenIconReward;

        if (body != null) {
            BodyParent = bodyHolder;
        }

        foreach (var icon in icons) {
            if (icon.activeSelf) {
                icon.SetActive(false);
            }
        }

        icons[currentIconIndex].SetActive(true);
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