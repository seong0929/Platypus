using Assets.PixelHeroes.Scripts.CharacterScripts;
using UnityEditor;
using UnityEngine;

namespace Assets.PixelHeroes.Scripts.Editor
{
    /// <summary>
    /// Adds "Rebuild" button to CharacterBuilder script.
    /// </summary>
    [CustomEditor(typeof(CharacterBuilder))]
    public class CharacterBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Rebuild"))
            {
                ((CharacterBuilder) target).Rebuild();
            }
        }
    }
}