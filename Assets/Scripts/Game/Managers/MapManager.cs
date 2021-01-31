using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// TODO: Move selection controller out of here and Input Manager
public class MapManager : MonoBehaviour
{
    public Vector2Int gridSize;

    [Space(10)]
    [SerializeField]
    private LayerMask m_groundLayer;
    public LayerMask GroundLayer => m_groundLayer;

    [SerializeField]
    private LayerMask m_blockerLayer;
    public LayerMask BlockerLayer => m_blockerLayer;

    [Header("Config")]
    [Tooltip("In hexes")]
    public float minDistance = 1.1f;
    [Tooltip("In hexes")]
    public float maxDistance = 5.1f;

    [Tooltip("How often blockers spawn")]
    public float blockerChance = .15f;

    [Header("References")] 
    public GameManager gameManager;
    
    [Space(10)]
    public HexTile tilePrefab;
    public HexTile tileBlockerPrefab;

    public readonly float tileXOffset = 1.8f;
    public readonly float tileZOffset = 1.565f;

    public HexTile lastSelectedTile { private set; get; }
    public HexTile lastHooverTile { private set; get; }
    public List<HexTile> lastPath { private set; get; }

    public Action<List<HexTile>> onMapReady;

    public List<HexTile> Tiles { private set; get; }
    
    public HexTile CenterTile { private set; get; }

    public Action<HexTile> OnTileSelected;
    public Action<HexTile> OnTileDeselected;

    void Awake()
    {
        CreateTilemap();
    }

    private void Start()
    {
        gameManager.teamsManager.OnTeamPathStepReached += r => Reveal(r);
    }

    [ContextMenu("Create Tilemap")]
    private void CreateTilemap()
    {
        // TODO works only for even x and y
        float ox = -gridSize.x * tileXOffset / 2;
        float oy = -gridSize.y * tileZOffset / 2;

        Vector3 center = gameObject.transform.position;

        Tiles = new List<HexTile>();

        for (var x = 0; x <= gridSize.x; x++)
        {
            for (int y = 0; y <= gridSize.y; y++)
            {
                Vector3 pos = new Vector3(x * tileXOffset - tileXOffset / 2 * (y % 2) + ox, 0, y * tileZOffset + oy);

                float Distance = Vector3.Distance(pos, center);

                // dont create if outside max range
                if (Distance >= maxDistance * tileXOffset)
                {
                    continue;
                }
                HexTile prefab = tilePrefab;
                // no blockers near center?
                if (Distance > 2 * tileXOffset && UnityEngine.Random.Range(0f, 1f) <= blockerChance)
                {
                    prefab = tileBlockerPrefab;
                }
                HexTile newTile = Instantiate(prefab, gameObject.transform);
                newTile.gameObject.isStatic = true;
                newTile.name = "Tile_" + x + "_" + y;
                newTile.transform.position = pos;
                Tiles.Add(newTile);
            }
        }

        // TODO figure out neighbour tiles for each of them and store, properly


        StartCoroutine(ScanDelayed());
    }

    IEnumerator ScanDelayed()
    {
        yield return 0;

        // due to how GetTile works, we need to wait
        // perhaps there is a way to do it some other way, but whatever
        // look for neighbours before we deactivate tiles out of range
        foreach (var tile in Tiles)
        {
            tile.neighbours = Neighbours(tile);
        }

        Vector3 center = gameObject.transform.position;
        // deactivate tiles out of range, hide instead?
        foreach (var tile in Tiles)
        {
            if (Vector3.Distance(tile.transform.position, center) >= minDistance * tileXOffset)
            {
                tile.SetHidden();
            }
            
        }

        if (AstarPath.active == null)
        {
            Debug.LogError("Astar not active", this);
        }
        else
        {
            AstarPath.active.Scan();
        }
        
        CenterTile = GetTileAt(new Vector3(0, 0, 0));

        onMapReady?.Invoke(Tiles);
    }

    public void SetHooverTile(HexTile newHooverTile)
    {
        if (newHooverTile || newHooverTile != lastHooverTile)
        {
            if (lastHooverTile && lastHooverTile.tileState == HexTile.TileState.Hoover)
            {
                lastHooverTile.SetState(HexTile.TileState.None);
            }
            if (newHooverTile && newHooverTile.tileState == HexTile.TileState.None)
            {
                newHooverTile.SetState(HexTile.TileState.Hoover);
            }
        }
        lastHooverTile = newHooverTile;
    }

