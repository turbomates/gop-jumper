using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    private TextMeshProUGUI levelText;

    private void Awake() {
        var obj = GameObject.Find("Level");
        levelText = obj.GetComponent<TextMeshProUGUI>();
    }
    
    public void SetCurrentLevel(int level) {    
        levelText.SetText(level.ToString());
    }
}
