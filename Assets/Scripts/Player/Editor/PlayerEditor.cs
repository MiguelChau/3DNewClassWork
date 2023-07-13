/*using System.Linq;
using UnityEditor;

[CustomEditor(typeof(PlayerStateMachine))]
public class PlayerEditor : Editor
{
    private bool showFoldOut;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerStateMachine playerStateMachine = (PlayerStateMachine)(object)target;

        if (playerStateMachine == null)
            return;

        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("State Machine");

        if (playerStateMachine.dictionaryState == null)
            return;

        if (playerStateMachine.CurrentState != null)
            EditorGUILayout.LabelField("Current State", playerStateMachine.CurrentState.ToString());

        showFoldOut = EditorGUILayout.Foldout(showFoldOut, "Available States");

        if (showFoldOut)
        {
            var keys = playerStateMachine.dictionaryState.Keys.ToArray();
            var vals = playerStateMachine.dictionaryState.Values.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                EditorGUILayout.LabelField(string.Format("{0} :: {1}", keys[i], vals[i]));
            }
        }
    }
}
*/
