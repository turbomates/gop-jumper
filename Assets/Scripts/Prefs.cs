using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Prefs 
{
  public static int GetLevel() {
    return PlayerPrefs.GetInt("level", 1);
  }

  public static void SetLevel(int level) {
    PlayerPrefs.SetInt("level", level);
  }

  public static int GetCoins() {
    return PlayerPrefs.GetInt("coins", 0);
  }

  public static void SetCoins(int coins) {
    PlayerPrefs.SetInt("coins", coins);
  }
}