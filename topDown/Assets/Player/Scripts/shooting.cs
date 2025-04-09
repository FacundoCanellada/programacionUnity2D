using UnityEngine;
using System.Collections.Generic;
public class shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;

    public GameObject bulletprefab;
    public Transform firepoint;
    
    private float firecooldown = 0.25f;
    private float firetimer = 0f;

    public int poolSize = 5;
    private GameObject[] bulletPool;
    private void Start()
    {
       // mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
       mainCam = Camera.main;
       if (bulletprefab == null)
       {
           Debug.LogError("El prefab de la bala no está asignado en el Inspector.");
           this.enabled = false;
           return;
       }
        bulletPool = new GameObject[poolSize];
        float bulletLifetime = firecooldown * poolSize - 0.01f;
        for (int i = 0; i < poolSize; i++)
        {
            bulletPool[i] = Instantiate(bulletprefab, firepoint.position, firepoint.rotation);
            bulletPool[i].SetActive(false);
            bulletPool[i].GetComponent<Bullet>().SetLifetime(bulletLifetime);
        }
      
    }

    private void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        firetimer += Time.deltaTime;

        if (firetimer >= firecooldown)
        {
            Fire();
            firetimer = 0f;
            Debug.Log("fuego");

        }

    }

    void Fire()
    {
        GameObject bullet = GetAvaibleBullet();
        if (bullet!= null)
        {
            
            bullet.transform.position = firepoint.position;
            bullet.transform.rotation = firepoint.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().Launch();
        }
    }

    GameObject GetAvaibleBullet()
    {
        foreach (GameObject b in bulletPool)
        {
            if (b != null && !b.activeInHierarchy)
            {
                return b;
            }
               
        }
        return null;
    }

}
