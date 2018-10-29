using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLabel : MonoBehaviour {
    private Text label;

    [SerializeField]
    private Pumpking pumpking;

    private float lastUpdate;
    private const float updateInterval = 0.5f;

    private void Awake() {
        label = GetComponent<Text>();
    }

    private void Update() {
        if (Time.time > lastUpdate + updateInterval) {
            var score = pumpking.GetScore();
            var hiScore = pumpking.GetHiScore();

            label.text = $"SCORE {score:000,000,000}\nHI {hiScore:000,000,000}";
            lastUpdate = Time.time;
        }
    }
}
