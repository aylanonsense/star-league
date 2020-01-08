using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Pitcher : Entity
    {
        [SerializeField]
        private GameObject ballPrefab;

        private EntityPool balls;

        public override void Created()
        {
            balls = new EntityPool(ballPrefab);
        }

        public override void Initialize()
        {
            //for (int x = -15; x <= 15; x += 5)
            //{
            //    for (int z = 0; z <= 100; z += 10)
            //    {
            //        Ball ball = balls.Withdraw<Ball>();
            //        ball.gameObject.transform.position = new Vector3(x, 0, z);
            //    }
            //}
            for (int x = -10; x <= 10; x += 20)
            {
                for (int y = -10; y <= 10; y += 20)
                {
                    for (int z = -1000; z <= 100; z += 100)
                    {
                        Ball ball = balls.Withdraw<Ball>();
                        ball.gameObject.transform.position = new Vector3(x, y, z);
                    }
                }
            }
            //for (int x = -15; x <= 15; x += 30)
            //{
            //    for (int y = -15; y <= 15; y += 30)
            //    {
            //        for (int z = 0; z <= 100; z += 20)
            //        {
            //            Ball ball = balls.Withdraw<Ball>();
            //            ball.gameObject.transform.position = new Vector3(x, y, z);
            //        }
            //    }
            //}
        }
    }
}
