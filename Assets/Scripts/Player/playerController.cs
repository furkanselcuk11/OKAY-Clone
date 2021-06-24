using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public event Action OnMouseClick;

    public inputData _inputData;
    public LayerMask layerToColideWith;

    public float moveSpeed=1f;

    private Vector3 mClickedPos;   // Farenin ilk t�kland��� posizyon
    private Vector3 mReleasePos;   // Fare serbest b�rak�ld��� posizyon
    private Vector3 mdir;

    private Rigidbody2D rb;
    private Camera mCamera;

    private playerVFX mPlayerVFX;
    private bool mHitBlock;
    private void Start()
    {
        GetComponents();
    }
    private void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        mPlayerVFX = GetComponent<playerVFX>();
        mCamera = Camera.main;
    }
    private void Update()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        if (_inputData.isPressed)   // Fare t�kland���nda
        {
            mHitBlock = CheckIfHitBlock();
            if (mHitBlock)
                return;

            mClickedPos = mCamera.ScreenToWorldPoint(Input.mousePosition);  // Farenin ilk t�kland��� an cameradaki posizyonu
            mClickedPos = new Vector3(mClickedPos.x, mClickedPos.y, 0f);    // Farenin ilk t�kland��� anki posizyonun koordinatlar�

            ResetPlayerPosition();
            mPlayerVFX.ChangeTrailState(false, 0f); // Trail efektinin iz b�rkamas� ve s�resi
            mPlayerVFX.SetDotStartPos(mClickedPos); // Topun arkas�nda ��kan noktalar�n posizyonu
            mPlayerVFX.ChangeDotActiveState(true);  // Topun arkas�nda ��kan noktalar�n aktiflik durumu

            if (OnMouseClick != null)
            {
                OnMouseClick();
            }
        }
        if (_inputData.isHeld)  // Fare bas�l� tutuldu�unda
        {
            if (mHitBlock)
                return;

            mPlayerVFX.SetDotPos(mClickedPos, mCamera.ScreenToWorldPoint(Input.mousePosition));
            // Fare bas�l� tutul�u an Playerin pos.'dan farein o anki posizyanuna noktalar olu�turur
            mPlayerVFX.MakeBallPulse();
        }
        if (_inputData.isReleased)  // Fare serbest b�rak�ld���nda
        {
            if (mHitBlock)
                return;

            mReleasePos = mCamera.ScreenToWorldPoint(Input.mousePosition);  // Farenin son t�kland��� an cameradaki posizyonu
            mReleasePos = new Vector3(mReleasePos.x, mReleasePos.y, 0f);     // Farenin son t�kland��� anndaki posizyonun koordinatlar�

            mPlayerVFX.ChangeTrailState(true, 0.75f);   // Trail efektinin iz b�rkamas� ve s�resi
            mPlayerVFX.ChangeDotActiveState(false); // Topun arkas�nda ��kan noktalar�n aktiflik durumu
            mPlayerVFX.ResetBallSize();
            CalculateDirection();
            MovePlayerInDirection();
        }
    }
    private void CalculateDirection()
    {
        mdir = (mReleasePos - mClickedPos).normalized;  // Farenin son t�kland��� yerden ilk t�kland��� yer ��kar�l�p normali al�n�r
    }
    private void MovePlayerInDirection()
    {
        rb.velocity = mdir * (moveSpeed*5); // Farenin son posizyonuna hareket eder
    }
    private void ResetPlayerPosition()
    {
        transform.position = mClickedPos;
        rb.velocity = Vector3.zero;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="block")
        {
            Vector2 wallNormal = collision.contacts[0].normal;
            mdir = Vector2.Reflect(rb.velocity, wallNormal).normalized;
            rb.velocity = mdir * (moveSpeed * 5);
        }
    }
    private bool CheckIfHitBlock()
    {
        Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitBlock = Physics2D.Raycast(ray.origin, ray.direction, 100f, layerToColideWith);
        return hitBlock;
    }
}
