using System;
using System.Threading;
using UnityEngine;


/// <summary>
/// It is a serializable class witch can limit the number of instances that 
/// can be created of a PlayObject in the game.
/// Can be locked. If it is locked will never create a instance of the object.
/// </summary>
[Serializable]
public class PlayObjectSlot {

	#region Serialize fields
	[SerializeField]
	[Tooltip("Object witch get references.")]
	private PlayObject targetObjectPrefab;
	[SerializeField]
	[Tooltip("Maximum number of instances that can be returned. If < 0 the instances are infinite.")]
	private int capacity;
	[SerializeField]
	[Tooltip("If it is locked will never create a instance of the object. Stil can be added new objects.")]
	private bool locked = false;
	#endregion

	#region Private fields
	//Current number of instances of the targetOpbject witch can be isntantiated.
	private int currentQuantity;
	#endregion

	#region Properties
	public PlayObjectData ObjectData {
		get => targetObjectPrefab.Data;
	}

	public int Capacity {
		get => capacity;
	}

	public int CurrentQuantity {
		get => currentQuantity;
	}

	public bool IsLocked {
		get => locked;
		set => locked = value;
	}

    public bool IsEmpty {
		get => currentQuantity == 0;

    }

	public bool IsFull {
		get => capacity < 0 || currentQuantity == capacity; 
	}
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a slot with an maximum number of instances of 
    /// a PlayObject witch can be instantiated.
    /// </summary>
    /// <param name="playObject">The PlayObject.</param>
	/// <param name="maxInstances">The maximum number of instances</param>
    public PlayObjectSlot(PlayObject playObject, int maxInstances = -1) {
		targetObjectPrefab = playObject;
		capacity = maxInstances;
		currentQuantity = capacity;
	}
    #endregion


    #region Public methods
    /// <summary>
    /// </summary>
    /// <returns>A refence to targetObjectPrefab or null if it is locked or amount == 0.</returns>
    public PlayObject GetObject() {
		if (!locked && currentQuantity != 0) {
			if (currentQuantity > 0) {
				currentQuantity--;
			}

			return targetObjectPrefab;
		}
		else {
			return null; 
		}
	}

	/// <summary>
	/// Add 1 to amount of instnaces of targetObjectPrefab.
	/// </summary>
	public void AddObject() {
		if (!locked && capacity > 0) {
			currentQuantity = Math.Min(currentQuantity + 1, capacity);
		}
	}

	/// <summary>
	/// Sets the current quantity to 0.
	/// </summary>
	public void Clear() {
		currentQuantity = 0;
	}
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
	#endregion

	#region Coroutines
	#endregion
}