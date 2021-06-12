using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationService
{
    private AnimationController _animationController;
    public delegate void OnAnimationEnd();
    public static event OnAnimationEnd OnAnimationEndEvent;

    public AnimationService(AnimationController controller)
    {
        _animationController = controller;
    }
    
    public void Swap(IMatchThreeItem first, IMatchThreeItem second)
    {
        var animFirst = first.GetGameObject().transform.DOLocalMove(Vector3.zero, 0.5f).Pause();
        var animSecond = second.GetGameObject().transform.DOLocalMove(Vector3.zero, 0.5f).Pause();

        animFirst.SetEase(Ease.OutQuad);
        animSecond.SetEase(Ease.OutQuad);

        animFirst.OnComplete(() => OnAnimationEndEvent?.Invoke());
        animFirst.Play();
        animSecond.Play();
    }

    public void Fall(IMatchThreeItem item)
    {
        var animFirst = item.GetGameObject().transform.DOLocalMove(Vector3.zero, 0.75f).Pause();

        animFirst.SetEase(Ease.Linear);

        //animFirst.OnComplete(() => OnAnimationEndEvent?.Invoke());
        animFirst.Play();
    }
}
