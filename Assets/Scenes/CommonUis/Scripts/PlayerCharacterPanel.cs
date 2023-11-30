using Assets.PixelHeroes.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterPanel : MonoBehaviour
{
    public CharacterAppearance CharacterAppearance;
    
    // Get and Set CharacterBuilder
    private CharacterBuilder _characterBuilder;

    public void SetCharacterPanel(CharacterAppearance characterAppearance)
    {
        CharacterAppearance = characterAppearance;
        SetCharacter();
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

}
