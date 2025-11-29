using System;
using BW.EventSystem;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    [SerializeField] private GameObject levelRoot;
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private GameObject winView;
    [SerializeField] private GameObject loseView;

    private void Awake()
    {
        Messenger.AddListener(EventKey.LevelComplete, StartNewLevel);
        Messenger.AddListener(EventKey.LevelWin, LevelWinHandler);
        Messenger.AddListener(EventKey.LevelLose, LevelLoseHandler);
    }

    private void LevelWinHandler()
    {
        this.winView.SetActive(true);
    }

    private void LevelLoseHandler()
    {
        this.loseView.SetActive(true);
    }

    private void StartNewLevel()
    {
        Destroy(this.levelRoot);
        this.levelRoot = null;

        this.levelRoot = Instantiate(this.levelPrefab);
    }
}