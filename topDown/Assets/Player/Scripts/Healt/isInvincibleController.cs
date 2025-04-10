using System.Collections;
using UnityEngine;

public class isInvincibleController : MonoBehaviour
{
    private healt healt;

    private void Awake()
    {
        healt = GetComponent<healt>();
    }

    public void startInvincibility(float invincibilityDuration)
    {
        StartCoroutine(invincibilityCoroutine(invincibilityDuration));
    }

    private IEnumerator invincibilityCoroutine(float invincibilityDuration)
    {
        healt.isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        healt.isInvincible = false;
    }
}
