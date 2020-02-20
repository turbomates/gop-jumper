using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupScreen : MonoBehaviour {
    public GameObject costGameObject;
    public GameObject costButton;
    public List<GameObject> powerupsGameObjects;
    public GameObject uiGameObject;
    public GameObject gameGameObject;

    private Game game;
    private UI ui;
    private List<Image> powerupsSR;
    private GameObject previousScreen;
    private Text costText;
    private int powerup;
    private double cost;

    private void Awake() {
        powerupsSR = powerupsGameObjects.ConvertAll(gameObject => gameObject.GetComponent<Image>());
        game = gameGameObject.GetComponent<Game>();
        costText = costGameObject.GetComponent<Text>();
        ui = uiGameObject.GetComponent<UI>();
        powerup = Prefs.GetPowerup();
        UpdatePowerupColors();
        UpdateCost();
    }

    public void SetPreviousScreen(GameObject previousScreen) {
        this.previousScreen = previousScreen;
    }

    public void CloseScreen() {
        gameObject.SetActive(false);
        previousScreen.SetActive(true);
    }

    private void UpdatePowerupColors() {
        for (var i = 0; i < powerup; i++) {
            powerupsSR[i].color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void UpdateCost() {
        if (powerup == 9) {
            costButton.SetActive(false);
        } else {
            cost = powerup == 0 ? 25 : 25 * (powerup + 1) + 10 * (powerup - 1);
            costText.text = cost.ToString();
        }
    }

    public void BuyPowerup() {
        int coins = Prefs.GetCoins();
        if (coins >= cost) {
            powerup += 1;
            Prefs.SetPowerup(powerup);
            game.SetCoins(coins - (int) cost);
            
            UpdatePowerupColors();
            UpdateCost();
        }
    }
}
