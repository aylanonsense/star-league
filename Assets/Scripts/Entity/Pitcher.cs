using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Pitcher : Entity
    {
        //[SerializeField] private GameObject ballPrefab;
        //private EntityPool balls;
        //[SerializeField] private float timeBetweenBallSpawns = 1.00f;
        //private float timeToNextBallSpawn;

        //private void Start()
        //{
            //balls = new EntityPool(ballPrefab);
            //timeToNextBallSpawn = timeBetweenBallSpawns;
        //}

        //private void Update()
        //{
            //timeToNextBallSpawn -= Time.deltaTime;
            //if (timeToNextBallSpawn <= 0.00f)
            //{
            //    timeToNextBallSpawn = timeBetweenBallSpawns;
            //    GameObject ball = balls.Withdraw();
            //    ball.transform.position = new Vector3(transform.position.x, 0.50f, transform.position.z);
            //}
        //}
    }
}
