using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    public inputData _inputData;   
    void Update()
    {
        WriteInputData();
    }
    private void WriteInputData()
    {
        _inputData.isPressed = Input.GetMouseButtonDown(0); // Fare t�kland���nda "True" - T�klanmad���nda "False"
        _inputData.isHeld = Input.GetMouseButton(0);        // Fare bas�l� tutuldu�unda "True" - Bas�l� de�ilse "False"
        _inputData.isReleased = Input.GetMouseButtonUp(0);  // Fare serbest b�rak�ld���nda "True" - De�ilse "False"
    }
}
