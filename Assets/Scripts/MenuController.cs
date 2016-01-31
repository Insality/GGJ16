using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public tk2dUIItem EasyButton;
    public tk2dUIItem HardButton;

    public List<Shaman> MapShamans;


    void Start()
    {
        foreach (var shaman in MapShamans)
        {
            shaman.LoopAction(ActionType.Magic);
        }
        SoundController.PlayMusic(MusicType.Menu);
        SoundController.SetPitch(0.9f);
    }
    void OnEnable()
    {
        EasyButton.OnClickUIItem += OnEasyButtonClick;
        HardButton.OnClickUIItem += OnHardButtonClick;
    }

    void OnDisable()
    {
        EasyButton.OnClickUIItem -= OnEasyButtonClick;
        HardButton.OnClickUIItem -= OnHardButtonClick;
    }

    private void OnEasyButtonClick(tk2dUIItem tk2dUiItem)
    {
        AppController.GetInstance().Difc = 0;
        SceneManager.LoadScene(Constants.SCENE_GAME);
    }

    private void OnHardButtonClick(tk2dUIItem tk2dUiItem)
    {
        AppController.GetInstance().Difc = 1;
        SceneManager.LoadScene(Constants.SCENE_GAME);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
