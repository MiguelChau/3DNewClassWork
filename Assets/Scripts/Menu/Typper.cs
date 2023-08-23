using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class Typper : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float timeBetweenLetters = .1f;
    public string phrase;

    private Coroutine typeCoroutine;

    private void Awake()
    {
        textMesh.text = ""; 
    }

    [Button]
    public void StartType()
    {
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
        }
        typeCoroutine = StartCoroutine(Type(phrase));
    }
     IEnumerator Type(string s)
     {
        textMesh.text = ""; 
        foreach (char l in s.ToCharArray()) 
        {
            textMesh.text += l;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
        typeCoroutine = null;
    }
}
