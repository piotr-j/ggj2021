using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Config")]
    [Tooltip("Artifacts that can be potentailly found in tiles")]
    public List<Artifact> artifacts;

    public InterestingArea interestingAreaPrefab;

    // RangeCollection or something?
    [Tooltip("chance for picked hex to start a cluster")]
    public float chance = .5f;

    [Tooltip("min distance from center for cluster center, in hexes")]
    public float minDistance = 4;
    [Tooltip("max distance from center for cluster center, in hexes")]
    public float maxDistance = 8;


    [Tooltip("artifacts in a cluster +- spread")]
    public int clusterSize = 4;
    public int clusterSizeSpread = 1;

    [Tooltip("total number of artifacts to spawn")]
    public int totalArtifacts = 16;

    [Tooltip("minimal distance between clusters, in hexes")]
    public int margin = 1;

    [Tooltip("chance for decal to be visible on empty tiles")]
    public float decalChance = .4f;

    void Start()
    {
        // wait for map to be ready
        gameManager.mapManager.onMapReady += BuryArtifacts;
    }

    private void BuryArtifacts(List<HexTile> tiles)
    {
        MapManager mm = gameManager.mapManager;
        float min = minDistance * mm.tileXOffset;
        float max = maxDistance * mm.tileXOffset;

        Vector3 center = mm.transform.position;
        

        // first find valid targets
        List<HexTile> hexes = new List<HexTile>();
        foreach (var tile in tiles)
        {
            float dst = Vector3.Distance(center, tile.transform.position);
            if (dst >= max) continue;
            if (dst <= min) continue;
            if (tile.blocked) continue;
            hexes.Add(tile);
        }
        

        // bury some loot!
        int totalAdded = 0;
        while (hexes.Count > 0)
        {
            HexTile hex = hexes.RandomRemove();
            if (Random.Range(0f, 1f) > chance) continue;
            

            hex.Add(artifacts.Random());

            // pick random hex from the cluster for marker
            List<HexTile> tmps = new List<HexTile>(hex.neighbours);
            tmps.Add(hex);
            while(tmps.Count > 0)
            {
                HexTile tmp = tmps.RandomRemove();
                if (tmp.IsRevealed()) continue;
                
                InterestingArea interestingArea = Instantiate(interestingAreaPrefab, tmp.gameObject.transform);
                tmp.OnRevealed += interestingArea.Revealed;
                break;
            }

            // we dont want overlapping clusters, remove neighbors
            RemoveNeighbours(hex, hexes, margin + 1);

            int toAdd = Random.Range(clusterSize - clusterSizeSpread, clusterSize + clusterSizeSpread);
            // the one above
            int added = 1;
            for (int i = 0; i < 12; i++)
            {
                HexTile hexNeighbour = hex.neighbours.Random();
                if (Random.Range(0f, 1f) > chance) continue;
                // could copy and remove but whatever
                if (hexNeighbour.HasArtifacts()) continue;
                if (hexNeighbour.blocked) continue;

                hexNeighbour.Add(artifacts.Random());
                added++;
                totalAdded++;

                if (added >= toAdd) break;

                if (totalAdded >= totalArtifacts)
                {

                    // break from outer while loop
                    goto refresh;
                    // or clear, but goto is fun :d
                    //hexes.Clear();
                }
            }
        };

        refresh:

        if (totalAdded < totalArtifacts)
        {
            Debug.LogError("Failed to add " + totalArtifacts + ", available " + totalAdded);
        }

        // refresh counts on neighbours
        foreach (HexTile tile in tiles)
        {
            tile.Refresh();

            if (tile.UIArtifactCount == 0 && !tile.blocked && Random.Range(0f, 1f) <= decalChance) 
            {
                tile.GetComponent<EnableRandom>()?.EnableRandomTarget();
            }
        }
    }

    private void RemoveNeighbours(HexTile hex, List<HexTile> hexes, int margin)
    {
        if (margin == 0) return;

        hexes.RemoveAll(hex.neighbours);
        foreach (var n in hex.neighbours)
        {
            RemoveNeighbours(n, hexes, margin - 1);
        }
    }

    public void ResetState()
    {
        gameManager.appData.data.gameState.artifactsGot.SetValue(0);
    }

    private void OnDrawGizmosSelected()
    {
        //MapManager mm = gameManager.mapManager;
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, minDistance * mm.tileXOffset);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, maxDistance * mm.tileXOffset);
    }
}
