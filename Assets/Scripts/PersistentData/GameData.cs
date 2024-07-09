using System.Collections.Generic;
using System;

[Serializable]
public class GameData
{
	#region public fields
	public bool GameEnded = false;
	public int 
		level = 1,
		money = 1500;

	public string
		equipedBall = Constants.PoolBallId,
		equipedCloud = Constants.CumulusId;

	public List<string>
        balls = new List<string>(),
		clouds = new List<string>();
		
	#endregion

	#region Constructors
	public GameData() {
		balls.Add(Constants.PoolBallId);
		clouds.Add(Constants.CumulusId);
	}
	#endregion

	#region Public methods

	public void EquipPlayObject(PlayObjectData playObjectData) {
        switch (playObjectData.Type) {
            case PlayObjectType.Ball:
                equipedBall = playObjectData.Identifier;
                break;

            case PlayObjectType.Cloud:
                equipedCloud = playObjectData.Identifier;
                break;
        }
    }
	#endregion
}
