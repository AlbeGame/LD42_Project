using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager I;

    public FrogController Frog;
    public LilypadSpawner LilySpawner;
    public BugsSpawner BugsSpawner;
    public UIController UICtrl;

    InputController inputCtrl;

    private void Awake()
    {
        if (I != null)
            DestroyImmediate(this.gameObject);
        else
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        MainMenu();
    }

    public void StartGame()
    {
        if(LilySpawner == null)
            LilySpawner = FindObjectOfType<LilypadSpawner>();
        LilySpawner.gameObject.SetActive(true);

        if (BugsSpawner == null)
            BugsSpawner = FindObjectOfType<BugsSpawner>();
        BugsSpawner.gameObject.SetActive(true);

        LilySpawner.SpawnLilypad();

        if(Frog == null)
            Frog = FindObjectOfType<FrogController>();
        Frog.gameObject.SetActive(true);

        LilypadController spawnedLily = GetCloseLilyPad(Frog.transform.position, 100);
        Frog.transform.position = spawnedLily.transform.position;
        Frog.SetParentLily(spawnedLily);

        if(UICtrl == null)
            UICtrl = FindObjectOfType<UIController>();
        UICtrl.ToggleMainMenu(false);

        if(inputCtrl == null)
            inputCtrl = GetComponent<InputController>();
        inputCtrl.enabled = true;
    }

    public void MainMenu()
    {
        inputCtrl = GetComponent<InputController>();
        inputCtrl.enabled = false;
        UICtrl.ToggleMainMenu(true);
        Frog.gameObject.SetActive(false);
        LilySpawner.gameObject.SetActive(false);
        BugsSpawner.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.sceneLoaded += OnSceneReload;
        SceneManager.LoadScene("GameplayScene");
    }

    public void OnSceneReload(Scene _scene, LoadSceneMode _mode)
    {
        StartGame();
        SceneManager.sceneLoaded -= OnSceneReload;
    }

    public LilypadController GetCloseLilyPad(Vector2 from,float min_dist){
        foreach(LilypadController l in LilySpawner.GetLilipads()) {
            if(Vector2.Distance(l.transform.position, from) < min_dist)
                return l;
        }
        return null;
    }
}