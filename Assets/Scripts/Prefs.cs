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

  
}