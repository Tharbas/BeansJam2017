using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionComponent : MonoBehaviour
{
    public enum CharacterType
    {
        NPC,
        Mafioso
    }

    [SerializeField]
    private CharacterType characterType;


}
