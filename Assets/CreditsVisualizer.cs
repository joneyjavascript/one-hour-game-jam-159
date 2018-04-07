using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsVisualizer : MonoBehaviour {
    public CreditCollection credits;
    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        string textString = "";
        Dictionary<CreditFunction, List<Credit>> creditList = credits.GetCreditsGroupByFunction();
        foreach (KeyValuePair<CreditFunction, List<Credit>> function in creditList)
        {          
            string functionName = function.Key.ToString();            
            string[] authorsNames = credits.GetAuthorsNameByFunction(function.Key);
            string authors = string.Join(", ", authorsNames);
            string creditDescription = "<b> " + functionName + ": </b>" + authors + "\n";
            textString += creditDescription;
        }
        text.text = textString;
    }
}
