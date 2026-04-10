using UnityEngine;

[System.Serializable]
public class DoorTagPair
{
    public string lockedDoorTag;
    public string unlockedDoorTag;
}

public class DoorKeyData : MonoBehaviour
{
    [Header("What this key unlocks")]
    public DoorTagPair[] doorPairs;

    [Header("Optional")]
    public string keyId = "Key A";
}