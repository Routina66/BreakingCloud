using UnityEngine;
using System;

/// <summary>
/// Defines the data of a PlayObject.
/// </summary>
[Serializable]
public class PlayObjectData {

    #region Fields
    [SerializeField]
    private string identifier;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private AudioClip sound;
    [SerializeField]
    private PlayObjectType type;
    [SerializeField]
    private string description;
    [SerializeField]
    private Money[] prices;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    public string Identifier {
        get => identifier;
    }

    public Sprite Icon {
        get => icon;
    }

    public AudioClip Sound {
        get => sound;
    }

    public PlayObjectType Type {
        get => type;
    }

    public string Description {
        get => description;
    }
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    /// <summary>
    /// Returns the price of the PlayObject in a MoneyType.
    /// </summary>
    /// <param name="moneyType">The MoneyType.</param>
    /// <returns>-1 if doesn't have got price of moneyType.</returns>
    public int GetPrice(MoneyType moneyType) {
        var money = Array.Find(prices, p => p.MoneyType == moneyType);
        
        return money == null ? -1 : money.Amount;
    }
    #endregion
}