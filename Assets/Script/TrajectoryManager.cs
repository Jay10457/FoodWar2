using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class TrajectoryManager : MonoBehaviour
{
    
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] bool showTrajectory;
    [SerializeField] bool showTrajectoryAlways;

    [SerializeField] int archLineCount;
    [SerializeField] float archCalcInterval;
    [SerializeField] float archHeightLimit;

    [Range(0, 90)]
    public float throwAngle;
    public float shootRange;

    [SerializeField] GameObject ground;
    [SerializeField] Transform launchPoint;

    [Header("LineSetting")]
    [SerializeField] float startLineWidth;
    [SerializeField] float endLineWidth;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [Range(0, 1)]
    [SerializeField] float startAlpha;
    [Range(0, 1)]
    [SerializeField] float endAlpha;

    float spdVec;
    Vector3 launchVec;
    public LineRenderer line;

    private enum ArchLimit
    {
        Height = 0,
        Time = 1
    }
    private void Start()
    {
        Init();
        InitVariable();
    }

    private void Init()
    {
        line = GetComponent<LineRenderer>();
        ground = GameObject.FindGameObjectWithTag("Floor");
        line.startWidth = startLineWidth;
        line.endWidth = endLineWidth;
       
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {new GradientColorKey(endColor, 0.0f), new GradientColorKey(startColor, 1.0f)},
            new GradientAlphaKey[] {new GradientAlphaKey(endAlpha, 1.0f), new GradientAlphaKey(startAlpha, 1.0f)}
            );
        line.colorGradient = gradient;
    }

    private void InitVariable()
    {
        showTrajectory = true;
        showTrajectoryAlways = true;

        archLineCount = 50;
        archCalcInterval = 0.2f;
        archHeightLimit = ground.transform.position.y;
        shootRange = 150f;

        startLineWidth = 0.3f;
        endLineWidth = 0.2f;

        startColor = Color.red;
        endColor = Color.cyan;

        startAlpha = 0.8f;
        endAlpha = 0.5f;

        throwAngle = 30f;

    }

    public void ShootObj(GameObject shootObj, Vector3 hitPos)
    {
        CheckVector(hitPos);
        GameObject obj = Instantiate(shootObj, launchPoint.transform.position, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Vector3 force = launchVec * rb.mass;
        rb.AddForce(force, ForceMode.Impulse);
        line.positionCount = 0;
    }

    public void CheckVector(Vector3 hitPos)
    {
        spdVec = CalculateVectorFromAngle(hitPos, throwAngle);
        if (spdVec <= 0.0)
        {
            return;
        }
        launchVec = CovertVectorToVector3(spdVec, throwAngle, hitPos);
        if (showTrajectory)
        {
            DisplayTrajectory(hitPos);
            //Debug.LogError("display");
        }
       
        
    }

    private float CalculateVectorFromAngle(Vector3 pos, float angle)
    {
        Vector2 shootPos = new Vector2(launchPoint.transform.position.x, launchPoint.transform.position.z);
        Vector2 hitPos = new Vector2(pos.x, pos.z);
        float x = Vector2.Distance(shootPos, hitPos);
        float g = Physics.gravity.y;
        float y0 = launchPoint.transform.position.y;
        float y = pos.y;
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float tan = Mathf.Tan(rad);

        float v0Sq = g * x * x / (2 * cos * cos * (y - y0 - x * tan));
        if (v0Sq <= 0.0f)
        {
            return 0.0f;

        }
        return Mathf.Sqrt(v0Sq);
    }

    private Vector3 CovertVectorToVector3(float spdVec, float angle, Vector3 pos)
    {
        Vector3 launchPos = launchPoint.transform.position;
        Vector3 hitPos = pos;
        launchPos.y = 0;
        hitPos.y = 0;

        Vector3 dir = (hitPos - launchPos).normalized;
        Quaternion Rot3D = Quaternion.FromToRotation(Vector3.right, dir);
        Vector3 vec = spdVec * Vector3.right;
        vec = Rot3D * Quaternion.AngleAxis(angle, Vector3.forward) * vec;

        return vec;
    }

   
    private void DisplayTrajectory(Vector3 hitPos)
    {
        float x;
        float y = launchPoint.transform.position.y;
        float y0 = y;
        float g = Physics.gravity.y;
        float rad = throwAngle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        float time;

        List<Vector3> archVerts = new List<Vector3>();
        Vector3 shootPos3 = launchPoint.transform.position;
        hitPos.y = shootPos3.y;
        Vector3 dir = (hitPos - shootPos3).normalized;
        float spd = spdVec;
        Quaternion yawRot = Quaternion.FromToRotation(Vector3.right, dir);
       

        for (int i = 0; y > archHeightLimit; i++)
        {
            time = archCalcInterval * i;
            x = spd * cos * time;
            y = spd * sin * time + y0 + g * time * time / 2;
            archVerts.Add(new Vector3(x, y, 0));
            archVerts[i] = yawRot * archVerts[i];
            archVerts[i] = new Vector3(archVerts[i].x + shootPos3.x, archVerts[i].y, archVerts[i].z + shootPos3.z);

           
        }
        int lineLength = archLineCount;
        archVerts.Reverse();
        if (archVerts.Count < lineLength)
        {
            lineLength = archVerts.Count;
        }

        line.startWidth = endLineWidth;
        line.endWidth = startLineWidth;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(endColor, 0.0f), new GradientColorKey(startColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(endAlpha, 0.0f), new GradientAlphaKey(startAlpha, 1.0f) }
        );
        line.colorGradient = gradient;
        line.positionCount = archVerts.Count - (archVerts.Count - lineLength);
        line.SetPositions(archVerts.ToArray());
        line.useWorldSpace = true;

    }
}
