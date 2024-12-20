using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class SpawnerAI : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints = new();
        [SerializeField] private int _spawnCount = 2;
        [SerializeField] private int _maxCount = 4;
        [SerializeField] private float _spawnCooldown = 30f;
        [SerializeField] private float _clearCooldown = 10f;

        private List<AIController> _spawnedAI = new();
        private int _pointIndex;
        private float _spawnTime;
        private float _clearTime;

        public void Initialize()
        {
            _pointIndex = 0;
            _spawnTime = _spawnCooldown;
        }

        public void OnFixedUpdate()
        {
            _clearTime += Time.fixedDeltaTime;
            if (_clearTime >= _clearCooldown)
            {
                if (_spawnedAI.Count > 0)// remove corpses
                {
                    for (int i = _spawnedAI.Count - 1; i >= 0; i--)
                    {
                        if (_spawnedAI[i].IsDead)
                        {
                            WorldManager.StaticInstance.AIManager.DespawnAI(_spawnedAI[i]);
                            _spawnedAI.RemoveAt(i);
                        }
                    }
                }
                _clearTime = 0f;
                return;
            }
            _spawnTime += Time.fixedDeltaTime;
            if (_spawnTime >= _spawnCooldown)
            {
                for (int i = 0; i < _spawnCount; i++)
                {
                    if (_spawnedAI.Count - 1 == _maxCount)
                    {
                        break;
                    }
                    Spawn();
                }
                _spawnTime = 0f;
            }
        }

        private void Spawn()
        {
            _spawnedAI.Add(WorldManager.StaticInstance.AIManager.SpawnAI(_spawnPoints[_pointIndex].position));
            if (_pointIndex == _spawnPoints.Count - 1)
            {
                _pointIndex = 0;
            }
            else
            {
                _pointIndex++;
            }
        }
    }
}