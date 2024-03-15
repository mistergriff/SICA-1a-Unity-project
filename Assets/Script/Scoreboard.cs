using UnityEngine;

public class Scoreboard : MonoBehaviour
{

    [SerializeField] private GameObject playerScoreboardItem;
    [SerializeField] private Transform playerScoreboardList;

    private void OnEnable()
    {
        // Récupérer une array de tous les joueurs du serveur
        Player[] players = GameManager.GetAllPlayers();

        // Loop sur l'array et mise en place d'une ligne d'UI pour chaque joueurs + remplissage des données
        foreach (Player player in players)
        {
            GameObject itemGo = Instantiate(playerScoreboardItem, playerScoreboardList);
            PlayerScoreboardItem item = itemGo.GetComponent<PlayerScoreboardItem>();
            if(item != null )
            {
                item.Setup(player);
            }
        }
    }

    private void OnDisable()
    {
        // Vider / nettoyer la liste des joueurs
        foreach (Transform child in playerScoreboardList)
        {
            Destroy(child.gameObject);
        }
    }
}
