using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float _actionTimer = -1;
    private float _maxTime = 1;


    public static int Difficulty;
    // Game Settings Difc.
    public int DanceCount = 2;
    public float PanelSpeed = 10;
    public int MaxActionsVar = 4;

	void Start ()
	{
//	    StartCoroutine(StartSpamActions());

        SoundController.StopMusic();
	    Difficulty = AppController.GetInstance().Difc;
        for (int i = 0; i < 4; i++)
        {
            var shaman = AppController.GetInstance().GetGenerator().GenerateShaman(Fire.transform);

            if (i == 1 || i == 2)
            {
                shaman.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            }
            if (i == 3)
            {
                shaman.transform.localScale = new Vector3(0.85f, 0.85f, 1);
            }
            Fire.AddShaman(shaman);
        }

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
	    if (Input.GetKeyDown(KeyCode.Alpha1))
	    {
            Fire.AllDance(ActionType.Jump);
	    }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Fire.AllDance(ActionType.Clap);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Fire.AllDance(ActionType.Magic);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Fire.AllDance(ActionType.Music);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Fire.AllDance(ActionType.Right);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Fire.AllDance(ActionType.Left);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Fire.AllDance(ActionType.Stump);
        }
#endif

	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        if (!_isRunning)
	        {
	            StartGame();
	        }
	    }

        UpdateTimer();
	    if (CurrentProgress >= 1 && _currentState != GameState.Winning)
	    {
//	        CurrentProgress = 0;
	        CurrentProgress = 1;
	        WinLevel();
	    }
        SetProgress(CurrentProgress);
	}

    void OnMouseDown()
    {
        if (!_isRunning)
        {
            StartGame();
        }
    }


    private void UpdateTimer()
    {

        if (_actionTimer > 0)
        {
            _actionTimer -= Time.deltaTime;
        }
        if (_actionTimer <= 0 && _actionTimer != -1)
        {
            _actionTimer = -1;
            OnTimerEnd();
        }

        if (_actionTimer < 0)
        {
            GameGUIController.TimePanel.SetPercentage(1);
        }
        else
        {
            GameGUIController.TimePanel.SetPercentage(_actionTimer / _maxTime);
        }
        
        
    }

    private void StartGame()
    {
        CurrentProgress = 0;
        SetProgress(0);
        _isRunning = true;
        SetGameState(GameState.Dance);
        SoundController.PlayMusic(MusicType.GameDrums);

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
        for (int i = 0; i < 6; i++)
        {
            PreGeneratedActions.Insert(Random.Range(0, PreGeneratedActions.Count), (ActionType)Random.Range(0, MaxActionsVar));
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
            yield return new WaitForSeconds(0.4f);    
        }
    }

    public void SetProgress(float perc)
    {
        Sun.SetTargetProgress(perc);
        GameGUIController.SetProgress(perc);
        SoundController.SetPitch(1f + perc/5f);

        if (Difficulty == 0)
        {
            if (perc < 0.33)
            {
                MaxActionsVar = 4;
            }
            else
            {
                MaxActionsVar = 5;
            }
        }
        if (Difficulty == 1)
        {
            if (perc < 0.33)
            {
                MaxActionsVar = 5;
            }
            if (perc < 0.66)
            {
                MaxActionsVar = 6;
            }
            else
            {
                MaxActionsVar = 7;
            }
        }
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
            _actionTimer = -1;
            GameGUIController.FlagClearActionPanel();
        }
    }

    public List<ActionType> GeneratePattern(int size)
    {
        var list = new List<ActionType>();

        for (int i = 0; i < size; i++)
        {
            var action = (ActionType)(Random.Range(0, MaxActionsVar));
            if (Fire.DanceAngle > 310 && action == ActionType.Right) action = ActionType.Left;
            if (Fire.DanceAngle < 250 && action == ActionType.Left) action = ActionType.Right;
            list.Add(action);
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
            var count = DanceCount;
            if (CurrentProgress > 0.30f)
            {
                count++;
            }
            if (CurrentProgress > 0.65f)
            {
                count++;
            }
            if (Difficulty == 1)
            {
                count += 2;
            }
            DanceList = GeneratePattern(count);

            foreach (var action in DanceList)
            {
                GameGUIController.ShowActionIcon(Fire.transform, action);
                Fire.ShamanDance(action);

                if (action == ActionType.Left)
                {
                    Fire.TurnLeft();
                }
                if (action == ActionType.Right)
                {
                    Fire.TurnRight();
                }

                if (DanceList.IndexOf(action) != DanceCount - 1)
                {
                    yield return new WaitForSeconds(Constants.TIME_BETWEEN_ACTIONS);
                }
                else
                {
                    yield return new WaitForSeconds(Constants.TIME_BETWEEN_ACTIONS/2);
                }
            }
            Debug.Log("Now Player turn");

            SetGameState(GameState.Repeat);
            StartCoroutine(StartSpamActions());
            StartTimer(6f + count/2);

            while (_currentState != GameState.Waiting)
            {
                yield return null;
            }

            yield return new WaitForSeconds(2.5f);
            SetGameState(GameState.Dance);
        }
    }

    private void StartTimer(float time)
    {
        Debug.Log("Start timer");
        _actionTimer = time;
        _maxTime = time;
    }

    public void OnActionChosed(ActionType aType)
    {
        bool canWinRound = true;
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
                canWinRound = false;
                OnErrorAction();
            }
        }
        else
        {
            canWinRound = false;
            OnErrorAction();
        }

        if (DanceList.Count == PlayerDanceList.Count)
        {
            SetGameState(GameState.Waiting);
            if (canWinRound)
            {
                Debug.Log("Win round");
                OnWinRound();
            }
        }

    }

    public void OnWinRound()
    {
        CurrentProgress += 0.15f;
//        CurrentProgress += 1f;
        SoundController.PlaySound(SoundType.WinRound);
    }

    public void OnTimerEnd()
    {
        OnErrorAction();
    }

    public void OnErrorAction()
    {
        SetGameState(GameState.Waiting);
        GameGUIController.ShowActionIcon(Fire.transform, ActionType.Sad);   
    }

    public void OnRightAction(ActionType action)
    {
        GameGUIController.ShowActionIcon(Fire.transform, action);
        Fire.PlayerDance(action);

        if (action == ActionType.Left)
        {
            Fire.TurnLeft();
        }
        if (action == ActionType.Right)
        {
            Fire.TurnRight();
        }
    }
    private void WinLevel()
    {
        SoundController.PlaySound(SoundType.WinLevel);
        SetGameState(GameState.Winning);
        StartCoroutine(StartWinGameAnimation());
    }

    private IEnumerator StartWinGameAnimation()
    {
        yield return  new WaitForSeconds(1f);
        Fire.AllDance(ActionType.Magic);
        Fire.AllDance(ActionType.Magic);
        Fire.AllDance(ActionType.Magic);
        Fire.AllDance(ActionType.Magic);

        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(Constants.SCENE_MENU);
    }

}


public enum GameState
{
    Dance = 0,
    Repeat = 1,
    Waiting = 2,
    Winning = 3,
}
