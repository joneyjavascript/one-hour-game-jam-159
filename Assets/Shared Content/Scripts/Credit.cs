using UnityEngine;
using UnityEngine.Audio;

public enum CreditFunction { GameDesigner, Programmer, Art, Music, SoundFX, Story, Thanks }

[CreateAssetMenu]
public class Credit : ScriptableObject
{
    public CreditFunction[] authorFunctions;
    public string authorName;

    public bool isValid() {
        bool invalid = authorName.Trim() == "" || authorFunctions == null || authorFunctions.Length <= 0;
        return invalid == false;
    }
}
