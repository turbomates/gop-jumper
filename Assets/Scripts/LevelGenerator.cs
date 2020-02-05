using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;

    public int numberOfPlatforms;
    public float levelWidth = 2f;
    public float minY;
    public float maxY;

    private void Start() {
        generatePlatforms();
    }

    private void generatePlatforms() {
        Vector3 spawnPosition = new Vector3();
        spawnPosition.y = -3;
        float screenWidth = Camera.main.aspect * 2f * Camera.main.orthographicSize;

        for (int i = 0; i < numberOfPlatforms; i++) {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);

            if (i % 5 == 0 && i != 0) {
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
