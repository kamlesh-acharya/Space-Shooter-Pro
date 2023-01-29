using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText, _bestText;
    int _bestScore;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _bestText.text = "Best: " + _bestScore;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.Log("The Game Manager is NULL.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateScoreOnUI(int score)
    {
        _scoreText.text = "Score: " + score;
        if(_bestScore < score)
        {
            _bestScore = score;
            PlayerPrefs.SetInt("BestScore", _bestScore);
            _bestText.text = "Best: " + _bestScore;
        }
    }

    public void UpdateLives(int currentLive)
    {
        _LivesImg.sprite = _liveSprites[currentLive];
        if(currentLive == 0)
        {
            showGameOver();
            _gameManager.GameOver();
        }
    }

    public void showGameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(FlickerGameOverText());
    }

    IEnumerator FlickerGameOverText()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
