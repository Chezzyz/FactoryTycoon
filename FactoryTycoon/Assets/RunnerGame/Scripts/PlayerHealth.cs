using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private string _obstacleTag = null;

    public delegate void Lose();

    public static event Lose GameLose;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_obstacleTag))
        {
            GameLose?.Invoke();
        }
    }
}
