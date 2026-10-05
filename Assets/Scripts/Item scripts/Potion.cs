using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Potion : MonoBehaviour, IPointerClickHandler
{
    public int HealthAmout = 50;

    private UI_Controler _uiControler;
    void Start()
    {
        _uiControler = GameObject.Find("GameManager").GetComponent<UI_Controler>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Player_Controler.CurrentHealth += HealthAmout * gameObject.GetComponent<Stack>().StackAmout;
            Debug.Log("Consumed " + transform.name);
            Debug.Log("Healed " + (HealthAmout * gameObject.GetComponent<Stack>().StackAmout));

            _uiControler.OnChangedHealth.Invoke();
            GameObject.Destroy(this.gameObject);
        }
    }
}
