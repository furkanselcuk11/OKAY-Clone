using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockManager : MonoBehaviour
{
    public block[] blockArray;
    [SerializeField] private int mBlockCount;
    private void Start()
    {
        blockArray = FindObjectsOfType<block>();    // Sahnediki objeler i�inde block Scripti olan objeleri bul
        mBlockCount = blockArray.Length;    // Sahnediki objeler i�inde block Scripti olan objelerin say�s�
        SubscribeToEvent();
    }
    private void SubscribeToEvent()
    {
        foreach (block _block in blockArray)
        {
            _block.OnBegingHit += DecreaseBlockCount;
        }
        FindObjectOfType<playerController>().OnMouseClick += ResetAllBlocks;    // E�er mouse t�klan�rsa ResetAllBlocks fonk. �al��s�n
    }

    private void DecreaseBlockCount()
    {
        mBlockCount--;
    }
    private void ResetAllBlocks()   // Sahnedeki block objelerinin aktifliini true yapar
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
