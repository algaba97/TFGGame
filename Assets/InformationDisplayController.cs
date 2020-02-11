using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationDisplayController : MonoBehaviour
{
    private TextMesh _textMesh;

    // Start is called before the first frame update
    private void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
    }

    public void DisplayText(string newText, float time = 0.0f){
        StopAllCoroutines();
        _textMesh.text = newText;
        _textMesh.color = Color.white;
        if(time > 0.1f){
            StartCoroutine(FadeOutCouritine(time));
        }
    }

    private IEnumerator FadeOutCouritine(float time){
        Color fadeAmount = Color.black;
        fadeAmount.a = 0.02f/time;
        while(time>0.0f){
            _textMesh.color -= fadeAmount;
           time -= Time.deltaTime;
           yield return new WaitForSeconds(0.01f);
        }
        fadeAmount.a = 1.0f;
        _textMesh.color -= fadeAmount;
    }

    public void ClearText(){
        DisplayText("");
    }
}
