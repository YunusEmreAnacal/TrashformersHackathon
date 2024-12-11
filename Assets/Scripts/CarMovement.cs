using System.Collections.Generic;
using UnityEngine;
public class CarMovement : MonoBehaviour
{
    public List<Transform> waypoints; // Waypointlerin listesi
    public float speed = 5f; // Araçlarýn hýzý
    public int startingWaypointIndex = 0; // Aracýn baþlayacaðý waypoint
    private int currentWaypointIndex; // Mevcut hedef waypoint

    void Start()
    {
        // Aracý baþlangýç waypoint’ine yerleþtir
        currentWaypointIndex = startingWaypointIndex;
        transform.position = waypoints[currentWaypointIndex].position;
    }

    void Update()
    {
        if (waypoints.Count == 0) return; // Waypoint yoksa çýk

        // Mevcut waypoint'e doðru hareket
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Waypoint'e ulaþýldý mý?
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count; // Sonra ki waypoint’e geç veya döngüyü baþlat
        }

        // Yönü waypoint’e doðru çevir
        transform.LookAt(targetWaypoint);
    }
}
