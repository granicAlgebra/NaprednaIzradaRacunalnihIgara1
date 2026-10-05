using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class Bag_Slots : MonoBehaviour, IEquipable, IPointerClickHandler
{
    public int BagSlots;

    private bool _isEquiped = false;
    private bool _isUnequiped = true;

    //Flagovi koji se koristile kao prekidaci da bi se skripte samo jednom izrsile
    private bool _doOnceEquiped = true;
    private bool _doOnceUnEquiped = false;

    private UI_Controler _uiControler;
    private Player_Inventory _playerInventory;
    void Start()
    {
        _uiControler = GameObject.Find("GameManager").GetComponent<UI_Controler>();
        _playerInventory = GameObject.Find("GameManager").GetComponent<Player_Inventory>();
        if (transform.parent.tag == "BagSlot")
        {
            _doOnceEquiped = false;
            _doOnceUnEquiped = true;
            _isEquiped = true;
            _isUnequiped = false;
        }
    }
    void Update()
    {
        EquipItem();
        UnEquipItem();
        if (transform.parent.tag == "BagSlot")
        {
            _isEquiped = true;
            _isUnequiped = false;
        }
        else
        {
            _isUnequiped = true;
            _isEquiped = false;
        }
    }   

    public void EquipItem()
    {
        if (_isEquiped && _doOnceEquiped)
        {
            Player_Controler.InventoryCapacity += BagSlots;
            _doOnceEquiped = false;
            _doOnceUnEquiped = true;
            _playerInventory.OnChangedBag.Invoke();
        }
    }

    public void UnEquipItem()
    {
        if (_isUnequiped && _doOnceUnEquiped)
        {
            Player_Controler.InventoryCapacity -= BagSlots;
            _doOnceEquiped = true;
            _doOnceUnEquiped = false;
            _playerInventory.OnChangedBag.Invoke();
        }
    }

    public void EquipedOnPickup()
    {
        _playerInventory = GameObject.Find("GameManager").GetComponent<Player_Inventory>();
        Player_Controler.InventoryCapacity += BagSlots;
        _playerInventory.OnChangedBag.Invoke();
        Debug.Log("Equiped " + transform.name);
    }
    public void OnPointerClick(PointerEventData eventData)
    {   
        //Ako dropa iz equipment slota
        if (Keyboard.current.xKey.isPressed)
        {
            Player_Controler.InventoryCapacity -= BagSlots;
            _playerInventory.OnChangedBag.Invoke();
        }
        //Equipa /  unequipa item
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (_isEquiped)
            {
                //Trazi slobodan slot da unequipa, ako nema izbaci poruku
                bool hasEmptySlot = false;
                foreach (GameObject slot in Player_Inventory.Inventory)
                {
                    if (slot.transform.childCount == 0)
                    {
                        this.transform.SetParent(slot.transform);
                        this.transform.localPosition = Vector2.zero;
                        hasEmptySlot = true;
                        break;
                    }
                }
                if (!hasEmptySlot)
                {
                    Debug.Log("No free slot in inventory!");
                }
            }
            else
            {
                if (_playerInventory.BagSlot.transform.childCount == 0)
                {
                    this.transform.SetParent(_playerInventory.BagSlot.transform);
                    this.transform.localPosition = Vector2.zero;
                }
                //Zamjeni iteme u equip slotu ako vec postoji jedan
                else
                {
                    Transform inventorySlot = this.transform.parent;
                    _playerInventory.BagSlot.transform.GetChild(0).SetParent(inventorySlot);
                    this.transform.SetParent(_playerInventory.BagSlot.transform);
                    this.transform.localPosition = Vector2.zero;
                    inventorySlot.GetChild(0).localPosition = Vector2.zero;
                }
            }

        }
    }
}
