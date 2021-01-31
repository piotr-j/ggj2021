using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Global storage
/// </summary>
[CreateAssetMenu(fileName = "GameData", menuName = "Game/GameData")]
public class GameData : ScriptableObject
{
	public GameState gameState;
	public PlayerData playerData;
	public ShopData shopData;
	
	[Serializable]
	public class PlayerData
	{
		public IntVariable currentCharacter;
	}

	[Serializable]
	public class ShopData
	{
		public StringVariable lastDailyFreeGiftsResetTime;
	}
	
	[Serializable]
	public class GameState
	{
		public IntVariable turnsLeft;
		public IntVariable artifactsGot;
	}

	[Header("Level")]
    public IntVariable currentLevel;

    
    private void OnEnable()
    {
    }
    
}
