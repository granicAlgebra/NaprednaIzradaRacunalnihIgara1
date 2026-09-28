using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour
{
    //Pola od ukupne širine prikaza u unitima.  
    private float _cameraFieldSizeX = 0;
    //Pola od ukupne Visine prikaza u unitima.
    private float _cameraFieldSizeY = 0;
    //Definira granice mape.
    private float _mapBorderTop, _mapBorderDown, _mapBorderLeft, _mapBorderRight;  

    private float _posX = 0;
    private float _posY = 0;

    public GameObject playerCharacter;

    // Start is called before the first frame update.
    void Start()
    {
        _cameraFieldSizeX = Camera.main.orthographicSize * Screen.width / Screen.height;
        _cameraFieldSizeY = Camera.main.orthographicSize;
        _mapBorderDown = -15;
        _mapBorderTop = 15;
        _mapBorderLeft = -26;
        _mapBorderRight = 26;
    }

    // Update is called once per frame.
    void Update()
    {
        _cameraMoving();
    }

    //Pomice kameru u smjeru kretanja playera. Neide dalje od ruba mape.
    private void _cameraMoving()
    {         
        if(playerCharacter.transform.position.y < _mapBorderTop - _cameraFieldSizeY && playerCharacter.transform.position.y > _mapBorderDown + _cameraFieldSizeY)
        {
            _posY = playerCharacter.transform.position.y;
        }
        if (playerCharacter.transform.position.x < _mapBorderRight - _cameraFieldSizeX && playerCharacter.transform.position.x > _mapBorderLeft + _cameraFieldSizeX)
        {
            _posX = playerCharacter.transform.position.x;
        }
        transform.position = new Vector3(_posX, _posY, transform.position.z);
    }
}
