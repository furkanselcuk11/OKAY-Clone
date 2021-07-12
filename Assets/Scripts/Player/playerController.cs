using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public event Action OnMouseClick;   // Fare veya ekrana dokunma olduðu konrol eder

    public inputData _inputData;
    public LayerMask layerToColideWith;

    public float moveSpeed=1f;

    private Vector3 mClickedPos;   // Farenin ilk týklandýðý posizyon
    private Vector3 mReleasePos;   // Fare serbest býrakýldýðý posizyon
    private Vector3 mdir;   // Mouse yönü - Karakterin hareket edeceði yön

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
        HandleMovement();   // Dokunma harekletlinin çalýþmasý
    }
    private void HandleMovement()
    {
        if (_inputData.isPressed)   // Fare týklandýðýnda
        {
            mHitBlock = CheckIfHitBlock();
            if (mHitBlock)
                return;

            mClickedPos = mCamera.ScreenToWorldPoint(Input.mousePosition);  // Farenin veya dokunmanýn ilk týklandýðý zaman kameradaki posizyonu
            mClickedPos = new Vector3(mClickedPos.x, mClickedPos.y, 0f);    // Farenin veya dokunmanýn ilk týklandýðý zamanki posizyonun koordinatlarý

            ResetPlayerPosition();  // Karakterin pozisyonu Farenin veya dokunmanýn týklandýðý pozisyona gider - Her dokunmada dokunulan pozisyona gider
            mPlayerVFX.ChangeTrailState(false, 0f); // Trail efektinin aktifliði ve süresi - Karakterin arkasýndan býrakýlan izin
            mPlayerVFX.SetDotStartPos(mClickedPos); // Oluþturulacak noktalarýn ilk posizyonu týklanan ilk pozisyon deðerini alýr
            mPlayerVFX.ChangeDotActiveState(true);  // Oluþturulacak noktalarýn aktiflik durumunu true yapar

            if (OnMouseClick != null)
            {
                OnMouseClick(); // Fare veya ekrana dokunma olduðu zaman aktif olur
            }
        }
        if (_inputData.isHeld)  // Fare basýlý tutulduðunda
        {
            if (mHitBlock)
                return;

            mPlayerVFX.SetDotPos(mClickedPos, mCamera.ScreenToWorldPoint(Input.mousePosition));
            // Farenin veya dokunmanýn basýlý tutulduðu zaman Playerin pozisyonundan Farenin veya dokunmanýn son pozisyonuna noktalar oluþturur
            mPlayerVFX.MakeBallPulse(); // Karakterin "Scale" boyutu deðiþimi
        }
        if (_inputData.isReleased)  // Farenin veya dokunmanýn serbest býrakýldýðýnda - basýlý tutma bittiðinde
        {
            if (mHitBlock)
                return;

            mReleasePos = mCamera.ScreenToWorldPoint(Input.mousePosition);  // Farenin veya dokunmanýn býrakýldýðý zaman kameradaki posizyonu
            mReleasePos = new Vector3(mReleasePos.x, mReleasePos.y, 0f);     // Farenin veya dokunmanýn býrakýldýðý zamanki posizyonun koordinatlarý

            mPlayerVFX.ChangeTrailState(true, 0.75f);   // Trail efektinin aktifliði ve süresi - Karakterin arkasýndan býrakýlan izin
            mPlayerVFX.ChangeDotActiveState(false); // Oluþturulacak noktalarýn aktiflik durumunu false yapar
            mPlayerVFX.ResetBallSize(); // Karakterin "Scale" boyutu baþlangýç boyutuna sýfýrlar
            CalculateDirection();
            MovePlayerInDirection();    // Karekterin hareketini saðlar
        }
    }
    private void CalculateDirection()
    {
        mdir = (mReleasePos - mClickedPos).normalized;  // Farenin veya dokunmanýn son týklandýðý yerden ilk týklandýðý yer çýkarýlýp normali alýnýr
        // Karakterin hareket edeceði yön belirlenir
    }
    private void MovePlayerInDirection()
    {
        rb.velocity = mdir * (moveSpeed*5); // Karakterin belirtilen yöne doðro hareket eder
    }
    private void ResetPlayerPosition()
    {
        transform.position = mClickedPos;   // Karakterin pozisyonu Farenin veya dokunmanýn týklandýðý pozisyona gider
        rb.velocity = Vector3.zero; // Karakterin hýzý sýfýrlar ve hareket etmesini durdurur
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="block")  // Eðer çarpýlan nesne "block" ise
        {
            Vector2 wallNormal = collision.contacts[0].normal;
            mdir = Vector2.Reflect(rb.velocity, wallNormal).normalized;
            // Karakter "block" nesnesine çarptýgý zamanki açýyý alýr ve o açý kadar karþý açý yönü hesaplar - duvardan ayný açý ile çarpýþmasýný saðlar 
            rb.velocity = mdir * (moveSpeed * 5);   // Çarptýðý zamanki açýsýný göre çarpýp o yönde hareket eder
        }
    }
    private bool CheckIfHitBlock()
    {
        Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hitBlock = Physics2D.Raycast(ray.origin, ray.direction, 100f, layerToColideWith);
        return hitBlock;
    }
}
