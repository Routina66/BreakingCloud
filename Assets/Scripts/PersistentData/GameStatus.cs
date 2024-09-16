using System.Collections.Generic;
using System;

[Serializable]
public class GameStatus {
	#region public fields
	public bool gameEnded = false;
	public int
		currentLevel = 1,
		playerMoney = 1500;


	public string equipedObject = Constants.InitialPlayerId;

	public List<string> playerInventory = new List<string> { Constants.InitialPlayerId };
		
	#endregion

	#region Constructors
	/// <summary>
	/// It creates a new Gamestatus.
	/// </summary>
	/*public GameStatus() {
		gameEnded = false;
		currentLevel = 1;
		playerMoney = 1500;
		equipedObject = Constants.InitialPlayerId;
		playerInventory = new List<string> { Constants.InitialPlayerId };

		playerInventory.Add(Constants.InitialPlayerId);
     }*/
	#endregion

	#region Public methods
	#endregion
}
