using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour
{
    [SerializeField] Text text;

    public void Setup(string player, string source)
    {
        text.text = source + " Killed " + player;
    }

    public void Join(string player)
    {
        text.text = player + " joined the game";
    }
}
