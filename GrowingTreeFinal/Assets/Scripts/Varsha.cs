using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Varsha : MonoBehaviour
{
    Animator animator;
    private ParticleSystem baarish;
    private void Start()
    {
        animator = GetComponent<Animator>();
        baarish= GetComponentInChildren<ParticleSystem>();
    }
    public void baarishAja()
    {
        baarish.Play();
        StartCoroutine(returnClouds());
    }
    public void destroyClouds()
    {
        Destroy(transform.parent.gameObject);
    }
    IEnumerator returnClouds()
    {
        yield return new WaitForSeconds(baarish.main.duration);
        animator.SetTrigger("Return");
    }
   

}
