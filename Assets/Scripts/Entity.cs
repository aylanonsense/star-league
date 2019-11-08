using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Entity : BaseMonoBehaviour
    {
        public EntityPool pool;

        public void RemoveFromGame()
        {
            if (pool != null)
            {
                pool.Deposit(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
