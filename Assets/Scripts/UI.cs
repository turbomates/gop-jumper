using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour
{
    public GameObject progress;
    public GameObject rewardGameObject;

    private Text levelText;
    private Text nextLevelText;
    private Text coinsText;
    private RectTransform progressRT;
    
    const float MAX_PROGRESS_WIDTH = 150f;

    private void Awake() {
        levelText = GameObject.Find("Level").GetComponent<Text>();
        nextLevelText = GameObject.Find("NextLevel").GetComponent<Text>();
        coinsText = GameObject.Find("CoinsCounter").GetComponent<Text>();
        progressRT = progress.GetComponent<RectTransform>();
    }

    public void SetLevelProgress(float completed) {
        Vector3 scale = progressRT.localScale;
        Vector3 position = progressRT.anchoredPosition;
        scale.x = completed;
        position.x = completed * MAX_PROGRESS_WIDTH / 2f - MAX_PROGRESS_WIDTH / 2f;

        progressRT.localScale = scale;
        progressRT.anchoredPosition = position;
    }
    
    public void SetCurrentLevel(int level) {    
        levelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();
    }

    public void SetCoins(int coins) {
        coinsText.text = coins.ToString();
    }

    public void SetReward(int picked, int max) {
        rewardGameObject.GetComponent<Reward>().SetCoins(picked, max);
    }

    public void Back(GameObject previousScreen) {
        previousScreen.SetActive(true);
    }
}
