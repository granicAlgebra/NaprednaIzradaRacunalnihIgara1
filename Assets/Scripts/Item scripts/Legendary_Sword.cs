using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class Legendary_Sword : MonoBehaviour, IWeapon, IEquipable, IPointerClickHandler
{
    public int DamageAmout { get; } = 50;
    public int SpeedAmout { get; } = 0;
    public int HealthAmout { get; } = 20;

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
        if (transform.parent != null && transform.parent.tag == "WeaponSlot")
        {
            _isEquiped = true;
            _isUnequiped = false;
            _doOnceEquiped = false;
            _doOnceUnEquiped = true;
            _uiControler.OnChangedDamage.Invoke();
            _uiControler.OnChangedSpeed.Invoke();
            _uiControler.OnChangedHealth.Invoke();
            _uiControler.OnChangedMaxHP.Invoke();
        }
    }
    void Update()
    {

        EquipItem();
        UnEquipItem();
        if (transform.parent != null && transform.parent.tag == "WeaponSlot")
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
            Player_Controler.Damage += DamageAmout;
            Player_Controler.Speed += SpeedAmout;
            Player_Controler.MaxHealth += HealthAmout;
            Player_Controler.CurrentHealth += HealthAmout;
            _doOnceEquiped = false;
            _doOnceUnEquiped = true;
            _uiControler.OnChangedDamage.Invoke();
            _uiControler.OnChangedSpeed.Invoke();
            _uiControler.OnChangedHealth.Invoke();
            _uiControler.OnChangedMaxHP.Invoke();
        }
    }

    public void UnEquipItem()
    {
        if (_isUnequiped && _doOnceUnEquiped)
        {
            Player_Controler.Damage -= DamageAmout;
            Player_Controler.Speed -= SpeedAmout;
            Player_Controler.MaxHealth -= HealthAmout;
            Player_Controler.CurrentHealth -= HealthAmout;
            _doOnceEquiped = true;
            _doOnceUnEquiped = false;
            _uiControler.OnChangedDamage.Invoke();
            _uiControler.OnChangedSpeed.Invoke();
            _uiControler.OnChangedHealth.Invoke();
            _uiControler.OnChangedMaxHP.Invoke();
        }
    }

    public void EquipedOnPickup()
    {
        Player_Controler.Damage += DamageAmout;
        Player_Controler.Speed += SpeedAmout;
        Player_Controler.MaxHealth += HealthAmout;
        Player_Controler.CurrentHealth += HealthAmout;
        Debug.Log("Equiped " + transform.name);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Ako dropa iz equipment slota
        if (Keyboard.current.xKey.isPressed)
        {
            Player_Controler.Damage -= DamageAmout;
            Player_Controler.Speed -= SpeedAmout;
            _uiControler.OnChangedDamage.Invoke();
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
                if (_playerInventory.WeaponSlot.transform.childCount == 0)
                {
                    this.transform.SetParent(_playerInventory.WeaponSlot.transform);
                    this.transform.localPosition = Vector2.zero;
                }
                //Zamjeni iteme u equip slotu ako vec postoji jedan
                else
                {
                    Transform inventorySlot = this.transform.parent;
                    _playerInventory.WeaponSlot.transform.GetChild(0).SetParent(inventorySlot);
                    this.transform.SetParent(_playerInventory.WeaponSlot.transform);
                    this.transform.localPosition = Vector2.zero;
                    inventorySlot.GetChild(0).localPosition = Vector2.zero;
                }
            }

        }
    }
}
