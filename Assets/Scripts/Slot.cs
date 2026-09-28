using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Player_Inventory _PlayerInventory;
    
    private void Start()
    {
        _PlayerInventory = GameObject.Find("GameManager").GetComponent<Player_Inventory>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount == 0 && transform.tag == "InventorySlot")
        {
            Player_Inventory.EmptySlot = this.gameObject;
            Player_Inventory.SlotPointer = "";
        }
        else if (transform.tag == "WeaponSlot" && Player_Inventory.DragedItemType == "Weapon")
        {
            Player_Inventory.EmptySlot = this.gameObject;
            Player_Inventory.SlotPointer = transform.tag;
        }
        else if (transform.tag == "GloveSlot" && Player_Inventory.DragedItemType == "Glove")
        {
            Player_Inventory.EmptySlot = this.gameObject;
            Player_Inventory.SlotPointer = transform.tag;
        }
        else if (transform.tag == "BagSlot" && Player_Inventory.DragedItemType == "Bag")
        {
            Player_Inventory.EmptySlot = this.gameObject;
            Player_Inventory.SlotPointer = transform.tag;
        }
        else if (transform.tag == "ShieldSlot" && Player_Inventory.DragedItemType == "Shield")
        {
            Player_Inventory.EmptySlot = this.gameObject;
            Player_Inventory.SlotPointer = transform.tag;
        }
        else if (transform.tag == "ArmourSlot" && Player_Inventory.DragedItemType == "Armour")
        {
            Player_Inventory.EmptySlot = this.gameObject;
            Player_Inventory.SlotPointer = transform.tag;
        }
        else if (transform.tag == "BootsSlot" && Player_Inventory.DragedItemType == "Boots")
        {
            Player_Inventory.EmptySlot = this.gameObject;
            Player_Inventory.SlotPointer = transform.tag;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Player_Inventory.EmptySlot = null;
    }
}
