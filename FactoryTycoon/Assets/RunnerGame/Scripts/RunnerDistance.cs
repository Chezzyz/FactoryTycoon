using UnityEngine;
using TMPro;

public class RunnerDistance : MonoBehaviour, ICheckable
{
    [Tooltip("TMPro text on UI showing current distance")]
    [SerializeField]
    private TextMeshProUGUI _distanceText = null;

    [Tooltip("TMPro text on UI showing current distance")]
    [SerializeField]
    private float _distanceToFinish = 0.0f;

    private float _startDistanceToFinish = 0.0f;

    private bool isWin = false;

    public delegate void Win();

    public static event Win GameWin;

    private void Start()
    {
        PlayerHealth.GameLose += OnGameLose;

        _startDistanceToFinish = _distanceToFinish;
    }

    private void Update()
    {
        if (RunnerGameState.IsGameRunning)
        {
            _distanceToFinish -= Time.deltaTime;

            if (_distanceToFinish <= 0.0f)
            {
                RunnerGameState.IsGameRunning = false;
                isWin = true;
                GameWin?.Invoke();
            }

            _distanceText.text = ((int)_distanceToFinish).ToString() + "m";
        }
    }

    private void OnGameLose()
    {
        _distanceToFinish = _startDistanceToFinish;
    }

    public bool CheckAnswer()
    {
        return isWin;
    }

    private void OnDestroy()
    {
        PlayerHealth.GameLose -= OnGameLose;
    }
}
