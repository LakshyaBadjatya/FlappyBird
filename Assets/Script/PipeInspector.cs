using UnityEngine;

public class PipeInspector : MonoBehaviour
{
    void Update()
    {
        // Press P in Play mode to list current pipes quickly
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("=== Pipe Inspector ===");
            // First try to find objects by tag "Obstacle"
            var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            if (obstacles != null && obstacles.Length > 0)
            {
                Debug.Log($"Found {obstacles.Length} objects with tag 'Obstacle'");
                foreach (var go in obstacles)
                {
                    LogColliderInfo(go);
                }
            }
            else
            {
                Debug.Log("No objects found with tag 'Obstacle'. Searching by name containing 'pipe' (case-insensitive).");
                // Fallback: search all scene objects whose name contains "pipe"
                var allTransforms = FindObjectsOfType<Transform>();
                int count = 0;
                foreach (var t in allTransforms)
                {
                    if (t.name.ToLower().Contains("pipe"))
                    {
                        count++;
                        LogColliderInfo(t.gameObject);
                    }
                }
                Debug.Log($"Found {count} objects with 'pipe' in the name.");
            }
        }
    }

    void LogColliderInfo(GameObject go)
    {
        var col = go.GetComponent<Collider2D>();
        if (col == null) col = go.GetComponentInChildren<Collider2D>();

        string colType = (col != null) ? col.GetType().Name : "NULL";
        string isTrigger = (col != null) ? col.isTrigger.ToString() : "NA";
        Debug.Log($"Object: {go.name} | active: {go.activeInHierarchy} | tag: {go.tag} | collider: {colType} | isTrigger: {isTrigger}");
    }
}
