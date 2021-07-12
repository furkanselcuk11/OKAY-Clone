using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public event Action OnMouseClick;   // Fare veya ekrana dokunma oldu�u konrol eder

    public inputData _inputData;
    public LayerMask layerToColideWith;

    public float moveSpeed=1f;

    private Vector3 mClickedPos;   // Farenin ilk t�kland��� posizyon
    private Vector3 mReleasePos;   // Fare serbest b�rak�ld��� posizyon
    private Vector3 mdir;   // Mouse y�n� - Karakterin hareket edece�i y�n

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
        HandleMovement();   // Dokunma harekletlinin �al��mas�
    }
    private void HandleMovement()
    {
        if (_inputData.isPressed)   // Fare t�kland���nda
        {
            mHitBlock = CheckIfHitBlock();
            if (mHitBlock)
                return;

            mClickedPos = mCamera.ScreenToWorldPoint(Input.mousePosition);  // Farenin veya dokunman�n ilk t�kland��� zaman kameradaki posizyonu
            mClickedPos = new Vector3(mClickedPos.x, mClickedPos.y, 0f);    // Farenin veya dokunman�n ilk t�kland��� zamanki posizyonun koordinatlar�

            ResetPlayerPosition();  // Karakterin pozisyonu Farenin veya dokunman�n t�kland��� pozisyona gider - Her dokunmada dokunulan pozisyona gider
            mPlayerVFX.ChangeTrailState(false, 0f); // Trail efektinin aktifli�i ve s�resi - Karakterin arkas�ndan b�rak�lan izin
            mPlayerVFX.SetDotStartPos(mClickedPos); // Olu�turulacak noktalar�n ilk posizyonu t�klanan ilk pozisyon de�erini al�r
            mPlayerVFX.ChangeDotActiveState(true);  // Olu�turulacak noktalar�n aktiflik durumunu true yapar

            if (OnMouseClick != null)
            {
                OnMouseClick(); // Fare veya ekrana dokunma oldu�u zaman aktif olur
            }
        }
        if (_inputData.isHeld)  // Fare bas�l� tutuldu�unda
        {
            if (mHitBlock)
                return;

            mPlayerVFX.SetDotPos(mClickedPos, mCamera.ScreenToWorldPoint(Input.mousePosition));
            // Farenin veya dokunman�n bas�l� tutuldu�u zaman Playerin pozisyonundan Farenin veya dokunman�n son pozisyonuna noktalar olu�turur
            mPlayerVFX.MakeBallPulse(); // Karakterin "Scale" boyutu de�i�imi
        }
        if (_inputData.isReleased)  // Farenin veya dokunman�n serbest b�rak�ld���nda - bas�l� tutma bitti�inde
        {
            if (mHitBlock)
                return;

            mReleasePos = mCamera.ScreenToWorldPoint(Input.mousePosition);  // Farenin veya dokunman�n b�rak�ld��� zaman kameradaki posizyonu
            mReleasePos = new Vector3(mReleasePos.x, mReleasePos.y, 0f);     // Farenin veya dokunman�n b�rak�ld��� zamanki posizyonun koordinatlar�

            mPlayerVFX.ChangeTrailState(true, 0.75f);   // Trail efektinin aktifli�i ve s�resi - Karakterin arkas�ndan b�rak�lan izin
            mPlayerVFX.ChangeDotActiveState(false); // Olu�turulacak noktalar�n aktiflik durumunu false yapar
            mPlayerVFX.ResetBallSize(); // Karakterin "Scale" boyutu ba�lang�� boyutuna s�f�rlar
            CalculateDirection();
            MovePlayerInDirection();    // Karekterin hareketini sa�lar
        }
    }
    private void CalculateDirection()
    {
        mdir = (mReleasePos - mClickedPos).normalized;  // Farenin veya dokunman�n son t�kland��� yerden ilk t�kland��� yer ��kar�l�p normali al�n�r
        // Karakterin hareket edece�i y�n belirlenir
    }
    private void MovePlayerInDirection()
    {
        rb.velocity = mdir * (moveSpeed*5); // Karakterin belirtilen y�ne do�ro hareket eder
    }
    private void ResetPlayerPosition()
    {
        transform.position = mClickedPos;   // Karakterin pozisyonu Farenin veya dokunman�n t�kland��� pozisyona gider
        rb.velocity = Vector3.zero; // Karakterin h�z� s�f�rlar ve hareket etmesini durdurur
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="block")  // E�er �arp�lan nesne "block" ise
        {
            Vector2 wallNormal = collision.contacts[0].normal;
            mdir = Vector2.Reflect(rb.velocity, wallNormal).normalized;
            // Karakter "block" nesnesine �arpt�g� zamanki a��y� al�r ve o a�� kadar kar�� a�� y�n� hesaplar - duvardan ayn� a�� ile �arp��mas�n� sa�lar 
            rb.velocity = mdir * (moveSpeed * 5);   // �arpt��� zamanki a��s�n� g�re �arp�p o y�nde hareket eder
        }
    }
    private bool CheckIfHitBlock()
    {
        Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitBlock = Physics2D.Raycast(ray.origin, ray.direction, 100f, layerToColideWith);
        return hitBlock;
    }
}
