using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject freshCornPrefab;
    public GameObject rottenCornPrefab;

    [Header("Spawn Area (world units)")]
    public Transform areaCenter;
    public Vector2 areaSize = new Vector2(10f, 6f);

    [Header("Spawn Control")]
    public int maxCorns = 6;
    public float spawnInterval = 1.2f;
    [Range(0f, 1f)] public float freshChance = 0.7f;

    readonly List<GameObject> _active = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            CleanupList();

            if (_active.Count >= maxCorns) continue;

            Vector2 half = areaSize * 0.5f;
            Vector3 basePos = areaCenter != null ? areaCenter.position : transform.position;
            Vector3 pos = basePos + new Vector3(
                Random.Range(-half.x, half.x),
                Random.Range(-half.y, half.y),
                0f
            );

            GameObject prefab = (Random.value <= freshChance) ? freshCornPrefab : rottenCornPrefab;
            if (prefab == null) continue;

            var go = Instantiate(prefab, pos, Quaternion.identity);
            _active.Add(go);
        }
    }

    void CleanupList()
    {
        for (int i = _active.Count - 1; i >= 0; i--)
            if (_active[i] == null) _active.RemoveAt(i);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 c = areaCenter ? areaCenter.position : transform.position;
        Gizmos.DrawWireCube(c, new Vector3(areaSize.x, areaSize.y, 0.1f));
    }
}
