using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject levelGeneratorGameObject;
    public GameObject UIGameObject;
    public GameObject backgroundGameObject;

    private bool gamePaused = false;
    private LevelGenerator levelGenerator;
    private UI ui;
    private Background background;

    private Level currentLevel;
    private Level nextLevel;
    private int levelCoins = 0;
    private int coins;

    private void Update() {
        if (gamePaused && Input.GetMouseButtonDown(0)) {
            Continue();
            ui.ToggleUI();
        }
    }

    private void Awake() {
        Pause();
        levelGenerator = levelGeneratorGameObject.GetComponent<LevelGenerator>();
        ui = UIGameObject.GetComponent<UI>(); 
        background = backgroundGameObject.GetComponent<Background>(); 
        coins = Prefs.GetCoins();
    }

    private void Start() {
        int levelNumber = Prefs.GetLevel();

        ui.SetCurrentLevel(levelNumber);
        ui.SetCoins(coins);
        background.ChangeColor(levelNumber);
        currentLevel = levelGenerator.GenerateLevel(levelNumber);
        nextLevel = levelGenerator.GenerateLevel(levelNumber + 1);
    }
    
    public void AddCoin() {
        coins += 1;
        levelCoins += 1;
        Prefs.SetCoins(coins);
        ui.SetCoins(coins);
    }

    public void UpdatePlayerProgress(float yPosition) {
        float completed = (yPosition - currentLevel.startPosition.y) / (currentLevel.endPosition.y - currentLevel.startPosition.y);
        ui.SetLevelProgress(completed);
    }

    public void FinishLevel() {
        Pause();

        ui.ToggleUI();
        ui.SetReward(levelCoins, currentLevel.maxCoins);

        levelCoins = 0;

        currentLevel.platforms.ForEach(gameObject => Destroy(gameObject));
        currentLevel = nextLevel;

        int nextLevelNumber = Prefs.GetLevel() + 1;
        Prefs.SetLevel(nextLevelNumber);
        nextLevel = levelGenerator.GenerateLevel(nextLevelNumber + 1);

        background.ChangeColor(nextLevelNumber);
        ui.SetCurrentLevel(nextLevelNumber);
    }

    public void Pause() {
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void Continue() {
        Time.timeScale = 1f;
        gamePaused = false;
    }
}
