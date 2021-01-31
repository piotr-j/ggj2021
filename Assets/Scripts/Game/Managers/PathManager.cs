using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    private Path lastPath;
    private List<HexTile> lastPathTiles;

    public void GetNewPath(HexTile startingTile, HexTile targetTile, Action<List<HexTile>> path)
    {
        StartCoroutine(GetNewPathInternal(targetTile, path));
    }

    IEnumerator GetNewPathInternal(HexTile targetCell, Action<List<HexTile>> action)
    {
        Path path = GetPath(gameManager.mapManager.lastSelectedTile, targetCell, OnNewPathReady);

        yield return path.WaitForPath();
        
        lastPathTiles = new List<HexTile>();
        foreach (var graphNode in lastPath.path)
        {
            lastPathTiles.Add(gameManager.mapManager.GetTileAt((Vector3) graphNode.position));
        }
        
        action?.Invoke(lastPathTiles);
    }

    private void OnNewPathReady(Path p)
    {
        if (p.error)
        {
            Debug.LogError(p.error, this);
            return;
        }

        lastPath = p;
    }

    private Path GetPath(HexTile startingCell, HexTile targetCell, OnPathDelegate onComplete)
    {
        if (AstarPath.active == null)
        {
            Debug.LogError("Astar not active", this);
            return null;
        }
        
        var p = ABPath.Construct(startingCell.transform.position, targetCell.transform.position, onComplete);

        AstarPath.StartPath (p);

        return p;
    }
}
