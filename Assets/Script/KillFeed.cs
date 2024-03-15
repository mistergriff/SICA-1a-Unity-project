using UnityEngine;

public class KillFeed : MonoBehaviour
{
    [SerializeField] GameObject killFeedItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onPlayerKilledCallBack += OnKill;
    }

    public void OnKill(string player, string source)
    {
        GameObject go = Instantiate(killFeedItemPrefab, transform);
        go.GetComponent<KillFeedItem>().Setup(player, source);
        Destroy(go, 5f);
    }

}
