using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public int state;
    public int framesSinceStateChange;
    public float timeSinceStateChange;

    public StateMachine(int initialState = 0)
    {
        state = initialState;
        framesSinceStateChange = 0;
        timeSinceStateChange = 0.00f;
    }

    public void Update()
    {
        framesSinceStateChange = framesSinceStateChange + 1;
    }

    public void FixedUpdate()
    {
        timeSinceStateChange = timeSinceStateChange + Time.deltaTime;
    }

    public void SetState(int state)
    {
        this.state = state;
        framesSinceStateChange = 0;
        timeSinceStateChange = 0.00f;
    }
}
