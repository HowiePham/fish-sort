using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelFlowHandler : MonoBehaviour
{
    [SerializeField] private GameInputHandler inputHandler;
    [SerializeField] private Fish fishPrefab;
    [SerializeField] private List<FishInformation> fishInformations;
    [SerializeField] private List<Fish> fishInLevel;
    [SerializeField] private FishTank[] fishTanks;

    private void Awake()
    {
        CreateFish();
        Init();
    }

    private void Init()
    {
        this.inputHandler.Init(this.fishInLevel, this.fishTanks);
    }

    private void CreateFish()
    {
        foreach (FishInformation fishInformation in this.fishInformations)
        {
            FishType fishType = fishInformation.FishType;
            Sprite fishSprite = fishInformation.FishSprite;
            int fishTankIndex = fishInformation.FishTankIndex;

            Fish fish = Instantiate(this.fishPrefab, this.transform);
            FishTank fishTank = this.fishTanks[fishTankIndex];
            FishHolder holder = fishTank.OccupyEmptyHolder(fishType.TypeNumber, fish.transform);

            fish.transform.position = holder.Position;
            fish.Init(holder, fishType, fishSprite);

            this.fishInLevel.Add(fish);
        }
    }

#if UNITY_EDITOR
    [Button]
    private void GetAllFishTanks()
    {
        this.fishTanks = GetComponentsInChildren<FishTank>();
    }
#endif
}