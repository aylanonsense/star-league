using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Ball : Entity
    {
        private void Update()
        {
            transform.position += Vector3.back * 20 * Time.deltaTime;
        }
    }
}
