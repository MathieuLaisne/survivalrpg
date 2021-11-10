using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public UIInventory inventory;
    private Image spriteImage;
    private UIItem selectedItem;
    private Tooltip tooltip;

    void Awake()
    {
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
    }

    public void UpdateItem(Item litem)
    {
        if (litem != null)
        {
            item = litem;
            spriteImage.color = Color.white;
            spriteImage.sprite = litem.icon;
        }
        else
        {
            spriteImage.color = Color.clear;
            item = null;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject thePlayer = GameObject.Find("monster_orc");
        Player player = thePlayer.GetComponent<Player>();
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(this.item != null)
            {
                if(this.item.type == "Food")
                {
                    float fill=0;
                    player.ate = true;
                    foreach(var stats in item.stats)
                    {
                        if(stats.Key == "Satiety")
                        {
                            fill = stats.Value/100f;
                        }
                    }
                    foreach (var stats in item.stats)
                    {
                        if (stats.Key == "Spoiling Stade")
                        {
                            float poison = Random.Range(0.0f, 100f);
                            float type = Random.Range(0.0f, 100f);
                            switch (stats.Value)
                            {
                                case 1:
                                    if(poison<5)
                                    {
                                        if(type<25)
                                        {
                                            fill = -fill;
                                        } else if(type<95)
                                        {
                                            player.healthCurrent -= fill / player.healthPool * 100;
                                        } else
                                        {
                                            player.blackOut = true;
                                            player.healthCurrent = player.healthPool/10;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (poison < 15)
                                    {
                                        if (type < 25)
                                        {
                                            fill = -fill;
                                        }
                                        else if (type < 95)
                                        {
                                            player.healthCurrent -= fill / player.healthPool * 100;
                                        }
                                        else
                                        {
                                            player.blackOut = true;
                                            player.healthCurrent = player.healthPool / 10;
                                        }
                                    }
                                    break;
                                case 3:
                                    if (poison < 50)
                                    {
                                        if (type < 25)
                                        {
                                            fill = -fill;
                                        }
                                        else if (type < 95)
                                        {
                                            player.healthCurrent -= fill / player.healthPool * 100;
                                        }
                                        else
                                        {
                                            player.blackOut = true;
                                            player.healthCurrent = player.healthPool / 10;
                                        }
                                    }
                                    break;
                                case 4:
                                    if (type < 25)
                                    {
                                        fill = -fill;
                                    }
                                    else if (type < 95)
                                    {
                                        player.healthCurrent -= fill / player.healthPool * 100;
                                    }
                                    else
                                    {
                                        player.blackOut = true;
                                        player.healthCurrent = player.healthPool / 10;
                                    }
                                    break;
                                case 5:
                                    player.blackOut = true;
                                    player.healthCurrent = player.healthPool / 10;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    player.hunger.fillAmount += fill;
                    if (player.hunger.fillAmount > 100)
                    {
                        player.hunger.fillAmount = 100;
                    }
                    UpdateItem(null);
                }
            }
        } else
        {
            if (this.item != null)
            {
                if (selectedItem.item != null)
                {
                    Item clone = new Item(selectedItem.item);
                    selectedItem.UpdateItem(this.item);
                    UpdateItem(clone);
                }
                else
                {
                    selectedItem.UpdateItem(this.item);
                    UpdateItem(null);
                }
            } else if (selectedItem.item != null)
            {
                UpdateItem(selectedItem.item);
                selectedItem.UpdateItem(null);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.item != null)
        {
            tooltip.GenerateTooltip(this.item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }
}
