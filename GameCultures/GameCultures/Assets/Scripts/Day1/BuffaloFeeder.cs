using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class BuffaloFeeder : MonoBehaviour
{

    [Header("References")]
    public Animator animator;
    public AudioSource audioSource;      // 🔊 Audio source on buffalo
    public AudioClip eatClip;            // one sound for both normal & rotten corn

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        var corn = other.GetComponent<DraggableCorn>();
        if (corn != null && corn.IsBeingCarried && !corn.consumed)
        {
            BuffaloStats.Instance.FeedCorn(corn);

            // Play eating animation depending on corn type
            if (corn.isFresh)
            {
                animator.SetBool("isEatingHappy", true);
                animator.SetBool("isEatingYuck", false);
            }
            else
            {
                animator.SetBool("isEatingHappy", false);
                animator.SetBool("isEatingYuck", true);
            }

            // Play eating sound (same for both)
            PlaySound(eatClip);

            corn.MarkConsumed();

            // Reset flags after short delay
            StartCoroutine(ResetEatingFlags());
        }
    }

    private IEnumerator ResetEatingFlags()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("isEatingHappy", false);
        animator.SetBool("isEatingYuck", false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}