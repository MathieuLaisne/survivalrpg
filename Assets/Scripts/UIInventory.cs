using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public List<UIItem> UIItems = new List<UIItem>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public int maxSlots = 16;

    private void Awake()
    {
        for(int i = 0; i < maxSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y, slotPanel.transform.position.z);
            instance.transform.localScale = new Vector3(1, 1, 1);
            UIItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    public void UpdateSlot(int slot, Item item)
    {
        UIItems[slot].UpdateItem(item);
    }

    public void AddNewItem(Item item)
    {
        UpdateSlot(UIItems.FindIndex(i => i.item == null), item);
    }

    public void RemoveItem(Item item)
    {
        UpdateSlot(UIItems.FindIndex(i => i.item == item), null);
    }
}
