using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySafeComponent : MonoBehaviour {

    public int SavedMoney;

    public float SaveTimer;
    public PlayerController Mafioso;
    private float timer;
    private int saveAmount;

	// Use this for initialization
	void Start () {
        SavedMoney = 0;
	}

    public void StashMoney(int amount, PlayerController mafioso)
    {
        if (saveAmount > 0)
        {
            Debug.Log("Already saving in " + timer);
        }
        else
        {
            timer = SaveTimer;
            saveAmount = amount;
        }
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                SavedMoney += saveAmount;
                Mafioso.Score -= saveAmount;
                saveAmount = 0;
            }
        }
    }
}
