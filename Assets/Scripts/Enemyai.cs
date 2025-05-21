using UnityEngine;
using UnityEngine.AI;

public class Enemyai : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;  // Punkty patrolowe
    int waypointIndex;  // Indeks bie¿¹cego punktu patrolowego
    Vector3 target;  // Docelowa pozycja, do której przeciwnik zmierza

    public string playerTag = "Player1";  // Tag gracza
    public float chaseSpeed = 5f;        // Szybkoœæ poœcigu
    public float normalSpeed = 3.5f;     // Normalna szybkoœæ (patrol)
    public float detectionRange = 10f;   // Zasiêg wykrywania gracza

    private Transform player;  // Transform gracza

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;  // Pocz¹tkowa szybkoœæ

        if (agent != null && agent.isActiveAndEnabled)
        {
            UpdateDestination();  // Zaktualizuj cel do pierwszego punktu patrolu
        }
        else
        {
            Debug.LogError("NavMeshAgent not assigned or not active.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Jeœli nie ma gracza, szukaj go
        if (player == null)
        {
            SearchForPlayer();
        }

        // Jeœli gracz zosta³ wykryty, goni go
        if (player != null)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();  // Jeœli gracz nie zosta³ wykryty, patroluj
        }
    }

    // Szuka gracza w zasiêgu
    void SearchForPlayer()
    {
        // SprawdŸ, czy w zasiêgu jest gracz
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(playerTag))  // SprawdŸ, czy trafiony obiekt to gracz
            {
                player = hit.transform;  // Przypisz transform gracza
                agent.speed = chaseSpeed; // Zwiêksz prêdkoœæ poœcigu
                Debug.Log("Gracz wykryty!");
                break;
            }
        }
    }

    // Goni gracza
    void ChasePlayer()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);  // Ustaw cel na pozycjê gracza

            // Jeœli przeciwnik jest blisko gracza
            if (Vector3.Distance(transform.position, player.position) < 1.5f)
            {
                Debug.Log("Po¿ar³a jak zwyk³ego fast fooda");  // Wyœwietl komunikat, gdy przeciwnik z³apie gracza
            }
        }
    }

    // Patrolowanie
    void Patrol()
    {
        if (waypoints.Length > 0)
        {
            // Jeœli przeciwnik dotrze do punktu patrolowego
            if (Vector3.Distance(transform.position, target) < 1f)
            {
                IterateWaypointIndex();  // PrzejdŸ do nastêpnego punktu
                UpdateDestination();  // Zaktualizuj cel
            }
        }
    }

    // Ustawienie nowego celu (punktu) do patrolowania
    void UpdateDestination()
    {
        if (waypoints.Length > 0)
        {
            target = waypoints[waypointIndex].position;  // Ustaw cel na punkt patrolowy
            agent.SetDestination(target);  // Ustaw cel nawigacji
        }
    }

    // Iteracja przez punkty patrolowe w pêtli
    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;  // Wracaj do pierwszego punktu po ostatnim
        }
    }
}