    public void SetPathHooverTile(HexTile tile)
    {        
        if (!lastSelectedTile)
        {
            Debug.Log("No path start!", this);
            return;
        }
        
        gameManager.pathManager.GetNewPath(lastSelectedTile, tile, OnPathHooverReady);
    }

    public void SetFinalPath(HexTile tile)
    {
        if (!lastSelectedTile)
        {
            Debug.Log("No path start!", this);
            return;
        }
        
        gameManager.pathManager.GetNewPath(lastSelectedTile, tile, OnFinalPathReady);
    }

    private void OnFinalPathReady(List<HexTile> path)
    {
        gameManager.TurnMoveTeam(lastSelectedTile, path);
        
        DeselectAll();
    }

    private void OnPathHooverReady(List<HexTile> path)
    {
        CancelPath();
        
        // We want to keep selected tile as it is
        path.RemoveAt(0);

        foreach (var hexTile in path)
        {
            hexTile.SetState(HexTile.TileState.PathPart);
        }

        lastPath = path;
    }

    public void SetSelectedTile(HexTile tile)
    {
        if(lastSelectedTile == tile)
            return;

        DeselectAll();

        if(!tile)
            return;

        lastSelectedTile = tile;
        
        tile.SetState(HexTile.TileState.Selected);
        
        OnTileSelected?.Invoke(tile);
    }

    /// <summary>
    /// Reveal tiles around this one
    /// </summary>
    /// <param name="center"></param>
    public bool Reveal(HexTile center)
    {
        // need a list of surrounding tiles, 
        bool rescan = false;
        int revealedCount = 0;
        for (var i = 0; i < center.neighbours.Count; i++)
        {
            var tile = center.neighbours[i];
            bool revealed = tile.Reveal(revealedCount * 0.1f);
            if (revealed)
                revealedCount++;

            rescan |= revealed;
        }

        if (!rescan) return false;
        // perhaps in real game we could disable certain nodes instead of rescanning
        // hidden tiles are deactivate and not visible to Scan() when we first init them
        // there can be 'holes' in map, so we can just scan all of them and disable after
        // unless we remove them from pathfinding somehow
        if (AstarPath.active == null)
        {
            Debug.LogError("Astar not active", this);
        }
        else
        {
            AstarPath.active.Scan();
        }
        return true;
    }

    private List<HexTile> Neighbours(HexTile center)
    {
        // TODO store them in HexTile instead
        Vector3 centerPos = center.transform.position;
        List <HexTile> tiles = new List<HexTile>();
        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60;
            Vector2 offset = new Vector2(tileXOffset, 0).SetAngle(angle);
            Vector3 at = new Vector3(centerPos.x + offset.x, 0, centerPos.z + offset.y);
            HexTile tile = GetTileAt(at, m_blockerLayer | m_groundLayer);
            if (tile != null) tiles.Add(tile);
            //Debug.DrawLine(new Vector3(centerPos.x, 1, centerPos.z), new Vector3(at.x, 1, at.z), Color.magenta, 15);
        }
        return tiles;
    }

    public void DeselectAll()
    {
        CancelPath();
        CancelSelection();
    }

    private void CancelSelection()
    {
        lastSelectedTile?.SetState(HexTile.TileState.None);
        OnTileDeselected?.Invoke(lastSelectedTile);
        lastSelectedTile = null;
    }

    private void CancelPath()
    {
        lastPath?.ForEach(e => e.SetState(HexTile.TileState.None));
        lastPath = null;
    }

    public HexTile GetTileAt(Vector3 pos)
    {
        return GetTileAt(pos, GroundLayer);
    }

    public HexTile GetTileAt(Vector3 pos, LayerMask layer)
    {
        // TODO: Horrible method - jam way to do it
        HexTile returnedTile = null;
        
        var source = new Vector3(pos.x, 5, pos.z);
        var ray = new Ray(source, new Vector3(0, -1, 0));
        
        if (Physics.Raycast(ray, out var hit, 100, layer))
        {
            returnedTile = hit.collider?.gameObject.GetComponentInParent<HexTile>();
        }

        return returnedTile;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, minDistance * tileXOffset);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxDistance * tileXOffset);
    }
}
