using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Game/Config")]
public class Config : ScriptableObject
{
    public GameConfig gameConfig;
    public LevelsConfig levelConfig;
    public PlayerConfig playerConfig;

    [Serializable]
    public class GameConfig
    {
        public int turnsPerLevel = 50;
    }
    
    [Serializable]
    public class PlayerConfig
    {
        
    }

    [Serializable]
    public class LevelsConfig
    {
        
    }

    /*public bool IsLastLevel(int index)
    {
        return index >= levelConfig.levelDefinitions.Count;
    }

    public bool HasLevel(int index)
    {
        return index <= levelConfig.levelDefinitions.Count;
    }*/
}
