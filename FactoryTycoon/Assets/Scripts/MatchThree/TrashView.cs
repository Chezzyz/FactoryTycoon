using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TrashService;

public class TrashView
{
    private TrashController _controller;
    private Sprite _sprite;
    public TrashView(TrashType type, TrashController controller)
    {
        _sprite = Resources.Load<Sprite>("item_" + type.ToString());
        _controller = controller;
    }

    public void SetSpriteToImage(Image image)
    {
        image.sprite = _sprite;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
