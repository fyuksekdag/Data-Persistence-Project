using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nameField;

    public string username = "";

    public string bestName;
    public int bestScore;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    } 
    void Start()
    {

    }

    public void SetBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            bestName = username;
            HighScoreTable.AddHighScoreEntry(bestScore, bestName);
        }
        Debug.Log("Score: " + score + " Player: " + username);
    }
    public void StartNew()
    {
        nameField = FindObjectOfType<TMP_InputField>();
        if (nameField.text != "")
        {
            username = nameField.text;
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.LogWarning("Please enter a name");
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }
}
