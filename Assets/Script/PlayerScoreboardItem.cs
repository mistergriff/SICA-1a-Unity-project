using UnityEngine;
using UnityEngine.UI;
public class PlayerScoreboardItem : MonoBehaviour
{
    [SerializeField] private Text usernameText;
    [SerializeField] private Text killsText;
    [SerializeField] private Text deathText;

    public void Setup(Player player)
    {
        usernameText.text = player.name;
        killsText.text = "Kills : " + player.kills;
        deathText.text = "Deaths : " + player.deaths;
    }
}
