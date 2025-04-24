using UnityEngine;

public class HealthCollectableBehavior : MonoBehaviour, ICollectableBehavior
{   
    [SerializeField]
    private float healthAmount;
    public void onCollected(GameObject player)
    {
        player.GetComponent<healt>().addHealth(healthAmount);
    }
}
