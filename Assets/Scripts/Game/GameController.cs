using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Fire Fire;
    public Sun Sun;
    public GameGUIController GameGUIController;
    public ActionController ActionController;

    public float CurrentProgress = 0;

    private GameState _currentState;
    private bool _isRunning = false;
    private List<ActionType> DanceList = new List<ActionType>(); 
    private List<ActionType> PlayerDanceList = new List<ActionType>(); 

	void Start ()
	{
//	    StartCoroutine(StartSpamActions());
        SoundController.PlayMusic(MusicType.GameDrums);
	}
	
	void Update () {
#if UNITY_EDITOR
	    if (Input.GetKeyDown(KeyCode.Q))
	    {
	        GameGUIController.SendRandomAction();
	    }

        if (Input.GetKeyDown(KeyCode.W)) {
            Fire.AddShaman(AppController.GetInstance().GetGenerator().GenerateShaman(Fire.transform));
        }
	    if (Input.GetKeyDown(KeyCode.E))
	    {
	        StartGame();
	    }
#endif

	    CurrentProgress += Time.deltaTime/5;
	    if (CurrentProgress > 1) CurrentProgress = 0;
        SetProgress(CurrentProgress);
	}

    private void StartGame()
    {
        _isRunning = true;
        SetGameState(GameState.Dance);
        StartCoroutine(GameCoroutine());
    }

    IEnumerator StartSpamActions(bool isSmartSpam = true)
    {
        List<ActionType> PreGeneratedActions = new List<ActionType>();
        if (isSmartSpam)
        {
            foreach (var actionType in DanceList)
            {
                PreGeneratedActions.Add(actionType);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            PreGeneratedActions.Insert(Random.Range(0, PreGeneratedActions.Count), (ActionType)Random.Range(0, Constants.ACTIONS_COUNT));
        }

        while (_currentState == GameState.Repeat)
        {

            if (isSmartSpam && PreGeneratedActions.Count > 0)
            {
                GameGUIController.SendPanelAction(PreGeneratedActions[0]);
                PreGeneratedActions.RemoveAt(0);
            }
            else
            {
                GameGUIController.SendRandomAction();
            }
            yield return new WaitForSeconds(0.5f);    
        }
    }

    public void SetProgress(float perc)
    {
        Sun.SetTargetProgress(perc);
        GameGUIController.SetProgress(perc);
    }

    private void SetGameState(GameState state)
    {
        Debug.Log("Change game state to " + state);
        _currentState = state;
        if (_currentState == GameState.Repeat)
        {
            PlayerDanceList.Clear();
        }
        if (_currentState == GameState.Waiting)
        {
            GameGUIController.FlagClearActionPanel();
        }
    }

    public List<ActionType> GeneratePattern(int size)
    {
        var list = new List<ActionType>();

        for (int i = 0; i < size; i++)
        {
            list.Add((ActionType)Random.Range(0, Constants.ACTIONS_COUNT));
        }

        Debug.Log("New patttern:");
        foreach (var a in list)
        {
            Debug.Log(a);
        }

        return list;
    }

    private IEnumerator GameCoroutine()
    {
        while (true)
        {
            DanceList = GeneratePattern(2);

            foreach (var action in DanceList)
            {
                GameGUIController.ShowActionIcon(Fire.transform, action);
                yield return new WaitForSeconds(Constants.TIME_BETWEEN_ACTIONS);
            }
            Debug.Log("Now Player turn");

            SetGameState(GameState.Repeat);
            StartCoroutine(StartSpamActions());
            StartTimer(5f);

            while (_currentState != GameState.Waiting)
            {
                yield return null;
            }

            yield return new WaitForSeconds(4);
            SetGameState(GameState.Dance);
        }
    }

    private void StartTimer(float time)
    {
        
    }

    public void OnActionChosed(ActionType aType)
    {
        Debug.Log("Action chosed: " + aType);
        PlayerDanceList.Add(aType);
        if (DanceList.Count >= PlayerDanceList.Count)
        {
            if (DanceList[PlayerDanceList.Count-1] == aType)
            {
                OnRightAction(aType);
            }
            else
            {
                OnErrorAction();
            }
        }
        else
        {
            OnErrorAction();
        }

        if (DanceList.Count == PlayerDanceList.Count)
        {
            SetGameState(GameState.Waiting);
        }

    }

    public void OnTimerEnd()
    {
        Debug.Log("Timer time is up!");
    }

    public void OnErrorAction()
    {
        Debug.Log("WRONG ACTION");
        SetGameState(GameState.Waiting);
        GameGUIController.ShowActionIcon(Fire.transform, ActionType.Sad);
    }

    public void OnRightAction(ActionType action)
    {
        Debug.Log("NICE ACTION: " + action);
        GameGUIController.ShowActionIcon(Fire.transform, action);
    }

}


public enum GameState
{
    Dance = 0,
    Repeat = 1,
    Waiting = 2,
}
