using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TeamsManager : MonoBehaviour
{
    private GameManager gameManager;
    
    [SerializeField]
    private TeamScript teamPrefab;

    private List<TeamScript> aliveTeams = new List<TeamScript>();
    
    private List<TeamMovement> scheduledTeamMoves = new List<TeamMovement>();
    private List<HexTile> scheduledDigging = new List<HexTile>();

    public Action<HexTile> OnTeamPathStepStarted;
    public Action<HexTile> OnTeamPathStepReached;
    
    public Action<List<Artifact>> OnDigResult;
    public Action<HexTile> OnDigStart;
    
    void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    public void CreateStartingTeam()
    {
        var newTeam = Instantiate(teamPrefab);
        gameManager.mapManager.CenterTile.SetOccupyingTeam(newTeam, true);

        aliveTeams.Add(newTeam);
    }

    public IEnumerator HandleMove(Action onTurnSpend)
    {
        foreach (var nextTeamMove in scheduledTeamMoves)
        {
            foreach (var pathTile in nextTeamMove.path)
            {
                onTurnSpend?.Invoke();

                float moveDuration = .5f;
                var team = nextTeamMove.team;
                var tween = team.transform
                    .DOMove(pathTile.transform.position, moveDuration)
                    .SetEase(Ease.InOutSine);

                team.HopModels(moveDuration - .05f, pathTile.transform.position);
                
                OnTeamPathStepStarted?.Invoke(pathTile);

                yield return tween.WaitForCompletion();

                pathTile.SetOccupyingTeam(team);
                OnTeamPathStepReached?.Invoke(pathTile);
                
                // yield return 0.75f;
            }
        }
        
        scheduledTeamMoves.Clear();
    }
    
    public IEnumerator HandleDigging(Action onTurnSpend)
    {
        foreach (var nextTileToDig in scheduledDigging)
        {
            onTurnSpend?.Invoke();
            OnDigStart?.Invoke(nextTileToDig);

            yield return 2.5f;

            var artifacts = nextTileToDig.Excavate();
            if (artifacts != null && artifacts.Count > 0)
            {
                gameManager.ArtifactsFound(artifacts);
            }

            OnDigResult?.Invoke(artifacts);
                
            yield return 0.2f;
        }
        
        scheduledDigging.Clear();
    }

    public void ScheduleMove(HexTile teamTile, List<HexTile> path)
    {
        path.RemoveAt(0);
        
        var teamMovement = new TeamMovement { team = teamTile.OccupyingTeam, path = path };

        scheduledTeamMoves.Add(teamMovement);
    }
    
    public void ScheduleDigging(HexTile teamTile)
    {
        if (!teamTile || !teamTile.HasTeam())
        {
            Debug.LogError("Null tile or without team!", teamTile);
        }
        
        scheduledDigging.Add(teamTile);
    }

    private class TeamMovement
    {
        public TeamScript team;
        public List<HexTile> path;
    }
}
