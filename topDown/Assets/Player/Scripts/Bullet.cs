using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetLifetime(float time)
    {
        lifetime = time;
    }
    public void Launch()
    {
        StopAllCoroutines();
        StartCoroutine(MoveAndReturn());
    }

    private IEnumerator MoveAndReturn()
    {
        float timer = 0f;
        while (timer < lifetime)
        {
            transform.position += transform.right * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
    // Update is called once per frame
  
}
