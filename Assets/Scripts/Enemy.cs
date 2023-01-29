using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private Animator _enemyAnim;

    private AudioSource _explosionAudioSource;
    [SerializeField]
    private AudioClip _explosionAudioClip;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemyAnim = GetComponent<Animator>();
        _explosionAudioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.Log("The Player is NULL.");
        }

        if (_enemyAnim == null)
        {
            Debug.Log("The Enemy Animator is NULL.");
        }

        if (_explosionAudioSource == null)
        {
            Debug.Log("Audio Source on Enemy is NULL.");
        } else
        {
            _explosionAudioSource.clip = _explosionAudioClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMove();
        FireLaser();
    }

    void CalculateMove()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f && this.GetComponent<Collider2D>())
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 8f, 0);
        }
    }

    void FireLaser()
    {
        if(Time.time > _canFire && _enemyAnim.GetBool("OnEnemyDeath") == false)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            Laser[] _laserObject = Instantiate(_laserPrefab, transform.position, Quaternion.identity).GetComponentsInChildren<Laser>();
            for(int i = 0; i < _laserObject.Length; i++)
            {
                _laserObject[i].AssignEnemyLaser();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if (collision.tag == "Player")
        {
            //Destroy(other.gameObject);
            //other.transform.GetComponent<Player>().Damage();
            //Player player = collision.transform.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();
            }
            _enemyAnim.SetBool("OnEnemyDeath", true);
            Destroy(this.GetComponent<Collider2D>());
            _explosionAudioSource.Play();
            Destroy(this.gameObject, 2.8f);

        }

        if (collision.tag == "Laser" && collision.gameObject.GetComponent<Laser>().isEnemyLaser() == false)
        {
            Destroy(collision.gameObject);
            //Player player = GameObject.Find("Player").GetComponent<Player>();

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _enemyAnim.SetBool("OnEnemyDeath", true);
            Destroy(this.GetComponent<Collider2D>());
            _explosionAudioSource.Play();
            Destroy(this.gameObject, 2.8f);

        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.name);
    //    if(other.tag == "Player")
    //    {
    //        //Destroy(other.gameObject);
    //        //other.transform.GetComponent<Player>().Damage();
    //        Player player = other.transform.GetComponent<Player>();

    //        if(player != null)
    //        {
    //            player.Damage();
    //        }

    //        Destroy(this.gameObject);
    //    }

    //    if(other.tag == "Laser")
    //    {
    //        Destroy(other.gameObject);
    //        Destroy(this.gameObject);
    //    }
    //}
}
