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
        private float timeUntilThrowBall;

        public override void Created()
        {
            balls = new EntityPool(ballPrefab);
        }

        public override void Initialize()
        {
            timeUntilThrowBall = 0.5f;
        }

        public override void UpdateState()
        {
            timeUntilThrowBall -= Time.deltaTime;
            if (timeUntilThrowBall < 0.0f)
            {
                timeUntilThrowBall += 2.0f;
                Ball ball = balls.Withdraw<Ball>();
                ball.gameObject.transform.position = new Vector3(Mathf.FloorToInt(Random.Range(0, 3) - 1) * 10.0f, Mathf.FloorToInt(Random.Range(0, 3) - 1) * 10.0f, 100.0f);
            }
        }

        public override void Destroyed()
        {
            if (!balls.IsDestroyed())
            {
                balls.Destroy();
            }
        }
    }
}
