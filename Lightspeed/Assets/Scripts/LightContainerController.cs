using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightContainerController : MonoBehaviour
{
    public CircleCollider2D col;
    private float colRadius = 0f;
    private Vector3[] toPositions;
    private Vector3[] fromPositions;
    private List<GameObject> lights;
    private float time;

    private bool lightsStillMoving = false;

    [Range(0f, 1f)] public float collectedSize = 0.5f;
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        lights = new List<GameObject>();
        colRadius = col.radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool moved = false;
        if(lightsStillMoving)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                GameObject light = lights[i];
                if(light.transform.position != toPositions[i])
                {
                    light.transform.position = Vector3.Lerp(fromPositions[i], toPositions[i] + playerTransform.position, time);
                    moved = true;
                }
                if (light.transform.localScale.x > collectedSize)
                {
                    GameObject rays = light.GetComponent<LightColliderController>().rays;
                    rays.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(collectedSize, collectedSize, collectedSize), time);
                }
            }
            time += Time.deltaTime;
        }
        if (!moved) lightsStillMoving = false;
    }

    public void AddLight(GameObject addme)
    {
        lights.Add(addme);
        Debug.Log("COUNT " + lights.Count);
        addme.transform.SetParent(transform.parent);
        toPositions = getPositions(lights.Count, colRadius);
        lightsStillMoving = true;
        time = 0f;
        Vector3[] from = new Vector3[lights.Count];
        for (int i = 0; i < lights.Count; i++)
        {
            from[i] = lights[i].transform.position;
            Debug.Log("ToPosition " + i + " " + toPositions[i]);
        }
        fromPositions = from;
    }

    private Vector3[] getPositions(int n, float radius)
    {
        Vector3[] positions = new Vector3[n];
        if (n < 1) return positions;
        else if(n == 1)
        {
            positions[0] = Vector3.zero;
            positions[0] += col.transform.localPosition;
            return positions;
        }

        float radInterval = 2 * Mathf.PI / n;
        
        for (int i = 0; i < n; i++)
        {
            Vector3 point = new Vector3(radius * Mathf.Cos(i * radInterval), radius * Mathf.Sin(i * radInterval), 0f);
            point = point + col.transform.localPosition;
            positions[i] = point;
        }
        return positions;
    }
}
