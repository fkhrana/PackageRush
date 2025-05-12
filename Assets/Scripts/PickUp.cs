using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Score score;

    [SerializeField]
    private int scoreValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        score.AddScore(scoreValue);
        Destroy(gameObject);
    }
}