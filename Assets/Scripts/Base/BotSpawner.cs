using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private int _botSpawnPrice = 3;
    [SerializeField] private float _maxSpawnRange = 10f;
    [SerializeField] private Bot _prefab;
    [SerializeField] private Transform _botsContainer;

    private float _minSpawnRange;

    public List<Bot> SpawnedBots { get; private set; }
    public int BotSpawnPrice => _botSpawnPrice;

    private void Awake() => _minSpawnRange = -_maxSpawnRange;

    private void OnEnable() => SpawnedBots = new List<Bot>();

    public void SpawnBots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float offsetX = Random.Range(_minSpawnRange, _maxSpawnRange);
            float offsetZ = Random.Range(_minSpawnRange, _maxSpawnRange);
            Vector3 spawnpoint =
                new(transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ);

            Bot bot = Instantiate(_prefab);
            SpawnedBots.Add(bot);
            bot.transform.parent = _botsContainer.transform;
            bot.transform.position = spawnpoint;
            bot.SetBase(gameObject.GetComponent<Base>());
        }
    }

    public void RemoveBot(Bot bot)
    {
        SpawnedBots.Remove(bot);
    }
}
