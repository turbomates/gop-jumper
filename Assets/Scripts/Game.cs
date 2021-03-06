﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Game : MonoBehaviour
{
    public GameObject levelGeneratorGameObject;
    public GameObject UIGameObject;
    public GameObject backgroundGameObject;
    public GameObject completeLevelScreen;
    public GameObject pauseButton;

    private bool gamePaused = false;
    private LevelGenerator levelGenerator;
    private UI ui;
    private Background background;

    private Level currentLevel;
    private Level nextLevel;
    private int levelCoins = 0;
    private int coins;
    private List<int> coinIds = new List<int>();

    private InterstitialAd interstitial;

    private void Awake() {
        Pause();
        levelGenerator = levelGeneratorGameObject.GetComponent<LevelGenerator>();
        ui = UIGameObject.GetComponent<UI>(); 
        background = backgroundGameObject.GetComponent<Background>(); 
        coins = Prefs.GetCoins();
    }

    private void Start() {
        InitializeMobileAds();

        Application.targetFrameRate = 60;

        int levelNumber = Prefs.GetLevel();

        ui.SetCurrentLevel(levelNumber);
        ui.SetCoins(coins);
        background.ChangeColor(levelNumber);
        currentLevel = levelGenerator.GenerateLevel(levelNumber);
        nextLevel = levelGenerator.GenerateLevel(levelNumber + 1);

        RequestInterstitial();

        Prefs.SetPowerup(0);
    }

    private void InitializeMobileAds() {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-2682671172904405~5734676598";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-2682671172904405~5926016776";
        #else
            string appId = "unexpected_platform";
        #endif

        MobileAds.Initialize(appId);
    }

    private void RequestInterstitial() {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-2682671172904405/4204944834";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    private void OnInterstitialLoaded(object sender, System.EventArgs args) {
        interstitial.Show();
    }
    
    public void AddCoin(int id) {
        if (!coinIds.Contains(id)) {
            coins += 1;
            levelCoins += 1;
            coinIds.Add(id);
            SetCoins(coins);
        }
    }

    public void SetCoins(int amount) {
            coins = amount;
            Prefs.SetCoins(amount);
            ui.SetCoins(amount);
    }

    public void UpdatePlayerProgress(float yPosition) {
        float completed = (yPosition - currentLevel.startPosition.y) / (currentLevel.endPosition.y - currentLevel.startPosition.y);
        ui.SetLevelProgress(completed);
    }

    public void FinishLevel() {
        if (interstitial.IsLoaded()) {
            interstitial.Show();
        }
        
        Pause();

        pauseButton.SetActive(false);
        completeLevelScreen.SetActive(true);
        ui.SetReward(levelCoins, currentLevel.maxCoins);

        levelCoins = 0;

        currentLevel.platforms.ForEach(gameObject => Destroy(gameObject));
        currentLevel = nextLevel;

        int nextLevelNumber = Prefs.GetLevel() + 1;
        Prefs.SetLevel(nextLevelNumber);
        nextLevel = levelGenerator.GenerateLevel(nextLevelNumber + 1);
        coinIds.Clear();

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
