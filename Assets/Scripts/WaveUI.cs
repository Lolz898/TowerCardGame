using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    private TMP_Text waveText;
    private GameManager gameManager;
    private WaveManager waveManager;

    private void Start()
    {
        waveText = GetComponent<TMP_Text>();
        gameManager = GameManager.Instance; // Make sure GameManager is a Singleton or accessible statically.
        // Get a reference to the WaveManager script on the GameManager object
        waveManager = gameManager.GetComponent<WaveManager>();
    }

    private void Update()
    {
        if (waveManager != null)
        {
            int currentWave = waveManager.GetCurrentWaveIndex() + 1;
            int totalWaves = waveManager.GetTotalWaves();

            if (currentWave > totalWaves)
            {
                // Update the waveText with the current wave and total waves
                waveText.text = (currentWave - 1) + "/" + totalWaves;
            }
            else
            {
                // Update the waveText with the current wave and total waves
                waveText.text = currentWave + "/" + totalWaves;
            }
        }
    }
}
