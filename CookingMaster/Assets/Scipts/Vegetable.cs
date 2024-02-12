using UnityEngine;

//Enum to represent the state of the vegetable
public enum VegetableState
{
    Idle, //Placed in the world, not being interacted with
    Carried, //Being carried by the player
    Chopping, //Placed on the chopping board and being chopped
    Chopped //Chopped and ready to use
}

[CreateAssetMenu(fileName = "New Vegetable", menuName = "Vegetable")]
public class Vegetable : ScriptableObject
{
    public string _vegetableName;
    public float _chopTime = 5.0f;
    public GameObject _vegPrefab;
}
