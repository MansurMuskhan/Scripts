using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ICustomizationImagesConfig
{
    string GetHeadFileNameById(int id);
    TraitInfo GetBodyTraitsById(int id);
}

public class TraitInfo
{
    public int ID;
    public string Category;
    public string Url;
}
public class CustomizationImagesConfig : ICustomizationImagesConfig
{
    private Dictionary<int, string> _idToFileName;
    public string GetHeadFileNameById(int id)
    {
        if(_idToFileName == null)
        {
            var file = Resources.Load<TextAsset>("IdToFilename").text;
            string[] lines = file.Split(new char[] { '\n' });
            _idToFileName = lines.Select(line => line.Split(',')).ToDictionary(line => int.Parse(line[0]), line => line[1]);

        }
        return _idToFileName[id];
    }

    private Dictionary<int, TraitInfo> _loadedTraits;

    public TraitInfo GetBodyTraitsById(int id)
    {
        if (_loadedTraits == null)
        {
            _loadedTraits = new Dictionary<int, TraitInfo>();
            var file = Resources.Load<TextAsset>("Traits").text;
            string[] lines = file.Split(new char[] { '\n' });
            foreach(var line in lines)
            {
                var splitted = line.Split(',');
                var intId = int.Parse(splitted[0]);
                var traitInfo = new TraitInfo()
                {
                    ID = intId,
                    Category = splitted[1],
                    Url = splitted[6]
                };
                _loadedTraits.Add(intId, traitInfo);
            }
        }
        if (!_loadedTraits.ContainsKey(id))
            return null;
        return _loadedTraits[id];
    }
}
