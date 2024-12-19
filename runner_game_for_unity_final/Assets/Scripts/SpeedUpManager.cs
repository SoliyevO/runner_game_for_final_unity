using UnityEngine;

public class SpeedUpManager : MonoBehaviour
{
    private PlayerController playerController;
    private SpeedUp[] speedUps;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (playerController.IsMaxSpeedReached())
        {
            // Sahnadagi barcha SpeedUp obyektlarini o‘chirib tashlash
            speedUps = FindObjectsOfType<SpeedUp>();
            foreach (var speedUp in speedUps)
            {
                Destroy(speedUp.gameObject);
            }
        }
    }
}
