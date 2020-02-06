using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject levelGeneratorGameObject;
    public GameObject UIGameObject;

    private LevelGenerator levelGenerator;
    private UI ui;

    private void Awake() {
        levelGenerator = levelGeneratorGameObject.GetComponent<LevelGenerator>();
        ui = UIGameObject.GetComponent<UI>(); 
    }

    void Start() {
        

        int level = Prefs.GetLevel();

        ui.SetCurrentLevel(level);
        levelGenerator.GenerateLevel(level);
    }

    void Update() {
        
    }
}
