using UnityEngine;

[CreateAssetMenu(fileName ="input_data")]
public class inputData : ScriptableObject
{
    public bool isPressed;  // Fare t�kland�m�
    public bool isHeld; // Fareye t�klan�yor mu
    public bool isReleased; // Fare t�klanms� b�rak�ld� m�
}
