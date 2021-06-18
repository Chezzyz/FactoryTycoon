using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimationService
{
    private AnimationController _animationController;
    public delegate void OnSwapAnimationEnd();
    public static event OnSwapAnimationEnd OnAnimationSwapEndEvent;
    public delegate void OnDestoyAnimationEnd(IMatchThreeItem item);
    public static event OnDestoyAnimationEnd OnAnimationDestroyEndEvent;
    public delegate void OnFallAnimationEnd(bool flag);
    public static event OnFallAnimationEnd OnAnimationFallEndEvent;
    public static event OnFallAnimationEnd OnAnimationStateChangeEvent;

    public AnimationService(AnimationController controller)
    {
        _animationController = controller;
    }
    
    public void Swap(IMatchThreeItem first, IMatchThreeItem second, bool sendEvent)
    {
        var animFirst = first.GetGameObject().transform.DOLocalMove(Vector3.zero, 0.5f).Pause();
        var animSecond = second.GetGameObject().transform.DOLocalMove(Vector3.zero, 0.5f).Pause();

        animFirst.SetEase(Ease.OutQuad);
        animSecond.SetEase(Ease.OutQuad);

        if (sendEvent)
        {
            animSecond.OnKill(() => OnAnimationSwapEndEvent?.Invoke());
        }

        animFirst.OnStart(() => OnAnimationStateChangeEvent?.Invoke(true));
        animSecond.OnComplete(() => OnAnimationStateChangeEvent?.Invoke(false));

        animFirst.Play();
        animSecond.Play();
    }

    public void Fall(IMatchThreeItem item, bool isLast)
    {
        var animation = item.GetGameObject().transform.DOLocalMove(Vector3.zero, 0.75f).Pause();

        animation.SetEase(Ease.Linear);

        if(isLast) animation.OnComplete(() =>
        {
            OnAnimationFallEndEvent?.Invoke(false);
            OnAnimationStateChangeEvent?.Invoke(false);
        });

        animation.OnStart(() => OnAnimationStateChangeEvent?.Invoke(true));
        animation.Play();
    }

    public void SelfDestroy(IMatchThreeItem item)
    {
        var animation = item.GetGameObject().transform.DOScale(Vector3.zero, 0.4f).Pause();

        animation.SetEase(Ease.OutQuad);

        animation.OnKill(() => OnDestroyCallBack(item));

        animation.Play();
    }

    public void Reshuffle(Image image)
    {
        var animation = image.rectTransform.DOScaleY(6, 1.5f)
            .SetEase(Ease.OutQuad)
            .SetLoops(2, LoopType.Yoyo)
            .Play();
    }

    private void OnDestroyCallBack(IMatchThreeItem item)
    {
        OnAnimationDestroyEndEvent?.Invoke(item);
       // OnAnimationFallEndEvent?.Invoke(true); ЕБАНАЯ СУКА ХУЕТА БЛЯТЬ НА КОТОРУЮ Я БЛЯТЬ 3 ЧАСА БЛЯТЬ ПОТРАТИЛ НЕ РАССКОМЕНЧВАТЬ
    }
}
