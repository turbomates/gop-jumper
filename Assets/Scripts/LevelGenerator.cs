using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;
    public GameObject groundPrefab;

    public float levelWidth = 2f;
    public float minY;
    public float maxY;

    private Vector3 spawnPosition;

    private void Awake() {
        spawnPosition = new Vector3(0, -Camera.main.orthographicSize + 0.5f);
    }

    public List<GameObject> GenerateLevel(int level) {
        List<GameObject> levelGameObjects = new List<GameObject>();
        var platforms = GeneratePlatforms(level);
        levelGameObjects.AddRange(platforms);
        GenerateGround();

        return levelGameObjects;
    }

    private void GenerateGround() {
        Instantiate(groundPrefab, spawnPosition, Quaternion.identity);
    }

    private List<GameObject> GeneratePlatforms(int level) {
        List<GameObject> platforms = new List<GameObject>();
        int numberOfPlatforms = 20 + level * 2;
        float screenWidth = Camera.main.aspect * 2f * Camera.main.orthographicSize;
        spawnPosition.y += 2f;

        for (int i = 0; i < numberOfPlatforms; i++) {
            spawnPosition.y += Random.Range(minY, maxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);

            if (i == numberOfPlatforms - 1) {
                spawnPosition.y += Random.Range(minY, maxY);
                spawnPosition.x = 0;
            } else {
                if (i % 5 == 0 && i != 0 && level > 2) {
                    GameObject platform = Instantiate(movingPlatformPrefab, spawnPosition, Quaternion.identity);
                    platforms.Add(platform);
                    SetRandomPlatformSprite(platform);
                } else {
                    GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
                    SetRandomPlatformSprite(platform);
                    float shadowPlatformXPosition = spawnPosition.x > 0 ? spawnPosition.x - screenWidth : spawnPosition.x + screenWidth;
                    spawnPosition.x = shadowPlatformXPosition;
                    platforms.Add(platform);
                    platforms.Add(Instantiate(platform, spawnPosition, Quaternion.identity));
                }
            }
        }

        return platforms;
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
            new Vector2(-edgeColiderPointPositionX + 0.1f, edgeColiderPointPositionY), 
            new Vector2(edgeColiderPointPositionX - 0.1f, edgeColiderPointPositionY) 
        };
    }
}
