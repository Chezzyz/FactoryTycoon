using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AnimationService;

public class AnimationController : MonoBehaviour
{
    private AnimationService _animationService;
    

    public void InitService()
    {
        _animationService = new AnimationService(this);
    }

    public void Swap(IMatchThreeItem first, IMatchThreeItem second, bool sendEvent) => _animationService.Swap(first, second, sendEvent);

    public void Fall(IMatchThreeItem item, bool isLast) => _animationService.Fall(item, isLast);

    public void SelfDestroy(IMatchThreeItem item) => _animationService.SelfDestroy(item);

    public void Reshuffle(Image image) => _animationService.Reshuffle(image);
}
