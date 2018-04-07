using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

[CreateAssetMenu]
public class CreditCollection : ScriptableObject
{
    public Credit[] credits;

    // Get credits agruped by author function
    public Dictionary<CreditFunction, List<Credit>> GetCreditsGroupByFunction()
    {
        Dictionary<CreditFunction, List<Credit>> groupedCredits = new Dictionary<CreditFunction, List<Credit>>();

        foreach (Credit credit in credits)
        {
            if (!credit.isValid()) {
                Debug.LogError("Invalid credit object: credit objects require authorName and one or more authorFunctions ");
            }

            foreach (CreditFunction authorFunction in credit.authorFunctions)
            {
                if (!groupedCredits.ContainsKey(authorFunction))
                {
                    groupedCredits.Add(authorFunction, new List<Credit>());
                }

                List<Credit> temp;
                if (groupedCredits.TryGetValue(authorFunction, out temp))
                {
                    temp.Add(credit);
                }
                else
                {
                    Debug.LogError(authorFunction + " not found");
                }
            }
        }

        return groupedCredits;
    }

    // Get author's names array from a function
    public string[] GetAuthorsNameByFunction(CreditFunction function)
    {
        Dictionary<CreditFunction, List<Credit>> groupedCredits = GetCreditsGroupByFunction();
        List<string> authorsNames = new List<string>();
        foreach (KeyValuePair<CreditFunction, List<Credit>> relation in groupedCredits)
        {
            if (relation.Key == function)
            {
                relation.Value.ForEach((c) => {
                    authorsNames.Add(c.authorName);
                });               
            }
        }
        return authorsNames.ToArray();
    }

}
