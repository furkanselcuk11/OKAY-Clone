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
        _inputData.isPressed = Input.GetMouseButtonDown(0); // Fare týklandýðýnda "True" - Týklanmadýðýnda "False"
        _inputData.isHeld = Input.GetMouseButton(0);        // Fare basýlý tutulduðunda "True" - Basýlý deðilse "False"
        _inputData.isReleased = Input.GetMouseButtonUp(0);  // Fare serbest býrakýldýðýnda "True" - Deðilse "False"
    }
}
