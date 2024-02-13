using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public List<Vegetable> Order { get; private set; }
    public float WaitTime { get; private set; }
    public bool IsServed { get; private set; } = false;
    public bool IsAngry { get; private set; } = false;

    public void Initialise(List<Vegetable> order, float waitTime)
    {
        Order = order;
        WaitTime = waitTime;
        StartCoroutine(WaitForOrder());
    }

    private IEnumerator WaitForOrder()
    {
        yield return new WaitForSeconds(WaitTime);
        if (!IsServed)
            LeaveUnsatisfied();
    }

    public void ServeOrder(List<Vegetable> servedOrder)
    {
        if (servedOrder.Equals(Order))
        {
            IsServed = true;
        }
        else
        {
            IsAngry = true;
            LeaveUnsatisfied();
        }
    }

    void LeaveUnsatisfied()
    {
        //Penalize player
    }

}
