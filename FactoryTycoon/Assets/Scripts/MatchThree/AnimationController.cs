using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimationService;

public class AnimationController : MonoBehaviour
{
    private AnimationService _animationService;
    

    public void InitService()
    {
        _animationService = new AnimationService(this);
    }

    public void Swap(IMatchThreeItem first, IMatchThreeItem second) => _animationService.Swap(first, second);

    public void Fall(IMatchThreeItem item) => _animationService.Fall(item);
}
