using System.Collections.Generic;
using UnityEngine;

public class GameGUIController : MonoBehaviour
{
    public List<Action> ActionPanelList = new List<Action>();
    public List<Action> ActionStackList = new List<Action>();
    
    public GameObject PressZone;
    private BoxCollider _pressZoneCollider;
    private Vector3 _actionPanelTarget;

    public GameObject StackZone;
    private float _gap = 1f;

    public TimePanel TimePanel;

    


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
    }

    public void SetProgress(float perc)
    {
        TimePanel.SetPercentage(perc);
    }

    void UpdateActionPanelList()
    {
        foreach (var action in ActionPanelList)
        {
            action.MoveTo(_actionPanelTarget, 6);
        }

        RefreshPanel();
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
            foreach (var action in ActionPanelList)
            {
                if (action.IsCollideWith(_pressZoneCollider))
                {
                    ActionStackList.Add(action);
                    action.PlaySound();
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
    }

    public void SendRandomAction()
    {
        var gen = AppController.GetInstance().GetGenerator();
        Action action = gen.GenerateAction(transform);

        action.Randomize();
        SetPositionStartActionPanel(action.gameObject);
        ActionPanelList.Add(action);
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
}
