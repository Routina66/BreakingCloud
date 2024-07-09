using UnityEngine;
using System;


/// <summary>
/// A serializable class witch shows the currentQuantity of some MoneyType.
/// </summary>
[Serializable]
public class Money
{
	#region Serialize fields
	[SerializeField]
	private MoneyDefinition definition;
	[SerializeField]
	private int amount;
	#endregion

	#region Private fields
	private AudioManager audioManager;
    #endregion

    #region Properties
    public MoneyType MoneyType {
		get => definition.MoneyType;
	}
	public int Amount {
		get {
			PlaySound();

			return amount;
		}

		set {
			amount = value;

            PlaySound();
        }
	}

	public Sprite Icon {
		get => definition.Icon;
	}

	public AudioClip Sound {
		get => definition.Sound;
	}
    #endregion

    #region Constructors
    public Money(MoneyDefinition moneyDefinition, int moneyQuantity = 0) {
		amount = moneyQuantity;
		definition = moneyDefinition;

		if (audioManager == null) {
			audioManager = AudioManager.Instance;
		}
	}
	#endregion

	#region Public methods
	#endregion

	#region Private methods
	private void PlaySound() {
		if (audioManager == null) {
			audioManager = AudioManager.Instance;
		}

        audioManager.PlayEffect(definition.Sound);
    }
    #endregion

}
