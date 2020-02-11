using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Game game;
    private List<GameObject> coins = new List<GameObject>();

    private void Start() {
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    public void SetCoins(GameObject coinOne, GameObject coinTwo) {
        coins.Add(coinOne);
        coins.Add(coinTwo);
    }

    private void OnTriggerEnter2D(Collider2D colider) {
        coins.ForEach(coin => Destroy(coin));
        Destroy(gameObject);
        game.AddCoin();
    }
}
