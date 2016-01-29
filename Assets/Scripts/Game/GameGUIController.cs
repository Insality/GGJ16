using System.Collections.Generic;
using UnityEngine;

public class GameGUIController : MonoBehaviour
{
    public List<Action> ActionPanelList = new List<Action>();
    public GameObject PressZone;

    void Update()
    {
        UpdateActionPanelList();

        UpdateControl();
    }

    void UpdateActionPanelList()
    {
        foreach (var action in ActionPanelList)
        {
            action.MoveTo(PressZone.transform.position - new Vector3(2, 0, 0), 6);
        }
        
    }

    void UpdateControl()
    {
//        if (Coll)
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
}
