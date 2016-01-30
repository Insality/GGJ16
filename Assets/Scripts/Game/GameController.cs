using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Fire Fire;
    public Sun Sun;
    public GameGUIController GameGUIController;
    public ActionController ActionController;

    public float CurrentProgress = 0;

	void Start ()
	{
	    StartCoroutine(StartSpamActions());
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
#endif

	    CurrentProgress += Time.deltaTime/5;
	    if (CurrentProgress > 1) CurrentProgress = 0;
        SetProgress(CurrentProgress);
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

    public void SetProgress(float perc)
    {
        Sun.SetTargetProgress(perc);
        GameGUIController.SetProgress(perc);
    }
}
