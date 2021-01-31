using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HexTile : MonoBehaviour
{
    [Header("Configuration")]
    public List<Material> normalMaterials;
    public Color selectedColor;
    public Color hooverColor;
    public Color pathPartColor;


    [Header("References")]
    public Transform model;
    public MeshRenderer meshRenderer;
    public TextMeshPro text;
    public TextMeshPro textDebug;
    // TODO create if needed?
    public HexFlag flag;


    private Material normalMaterial;

    public TileState tileState { get; private set; }

    [HideInInspector]
    public List<HexTile> neighbours;
    
    // can be empty
    protected List<Artifact> artifacts = new List<Artifact>();

    public TeamScript OccupyingTeam { get; private set; }

    [Tooltip("If can be walked on")]
    public bool blocked = false;

    [HideInInspector]
    public Action<HexTile> OnRevealed;

    [HideInInspector]
    public int UIArtifactCount { get; private set; }

    private void Awake()
    {
        text.SetText("");
        // copy as we change color based on state
        normalMaterial = new Material(normalMaterials.Random());
        SetState(TileState.None);

#if UNITY_EDITOR && FALSE
        textDebug.SetText("");
        textDebug.gameObject.SetActive(true);
#endif
    }

    public void Add (Artifact artifact)
    {
        artifacts.Add(artifact);
        int total = artifacts.Count;
    }

    public List<Artifact> Excavate ()
    {
        flag.Excavated(artifacts.Count > 0);

        List<Artifact> excavated = new List<Artifact>(artifacts);
        artifacts.Clear();

        Refresh();
        foreach (var tile in neighbours)
        {
            tile.Refresh();
        }

        return excavated;
    }

    public bool HasArtifacts()
    {
        return artifacts.Count != 0;
    }

    public int ArtifactCount()
    {
        return artifacts.Count;
    }

    public bool Excavated()
    {
        // disabled by default, add an explicit flag?
        return flag.gameObject.activeInHierarchy;
    }

    public void Refresh()
    {

        int count = artifacts.Count;
        foreach (var tile in neighbours)
        {
            count += tile.ArtifactCount();
        }
        // dont show text if we are a blocker
        if (count == 0 || blocked)
        {
            text.SetText("");
        }
        else
        {
            text.SetText(count.ToString());
        }

        UIArtifactCount = count;

#if UNITY_EDITOR && FALSE
        if (artifacts.Count == 0)
        { 
            textDebug.SetText("");
        }
        else
        {
            textDebug.SetText(artifacts.Count.ToString());
        }
        textDebug.gameObject.SetActive(true);
#endif

    }


    public void SetState(TileState state)
    {
        switch (state)
        {
            case TileState.None:
                normalMaterial.color = Color.white;
                break;
            case TileState.Hoover:
                normalMaterial.color = hooverColor;
                break;
            case TileState.Selected:
                normalMaterial.color = selectedColor;
                break;
            case TileState.PathPart:
                normalMaterial.color = pathPartColor;
                break;
        }

        meshRenderer.material = normalMaterial;
        tileState = state;
    }

    public enum TileState
    {
        None,
        Hoover,
        Selected,
        PathPart,
    }

    internal void SetHidden ()
    {
        model.gameObject.SetActive(false);
    }

    internal bool Reveal(float delay = 0f)
    {
        if (IsRevealed()) return false;
        model.gameObject.SetActive(true);

        float duration = .5f;
        // TODO tweak?
        model.transform.localPosition = new Vector3(0, -5, 0);
        model.transform.DOLocalMoveY(0, duration).SetEase(Ease.OutBack).SetDelay(delay).OnComplete(() => {
            OnRevealed?.Invoke(this);
        });

        model.localScale = new Vector3(.1f, .1f, .1f);
        model.transform.DOScale(new Vector3(1, 1, 1), duration).SetEase(Ease.OutBack).SetDelay(delay);

        return true;
    }

    public bool IsRevealed ()
    {
        return model.gameObject.activeInHierarchy;
    }

    public void SetOccupyingTeam(TeamScript team, bool forceCenterPosition = false)
    {
        if (OccupyingTeam != null)
        {
            Debug.LogError("Already occupied!", this);
            return;
        }

        if (team.occupiedTile) team.occupiedTile.ClearOccupation();

        team.transform.parent = transform;
        if(forceCenterPosition)
            team.transform.localPosition = new Vector3(0, 0, 0);

        OccupyingTeam = team;
        
        team.SetOccupiedTile(this);
    }

    public void ClearOccupation()
    {
        OccupyingTeam = null;
    }

    public bool HasTeam()
    {
        return OccupyingTeam;
    }
}
