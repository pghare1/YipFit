using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public YipliConfig currentYipliConfig;
    public string gameId;

    private void Awake()
    {   //playfit
        currentYipliConfig.gameId = "yipfit";
        currentYipliConfig.gameType = GameType.FITNESS_GAMING;
    }
}
