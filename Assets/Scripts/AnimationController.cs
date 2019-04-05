using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animLeft;
    public Animator animRight;
    private static readonly int IsPaddling = Animator.StringToHash("isPaddling");
    public GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        gm = this.gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    public void UpdateAnimations(bool leftPlayerIsPaddling, bool rightPlayerIsPaddling)
    {
        animLeft.SetBool(IsPaddling, leftPlayerIsPaddling);
        animRight.SetBool(IsPaddling, rightPlayerIsPaddling);
    }
}