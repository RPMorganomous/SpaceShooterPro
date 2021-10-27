using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = (false);
    public bool _isNewWave = (false);
    //[SerializeField]
    //public UIManager _uiManager;
    public int _wave;

    private void Start()
    {
        //_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            _isGameOver = (false);
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void GameOver()
    {
        _isGameOver = (true);
    }

    public void NextWave()
    {
        _isNewWave = (true);
        //_uiManager.Wave_text = (true);

    }
}
