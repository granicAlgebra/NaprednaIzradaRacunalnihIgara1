using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Apple_item : MonoBehaviour, IPointerClickHandler
{
    public int HealthAmout = 10;

    private UI_Controler _uiControler;
    void Start()
    {
        _uiControler = GameObject.Find("GameManager").GetComponent<UI_Controler>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Player_Controler.CurrentHealth += HealthAmout;
            Debug.Log("Consumed " + transform.name);
            _uiControler.OnChangedHealth.Invoke();
            GameObject.Destroy(this.gameObject);
        } 
    }
}
