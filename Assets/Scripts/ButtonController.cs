using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ButtonController : MonoBehaviour
{
    public void SceneChanger(int number)
    {
        SceneManager.LoadScene(number);
    }
    public void StartNew()
    {

        GameManager.Instance.StartNew();

    }
    public void ExitGame()
    {
        GameManager.Instance.Exit();
    }

}
