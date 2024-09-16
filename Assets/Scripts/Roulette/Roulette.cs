using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Roulette : MonoBehaviour {
    #region Readonly fields
    #endregion

    #region Serialize fields
    [SerializeField]
    private TextMeshProUGUI rewardMessage;
    [SerializeField]
    private GameObject rewardBox;
    [SerializeField]
    private Rigidbody2D pivot;
    [SerializeField]
    private RouletteItem[] rouletteItems;

    [Header("Spin options")]
    [SerializeField]
    private float minSpinForce;
    [SerializeField]
    private float maxSpinForce;

    [Header("Roulete Sounds")]
    [SerializeField]
    private AudioClip spinSound;
    [SerializeField]
    private AudioClip winSound;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    public bool IsStop {
        get {
            if (Mathf.Abs(pivot.angularVelocity) < 1.0f)
                pivot.angularVelocity = .0f;

            return pivot.angularVelocity == .0f;
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When the roulete is spining, it sends an event every time a RoletteItem enters in arrow Collider. ")]
    public UnityEvent OnSpinRoulette;
    [Tooltip("When the roulete ends to spin, it sends the result.")]
    public UnityEvent<int> OnEndSpinRoulette;
    #endregion

    #region Unity methods
    private void OnEnable() {
        if (rewardBox.activeSelf) {
            rewardBox.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        OnSpinRoulette.Invoke();
    }
    #endregion

    #region Public methods
    public void SpinRoulete() {
        pivot.AddTorque(Random.Range(minSpinForce, maxSpinForce));

        StartCoroutine(CheckResult());

        OnSpinRoulette.Invoke();
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    private IEnumerator CheckResult() {
        RouletteItem winnerItem = rouletteItems[0];
        float 
            winnerItemRotation,
            itemAngle = 360 / rouletteItems.Length;

        yield return new WaitForSeconds(.1f);

        while (!IsStop) {
            for (int i = 0; i < 6; i++) {
                rouletteItems[i].transform.rotation = Quaternion.identity;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        winnerItemRotation = 
            (pivot.transform.rotation.eulerAngles.z + (itemAngle/2)) % 360;

        winnerItem = 
            rouletteItems[(int)(winnerItemRotation / itemAngle)];

        rewardMessage.text = "Reward X " + winnerItem.Reward;

        rewardBox.SetActive(true);
        OnEndSpinRoulette.Invoke(winnerItem.Reward);        
    }
    #endregion
}
