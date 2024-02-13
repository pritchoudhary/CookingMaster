using System.Collections.Generic;
using UnityEngine;

public class Combination : MonoBehaviour
{
    [SerializeField] private List<Vegetable> ingredients = new();


    public void AddIngredients(Vegetable ingredient)
    {
        if(!ingredients.Contains(ingredient))
            ingredients.Add(ingredient);
    }

    public void Clear()
    {
        ingredients.Clear();
    }
    
}
