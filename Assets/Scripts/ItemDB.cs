using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        CreateDB();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string name)
    {
        return items.Find(item => item.name == name);
    }

    void CreateDB()
    {
        items = new List<Item>()
                {
                    new Item(0, "Fresh Meat", "Food", "A piece of meat taken out of a fresh corpse.",
                             new Dictionary<string, int>
                             {
                                 {"Satiety", 20},
                                 {"Spoiling Stade", 0},
                                 {"Value", 3}
                             }),
                    new Item(1, "Spoiling Meat", "Food", "A piece of meat taken out of a spoiling corpse.",
                             new Dictionary<string, int>
                             {
                                 {"Satiety", 15},
                                 {"Spoiling Stade", 1},
                                 {"Value", 1}
                             }),
                    new Item(2, "Spoilt Meat", "Food", "A piece of meat taken out of a spoilt corpse.",
                             new Dictionary<string, int>
                             {
                                 {"Satiety", 10},
                                 {"Spoiling Stade", 2},
                                 {"Value", 0}
                             }),
                    new Item(3, "Rotting Meat", "Food", "A piece of meat taken out of a rotting carcass.",
                             new Dictionary<string, int>
                             {
                                 {"Satiety", 7},
                                 {"Spoiling Stade", 3},
                                 {"Value", 0}
                             }),
                    new Item(4, "Rotten Meat", "Food", "A piece of meat taken out of a rotten carcass.",
                             new Dictionary<string, int>
                             {
                                 {"Satiety", 6},
                                 {"Spoiling Stade", 4},
                                 {"Value", 0}
                             }),
                    new Item(5, "Putrid Meat", "Food", "A piece of meat taken out of a decayed carcass.",
                             new Dictionary<string, int>
                             {
                                 {"Satiety", 4},
                                 {"Spoiling Stade", 5},
                                 {"Value", 0}
                             }),
                    new Item(6, "Stone", "Misc", "A simple stone you can find anywhere on the ground. Might be useful for basic tools.",
                             new Dictionary<string, int>
                             {
                                 {"Weight", 20},
                                 {"Value", 0}
                             }),
                    new Item(7, "Branch", "Misc", "A simple branch you can find anywhere in a forest. Might be useful for basic tools.",
                             new Dictionary<string, int>
                             {
                                 {"Weight", 10},
                                 {"Value", 0}
                             }),
                    new Item(8, " Sturdy Stone", "Misc", "A sturdy stone obtained from mining. Might be useful for tools.",
                             new Dictionary<string, int>
                             {
                                 {"Weight", 30},
                                 {"Value", 1}
                             }),
                    new Item(9, "Wood", "Misc", "Wood obtained from ctting down trees. Might be useful for tools.",
                             new Dictionary<string, int>
                             {
                                 {"Weight", 30},
                                 {"Value", 3}
                             }),
                    new Item(10, "Iron Ore", "Misc", "Iron ore found in rare veins in the ground. Useless as it is.",
                             new Dictionary<string, int>
                             {
                                 {"Weight", 50},
                                 {"Value", 5}
                             }),
                    new Item(11, "Sapphire", "Misc", "Blue gem shining with a peculiar cerulean blue colour. Useless as it is.",
                             new Dictionary<string, int>
                             {
                                 {"Weight", 5},
                                 {"Value", 50}
                             }),
                    new Item(12, "Ruby", "Misc", "Red gem shining with a peculiar crimson red colour. Useless as it is.",
                             new Dictionary<string, int>
                             {
                                 {"Weight", 5},
                                 {"Value", 50}
                             }),
                    new Item(13, "Emerald", "Misc", "Green gem shining with a peculiar verdoyant green colour. Useless as it is.",
                             new Dictionary<string, int>
                             {
                                 {"Weight", 5},
                                 {"Value", 50}
                             })
                };
    }
}
