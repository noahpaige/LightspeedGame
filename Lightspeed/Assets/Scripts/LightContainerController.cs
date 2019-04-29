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

    [Range(0f, 1f)] public float collectedScaleFactor = 0.5f;
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
        bool lightsStillMoving = ArrangeLights();
        if(!lightsStillMoving) FollowPlayer();
    }

    public void AddLight(GameObject addme)
    {
        lights.Add(addme);
        //addme.transform.SetParent(transform.parent);
        toPositions = getPositions(lights.Count, colRadius);
        lightsStillMoving = true;
        time = 0f;
        Vector3[] from = new Vector3[lights.Count];
        for (int i = 0; i < lights.Count; i++)
        {
            from[i] = lights[i].transform.position;
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
        float oddOffset = Mathf.PI / n / 2 * (n % 2);

        for (int i = 0; i < n; i++)
        {
            Vector3 point = new Vector3(radius * Mathf.Cos(i * radInterval + oddOffset), radius * Mathf.Sin(i * radInterval + oddOffset), 0f);
            point = point + col.transform.localPosition;
            positions[i] = point;
            //Debug.Log("Point " + i + ": " + point);
        }
        return positions;
    }

    private bool ArrangeLights()
    {
        bool moved = false;
        if (lightsStillMoving)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                GameObject light = lights[i];
                float scale = collectedScaleFactor + (1f - collectedScaleFactor) * (1f / Mathf.Pow(1.5f, lights.Count - 1.0f));
                float modifiedTime = time * light.GetComponent<LightController>().moveSpeed;
                if (modifiedTime < 1f)
                {
                    //arrange lights
                    light.transform.position = Vector3.Lerp(fromPositions[i], toPositions[i] + playerTransform.position, modifiedTime);
                    moved = true;

                    //reduce size of rays and trail particles
                    GameObject rays = light.GetComponent<LightController>().rays;
                    GameObject trails = light.GetComponent<LightController>().trails;
                    Vector3 newScale = Vector3.Lerp(Vector3.one, new Vector3(scale, scale, scale), time);
                    rays.transform.localScale = newScale;
                    trails.transform.localScale = newScale;

                    //reduce alpha
                    ParticleSystem.MainModule settings = rays.GetComponent<ParticleSystem>().main;
                    Color reducedAlpha = new Color(settings.startColor.color.r, settings.startColor.color.g, settings.startColor.color.b, scale);
                    //settings.startColor = new ParticleSystem.MinMaxGradient(reducedAlpha);
                    settings.startColor = reducedAlpha;
                }
            }
            time += Time.deltaTime;
        }
        if (!moved)
        {
            lightsStillMoving = false;
        }
        return lightsStillMoving;
    }

    private void FollowPlayer()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            Vector3 desiredPos = toPositions[i] + playerTransform.position;
            lights[i].transform.position = Vector3.Lerp(lights[i].transform.position, desiredPos, lights[i].GetComponent<LightController>().moveSpeed / 2f);
        }
    }

    public List<GameObject> GetLights()
    {
        return lights;
    }
    public void ClearLights()
    {
        lights.Clear();
    }
}
