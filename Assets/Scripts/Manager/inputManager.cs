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
        _inputData.isPressed = Input.GetMouseButtonDown(0); // Fare tıklandığında "True" - Tıklanmadığında "False"
        _inputData.isHeld = Input.GetMouseButton(0);        // Fare basılı tutulduğunda "True" - Basılı değilse "False"
        _inputData.isReleased = Input.GetMouseButtonUp(0);  // Fare serbest bırakıldığında "True" - Değilse "False"
    }
}
