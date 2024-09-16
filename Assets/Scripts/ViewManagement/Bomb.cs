using UnityEngine;
using System.Collections;
using UnityEngine.Events;


/// <summary>
/// A bomb.
/// </summary>
public class Bomb : MonoBehaviour {
	#region Readonly fields
	#endregion

	#region Serialize fields
	[SerializeField]
    [Tooltip("The possible explosion sounds of the bomb. The bomb will explode with only one of this sounds. The selected sound is random.")]
    private AudioClip[] explosionSounds;
    [SerializeField]
	[Tooltip("The possible explosion effects of the bomb. The bomb will explode with only one of this effects. The selected effect is random.")]
    private ParticleSystem[] explosionEffects;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    [Header("Events")]
	[Tooltip("When it explodes, it sends an event.")]
	public UnityEvent<Bomb> OnExplode;
	#endregion

	#region Unity methods
	#endregion

	#region Public methods
	public void ExplodeBomb() {
		explosionEffects[Random.Range(0, explosionEffects.Length)].Play();

		OnExplode.Invoke(this);
	}
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
	#endregion

	#region Coroutines
	#endregion
}