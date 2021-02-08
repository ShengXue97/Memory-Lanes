using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class JsonLevelDatabase
{
    [SerializeField]
    private string[] levels;

    public List<string> GetLevels()
    {
        return levels.ToList();
    }
}
