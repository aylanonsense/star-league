using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Ball : Entity
    {
        public new readonly EntityType type = EntityType.Ball;

        public override void UpdateState()
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 100 * Time.deltaTime);
        }
    }
}
