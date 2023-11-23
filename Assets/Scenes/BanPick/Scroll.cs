using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static Enums;

public class Scroll : MonoBehaviour
{
    [SerializeField]
    private Sprite BlueSprite;
    [SerializeField]
    private Sprite RedSprite;

    private Image _scrollImage;
    [SerializeField] 
    private TextMeshProUGUI _scrollText;


    void Awake()
    {
        _scrollImage = gameObject.GetComponent<Image>();
    }

    public void SetText(string message)
    {
        _scrollText.text = message;
    }

    public void TurnRedScroll()
    {
        _scrollImage.sprite = RedSprite;
    }
    public void TurnBlueScroll()
    {
        _scrollImage.sprite = BlueSprite;
    }
}
