using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetableInstance : MonoBehaviour
{
    public Vegetable _vegetableData;
    private VegetableState _currentState = VegetableState.Idle;
    public event Action onPickedUp;

    public void PickUp()
    {
        if(_currentState == VegetableState.Idle)
        {
            UpdateState(VegetableState.Carried);
            onPickedUp?.Invoke();
        }
    }

    //Update the state of the vegetable
    private void UpdateState(VegetableState newState)
    {
        _currentState = newState;

        switch(newState)
        {
            case VegetableState.Idle:
                break;
            case VegetableState.Carried:
                break;
            case VegetableState.Chopping:
                break;
            case VegetableState.Chopped:
                break;
        }
    }
}
