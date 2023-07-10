using System.Linq;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerEditor : Editor
{
    private bool showFoldOut;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerController playerController = (PlayerController)target;

        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("State Machine");

        if (playerController.stateMachine == null)
            return;

        if (playerController.stateMachine.CurrentState != null)
            EditorGUILayout.LabelField("Current State", playerController.stateMachine.CurrentState.ToString());

        showFoldOut = EditorGUILayout.Foldout(showFoldOut, "Available States");

        if (showFoldOut)
        {
            if (playerController.stateMachine.dictionaryState != null)
            {
                var keys = playerController.stateMachine.dictionaryState.Keys.ToArray();
                var vals = playerController.stateMachine.dictionaryState.Values.ToArray();

                for (int i = 0; i < keys.Length; i++)
                {
                    EditorGUILayout.LabelField(string.Format("{0} :: {1}", keys[i], vals[i]));
                }
            }
        }
    }
}