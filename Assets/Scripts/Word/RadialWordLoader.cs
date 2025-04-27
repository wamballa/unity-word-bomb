using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class RadialWordLoader : MonoBehaviour
{
    public Dictionary<string, List<string>> radialWordMap;

    void Awake()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("RadialLetterWordMap");
        if (!jsonFile) Debug.LogError("[RWL] no json file");
        radialWordMap = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonFile.text);

        // radialWordMap = JsonUtilityWrapper.FromJson<Dictionary<string, List<string>>>(jsonFile.text);
    }

    // Example usage
    public List<string> GetWordsForLetterSet(string letterSet, int desiredLength)
    {
        if (radialWordMap.TryGetValue(letterSet, out var words))
            return words.FindAll(w => w.Length == desiredLength);
        return new List<string>();
    }

}
