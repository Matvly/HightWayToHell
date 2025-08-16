using Unity.VisualScripting;
using UnityEngine;

public class HandsController : MonoBehaviour
{
    [SerializeField]  private Animator anim;
    [SerializeField]  private Transform leftHand;
    [SerializeField] private Transform rightHand;

    public Transform LeftHand => leftHand;
    public Transform RightHand => rightHand;

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
