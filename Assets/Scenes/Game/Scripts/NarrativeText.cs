using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class NarrativeTextPart {
    public string text;    
}

public enum NarrativeTextPartState { none, start, loop, end, waintingNext };
public enum NarrativeTextPartSpeed { slow, normal, fast};

[RequireComponent(typeof(Text))]
public class NarrativeText : MonoBehaviour {

    public NarrativeTextPartSpeed narrativeSpeed;
    public NarrativeTextPartSpeed narrativeActionSpeed;
    private NarrativeTextPartSpeed currentNarrativeSpeed = NarrativeTextPartSpeed.normal;
    public float waitTimeForNextPart = 0;

    public List<NarrativeTextPart> narrative;
    public NarrativeTextPart currentPartOfNarrative;
    Text text;

    public UnityEvent onEndNarrative;

    private void Start()
    {
        text = GetComponent<Text>();
        SetCurrentNarrativeTextPart(narrative[0]);
    }

    // Update is called once per frame
    void Update() {
        UpdateText();

        // control narrative speed
        currentNarrativeSpeed = NarrativeTextPartSpeed.normal;
        if (Input.GetButton("Fire1")) { 
            currentNarrativeSpeed = NarrativeTextPartSpeed.fast;
        }

        // control next part of narrative 
        if (Input.GetButtonDown("Fire1"))
        {
            if (partState == NarrativeTextPartState.waintingNext)
            {
                Next();
            }
        }      
    }

    public string currentWriteText;
    public int currentWriteTextCharIndex;
    public float lastWriteTimePassed;
    public NarrativeTextPartState partState = NarrativeTextPartState.none;
    public float timePassedAfterLastNarrativeTextPart = 0;

    private void StartNewNarrativeTextPart() {        
        currentWriteText = currentPartOfNarrative.text;
        text.text = "";
        currentWriteTextCharIndex = 0;
        lastWriteTimePassed = 0;
        partState = NarrativeTextPartState.loop;
    }

    private void UpdateNewNarrativeTextPart()
    {
        lastWriteTimePassed += Time.deltaTime;
        
        if (currentWriteTextCharIndex >= currentPartOfNarrative.text.Length) {
            EndNewNarrativeTextPart();
            return;
        }

        float digitTime = 0f;
        switch (currentNarrativeSpeed) {
            case NarrativeTextPartSpeed.slow:
                digitTime = 1f;
                break;
            case NarrativeTextPartSpeed.normal:
                digitTime = .1f;
                break;
            case NarrativeTextPartSpeed.fast:
                digitTime = 0;
                break;
        }

        if (lastWriteTimePassed > digitTime)
        {
            text.text += currentPartOfNarrative.text.Substring(currentWriteTextCharIndex, 1);
            currentWriteTextCharIndex++;
            lastWriteTimePassed = 0;
        }        
    }

    private void EndNewNarrativeTextPart()
    {
        partState = NarrativeTextPartState.waintingNext;        
    }

    private void UpdateText()
    {
        if (partState == NarrativeTextPartState.none) {
            timePassedAfterLastNarrativeTextPart += Time.deltaTime;
        }

        if (currentPartOfNarrative != null && partState == NarrativeTextPartState.none && timePassedAfterLastNarrativeTextPart > waitTimeForNextPart)
        {           
            partState = NarrativeTextPartState.start;
        }

        if (partState == NarrativeTextPartState.start)
        {
            StartNewNarrativeTextPart();
        }
        
        if (partState == NarrativeTextPartState.loop)
        {
            UpdateNewNarrativeTextPart();
        }

        if (partState == NarrativeTextPartState.end)
        {
            EndNewNarrativeTextPart();
        }
    }

    public void ChangeNarrativeTextPartSpeedMultiple(float multiple) {

    }

    public void Next() {       
        NarrativeTextPart narrativeTextPart = GetRelativeNarrativePart(1);

        if (narrativeTextPart == null)
        {
            onEndNarrative.Invoke();
            return;
        }

        SetCurrentNarrativeTextPart(narrativeTextPart);
        partState = NarrativeTextPartState.none;
        timePassedAfterLastNarrativeTextPart = 0f;
    }

    public void Prev(){
        NarrativeTextPart narrativeTextPart = GetRelativeNarrativePart(-1);
        SetCurrentNarrativeTextPart(narrativeTextPart);
    }

    public void SetCurrentNarrativeTextPart(NarrativeTextPart narrativeTextPart) {
        currentPartOfNarrative = narrativeTextPart;
    }
     
    public NarrativeTextPart GetRelativeNarrativePart(int offset)
    {
        if (narrative.Count <= 0) {
            return null;
        }

        if (currentPartOfNarrative == null)
        {
            return narrative[0];
        } else {           
            int nextIndex = GetIndexOfCurrentNarrativeTextPart() + offset;

            if (nextIndex < 0 || nextIndex >= narrative.Count)
            {
                return null;
            }
          
            return narrative[nextIndex];
        }
    }

    public int GetIndexOfCurrentNarrativeTextPart() {
        int currentIndex = narrative.IndexOf(currentPartOfNarrative);
        return currentIndex;
    }

}
