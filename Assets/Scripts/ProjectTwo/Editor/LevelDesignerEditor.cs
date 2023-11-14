using UnityEditor;
using UnityEngine;

namespace GameTwo
{
    [CustomEditor(typeof(LevelDesigner))]
    public class LevelDesignerEditor : Editor
    {
#if UNITY_EDITOR
        public override void OnInspectorGUI()
        {
            var levelDesigner = (LevelDesigner)target;
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Create Level"))
            {
                levelDesigner.CreateLevel();
            }
            if (GUILayout.Button("Load Level"))
            {
                levelDesigner.LoadLevel();
            }
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
#endif
    }
}