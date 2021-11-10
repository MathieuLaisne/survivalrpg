using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
    public ItemDB items;
    public UIInventory inventoryUI;

    void Start()
    {
        GiveItem(1);        //
        GiveItem(0);        //
        GiveItem(1);        //
        GiveItem(2);        //Utiliser pour tester
        GiveItem(0);        //
        GiveItem(5);        //
        GiveItem(4);        //
        GiveItem(8);        //
        inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        }

        int stade = 0;
        int index = 0;
        int newId = 0;
        foreach (Item item in characterItems)
        { 
            if (item.type == "Food")
            { 
                foreach (KeyValuePair<string, int> test in item.stats)
                {
                    if (test.Key == "Spoiling Stade")
                    {
                        stade = test.Value;
                        item.decay++;
                        index++;
                    }
                }
                if (item.decay == 600 * (stade + 1) && stade < 5)
                {
                    newId = item.id + 1;
                    item.decay = 0;
                    RemoveItem(index);
                    GiveItem(newId);
                }
            }
        }
    }

    public void GiveItem(int id) //ajoute un item dans l'inventaire en fonction de son id dans la database ItemDB
    {
        Item itemToAdd = items.GetItem(id);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.name);
    }

    public void GiveItem(string itemName) //ajoute un item dans l'inventaire en fonnction de son nom dans la database ItemDB
    {
        Item itemToAdd = items.GetItem(itemName);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.name);
    }

    public Item CheckForItem(int id) //trouve un item dans l'inventaire en fonction de son id dans la database ItemDB
    {
        return characterItems.Find(item => item.id == id);
    }

    public void RemoveItem(int id) //retire un item de l'inventaire en fonction de son id dans la database ItemDB
    {
        Item itemToRemove = CheckForItem(id);
        if (itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log("Removed item: " + itemToRemove.name);
        }
    }
}
