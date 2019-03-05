using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;

    private string[] levels = { "level1", "level2" };

    [HideInInspector] public int currentLevel;

    public GameObject Player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
        //levels = new string[] { "MainMenu", "level1", "level2" };
        currentLevel = 0;
        Debug.Log("LEVELS: " + levels.Length);
        foreach (var l in levels)
        {
            Debug.Log(l);
        }
        
    }

	public void PlayerWon()
    {
        Player.SetActive(false);
    }

    public void GoToNextScene()
    {
        currentLevel = (currentLevel + 1) % levels.Length;
        SceneManager.LoadScene(levels[currentLevel]);
    }

    public void PlayCurrentLevel()
    {
        SceneManager.LoadScene(levels[currentLevel]);
    }
}
