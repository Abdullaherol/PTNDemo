using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Entity))]
public class EntityEditor : Editor //For scriptable object. It provides more understandable entities for Editor
{
    public override void OnInspectorGUI()
    {
        var entity = (Entity)target;

        if (entity.image != null)
        {
            var texture = entity.image.texture;

            var width = EditorGUIUtility.currentViewWidth / 2;

            GUI.DrawTexture(new Rect(width / 2, 0, width, width), texture, ScaleMode.ScaleToFit);
            EditorGUILayout.Space(width);
        }

        base.OnInspectorGUI();
    }
}