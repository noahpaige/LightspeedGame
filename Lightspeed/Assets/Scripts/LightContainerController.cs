using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightContainerController : MonoBehaviour
{
    public CircleCollider2D col;
    [Range(0f, 1f)] public float collectedScaleFactor = 0.5f;
    public Transform playerTransform;

    private float colRadius = 0f;
    private Vector3[] toPositions;
    private Vector3[] fromPositions;
    private List<GameObject> lights;
    private float time;
    private bool lightsStillArranging = false;

    private GameObject mostRecentlyCollected;

    // Start is called before the first frame update
    void Start()
    {
        lights = new List<GameObject>();
        colRadius = col.radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lightsStillArranging) ArrangeLights();
        else
        {
            Decay();
        }
        FollowPlayer();
    }

    private Vector3[] GetRadialDestinationPositions(int n, float radius)
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

    private void ArrangeLights()
    {
        bool moved = false;
        
        for (int i = 0; i < lights.Count; i++)
        {
            GameObject light = lights[i];
            float scale = CalculateScale();
            float modifiedTime = time * light.GetComponent<LightController>().moveSpeed;
            if (modifiedTime < 1f)
            {
                //arrange lights
                Vector3 toPosition = new Vector3(toPositions[i].x + playerTransform.position.x, toPositions[i].y + playerTransform.position.y, light.transform.position.z);
                light.transform.position = Vector3.Lerp(fromPositions[i], toPosition, modifiedTime);
                moved = true;

                //reduce size of rays and trail particles
                GameObject rays = light.GetComponent<LightController>().rays;
                GameObject trails = light.GetComponent<LightController>().trails;
                Vector3 newScale = Vector3.Lerp(Vector3.one, new Vector3(scale, scale, 1), time);
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
        
        if (!moved)
        {
            lightsStillArranging = false;
        }
    }

    private void FollowPlayer()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            Vector3 desiredPos = toPositions[i] + playerTransform.position;
            lights[i].transform.position = Vector3.Lerp(lights[i].transform.position, desiredPos, lights[i].GetComponent<LightController>().moveSpeed / 2f);
        }
    }

    private void Decay()
    {
        if (mostRecentlyCollected != null)
        {
            float scale = CalculateScale();
            scale *= mostRecentlyCollected.GetComponent<LightController>().GetDecayAmount();
            //reduce size of rays and trail particles
            GameObject rays = mostRecentlyCollected.GetComponent<LightController>().rays;
            GameObject trails = mostRecentlyCollected.GetComponent<LightController>().trails;
            Vector3 newScale = new Vector3(scale, scale, 1);
            rays.transform.localScale = newScale;
            trails.transform.localScale = newScale;

            mostRecentlyCollected.GetComponent<LightController>().DecayUpdate();
        }
    }

    private float CalculateScale()
    {
        return collectedScaleFactor + (1f - collectedScaleFactor) * (1f / Mathf.Pow(1.5f, lights.Count - 1.0f));
    }

    public void AddLight(GameObject addme)
    {
        lights.Add(addme);
        mostRecentlyCollected = addme;
        lightsStillArranging = true;
        toPositions = GetRadialDestinationPositions(lights.Count, colRadius);
        time = 0f;
        Vector3[] from = new Vector3[lights.Count];
        for (int i = 0; i < lights.Count; i++)
        {
            from[i] = lights[i].transform.position;
        }
        fromPositions = from;
    }

    public void RemoveLight(GameObject light)
    {
        lights.Remove(light);
        toPositions = GetRadialDestinationPositions(lights.Count, colRadius);
        lightsStillArranging = true;
        time = 0f;
        Vector3[] from = new Vector3[lights.Count];
        for (int i = 0; i < lights.Count; i++)
        {
            from[i] = lights[i].transform.position;
        }
        fromPositions = from;
        if (lights.Count > 0) mostRecentlyCollected = lights[lights.Count - 1];
        else mostRecentlyCollected = null;
    }

    public List<GameObject> GetLights()
    {
        return lights;
    }
    public void ClearLights()
    {
        lights.Clear();
    }

    public int GetLightCount() { return lights.Count; }

    public float GetLightMovementFactor()
    {
        if (lights.Count <= 0) return 0;
        else if (lightsStillArranging)
        {
            return lights.Count * CalculateScale();
        }
        else
        {
            float factor = (lights.Count - 1) * CalculateScale();
            return factor += mostRecentlyCollected.GetComponent<LightController>().rays.transform.localScale.x;
        }

    }
}
