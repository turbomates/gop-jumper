using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Level
{
    public List<GameObject> platforms;
    public GameObject ground;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public int maxCoins;
}

public struct PlatformsAndCoins
{
    public List<GameObject> platforms;
    public int maxCoins;
}

public class LevelGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;
    public GameObject groundPrefab;
    public GameObject coinPrefab;

    public float levelWidth = 2f;
    public float minY;
    public float maxY;

    private Vector3 spawnPosition;

    private void Awake() {
        spawnPosition = new Vector3(0, -Camera.main.orthographicSize + 0.5f);
    }

    public Level GenerateLevel(int levelNumber) {
        Level level;
        level.startPosition = spawnPosition;
        PlatformsAndCoins platformAndCoins = GeneratePlatformsAndCoins(levelNumber);
        level.platforms = platformAndCoins.platforms;
        level.ground = GenerateGround();
        level.endPosition = spawnPosition;
        level.maxCoins = platformAndCoins.maxCoins;

        return level;
    }

    private GameObject GenerateGround() {
        return Instantiate(groundPrefab, spawnPosition, Quaternion.identity);
    }

    private PlatformsAndCoins GeneratePlatformsAndCoins(int level) {
        PlatformsAndCoins platformsAnsCoins;
        platformsAnsCoins.maxCoins = 0;
        platformsAnsCoins.platforms = new List<GameObject>();

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
                    platformsAnsCoins.platforms.Add(platform);
                    SetRandomPlatformSprite(platform);
                } else {
                    GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
                    SetRandomPlatformSprite(platform);
                    

                    if (Random.Range(0, 3) > 1) {
                        GenerateCoinAtPosition(spawnPosition);
                        platformsAnsCoins.maxCoins += 1;
                    }

                    float shadowPlatformXPosition = spawnPosition.x > 0 ? spawnPosition.x - screenWidth : spawnPosition.x + screenWidth;
                    spawnPosition.x = shadowPlatformXPosition;
                    platformsAnsCoins.platforms.Add(platform);
                    platformsAnsCoins.platforms.Add(Instantiate(platform, spawnPosition, Quaternion.identity));
                }
            }
        }

        return platformsAnsCoins;
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

    private void GenerateCoinAtPosition(Vector3 position) {
        float screenWidth = Camera.main.aspect * 2f * Camera.main.orthographicSize;
        position.y += 0.7f;
        GameObject middleCoin = Instantiate(coinPrefab, position, Quaternion.identity);
        position.x -= screenWidth;
        GameObject leftCoin = Instantiate(coinPrefab, position, Quaternion.identity);
        position.x += screenWidth * 2;
        GameObject rightCoin = Instantiate(coinPrefab, position, Quaternion.identity);

        middleCoin.GetComponent<Coin>().SetCoins(leftCoin, rightCoin);
        leftCoin.GetComponent<Coin>().SetCoins(middleCoin, rightCoin);
        rightCoin.GetComponent<Coin>().SetCoins(leftCoin, middleCoin);
    }
}
