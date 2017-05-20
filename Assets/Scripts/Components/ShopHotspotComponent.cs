using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHotspotComponent : MonoBehaviour {
    
    public float UpdateTimer = 0;

    public int CurrentValue = 0;
    public int ValueIncreasePerUpdate = 25;

    public bool WasRobbed = false;
}
