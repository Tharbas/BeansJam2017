using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class RandomSpriteComponent : MonoBehaviour
{
    [SerializeField]
    private int npcVariantsCount = 5;

    [SerializeField]
    private AnimatorController[] allControllers;

    [SerializeField]
    private Animator animator;

    public void Start()
    {
        int index = UnityEngine.Random.Range(0, this.npcVariantsCount);
        this.animator.runtimeAnimatorController = this.allControllers[index];
    }
}
