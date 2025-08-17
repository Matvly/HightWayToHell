using Unity.VisualScripting;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    [SerializeField]  private Animator anim;
    [SerializeField]  private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform MainLefthand;
    [SerializeField] private Transform MainRighthand;

    public Transform LeftHand => leftHand;
    public Transform RightHand => rightHand;
    public Transform MainLeftHand => MainLefthand;
    public Transform MainRightHand => MainRighthand;

    public Animator Anim => anim;
    
    private void Start()
    {
        anim = leftHand.GetComponent<Animator>();
    }

    public void StartAnim()
    {
        anim.SetTrigger("Start");
    }
}
