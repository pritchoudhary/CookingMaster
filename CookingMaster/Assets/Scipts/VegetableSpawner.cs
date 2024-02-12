using UnityEngine;

public class VegetableSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnPoint
    {
        public Transform _spawnPoint;
        public Vegetable _vegetablePrefab;
    }

    public SpawnPoint[] _spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAllVegetables();
    }

    private void SpawnAllVegetables()
    {
        foreach(var spawnPoint in _spawnPoints)
        {
            SpawnVegetables(spawnPoint);
        }
    }

    private void SpawnVegetables(SpawnPoint spawnPoint)
    {
        if(spawnPoint._spawnPoint != null && spawnPoint._vegetablePrefab != null)
        {
            VegetableInstance spawnedVegetable = Instantiate(spawnPoint._vegetablePrefab._vegPrefab, spawnPoint._spawnPoint.position, Quaternion.identity, spawnPoint._spawnPoint).GetComponent<VegetableInstance>();

            //Assign the vegetable data to the instance
            spawnedVegetable._vegetableData = spawnPoint._vegetablePrefab;

            //Subscrive to OnPickUp event
            spawnedVegetable.onPickedUp += () => SpawnVegetables(spawnPoint);
        }
    }
}
