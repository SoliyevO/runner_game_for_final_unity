using UnityEngine;
using DG.Tweening;

public class SpeedUp : MonoBehaviour
{
    public float speedIncrease = 5f; // Tezlikni oshirish miqdori
    public float boostDuration = 5f; // Boost qancha vaqt davom etadi

    void Start()
    {
        // Ob'ektni cheksiz aylantirish
        transform.DORotate(new Vector3(0, 180, 0), 40f)
            .SetLoops(-1, LoopType.Incremental);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Agar forwardSpeed allaqachon maxSpeed'ga teng yoki undan katta bo'lsa
                if (playerController.forwardSpeed >= playerController.maxSpeed)
                {
                    Destroy(gameObject); // SpeedUp ob'ektini hech narsa qilmasdan yo'q qilamiz
                    return; // Funktsiyani yakunlaymiz
                }

                // Tezlikni vaqtinchalik oshirish
                playerController.IncreaseSpeedTemporarily(speedIncrease, boostDuration);
                Destroy(gameObject); // SpeedUp obyektini yo‘q qilish
            }
        }
    }
}
