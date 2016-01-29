using UnityEngine;

public class AppController : MonoBehaviour
{

    private static AppController _appController;
    public PrefabGenerator PrefabGenerator;

    void Start()
    {
        _appController = this;
        DontDestroyOnLoad(gameObject);
        LoadScene(Constants.SCENE_GAME);
    }

    public void LoadScene(string sceneName)
    {
        Application.LoadLevelAsync(sceneName);
    }

    public PrefabGenerator GetGenerator()
    {
        return PrefabGenerator;
    }

    public static AppController GetInstance()
    {
        if (_appController == null)
        {
            _appController = new AppController();
        }
        return _appController;
    }
}
