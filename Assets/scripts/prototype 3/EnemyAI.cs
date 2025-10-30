using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public Combatant playerCombatant;
    public float attackDamage = 10f;

    public float chaseSpeed = 3f;
    public float attackRange = 0.5f;
    public float fieldOfView = 60f;
    public float sightMaxDistance = 50f;
    public float enemyCooldown = 1.5f;
    public bool Attackable = true;

    public Transform playerCamera;
    private Transform playerTransform;

    public LayerMask sightObstructionLayers;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            //playerCamera = Camera.main.transform;
        }

        if (playerCombatant == null && playerObject != null)
        {
            playerCombatant = playerObject.GetComponent<Combatant>();

        }
    }
    public void ExecuteAttack()
    {
        if (playerCombatant != null)
        {
            playerCombatant.TakeDamage(attackDamage);
            Debug.Log($"Enemy {gameObject.name} attacks!");
        }
    }
    void Update()
    {
        if (playerTransform == null || playerCamera == null) return;

        if (IsBeingSeen())
        {
            Debug.Log("frozen");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > attackRange)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * chaseSpeed * Time.deltaTime, Space.World);

            Vector3 targetDirection = new Vector3(direction.x, 0, direction.z);

            if (targetDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }


        if (distanceToPlayer <= attackRange && Attackable == true && !playerCombatant.dead)
        {
            ExecuteAttack();
            StartCoroutine(attackWCoolDown());
        }

        Debug.Log(distanceToPlayer);
    }

    private IEnumerator attackWCoolDown()
    {
        Attackable = false;
        yield return new WaitForSeconds(enemyCooldown);
        Attackable = true;
    }


    private bool IsBeingSeen()
    {
        if (playerCamera == null) return false;

        Vector3 enemyDirection = (transform.position - playerCamera.position).normalized;
        float angle = Vector3.Angle(playerCamera.forward, enemyDirection);

        if (angle < fieldOfView * 0.5f)
        {
            RaycastHit hit;
            float distance = Vector3.Distance(transform.position, playerCamera.position);

            if (Physics.Raycast(playerCamera.position, enemyDirection, out hit, sightMaxDistance, sightObstructionLayers))
            {
                if (hit.collider.transform == transform || hit.collider.transform.root == transform.root)
                {
                    return true;
                }

                if (hit.distance < distance - 0.1f)
                {
                    return false;
                }
            }

            if (distance <= sightMaxDistance)
            {
                return true;
            }
        }
        return false;
    }
}