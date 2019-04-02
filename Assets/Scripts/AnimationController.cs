using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator anim;
    public bool isTrue;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    public GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        gm = this.gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.leftPlayerIsPaddling)
        {
            anim.SetBool(IsMoving, true);
        }
        else
        {
            anim.SetBool(IsMoving, false);
        }
    }
}