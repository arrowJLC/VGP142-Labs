using System.Collections;
using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    public Transform player;
    private MeshRenderer mr;
    public Renderer enemyRenderer;
    public float fieldOfViewAngle = 45f; 
    public float maxSightDistance = 20f; 
    private Material enemyMaterial;
    public float detectionRadius = 10f; 
    public float invisibilityDistance = 5f; 
    private bool enemyLookingAtPlayer = false;

    void Start()
    {
        
        enemyRenderer = GetComponent<Renderer>();
        enemyMaterial = enemyRenderer.material;

        
        StartCoroutine(CheckPlayerProximity());
    }

    void Update()
    {
        
        Vector3 toPlayer = player.position - transform.position;

        
        float angle = Vector3.Angle(transform.forward, toPlayer);
        RaycastHit hit;
        bool isPlayerVisible = angle < fieldOfViewAngle &&
                               Physics.Raycast(transform.position, toPlayer.normalized, out hit, maxSightDistance) &&
                               hit.transform == player;

        if (isPlayerVisible)
        {
            enemyRenderer.enabled = true;
            SetTransparency(1f); 
        }
        else
        {
            enemyRenderer.enabled = true;
            SetTransparency(0f); // Ft
        }
    }

    private IEnumerator CheckPlayerProximity()
    {
        while (true)
        {
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                
                enemyLookingAtPlayer = IsPlayerInFrontOfEnemy();

                if (distanceToPlayer <= invisibilityDistance && !enemyLookingAtPlayer)
                {
                    SetEnemyVisibility(false); 
                }
                else
                {
                    SetEnemyVisibility(true); 
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private bool IsPlayerInFrontOfEnemy()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);
        return dotProduct > 0.5f; 
    }

    private void SetTransparency(float alpha)
    {
        Color color = enemyMaterial.color;
        color.a = alpha;
        enemyMaterial.color = color;
    }

    private void SetEnemyVisibility(bool visible)
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.enabled = visible;
        }
    }
}
