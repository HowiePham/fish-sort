using System;
using UnityEngine;

[Serializable]
public class FishInformation
{
    [SerializeField] private FishType fishType;
    [SerializeField] private Sprite fishSprite;
    [SerializeField] private int fishTankIndex;

    public FishType FishType => this.fishType;
    public Sprite FishSprite => this.fishSprite;
    public int FishTankIndex => this.fishTankIndex;
}