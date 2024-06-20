using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class BotSpawner : MonoBehaviour
{
    [SerializeField] private int _botSpawnPrice = 3;
    [SerializeField] private float _maxSpawnRange = 10f;
    [SerializeField] private Bot _prefab;

    private float _minSpawnRange;

    public int BotSpawnPrice => _botSpawnPrice;

    private void Awake() => _minSpawnRange = -_maxSpawnRange;

    public Bot[] SpawnBots(int count, Transform container)
    {
        Bot[] bots = new Bot[count];

        for (int i = 0; i < count; i++)
        {
            float offsetX = Random.Range(_minSpawnRange, _maxSpawnRange);
            float offsetZ = Random.Range(_minSpawnRange, _maxSpawnRange);
            Vector3 spawnpoint =
                new(transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ);

            Bot bot = Instantiate(_prefab);
            bots[i] = bot;
            bot.transform.position = spawnpoint;
            bot.transform.parent = container;
            bot.SetBase(gameObject.GetComponent<Base>());
        }

        return bots;
    }
}
