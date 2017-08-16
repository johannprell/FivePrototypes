using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domination
{
	public class ObstacleSpawner : MonoBehaviour 
	{
		/* --- MEMBER VARIABLES --- */
		[Header("Prefabs to spawn")]
		public List<GameObject> ObstaclePrefabs;
		[Header("Grid layout")]
		public float Spacing = 20f;
		public float Offset = 10f;
		public int GridSize = 10;
		[Header("Randomization variables")]
		[Range(0, 100)]
		public int ChanceOfSpawnMin = 25;
		[Range(0, 100)]
		public int ChanceOfSpawnMax = 75;
		[Range(0, 100)]
		public int ChanceOfSpawn = 0;
		[Range(0f, 8f)]
		public float RandomizeOffsetRange = 2.5f;


		
		/* --- UNITY METHODS --- */
		void Start() 
		{
			ChanceOfSpawn = Random.Range(ChanceOfSpawnMin, ChanceOfSpawnMax);
			InstantiateObstacles();
		}
		
		void Update() 
		{
			
		}

		/* --- CUSTOM METHODS --- */
		private void InstantiateObstacles()
		{
			for (int x = -GridSize/2; x < GridSize/2; x++)
			{
				for (int y = -GridSize/2; y < GridSize/2; y++)
				{
					if(Random.Range(0, 100) < ChanceOfSpawn)
					{
						int randomIndex = Random.Range(0, ObstaclePrefabs.Count);
						float xOffset = Offset + Random.Range(-RandomizeOffsetRange, RandomizeOffsetRange);
						float yOffset = Offset + Random.Range(-RandomizeOffsetRange, RandomizeOffsetRange);
						Vector3 spawnPosition = new Vector3(x * Spacing + xOffset, 0f, y * Spacing + yOffset);
						Instantiate(ObstaclePrefabs[randomIndex], spawnPosition, Quaternion.identity, transform);
					}
				}
			}
		}
	}
}