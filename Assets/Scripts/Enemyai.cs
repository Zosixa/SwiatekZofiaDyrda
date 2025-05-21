using UnityEngine;
using UnityEngine.AI;

public class Enemyai : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;  // Punkty patrolowe
    int waypointIndex;  // Indeks bie��cego punktu patrolowego
    Vector3 target;  // Docelowa pozycja, do kt�rej przeciwnik zmierza

    public string playerTag = "Player1";  // Tag gracza
    public float chaseSpeed = 5f;        // Szybko�� po�cigu
    public float normalSpeed = 3.5f;     // Normalna szybko�� (patrol)
    public float detectionRange = 10f;   // Zasi�g wykrywania gracza

    private Transform player;  // Transform gracza

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;  // Pocz�tkowa szybko��

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
        // Je�li nie ma gracza, szukaj go
        if (player == null)
        {
            SearchForPlayer();
        }

        // Je�li gracz zosta� wykryty, goni go
        if (player != null)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();  // Je�li gracz nie zosta� wykryty, patroluj
        }
    }

    // Szuka gracza w zasi�gu
    void SearchForPlayer()
    {
        // Sprawd�, czy w zasi�gu jest gracz
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(playerTag))  // Sprawd�, czy trafiony obiekt to gracz
            {
                player = hit.transform;  // Przypisz transform gracza
                agent.speed = chaseSpeed; // Zwi�ksz pr�dko�� po�cigu
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
            agent.SetDestination(player.position);  // Ustaw cel na pozycj� gracza

            // Je�li przeciwnik jest blisko gracza
            if (Vector3.Distance(transform.position, player.position) < 1.5f)
            {
                Debug.Log("Po�ar�a jak zwyk�ego fast fooda");  // Wy�wietl komunikat, gdy przeciwnik z�apie gracza
            }
        }
    }

    // Patrolowanie
    void Patrol()
    {
        if (waypoints.Length > 0)
        {
            // Je�li przeciwnik dotrze do punktu patrolowego
            if (Vector3.Distance(transform.position, target) < 1f)
            {
                IterateWaypointIndex();  // Przejd� do nast�pnego punktu
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

    // Iteracja przez punkty patrolowe w p�tli
    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;  // Wracaj do pierwszego punktu po ostatnim
        }
    }
}
