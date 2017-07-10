using System.Collections;
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;


[Serializable]
public class TileSprite
{
    private string _fullName;
    public string FullName
    {
        get
        {
            if (_fullName != null)
                return _fullName;
            if (Path.Contains("/"))
                _fullName = Path.Split('/').Last();
            else
                _fullName = Path;
            return _fullName;
        }
    }

    private string _name;
    public string Name
    {
        get
        {
            if (_name != null)
                return _name;

            var fullName = FullName;

            if (fullName.Contains("-"))
            {
                _name = fullName.Split('-')[0] + "-";
            }
            else
            {
                _name = fullName;
            }

            return _name;
        }
    }
    public string Path;
    private Sprite _sprite;
    [JsonIgnore]
    public Sprite Sprite
    {
        get
        {
            if (_sprite == null)
            {
                _sprite = Resources.Load<Sprite>(Path);
            }

            return _sprite;
        }
        set
        {
            _sprite = value;
        }
    }
    public float Walkspeed = 1f;
    public TileType Type;

    public TileSprite()
    {

    }

    public TileSprite(string spritePath)
    {
        Sprite = Resources.Load<Sprite>(spritePath);
    }
}

