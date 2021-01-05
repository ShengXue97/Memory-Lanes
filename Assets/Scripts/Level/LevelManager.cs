using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, ITriggerListener, IGoalListener
{
    private AudioManager audioManager;
    
    [SerializeField]
    private LevelDatabase levelDatabase;
    private string currentLevelName;

    [SerializeField]
    private Level level;

    // private bool[] triggerStates;
    private bool[] activatorStates;

    public bool isSaved = false;
    public int saveCount = 0;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        level = FindObjectOfType<Level>();
        currentLevelName = SceneManager.GetActiveScene().name;
    }

    public void Start()
    {
        audioManager.Music = level.music;
        audioManager.PlayMusic();

        // triggerStates = level.triggers.Select(trigger => trigger.on).ToArray();
        activatorStates = level.activators.Select(activator => activator.on).ToArray();

        foreach (var trigger in level.triggers)
        {
            trigger.SetListener(this);
        }

        level.goal.SetListener(this);
    }

    public void OnTriggerEvent(TriggerEvent triggerEvent)
    {
        ActivatorEvent[] events = level.GetTriggerEvents(triggerEvent);
        foreach (ActivatorEvent evt in events)
        {
            switch (evt.action)
            {
                case ActivatorAction.On:
                    evt.activator.On();
                    break;
                case ActivatorAction.Off:
                    evt.activator.Off();
                    break;
                case ActivatorAction.Toggle:
                    evt.activator.Toggle();
                    break;
                default:
                    break;
            }
        }
    }

    public void OnFinishLevel()
    {
        DisablePlayer();
    }
    
    public void OnExitLevel()
    {
        LoadNextLevel();
    }

    public void SaveState()
    {
        // for (int i = 0; i < triggerStates.Length; i++)
        // {
        //     triggerStates[i] = level.triggers[i].on;
        // }
        for (int i = 0; i < activatorStates.Length; i++)
        {
            activatorStates[i] = level.activators[i].on;
        }
        isSaved = true;
        level.saveLoadIndicator.SetSaveCount(++saveCount);
        level.saveLoadIndicator.Save();
    }

    public void LoadState()
    {
        if (!isSaved)
        {
             return;
        }
        // for (int i = 0; i < triggerStates.Length; i++)
        // {
        //     level.triggers[i].on = triggerStates[i];
        // }
        for (int i = 0; i < activatorStates.Length; i++)
        {
            if (activatorStates[i])
            {
                level.activators[i].On();
            }
            else
            {
                level.activators[i].Off();
            }
        }
        isSaved = false;
        level.saveLoadIndicator.Load();
    }

    // Load Previous Scene
    public void LoadPreviousLevel()
    {
        string levelName = levelDatabase.GetPreviousLevel(currentLevelName);
        SceneManager.LoadScene(levelName);
    }

    // Load Next Scene
    public void LoadNextLevel()
    {
        string levelName = levelDatabase.GetNextLevel(currentLevelName);
        SceneManager.LoadScene(levelName);
    }

    // Reload current Scene
    public void ReloadLevel()
    {
        SceneManager.LoadScene(currentLevelName);
    }

    public void DisablePlayer()
    {
        level.player.gameObject.SetActive(false);
    }
}
