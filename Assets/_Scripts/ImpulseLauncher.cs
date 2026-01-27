using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class ImpulseLauncher : MonoBehaviour
{
    public float kickPower = 25f;
    public float verticalAngle = 10f;
    public float sideSpin = 50f;
    public float topSpin = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyImpulse();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void ApplyImpulse()
    {
        Vector3 launchDir = Quaternion.Euler(-verticalAngle, 0, 0) * transform.forward;
        Vector3 finalKickForce = launchDir * kickPower;
        rb.AddForce(finalKickForce, ForceMode.Impulse);
        Vector3 spinVector = new Vector3(topSpin, sideSpin, 0);
        rb.maxAngularVelocity = 1000f;
        rb.AddTorque(spinVector, ForceMode.Impulse);
    }
}