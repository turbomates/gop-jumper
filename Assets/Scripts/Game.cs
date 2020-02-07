using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject levelGeneratorGameObject;
    public GameObject UIGameObject;

    private LevelGenerator levelGenerator;
    private UI ui;

    private List<GameObject> currentLevelGameObjects = new List<GameObject>();
    private List<GameObject> nextLevelGameObjects = new List<GameObject>();

    private void Awake() {
        levelGenerator = levelGeneratorGameObject.GetComponent<LevelGenerator>();
        ui = UIGameObject.GetComponent<UI>(); 
    }

    private void Start() {
        int level = Prefs.GetLevel();

        ui.SetCurrentLevel(level);
        currentLevelGameObjects = levelGenerator.GenerateLevel(level);
        nextLevelGameObjects = levelGenerator.GenerateLevel(level + 1);
    }

    public void FinishLevel() {
        currentLevelGameObjects.ForEach(gameObject => Destroy(gameObject));
        currentLevelGameObjects = nextLevelGameObjects;

        int nextLevel = Prefs.GetLevel() + 1;
        Prefs.SetLevel(nextLevel);
        nextLevelGameObjects = levelGenerator.GenerateLevel(nextLevel + 1);

        ui.SetCurrentLevel(nextLevel);
    }
}
