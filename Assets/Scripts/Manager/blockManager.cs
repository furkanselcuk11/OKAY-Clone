using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockManager : MonoBehaviour
{
    public block[] blockArray;  // Blocklar�n tutuldu�u dizi
    [SerializeField] private int mBlockCount; // Block say�s�
    private void Start()
    {
        blockArray = FindObjectsOfType<block>();    // Sahnediki nesnelerin i�inde block Scripti olan nesneleri bul
        mBlockCount = blockArray.Length;    // Sahnediki nesnelerin i�inde block Scripti olan nesnelerin say�s�
        SubscribeToEvent();
    }
    private void SubscribeToEvent()
    {
        foreach (block _block in blockArray)
        {
            _block.OnBegingHit += DecreaseBlockCount;
        }
        FindObjectOfType<playerController>().OnMouseClick += ResetAllBlocks;
        // Fare veya ekrana dokunma oldu�u zaman "OnMouseClick" aktif olur ve ResetAllBlocks fonk. �al���r
    }

    private void DecreaseBlockCount()   
    {
        mBlockCount--; // Block say�s�n� azalt�r
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
