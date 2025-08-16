using Unity.VisualScripting;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    [SerializeField]  private Animator anim;
    [SerializeField] private Animator handsAnim;
    [SerializeField]  private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform MainLefthand;
    [SerializeField] private Transform MainRighthand;
    [SerializeField] private Transform hands;

    public Transform LeftHand => leftHand;
    public Transform RightHand => rightHand;
    public Transform MainLeftHand => MainLefthand;
    public Transform MainRightHand => MainRighthand;

    public Transform Hands => hands;

    public Animator Anim => anim;

    public Animator HandsAnim => handsAnim;
    
    private void Start()
    {
        anim = leftHand.GetComponent<Animator>();
        handsAnim = hands.GetComponent<Animator>();
    }

    public void StartAnim()
    {
        anim.SetTrigger("Start");
    }
}
