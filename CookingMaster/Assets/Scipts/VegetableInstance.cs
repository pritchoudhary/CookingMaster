using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetableInstance : MonoBehaviour
{
    public Vegetable _vegetableData;
    public VegetableState _currentState = VegetableState.Idle;
    public event Action onPickedUp;

    //This method can be called via the Player script while picking up the vegetable
    public void PickUp()
    {
        if(_currentState == VegetableState.Idle)
        {
            UpdateState(VegetableState.Carried);
            onPickedUp?.Invoke();
        }
    }

    //This method can be called via the Player script while placing the vegetable on the Chopping board
    public void PlaceOnChoppingBoard()
    {
        if(_currentState == VegetableState.Carried)
        {
            UpdateState(VegetableState.Chopping);
        }
    }

    //Update the state of the vegetable
    public void UpdateState(VegetableState newState)
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


    public void ResetVegetable()
    {
        UpdateState(VegetableState.Idle);
    }
}
