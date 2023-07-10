using System.Linq; //biblioteca do sistema
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(FSMExample))] //vamos "pintar o exemplo" -> se quisermos este editor em outros lugar é so mudar o nome que precisa e vamos ter este mesmo bloco de editor.
public class StateMachineEditor : Editor
{
    public bool showFoldOut; //basicamente é como fosse uma lista, ele abre e fecha uma lista
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FSMExample fsm = (FSMExample)target;

        //1º vamos desenhar a parte debaixo deste editor
        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("State Machine");

        if (fsm.stateMachine == null) return;

        if (fsm.stateMachine.CurrentState != null)
           EditorGUILayout.LabelField("Current State", fsm.stateMachine.CurrentState.ToString());     // se isto nao tiver nulo, vai salvar e mostrar a statemachine

        showFoldOut = EditorGUILayout.Foldout(showFoldOut, "Avaible States");

        if(showFoldOut)
        {
            if(fsm.stateMachine.dictionaryState != null)
            {

            
                var keys = fsm.stateMachine.dictionaryState.Keys.ToArray(); //estamos a pegar todas as keys e montando uma array
                var vals = fsm.stateMachine.dictionaryState.Values.ToArray(); //aqui estamos a pegar todos os valores
            
                for(int i = 0; i < keys.Length; i++)
           
                {
                    EditorGUILayout.LabelField(string.Format("(0) :: {1}", keys[i], vals[i]));  // format server para passar parametros e passando os valores 
            
                }
            }
        }
    }
}
