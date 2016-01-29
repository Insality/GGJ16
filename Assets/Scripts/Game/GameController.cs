using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Fire Fire;
    public Sun Sun;
    public GameGUIController GameGUIController;
    public ActionController ActionController;

	void Start ()
	{
	    StartCoroutine(StartSpamActions());
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
#endif
	}

    IEnumerator StartSpamActions()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            GameGUIController.SendRandomAction();
            yield return new WaitForSeconds(0.5f);    
        }
    }
}
