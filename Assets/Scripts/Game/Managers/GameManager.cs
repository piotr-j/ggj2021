using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("App")]
    public AppData appData;

    [Header("Managers")]
    public MapManager mapManager;
    public PathManager pathManager;
    public InputManager inputManager;
    public TeamsManager teamsManager;
    public ArtifactManager artifactManager;
    public SoundManager soundManager;
    
    [HideInInspector] public new CameraScript camera;

    [HideInInspector] public bool isGameStarted;
    [HideInInspector] public bool isGameCompleted;
    [HideInInspector] public bool isGamePaused;
    [HideInInspector] public bool isGameStopped;

    private float previousTimeScale;

    private bool playerTurnFinished;

    private enum EndState
    {
        Won,
        Failed
    }
    
	private void Awake()
    {
        // player.onDead += OnPlayerDeath;
        //
        // enemyManager.onEnemyDied += HandleEnemyDrop;
        // enemyManager.onEnemyDied += powerUpManager.OnEnemyDeathPowerupSpawn;
        // enemyManager.isTimerPaused += () => { return isGamePaused || isGameStopped; };
        //
        // timeManager.isGameStopped += () => { return isGameStopped; };

        // player.deathAllowedFuncs += IsPlayerAllowedToDie;
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = App.instance.camera.GetComponent<CameraScript>();

        // Gameplay
        mapManager.onMapReady += r => { StartCoroutine(SetupGame()); };
        mapManager.OnTileSelected += r => Events.singleton.changeDeselectVisibility.Raise(true);
        mapManager.OnTileDeselected += r => Events.singleton.changeDeselectVisibility.Raise(false);

        inputManager.IsInputLocked += () => playerTurnFinished;
        
        // Sfx
        inputManager.OnActionDenided += () => soundManager.PlaySound(SoundManager.SFXType.ActionDenied);
        inputManager.OnMoveOrder += () => soundManager.PlaySound(SoundManager.SFXType.MoveOrder);
        inputManager.OnSelected += () => soundManager.PlaySound(SoundManager.SFXType.Select);
        mapManager.OnTileDeselected += r => soundManager.PlaySound(SoundManager.SFXType.Deselect);
        teamsManager.OnDigStart += r => soundManager.PlaySound(SoundManager.SFXType.Dig);
        teamsManager.OnDigResult += r =>
        {
            if (r == null || r.Count == 0)
            {
                soundManager.PlaySound(SoundManager.SFXType.DigFail);
            }
            else
            {
                soundManager.PlaySound(SoundManager.SFXType.GotArtifact);
            }
        };
        teamsManager.OnTeamPathStepStarted += r => soundManager.PlaySound(SoundManager.SFXType.Land);
        
        // UI Raised
        Events.singleton.onUIDeselectRequest.action += mapManager.DeselectAll;
    }

    IEnumerator SetupGame()
    {
        ResetState();
        
        teamsManager.CreateStartingTeam();
        
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(RunGameTurns());
    }

    IEnumerator RunGameTurns()
    {
        EndState endState;
        while (!GameEndCondition(out endState))
        {
            OnPlayerTurnStart();
            yield return PlayerActions();
            OnPlayerTurnFinished();
            
            yield return new WaitForSeconds(0.3f);

            yield return TeamMovement();
            yield return TeamDigging();
        }
        
        Debug.Log("Game finished with state: " + endState);
    }

    private void OnPlayerTurnFinished()
    {
        
    }

    private void OnPlayerTurnStart()
    {
        playerTurnFinished = false;
    }


    private bool GameEndCondition(out EndState endState)
    {
        endState = EndState.Won;
        
        return false;
    }

    private IEnumerator PlayerActions()
    {
        yield return new WaitUntil(() => playerTurnFinished);
    }
    
    private IEnumerator TeamMovement()
    {
        yield return teamsManager.HandleMove(SpendTurn);
    }
    
    private IEnumerator TeamDigging()
    {
        yield return teamsManager.HandleDigging(SpendTurn);
    }


    public void ResetState()
    {
		isGameStarted = false;
        isGameCompleted = false;
        isGamePaused = false;
        isGameStopped = false;

        playerTurnFinished = true;
        // enemyManager.ResetState();
        
        appData.data.gameState.turnsLeft.SetValue(appData.config.gameConfig.turnsPerLevel);
        
        artifactManager.ResetState();
    }
    

    public void CompleteLevel()
    {
        if (isGameCompleted)
            return;

        StartCoroutine(CompleteLevelInternal());
    }

    private IEnumerator CompleteLevelInternal()
    {
        yield return new WaitForSeconds(1);

        Events.singleton.onLevelCompleted.Raise();
    }
    
    public void SetStopped(bool isStopped)
    {
        if (isStopped && !isGameStopped)
        {
            previousTimeScale = Time.timeScale;
            isGameStopped = true;
            Time.timeScale = 0;
        } 
        else if (!isStopped && isGameStopped)
        {
            Time.timeScale = previousTimeScale;
            isGameStopped = false;
        }
    }

    public void SetStared()
    {
        isGameStarted = true;
    }

    public void TurnMoveTeam(HexTile teamTile, List<HexTile> path)
    {
        teamsManager.ScheduleMove(teamTile, path);
        
        EndPlayerTurn();
    }
    
    public void TurnDig(HexTile teamTile)
    {
        teamsManager.ScheduleDigging(teamTile);
        
        EndPlayerTurn();
    }


    private void EndPlayerTurn()
    {
        playerTurnFinished = true;
    }

    public int TurnsLeft()
    {
        return appData.data.gameState.turnsLeft.Value;
    }

    private void SpendTurn()
    {
        appData.data.gameState.turnsLeft.ApplyChange(-1);
    }

    public void ArtifactsFound(List<Artifact> artifacts)
    {
        appData.data.gameState.artifactsGot.ApplyChange(artifacts.Count);
    }

}
