using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class Armour : MonoBehaviour, IEquipable, IPointerClickHandler
{
    public int SpeedAmout { get; } = -1;
    public int ArmourAmout { get; } = 50;

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
        if (transform.parent.tag == "ArmourSlot")
        {
            _doOnceEquiped = false;
            _doOnceUnEquiped = true;
            _isEquiped = true;
            _isUnequiped = false;
            _uiControler.OnChangedArmour.Invoke();
            _uiControler.OnChangedSpeed.Invoke();
        }
    }
    void Update()
    {

        EquipItem();
        UnEquipItem();
        if (transform.parent.tag == "ArmourSlot")
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
            Player_Controler.Armour += ArmourAmout;
            Player_Controler.Speed += SpeedAmout;
            _doOnceEquiped = false;
            _doOnceUnEquiped = true;
            _uiControler.OnChangedArmour.Invoke();
            _uiControler.OnChangedSpeed.Invoke();
        }
    }

    public void UnEquipItem()
    {
        if (_isUnequiped && _doOnceUnEquiped)
        {
            Player_Controler.Armour -= ArmourAmout;
            Player_Controler.Speed -= SpeedAmout;
            _doOnceEquiped = true;
            _doOnceUnEquiped = false;
            _uiControler.OnChangedArmour.Invoke();
            _uiControler.OnChangedSpeed.Invoke();
        }
    }

    public void EquipedOnPickup()
    {
        Player_Controler.Armour += ArmourAmout;
        Player_Controler.Speed += SpeedAmout;
        Debug.Log("Equiped " + transform.name);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Ako dropa iz equipment slota
        if (Keyboard.current.xKey.isPressed)
        {
            Player_Controler.Armour -= ArmourAmout;
            Player_Controler.Speed -= SpeedAmout;
            _uiControler.OnChangedArmour.Invoke();
            _uiControler.OnChangedSpeed.Invoke();
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
                if (_playerInventory.ArmourSlot.transform.childCount == 0)
                {
                    this.transform.SetParent(_playerInventory.ArmourSlot.transform);
                    this.transform.localPosition = Vector2.zero;
                }
                //Zamjeni iteme u equip slotu ako vec postoji jedan
                else
                {
                    Transform inventorySlot = this.transform.parent;
                    _playerInventory.ArmourSlot.transform.GetChild(0).SetParent(inventorySlot);
                    this.transform.SetParent(_playerInventory.ArmourSlot.transform);
                    this.transform.localPosition = Vector2.zero;
                    inventorySlot.GetChild(0).localPosition = Vector2.zero;
                }
            }

        }
    }
}
