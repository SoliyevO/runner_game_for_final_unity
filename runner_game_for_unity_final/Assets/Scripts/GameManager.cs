using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController playerController; // O'yinchini boshqaruvchi skript

    void Update()
    {
        // O'yinni boshlash uchun foydalanuvchi ekranga bosadi
        if (!playerController.isGameStarted && Input.GetMouseButtonDown(0))
        {
            playerController.StartGame(); // PlayerController ichidagi o'yinni boshlash funksiyasini chaqirish
            Debug.Log("O'yin boshlandi!"); // O'yinni boshlash logi
        }
    }
}
