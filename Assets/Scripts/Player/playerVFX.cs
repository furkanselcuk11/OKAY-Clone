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
        mDotGap = 1f / dotAmount;    // Bütüne göre bir noktanýn yüzdesi
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
        mStartSize = transform.localScale;  // Playerin baþlangýçtaki "Scale" boyutu
        mTargetSize = mStartSize * expandAmount;    // Playerin baþlangýçtaki "Scale" boyutundan kaç kat büyük olacaðý
    }

    private void SpawnDots()    // Topun arkasýnda çýkan noktalarýn sayýsý ve oluþturulmasý
    {
        mDotArray = new GameObject[dotAmount];
        for (int i = 0; i < dotAmount; i++)
        {
            GameObject _dot = Instantiate(dotPrefab);
            _dot.SetActive(false);  // Baþlangýçta noktalarý False yapar
            mDotArray[i] = _dot;
        }
    }
    public void SetDotPos(Vector3 startPos,Vector3 endPos)  // Fare basýlý tutulðu an Playerin pos.'dan farein o anki posizyanuna noktalar oluþturur
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
    public void ChangeDotActiveState(bool state)    // Topun arkasýnda çýkan noktalarýn aktiflik durumu
    {
        for (int i = 0; i < dotAmount; i++)
        {
            mDotArray[i].SetActive(state);
        }
    }
    public void SetDotStartPos(Vector3 pos) // Topun arkasýnda çýkan noktalarýn posizyonu
    {
        for (int i = 0; i < dotAmount; i++)
        {
            mDotArray[i].transform.position = pos;
        }
    }
    public void MakeBallPulse() // Playerin "Scale" boyutu baþlangýç boyundan hedef boyuta geçmesi
    {
        mScrollAmount += Time.deltaTime * expandAmount;
        float precent = expandCurve.Evaluate(mScrollAmount);
        transform.localScale = Vector2.Lerp(mStartSize, mTargetSize, precent);  // Playerin "Scale" boyutu baþlangýç boyundan hedef boyuta geçmesi
    }
    public void ResetBallSize()
    {
        transform.localScale = mStartSize;
        mScrollAmount = 0f;
    }
    public void ChangeTrailState(bool state,float time)
    {
        mTrailRenderer.emitting = state;    // Topun arkasýndan iz býrakýr
        mTrailRenderer.time = time; // Topun arkasýnda býrakýlam izin bitme süresi
    }
}
