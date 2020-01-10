using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Pitcher : Entity
    {
        public new readonly EntityType type = EntityType.Pitcher;

        [SerializeField]
        private GameObject ballPrefab;

        private EntityPool balls;

        public override void Created()
        {
            balls = new EntityPool(ballPrefab);
        }

        public override void Initialize()
        {
            //for (int x = -10; x <= 10; x += 20)
            //{
            //    for (int y = -10; y <= 10; y += 20)
            //    {
                    for (int z = 0; z <= 200; z += 50)
                    {
                        Ball ball = balls.Withdraw<Ball>();
                        //ball.gameObject.transform.position = new Vector3(Mathf.FloorToInt(Random.Range(0, 3) - 1) * 10, Mathf.FloorToInt(Random.Range(0, 3) - 1) * 10, z);
                        ball.gameObject.transform.position = new Vector3(10.0f, 10.0f, z);
                    }
            //    }
            //}
        }
    }
}
