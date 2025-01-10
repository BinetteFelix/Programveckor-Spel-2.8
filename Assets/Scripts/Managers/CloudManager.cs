using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public GameObject cloudPrefab; // Prefab for the clouds
    public int cloudCount = 10;    // Number of clouds to spawn
    public Vector3 cloudSpawnArea = new Vector3(100, 50, 100); // Area to spawn clouds

    private GameObject[] _clouds;

    void Start()
    {
        SpawnClouds();
    }

    public void UpdateClouds(float deltaTime)
    {
        foreach (GameObject cloud in _clouds)
        {
            if (cloud != null)
            {
                cloud.transform.position += new Vector3(0.01f, 0, 0) * deltaTime;

                // Wrap around if cloud moves out of bounds
                if (cloud.transform.position.x > cloudSpawnArea.x / 2)
                {
                    cloud.transform.position = new Vector3(-cloudSpawnArea.x / 2, cloud.transform.position.y, cloud.transform.position.z);
                }
            }
        }
    }

    void SpawnClouds()
    {
        _clouds = new GameObject[cloudCount];

        for (int i = 0; i < cloudCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-cloudSpawnArea.x / 2, cloudSpawnArea.x / 2),
                Random.Range(0, cloudSpawnArea.y),
                Random.Range(-cloudSpawnArea.z / 2, cloudSpawnArea.z / 2)
            );

            _clouds[i] = Instantiate(cloudPrefab, randomPosition, Quaternion.identity);
        }
    }
}
