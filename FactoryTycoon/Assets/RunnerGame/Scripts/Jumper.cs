using System.Collections;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [Tooltip("Height of player jump")]
    [SerializeField]
    private AnimationCurve _jumpCurve;

    [Tooltip("Height of player jump")]
    [SerializeField]
    private float _jumpHeight = 0.0f;

    [Tooltip("Height of player jump")]
    [SerializeField]
    private float _jumpDuration = 0.0f;

    private bool canJump = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            StartCoroutine(Jump(_jumpCurve, _jumpHeight, _jumpDuration));
        }
    }

    private IEnumerator Jump(AnimationCurve curve, float jumpHeight, float jumpDuration)
    {
        canJump = false;

        float totalTime = curve.keys[curve.keys.Length - 1].time;
        float currentTime = 0;

        Vector3 startPosition = transform.position;

        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime / jumpDuration;

            transform.position = startPosition + new Vector3(0, curve.Evaluate(currentTime) * jumpHeight, 0);

            yield return null;
        }

        canJump = true;
    }
}
