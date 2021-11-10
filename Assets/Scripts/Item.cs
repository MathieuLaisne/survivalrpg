using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string name;
    public string type;
    public string desc;
    public Sprite icon;
    public int decay=0;
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    public Item(int ID, string Name, string Type, string Desc, Dictionary<string, int> Stats)
    {
        id = ID; name = Name; type = Type; desc = Desc; 
        icon = Resources.Load<Sprite>("Sprites/Items/" + name);
        stats = Stats;
    }

    public Item(Item item)
    {
        id = item.id; name = item.name; type = item.type; desc = item.desc;
        icon = Resources.Load<Sprite>("Sprites/Items/" + item.name);
        stats = item.stats;
    }

}
