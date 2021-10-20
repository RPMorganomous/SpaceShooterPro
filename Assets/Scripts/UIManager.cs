using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //create handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;

    private bool stopFlashing = false;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        //_ammoText.text = "Ammo: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo;
    }


    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    public void GameOverSequence()
    {

        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlashingRoutine());
    }

    IEnumerator GameOverFlashingRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
