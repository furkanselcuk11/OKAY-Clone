using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockManager : MonoBehaviour
{
    public block[] blockArray;  // Blocklarýn tutulduðu dizi
    [SerializeField] private int mBlockCount; // Block sayýsý
    private void Start()
    {
        blockArray = FindObjectsOfType<block>();    // Sahnediki nesnelerin içinde block Scripti olan nesneleri bul
        mBlockCount = blockArray.Length;    // Sahnediki nesnelerin içinde block Scripti olan nesnelerin sayýsý
        SubscribeToEvent();
    }
    private void SubscribeToEvent()
    {
        foreach (block _block in blockArray)
        {
            _block.OnBegingHit += DecreaseBlockCount;
        }
        FindObjectOfType<playerController>().OnMouseClick += ResetAllBlocks;
        // Fare veya ekrana dokunma olduðu zaman "OnMouseClick" aktif olur ve ResetAllBlocks fonk. çalýþýr
    }

    private void DecreaseBlockCount()   
    {
        mBlockCount--; // Block sayýsýný azaltýr
    }
    private void ResetAllBlocks()   // Sahnedeki block nesnelerini aktifliini true yapar
    {
        foreach (block _block in blockArray)
        {
            if (_block.gameObject.activeSelf == false)
            {
                _block.gameObject.SetActive(true);
            }
        }
        mBlockCount = blockArray.Length;
    }
}
