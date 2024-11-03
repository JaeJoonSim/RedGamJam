using BlueRiver.UI;
using BlueRiver;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerDeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound("Death");
            GameManager.Instance.playerDeath = true;
            PopupManager.ShowPopup<UI_Popup>("Popup Death");
        }
    }
}
