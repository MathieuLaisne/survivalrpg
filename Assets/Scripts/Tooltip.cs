using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
    private Text tooltipText;

	void Start () 
    {
        tooltipText = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
	}

    public void GenerateTooltip(Item item)
    {
        string statText = "";
        string tooltip = "";
        if (item.stats.Count > 0)
        {
            foreach(var stat in item.stats)
            {
                statText += stat.Key.ToString() + ": " + stat.Value + "\n";
            }
        }
        if(item.type == "Food")
        {
            tooltip = string.Format("<b>{0}</b>\n{1} <b>Right Click to use.</b>\n\n<b>{2}</b>", item.name, item.desc, statText);
        } else
        {
            tooltip = string.Format("<b>{0}</b>\n{1}\n\n<b>{2}</b>", item.name, item.desc, statText);
        }
        tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }
}
