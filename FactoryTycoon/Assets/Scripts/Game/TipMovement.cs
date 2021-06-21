using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TipMovement : MonoBehaviour
{
    [SerializeField] GameObject tipImageGO;
    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject endPoint;
    [SerializeField] private float forwardSpeed = 3;
    [SerializeField] private float returnSpeed = 1;
    [SerializeField] private float holdingTime = 0.5f;
    [SerializeField] bool isLoop = true;
    private Tweener _move;
    public void StartTipAnimation()
    {
        tipImageGO.SetActive(true);
        tipImageGO.transform.position = startPoint.transform.position;

        StartCoroutine(MoveToPoint());
    }

    private IEnumerator MoveToPoint()
    {
        _move = tipImageGO.transform.DOMove(endPoint.transform.position, forwardSpeed).SetEase(Ease.OutQuad);

        yield return _move.WaitForCompletion();
        yield return new WaitForSeconds(holdingTime);

        _move = tipImageGO.transform.DOMove(startPoint.transform.position, returnSpeed).SetEase(Ease.Linear);

        yield return _move.WaitForCompletion();


        if (isLoop)
        {
            StartCoroutine(MoveToPoint());
        }
    }

    public void StopAnimation()
    {
        _move.Pause();
        tipImageGO.SetActive(false);
    }
}
