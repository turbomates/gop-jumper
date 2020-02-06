using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;

    public float levelWidth = 2f;
    public float minY;
    public float maxY;

    private Vector3 spawnPosition;
    private int currentLevel;

    private void Awake() {
        spawnPosition = new Vector3(0, -Camera.main.orthographicSize);
    }

    public void GenerateLevel(int level) {
        currentLevel = level;

        int numberOfPlatform = 40 + level * 2;
        GenerateGround();
        GeneratePlatforms(numberOfPlatform);
    }

    private void GenerateGround() {

    }

    private void GeneratePlatforms(int numberOfPlatforms) {
        float screenWidth = Camera.main.aspect * 2f * Camera.main.orthographicSize;

        for (int i = 0; i < numberOfPlatforms; i++) {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);

            if (i == numberOfPlatforms - 1) {
                // Finish platform
            }

            if (i % 5 == 0 && i != 0 && currentLevel > 2) {
                GameObject platform = Instantiate(movingPlatformPrefab, spawnPosition, Quaternion.identity);
                SetRandomPlatformSprite(platform);
            } else {
                GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
                SetRandomPlatformSprite(platform);
                float shadowPlatformXPosition = spawnPosition.x > 0 ? spawnPosition.x - screenWidth : spawnPosition.x + screenWidth;
                spawnPosition.x = shadowPlatformXPosition;
                Instantiate(platform, spawnPosition, Quaternion.identity);
            }

            
        }
    }

    private void SetRandomPlatformSprite(GameObject platform) {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Platforms");
        Sprite randomSprite = sprites[Random.Range(0, sprites.Length)];

        SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();
        sr.sprite = randomSprite;

        float edgeColiderPointPositionX = sr.bounds.size.x / 2;
        float edgeColiderPointPositionY = sr.bounds.size.y / 2;

        var ec = platform.GetComponent<EdgeCollider2D>();
        ec.points = new Vector2[] { 
            new Vector2(-edgeColiderPointPositionX + 0.2f, edgeColiderPointPositionY), 
            new Vector2(edgeColiderPointPositionX - 0.2f, edgeColiderPointPositionY) 
        };
    }
}
