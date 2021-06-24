using System;
using UnityEngine;

public class block : MonoBehaviour
{
    public event Action OnBegingHit;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnBegingHit != null)
        {
            OnBegingHit();
        }
        gameObject.SetActive(false);
    }

}
