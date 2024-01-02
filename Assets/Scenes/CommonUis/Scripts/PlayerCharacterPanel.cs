using Assets.PixelHeroes.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterPanel : MonoBehaviour
{
    public CharacterAppearance CharacterAppearance;
    
    // Get and Set CharacterBuilder
    private CharacterBuilder _characterBuilder;
    [SerializeField]
    private Image _characterImage;

    public void SetCharacterPanel(CharacterAppearance characterAppearance)
    {
        CharacterAppearance = characterAppearance;
        SetCharacter();
        SetCharacterImage();
    }

    private void SetCharacter()
    {
        _characterBuilder = GetComponentInChildren<CharacterBuilder>();
        if(_characterBuilder == null)
        {
            Debug.LogError("CharacterBuilder not found");
        }

        _characterBuilder.SetByCharacterAppearance(CharacterAppearance);
    }

    private void SetCharacterImage()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if(spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found");
        }
        if(_characterImage == null)
        {
            Debug.LogError("Image not found");
        }

        _characterImage.sprite = spriteRenderer.sprite;

        _characterImage.preserveAspect = true;

        _characterImage.rectTransform.sizeDelta = spriteRenderer.size;

        _characterImage.enabled = true;
        spriteRenderer.enabled = false;
    }

}
