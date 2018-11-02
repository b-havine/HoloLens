using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
public class asd : MonoBehaviour, ISpeechHandler
{
    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        if (eventData.RecognizedText.Equals("yes")) {
            Debug.Log("hey");
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
