using UnityEngine;

public class GameController : MonoBehaviour
{

    public Fire Fire;
    public GameGUIController GameGUIController;
    public ActionController ActionController;

	void Start () {
	
	}
	
	void Update () {
#if UNITY_EDITOR
	    if (Input.GetKeyDown(KeyCode.Q))
	    {
	        GameGUIController.SendRandomAction();
	    }
#endif
	}
}
