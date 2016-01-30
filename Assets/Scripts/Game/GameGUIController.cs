using System.Collections.Generic;
using UnityEngine;

public class GameGUIController : MonoBehaviour
{
    public List<Action> ActionPanelList = new List<Action>();
    public List<Action> ActionStackList = new List<Action>();
    
    public GameObject PressZone;
    private BoxCollider _pressZoneCollider;
    private Vector3 _actionPanelTarget;

    [SerializeField] private tk2dSprite GroundBackground;
    [SerializeField] private tk2dSprite SkyBackground;

    public GameObject StackZone;
    private float _gap = 1f;

    public TimePanel TimePanel;

    [SerializeField]
    private GameController _gameController;

    private bool _isNeedToCleanActionPanel = false;
    

    void OnEnable()
    {
        _pressZoneCollider = PressZone.GetComponent<BoxCollider>();
        _actionPanelTarget = PressZone.transform.position - new Vector3(2, 0, 0);
    }

    void Update()
    {
        UpdateActionPanelList();
        UpdateActionStackList();

        UpdateControl();

        if (_isNeedToCleanActionPanel)
        {
            ClearActionPanel();
            
        }
    }

    public void SetProgress(float perc)
    {
        UpdateBackgroundSprites(perc);
    }

    void UpdateActionPanelList()
    {
        foreach (var action in ActionPanelList)
        {
            action.MoveTo(_actionPanelTarget, 3);
        }

        RefreshPanel();
    }

    void UpdateBackgroundSprites(float perc)
    {
        if (perc < 0.33f)
        {
            GroundBackground.SetSprite("BackgroundGround1");
            SkyBackground.SetSprite("BackgroundSky1");
        }
        else if (perc < 0.67f)
        {
            GroundBackground.SetSprite("BackgroundGround2");
            SkyBackground.SetSprite("BackgroundSky2");
        }
        else
        {
            GroundBackground.SetSprite("BackgroundGround3");
            SkyBackground.SetSprite("BackgroundSky3");
        }
    }

    void UpdateActionStackList()
    {
        for (int i = 0; i < ActionStackList.Count; i++)
        {
            ActionStackList[i].MoveTo(
                StackZone.transform.position + 
                new Vector3(i * _gap, 0, 0) + 
                new Vector3(-2, 0, 0),
                8);
        }
    }

    void UpdateControl()    
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PressZone.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            foreach (var action in ActionPanelList)
            {
                if (action.IsCollideWith(_pressZoneCollider))
                {
                    action.PlaySound();
                    action.Hide();
                    _gameController.OnActionChosed(action.Type);
                }
            }
            // Remove From ActionLinePanel
            foreach (var action in ActionStackList)
            {
                if (ActionPanelList.Contains(action))
                {
                    ActionPanelList.Remove(action);
                }
            }
            RefreshStack();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            PressZone.transform.localScale = new Vector3(1f, 1f, 1);
        }
    }

    public void SendRandomAction()
    {
        var gen = AppController.GetInstance().GetGenerator();
        Action action = gen.GenerateAction(transform);

        action.Randomize(_gameController.MaxActionsVar);
        SetPositionStartActionPanel(action.gameObject);
        ActionPanelList.Add(action);
    }

    public void SendPanelAction(ActionType aType)
    {
        var gen = AppController.GetInstance().GetGenerator();
        Action action = gen.GenerateAction(transform);

        action.SetType(aType);
        SetPositionStartActionPanel(action.gameObject);
        ActionPanelList.Add(action);
    }

    public void ShowActionIcon(Transform parent, ActionType aType)
    {
        var gen = AppController.GetInstance().GetGenerator();   
        Action action = gen.GenerateAction(parent);

        action.SetType(aType);
        action.SetParticle();
    }

    private void SetPositionStartActionPanel(GameObject obj)
    {
        obj.transform.position = PressZone.transform.position + new Vector3(7, 0, 0);
    }

    private void RefreshPanel() {
        for (int i = ActionPanelList.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(ActionPanelList[i].transform.position, _actionPanelTarget) < 0.1f)
            {
                Destroy(ActionPanelList[i].gameObject);
                ActionPanelList.Remove(ActionPanelList[i]);
            }
        }
    }

    private void RefreshStack()
    {
        if (ActionStackList.Count >= 6)
        {
            var firstAction = ActionStackList[0];
            ActionStackList.Remove(firstAction);
            Destroy(firstAction.gameObject);
        }
    }

    public void FlagClearActionPanel()
    {
        _isNeedToCleanActionPanel = true;
        ;
    }

    private void ClearActionPanel()
    {
        foreach (var action in ActionPanelList)
        {
            Destroy(action.gameObject);
        }
        ActionPanelList.Clear();
        _isNeedToCleanActionPanel = false;
    }
}
