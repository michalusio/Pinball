using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Additional ball"))
            {
                ((GameManager) serializedObject.targetObject).BallIn();
            }
        }
    }
#endif
}
