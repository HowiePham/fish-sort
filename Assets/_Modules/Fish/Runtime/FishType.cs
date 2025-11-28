using System;

[Serializable]
public struct FishType
{
    public int TypeNumber;

    public bool IsSameType(FishType type)
    {
        return this.TypeNumber == type.TypeNumber;
    }
}