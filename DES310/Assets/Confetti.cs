using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confetti : MonoBehaviour
{
    [SerializeField]
    GameObject confettiPrefab = null;
    public int counter = 0;
    private bool hasShot = false;
    private float timer = 2.5f;

    public void Update()
    {
        if (counter == 4)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (!hasShot)
                {
                    ShootConfetti();
                }

                timer = 0;
            }
        }
    }

    public void ShootConfetti()
    {
        GameObject confetti = Instantiate(confettiPrefab, Vector3.zero, Quaternion.identity);
        hasShot = true;
    }
}
