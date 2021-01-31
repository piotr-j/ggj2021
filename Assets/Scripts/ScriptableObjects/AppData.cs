using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AppData", menuName = "Game/AppData", order = 0)]
public class AppData : ScriptableObject
{
	public Assets assets;
	public Config config;
	public GameData data;

    [Header("Currencies")]
    public IntVariable coins;
    
    /*public bool IsBossLevel(int level)
	{
		if (level == 0) return false;
		return config.GetLevelDefiniton(level).isBossLevel;
	}*/

    
}
