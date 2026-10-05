using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Move : MonoBehaviour
{
    //Definira granice mape (Level1 je 80 x 53, kamera nesmije prikazat previse izvan)
    private float _mapBorderTop, _mapBorderDown, _mapBorderLeft, _mapBorderRight;

    private float _posX = 0;
    private float _posZ = 0;

    //Udaljenost kamere od playera (uzima se iz scene na pocetku)
    private Vector3 _offset;

    public GameObject playerCharacter;
    public float smooth = 6;

    // Start is called before the first frame update.
    void Start()
    {
        if (playerCharacter == null)
        {
            playerCharacter = GameObject.Find("Player");
        }
        //kamera gleda pod kutem pa je offset = 34 unita unazad po smjeru gledanja
        _offset = -transform.forward * 24;
        _mapBorderDown = 6;
        _mapBorderTop = 48;
        _mapBorderLeft = 8;
        _mapBorderRight = 72;

        _posX = playerCharacter.transform.position.x;
        _posZ = playerCharacter.transform.position.z;
        transform.position = new Vector3(_posX, playerCharacter.transform.position.y, _posZ) + _offset;
    }

    // LateUpdate da se kamera mice nakon playera (inace trza)
    void LateUpdate()
    {
        _cameraMoving();
    }

    //Pomice kameru u smjeru kretanja playera. Neide dalje od ruba mape.
    private void _cameraMoving()
    {
        _posX = playerCharacter.transform.position.x;
        _posZ = playerCharacter.transform.position.z;
        if (_posX > _mapBorderRight)
        {
            _posX = _mapBorderRight;
        }
        if (_posX < _mapBorderLeft)
        {
            _posX = _mapBorderLeft;
        }
        if (_posZ > _mapBorderTop)
        {
            _posZ = _mapBorderTop;
        }
        if (_posZ < _mapBorderDown)
        {
            _posZ = _mapBorderDown;
        }
        Vector3 target = new Vector3(_posX, playerCharacter.transform.position.y, _posZ) + _offset;
        transform.position = Vector3.Lerp(transform.position, target, smooth * Time.deltaTime);
    }
}
