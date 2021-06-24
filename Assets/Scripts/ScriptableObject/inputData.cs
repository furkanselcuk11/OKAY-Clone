using UnityEngine;

[CreateAssetMenu(fileName ="input_data")]
public class inputData : ScriptableObject
{
    public bool isPressed;  // Fare týklandýmý
    public bool isHeld; // Fareye týklanýyor mu
    public bool isReleased; // Fare týklanmsý býrakýldý mý
}
