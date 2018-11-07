using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharaterMove : MonoBehaviour {

    public enum Players
    {
        Player1,
        Player2,
        Player3,
        Player4
    }

    static string[] XAxisNames = { "Player1X", "Player2X", "Player3X" , "Player4X" };    
    string XAxis;

    public Players Player;

    private Vector3 Fwd;
    private Vector3 OriRotate;
    public Vector3 next;

    private Transform Parent;
    private Animator _animator;
    private EffectManager _manager;

    [Range(0,2)]
    private float Speed = 0.06f;
    [Range(0, 5)]
    public float RotateSpeed = 1f;
    private float RotateAngle = 0;
    public float Rotations;

    private bool MoveDisable = false;
    private bool isRevert = false;
    public bool isBig = false;

    private void Start()
    {
        Parent = transform.parent;
        _animator = Parent.GetComponent<Animator>();
        _manager = EffectManager.Instance;
        _manager.PlayerContols.Add(this);
        XAxis = XAxisNames[(int)Player];
        OriRotate = transform.rotation.eulerAngles;
        RotateAngle = OriRotate.y;
    }

    void Update () {
        Move();
        Rotations = transform.rotation.eulerAngles.y;
    }

    void Move()
    {
        if(MoveDisable) return;
        Fwd = transform.up;
        XMove();
        var Next = Fwd * Speed;
        Parent.position += Next;
        _animator.SetFloat("PosX", Next.x);
        _animator.SetFloat("PosY", Next.z);
        next = Next;
        transform.rotation = Quaternion.Euler(new Vector3(OriRotate.x, RotateAngle, OriRotate.z));
    }

    void XMove()
    {
        float x = Input.GetAxis(XAxis);

        if (isRevert)
        {
            x *= -1;
        }
 
        if (x > 0)
        {
            RotateAngle += RotateSpeed;
        }
        else if(x < 0)
        {
            RotateAngle -= RotateSpeed;
        } 
    }

    public void RevertContol()
    {
        isRevert = true;
        Invoke("ResumeControl", 10);
    }

    public void ResumeControl()
    {
        isRevert = false;
    }

    public void TransparentControl()
    {
        var a = transform.GetComponentInParent<SpriteRenderer>();
        print(a);
        a.DOFade(0.1f, 0.5f);
        Invoke("ResumeTransparent", 10);
    }

    void ResumeTransparent()
    {
        var a = transform.GetComponentInParent<SpriteRenderer>();
        a.DOFade(1f, 0.5f);
    }

    public void DisableMove()
    {
        MoveDisable = true;
        Invoke("ResumeMove", 5);
    }

    void ResumeMove()
    {
        MoveDisable = false;
    }

    public void Bigger()
    {
        print("bigger");
        var v = transform.parent.localScale;
        transform.parent.DOScale(v*2,1).SetEase(Ease.InSine);
        Invoke("ResumeBig", 10);
        isBig = true;
    }

    void ResumeBig()
    {
        var v = transform.parent.localScale;
        transform.parent.DOScale(v/2,1).SetEase(Ease.InSine);
        isBig = false;
    }

    public void CantTouchItem()
    {
        //GetComponent<Collider>().isTrigger = true;
        Invoke("ResumeCantTouchItem", 10);
    }
    void ResumeCantTouchItem()
    {
        //GetComponent<Collider>().isTrigger = false;
    }
}