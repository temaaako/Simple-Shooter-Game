using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Game game;
    [SerializeField] private float lifeTime = 2;
    void Start()
    {
        game= FindObjectOfType<Game>();
    }

    void Update()
    {
        lifeTime-=Time.deltaTime;
        if (lifeTime<0)
        {
            Die();
            game.Score--;
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
