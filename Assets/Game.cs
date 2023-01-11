using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _gameOverScoreText;
    [SerializeField] private TMP_Text _timeLeftText;
    [SerializeField] private TMP_Text _startGapText;
    [SerializeField] private GameObject _spawnZone;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _endGamePanel;

    [SerializeField] private float _reloadTime=0.5f;
    [SerializeField] private float _playTime = 30f;
    [SerializeField] private float _hitDistance = 100f;
    [SerializeField] private float _startGap = 3f;

    [SerializeField] private int _spawnGap = 2;

    [SerializeField] private AudioSource _fireSound;
  
    private Vector3 _centerScreen;
    private float _reloadTimeLeft=0;
    private float _timeLeft ;

  
    private Vector3 _startPoint;
    private float _spawnTimer;
    private float _distanceX;
    private float _distanceZ;


    private int _score;

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            if (value < 0)
            {
                _score = 0;
            }
            else
            {
                _score= value;
            }
            _scoreText.text = $"Очки: {_score}";
        }
    }

    void Start()
    {
       Reset();
        _centerScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        _startPoint = _spawnZone.transform.position;
        _distanceX = _spawnZone.transform.localScale.x/2;
        _distanceZ = _spawnZone.transform.localScale.z/2;

    }

    void Update()
    {
        if (Time.timeScale == 0) { return; }
        _startGapText.text = _startGap.ToString("#");
        if (_startGap>0)
        {
            _startGap -= Time.deltaTime;
            return;
        }
        

        if (_timeLeft>0)
        {
            _timeLeft -= Time.deltaTime;
            _timeLeftText.text = $"Время: {_timeLeft.ToString("#.##")}";
        }
        else
        {
            GameOver();
        }

        if (_spawnTimer - _timeLeft>_spawnGap)
        {
            _spawnTimer -= _spawnGap;
            SpawnEnemy();
        }

        _reloadTimeLeft-=Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && _reloadTimeLeft<=0f)
        {
            _fireSound.Play();
            if (Physics.Raycast(_centerScreen, transform.TransformDirection(Vector3.forward), out var hit, _hitDistance))
            {
                if (hit.transform.TryGetComponent<Enemy>(out var hitEnemy))
                {
                    hitEnemy.Die();
                    Score++;
                }
            }
            _reloadTimeLeft = _reloadTime;
        }

    }

    private void GameOver()
    {
        _endGamePanel.SetActive(true);
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            enemy.Die();
        }
        _gameOverScoreText.text = $"Набрано очков {Score}";
        Time.timeScale= 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true ;
    }

    public void Reset()
    {
        _startGap = 3f;
        Score = 0;
        _endGamePanel.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _timeLeft = _playTime;
        _spawnTimer = _timeLeft;
        
        Time.timeScale = 1f;
    }

    private void SpawnEnemy()
    {
        
        Vector3 position = new Vector3(UnityEngine.Random.Range(_startPoint.x-_distanceX, _startPoint.x + _distanceX), _startPoint.y, UnityEngine.Random.Range(_startPoint.z - _distanceZ, _startPoint.z + _distanceZ));
        Instantiate(_enemyPrefab, position, Quaternion.identity);
    }
}
