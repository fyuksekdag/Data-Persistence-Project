using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    [SerializeField] Transform entryContainer;
    [SerializeField] Transform entryTemplate;
    private List<Transform> highScoreEntryTransformList;

    public static HighScoreTable Instance;
    private void Awake()
    {
        /*if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);*/

        /*entryContainer = transform.Find("highScoreEntryContainer");
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");*/
        // we hide the default template
        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            Debug.Log("Initializing table with default values...");
            AddHighScoreEntry(30, "CMK");
            AddHighScoreEntry(28, "JOE");
            AddHighScoreEntry(24, "DAV");
            AddHighScoreEntry(20, "CAT");
            AddHighScoreEntry(14, "MAX");
            AddHighScoreEntry(8, "AAA");
            // Reload
            jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }

            //Sort entry list by Score
        for (int i = 0; i < highscores.highScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highScoreEntryList.Count; j++)
            {
                if (highscores.highScoreEntryList[j].score > highscores.highScoreEntryList[i].score)
                {
                    // swap their positions
                    HighScoreEntry tmp = highscores.highScoreEntryList[i];
                    highscores.highScoreEntryList[i] = highscores.highScoreEntryList[j];
                    highscores.highScoreEntryList[j] = tmp;
                }
            }
        }

        highScoreEntryTransformList = new List<Transform>();
        if (highscores.highScoreEntryList.Count > 9)
        {
            for (int i = 0; i < 10; i++)
            {
                CreateHighscoreEntryTransform(highscores.highScoreEntryList[i], entryContainer, highScoreEntryTransformList);
            }
        }
        else
        {
            foreach (HighScoreEntry highScoreEntry in highscores.highScoreEntryList)
            {
                CreateHighscoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
            }
        }


    }

    private void CreateHighscoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 26f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1; //we dont want to 0 rank
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;

        }

        entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;

        int score = highScoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();

        string name = highScoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;

        entryTransform.Find("highlight").gameObject.SetActive(rank % 2 == 1);

        // Highlight First
        if (rank == 1)
        {
            entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        switch (rank)
        {
            default:
                entryTransform.Find("trophy").gameObject.SetActive(false);
                break;
            case 1:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color(1,0,0,1);
                break;
            case 2:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color(0, 1, 0, 1);
                break;
            case 3:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color(0, 0, 1, 1);
                break;
        }

        transformList.Add(entryTransform);
    }

    public static void AddHighScoreEntry(int score, string name)
    {
        // Create highscore entry
        HighScoreEntry highScoreEntry = new HighScoreEntry{score = score, name = name};

        // Load saved Highscore
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                highScoreEntryList = new List<HighScoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.highScoreEntryList.Add(highScoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        
    }

    public static string ReturnHighestScore()
    {
        // Create highscore entry
        HighScoreEntry highScoreEntry = new HighScoreEntry();

        // Load saved Highscore
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                highScoreEntryList = new List<HighScoreEntry>()
            };
        }
        List<HighScoreEntry> SortedList = highscores.highScoreEntryList.OrderBy(o => o.score).ToList();
        string bestScoreReturn = ("Best score: " + SortedList[SortedList.Count - 1].name + " " + SortedList[SortedList.Count - 1].score);
        return bestScoreReturn;


    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
        // when back to menu best score text null
    }
    private class Highscores
    {
        public List<HighScoreEntry> highScoreEntryList;
    }
    /*
     * Represents a single High score entry
     * */
    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }
}
