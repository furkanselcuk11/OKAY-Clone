using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerVFX : MonoBehaviour
{
    public GameObject dotPrefab;    // Oluþturulacak noktanýn prefabý
    public int dotAmount;   // Oluþturulacak nokta sayýsý

    [Space]
    [Header("Line Variables")]
    public AnimationCurve followCurve;  // Oluþturlan noktalarýn animasyonu
    public float followSpeed;   // Oluþturlan noktalarýn animasyon hýzý

    [Space]
    [Header("Pulse Variables")]
    public AnimationCurve expandCurve;  // Karakter geniþletme animasyonu
    public float expandAmount;  // Karakter geniþletme boyutu
    public float expandSpeed;   // Karakter geniþletme hýzý

    private Vector3 mStartSize;     // KarakterBaþlangýç boyutu
    private Vector3 mTargetSize;    // Hedef boyutu
    private float mScrollAmount;    // Kaydrýma sayýsý

    private float mDotGap;  // Oluþturulacak noktalar arasý boþluk
    private GameObject[] mDotArray; // Noktalarýn tutulduðu dizi

    private TrailRenderer mTrailRenderer;   // Karakerin arkasýnda býrakýlan iz 
    void Start()
    {
        mDotGap = 1f / dotAmount;    // Oluþturulacak noktalar arasý boþluk
        GetCompenents();
        InitPulseEffectVariables();
        SpawnDots(); // Noktalarýn oluþturulmasý       
    }
    private void GetCompenents()
    {
        mTrailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    private void InitPulseEffectVariables()
    {
        mStartSize = transform.localScale;  // Karakterin baþlangýçtaki "Scale" boyutu
        mTargetSize = mStartSize * expandAmount;    // Karakterin baþlangýçtaki "Scale" boyutundan kaç kat büyük olacaðý
    }

    private void SpawnDots()    // Karakterin belirtilen yöne doðru noktalarýn oluþturulmasý
    {
        mDotArray = new GameObject[dotAmount];
        for (int i = 0; i < dotAmount; i++)
        {
            GameObject _dot = Instantiate(dotPrefab);   // oluþturulacak nokta nesnesi
            _dot.SetActive(false);  // Baþlangýçta noktalarý False yapar
            mDotArray[i] = _dot;    // Oluþturukan noktayý diziye aktarýr
        }
    }
    public void SetDotPos(Vector3 startPos,Vector3 endPos)
    {   // Farenin veya dokunmanýn basýlý tutulduðu zaman Playerin pozisyonundan Farenin veya dokunmanýn son pozisyonuna noktalar oluþturur
        for (int i = 0; i < dotAmount; i++)
        {
            Vector3 dotPos = mDotArray[i].transform.position;   // Noktalarýn tutulduðu diziden "i" sýradaki noktanýn pozisyonu
            Vector3 targetPos = Vector2.Lerp(startPos, endPos, i * mDotGap);    // Noktanýn gideceði pozisyon
            // ilk pozisyonun "startPos" farenin veya dokunmanýn basýlý tutulduðu zaman Playerin pozisyon
            // Son pozisyon "endPos" Farenin veya dokunmanýn son pozisyonu

            float smoothSpeed = (1f-followCurve.Evaluate(i * mDotGap)) * followSpeed;   
            // Oluþturulan noktanýn hedefe geçiþin hýzý - daha yumuþak ve animasyon þeklinde geçiþi
            mDotArray[i].transform.position = Vector2.Lerp(dotPos, targetPos, smoothSpeed * Time.deltaTime);
            // Oluþturulan noktanýn ilk pozisyonundan hedef pozisyona yumuþak geçiþi
        }
    }
    public void ChangeDotActiveState(bool state)    // Oluþturulacak noktalarýn aktiflik durumu
    {
        for (int i = 0; i < dotAmount; i++)
        {
            mDotArray[i].SetActive(state);
        }
    }
    public void SetDotStartPos(Vector3 pos) // Oluþturulacak noktalarýn ilk posizyonu týklanan ilk pozisyon deðerini alýr
    {
        for (int i = 0; i < dotAmount; i++)
        {
            mDotArray[i].transform.position = pos;
        }
    }
    public void MakeBallPulse() // Karakterin "Scale" boyutu baþlangýç boyutundan hedef boyuta geçmesi
    {
        mScrollAmount += Time.deltaTime * expandAmount; // Animasyon yumuþatma deðeri
        float precent = expandCurve.Evaluate(mScrollAmount);    // Animasyon geçiþ hýzý
        transform.localScale = Vector2.Lerp(mStartSize, mTargetSize, precent);  // Karakterin "Scale" boyutu baþlangýç boyutundan hedef boyuta geçmesi
    }
    public void ResetBallSize() // Karakterin "Scale" boyutu baþlangýç boyutuna sýfýrlar
    {
        transform.localScale = mStartSize;  // Karakterin "Scale" boyutu baþlangýç boyutuna ayarlar
        mScrollAmount = 0f; // mScrollAmount deðrini sýfýrlar
    }
    public void ChangeTrailState(bool state,float time)
    {
        mTrailRenderer.emitting = state;    // Karakterin arkasýndan býrakýlan izin aktiflik durumu
        mTrailRenderer.time = time; // Karakterin arkasýndan býrakýlan izin aktif kalma süresi
    }
}
