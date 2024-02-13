using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private Customer _customerPrefab;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private float _baseWaitTime = 30f;

    private void SpawnCustomer()
    {
        var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        var customer = Instantiate(_customerPrefab, spawnPoint.position, Quaternion.identity);
        customer.Initialise(GenerateOrder(), _baseWaitTime);
    }

    private List<Vegetable> GenerateOrder()
    {
        //Generate a random order
        return new List<Vegetable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnCustomer();
    }
}
