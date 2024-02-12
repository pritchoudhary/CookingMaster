using System.Collections;
using UnityEngine;

public class ChoppingBoard : MonoBehaviour
{
    private VegetableInstance _currentVegetable = null;

    public bool ChopVegetable(VegetableInstance vegetable)
    {
        //Only allow a vegetable to be placed if there is no vegetable being chopped
        if(_currentVegetable == null && vegetable._currentState == VegetableState.Carried)
        {
            _currentVegetable = vegetable;
            _currentVegetable.UpdateState(VegetableState.Chopping);
            StartCoroutine(ChopVegetableCoroutine(_currentVegetable));
            return true; //vegetable is successfully placed for chopping

        }

        return false; //indicates chopping board is busy.
    }
    
    private IEnumerator ChopVegetableCoroutine(VegetableInstance vegetable)
    {
        yield return new WaitForSeconds(vegetable._vegetableData._chopTime);
        //Update the vegetable state to Chopped
        vegetable.UpdateState(VegetableState.Chopped);
        //Clear the chopping board for the next vegetable
        _currentVegetable = null;
    }

    //Method to check if the chopping board is available
    public bool IsAvailable()
    {
        return _currentVegetable == null;
    }
}
