using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerVFX : MonoBehaviour
{
    public GameObject dotPrefab;    // Olu�turulacak noktan�n prefab�
    public int dotAmount;   // Olu�turulacak nokta say�s�

    [Space]
    [Header("Line Variables")]
    public AnimationCurve followCurve;  // Olu�turlan noktalar�n animasyonu
    public float followSpeed;   // Olu�turlan noktalar�n animasyon h�z�

    [Space]
    [Header("Pulse Variables")]
    public AnimationCurve expandCurve;  // Karakter geni�letme animasyonu
    public float expandAmount;  // Karakter geni�letme boyutu
    public float expandSpeed;   // Karakter geni�letme h�z�

    private Vector3 mStartSize;     // KarakterBa�lang�� boyutu
    private Vector3 mTargetSize;    // Hedef boyutu
    private float mScrollAmount;    // Kaydr�ma say�s�

    private float mDotGap;  // Olu�turulacak noktalar aras� bo�luk
    private GameObject[] mDotArray; // Noktalar�n tutuldu�u dizi

    private TrailRenderer mTrailRenderer;   // Karakerin arkas�nda b�rak�lan iz 
    void Start()
    {
        mDotGap = 1f / dotAmount;    // Olu�turulacak noktalar aras� bo�luk
        GetCompenents();
        InitPulseEffectVariables();
        SpawnDots(); // Noktalar�n olu�turulmas�       
    }
    private void GetCompenents()
    {
        mTrailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    private void InitPulseEffectVariables()
    {
        mStartSize = transform.localScale;  // Karakterin ba�lang��taki "Scale" boyutu
        mTargetSize = mStartSize * expandAmount;    // Karakterin ba�lang��taki "Scale" boyutundan ka� kat b�y�k olaca��
    }

    private void SpawnDots()    // Karakterin belirtilen y�ne do�ru noktalar�n olu�turulmas�
    {
        mDotArray = new GameObject[dotAmount];
        for (int i = 0; i < dotAmount; i++)
        {
            GameObject _dot = Instantiate(dotPrefab);   // olu�turulacak nokta nesnesi
            _dot.SetActive(false);  // Ba�lang��ta noktalar� False yapar
            mDotArray[i] = _dot;    // Olu�turukan noktay� diziye aktar�r
        }
    }
    public void SetDotPos(Vector3 startPos,Vector3 endPos)
    {   // Farenin veya dokunman�n bas�l� tutuldu�u zaman Playerin pozisyonundan Farenin veya dokunman�n son pozisyonuna noktalar olu�turur
        for (int i = 0; i < dotAmount; i++)
        {
            Vector3 dotPos = mDotArray[i].transform.position;   // Noktalar�n tutuldu�u diziden "i" s�radaki noktan�n pozisyonu
            Vector3 targetPos = Vector2.Lerp(startPos, endPos, i * mDotGap);    // Noktan�n gidece�i pozisyon
            // ilk pozisyonun "startPos" farenin veya dokunman�n bas�l� tutuldu�u zaman Playerin pozisyon
            // Son pozisyon "endPos" Farenin veya dokunman�n son pozisyonu

            float smoothSpeed = (1f-followCurve.Evaluate(i * mDotGap)) * followSpeed;   
            // Olu�turulan noktan�n hedefe ge�i�in h�z� - daha yumu�ak ve animasyon �eklinde ge�i�i
            mDotArray[i].transform.position = Vector2.Lerp(dotPos, targetPos, smoothSpeed * Time.deltaTime);
            // Olu�turulan noktan�n ilk pozisyonundan hedef pozisyona yumu�ak ge�i�i
        }
    }
    public void ChangeDotActiveState(bool state)    // Olu�turulacak noktalar�n aktiflik durumu
    {
        for (int i = 0; i < dotAmount; i++)
        {
            mDotArray[i].SetActive(state);
        }
    }
    public void SetDotStartPos(Vector3 pos) // Olu�turulacak noktalar�n ilk posizyonu t�klanan ilk pozisyon de�erini al�r
    {
        for (int i = 0; i < dotAmount; i++)
        {
            mDotArray[i].transform.position = pos;
        }
    }
    public void MakeBallPulse() // Karakterin "Scale" boyutu ba�lang�� boyutundan hedef boyuta ge�mesi
    {
        mScrollAmount += Time.deltaTime * expandAmount; // Animasyon yumu�atma de�eri
        float precent = expandCurve.Evaluate(mScrollAmount);    // Animasyon ge�i� h�z�
        transform.localScale = Vector2.Lerp(mStartSize, mTargetSize, precent);  // Karakterin "Scale" boyutu ba�lang�� boyutundan hedef boyuta ge�mesi
    }
    public void ResetBallSize() // Karakterin "Scale" boyutu ba�lang�� boyutuna s�f�rlar
    {
        transform.localScale = mStartSize;  // Karakterin "Scale" boyutu ba�lang�� boyutuna ayarlar
        mScrollAmount = 0f; // mScrollAmount de�rini s�f�rlar
    }
    public void ChangeTrailState(bool state,float time)
    {
        mTrailRenderer.emitting = state;    // Karakterin arkas�ndan b�rak�lan izin aktiflik durumu
        mTrailRenderer.time = time; // Karakterin arkas�ndan b�rak�lan izin aktif kalma s�resi
    }
}
