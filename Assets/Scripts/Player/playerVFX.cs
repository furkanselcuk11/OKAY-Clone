using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerVFX : MonoBehaviour
{
    public GameObject dotPrefab;
    public int dotAmount;

    [Space]
    [Header("Line Variables")]
    public AnimationCurve followCurve;
    public float followSpeed;

    [Space]
    [Header("Pulse Variables")]
    public AnimationCurve expandCurve;
    public float expandAmount;
    public float expandSpeed;

    private Vector3 mStartSize;
    private Vector3 mTargetSize;
    private float mScrollAmount;

    private float mDotGap;
    private GameObject[] mDotArray;
    private TrailRenderer mTrailRenderer;
    void Start()
    {
        mDotGap = 1f / dotAmount;    // B�t�ne g�re bir noktan�n y�zdesi
        GetCompenents();
        InitPulseEffectVariables();
        SpawnDots();        
    }
    private void GetCompenents()
    {
        mTrailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    private void InitPulseEffectVariables()
    {
        mStartSize = transform.localScale;  // Playerin ba�lang��taki "Scale" boyutu
        mTargetSize = mStartSize * expandAmount;    // Playerin ba�lang��taki "Scale" boyutundan ka� kat b�y�k olaca��
    }

    private void SpawnDots()    // Topun arkas�nda ��kan noktalar�n say�s� ve olu�turulmas�
    {
        mDotArray = new GameObject[dotAmount];
        for (int i = 0; i < dotAmount; i++)
        {
            GameObject _dot = Instantiate(dotPrefab);
            _dot.SetActive(false);  // Ba�lang��ta noktalar� False yapar
            mDotArray[i] = _dot;
        }
    }
    public void SetDotPos(Vector3 startPos,Vector3 endPos)  // Fare bas�l� tutul�u an Playerin pos.'dan farein o anki posizyanuna noktalar olu�turur
    {
        for (int i = 0; i < dotAmount; i++)
        {
            Vector3 dotPos = mDotArray[i].transform.position;
            Vector3 targetPos = Vector2.Lerp(startPos, endPos, i * mDotGap);

            float smoothSpeed = (1f-followCurve.Evaluate(i * mDotGap)) * followSpeed;
            //mDotArray[i].transform.position = _targetPos;
            mDotArray[i].transform.position = Vector2.Lerp(dotPos, targetPos, smoothSpeed * Time.deltaTime);
        }
    }
    public void ChangeDotActiveState(bool state)    // Topun arkas�nda ��kan noktalar�n aktiflik durumu
    {
        for (int i = 0; i < dotAmount; i++)
        {
            mDotArray[i].SetActive(state);
        }
    }
    public void SetDotStartPos(Vector3 pos) // Topun arkas�nda ��kan noktalar�n posizyonu
    {
        for (int i = 0; i < dotAmount; i++)
        {
            mDotArray[i].transform.position = pos;
        }
    }
    public void MakeBallPulse() // Playerin "Scale" boyutu ba�lang�� boyundan hedef boyuta ge�mesi
    {
        mScrollAmount += Time.deltaTime * expandAmount;
        float precent = expandCurve.Evaluate(mScrollAmount);
        transform.localScale = Vector2.Lerp(mStartSize, mTargetSize, precent);  // Playerin "Scale" boyutu ba�lang�� boyundan hedef boyuta ge�mesi
    }
    public void ResetBallSize()
    {
        transform.localScale = mStartSize;
        mScrollAmount = 0f;
    }
    public void ChangeTrailState(bool state,float time)
    {
        mTrailRenderer.emitting = state;    // Topun arkas�ndan iz b�rak�r
        mTrailRenderer.time = time; // Topun arkas�nda b�rak�lam izin bitme s�resi
    }
}
