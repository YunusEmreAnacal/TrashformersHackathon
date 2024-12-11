using System.Collections.Generic;
using UnityEngine;
public class CarMovement : MonoBehaviour
{
    public List<Transform> waypoints; // Waypointlerin listesi
    public float speed = 5f; // Ara�lar�n h�z�
    public int startingWaypointIndex = 0; // Arac�n ba�layaca�� waypoint
    private int currentWaypointIndex; // Mevcut hedef waypoint

    void Start()
    {
        // Arac� ba�lang�� waypoint�ine yerle�tir
        currentWaypointIndex = startingWaypointIndex;
        transform.position = waypoints[currentWaypointIndex].position;
    }

    void Update()
    {
        if (waypoints.Count == 0) return; // Waypoint yoksa ��k

        // Mevcut waypoint'e do�ru hareket
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Waypoint'e ula��ld� m�?
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count; // Sonra ki waypoint�e ge� veya d�ng�y� ba�lat
        }

        // Y�n� waypoint�e do�ru �evir
        transform.LookAt(targetWaypoint);
    }
}
