using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    //[SerializeField]
    //private SpawnManager _spawnManager1;
    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _tripleShotLaserPrefab;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;

    private AudioSource _laserAudioSource;
    [SerializeField]
    private AudioClip _laserAudioClip;

    private Animator _enemyAnim;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -3.8f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _laserAudioSource = GetComponent<AudioSource>();
        _enemyAnim = GetComponent<Animator>();

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UI Manager is NULL.");
        }

        if (_enemyAnim == null)
        {
            Debug.Log("The Player Animator is NULL.");
        }

        if (_laserAudioSource == null)
        {
            Debug.Log("Audio Source on Player is NULL.");
        }
        else
        {
            _laserAudioSource.clip = _laserAudioClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        AnimatePlayer();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");
        //Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //transform.Translate(Input.GetAxis("Horizontal") * _speed * Time.deltaTime * Vector3.right);
        //transform.Translate(Input.GetAxis("Vertical") * _speed * Time.deltaTime * Vector3.up);

        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * _speed * Time.deltaTime);

        //if (transform.position.y >= 0)
        //{
        //    transform.position = new Vector3(transform.position.x, 0, 0);
        //}
        //else if (transform.position.y <= -3.8f)
        //{
        //    transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}

        //clam movement
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
        }
    }

    void AnimatePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput < 0)
        {
            _enemyAnim.SetBool("isIdle", false);
            _enemyAnim.SetBool("isTurningLeft", true);
        } 
        else if(horizontalInput > 0)
        {
            _enemyAnim.SetBool("isIdle", false);
            _enemyAnim.SetBool("isTurningRight", true);
        } else
        {
            _enemyAnim.SetBool("isTurningLeft", false);
            _enemyAnim.SetBool("isTurningRight", false);
            _enemyAnim.SetBool("isIdle", true);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotLaserPrefab, transform.position, Quaternion.identity);
        } else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);
        }

        _laserAudioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives -= 1;

        if(_lives == 2)
        {
            _rightEngine.SetActive(true);
        }

        if(_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            //_spawnManager1.OnPlayerDeath();
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(CoolDownTripleShot());
    }

    IEnumerator CoolDownTripleShot()
    {
        if (_isTripleShotActive)
        {
            yield return new WaitForSeconds(5.0f);
            _isTripleShotActive = false;
        }
    }

    public void ActivateSpeed()
    {
        _isSpeedActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(CoolDownSpeed());
    }

    IEnumerator CoolDownSpeed()
    {
        if (_isSpeedActive)
        {
            yield return new WaitForSeconds(5.0f);
            _speed /= _speedMultiplier;
            _isSpeedActive = false;
        }
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        //StartCoroutine(CoolDownSpeed());
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.updateScoreOnUI(_score);
    }
}
