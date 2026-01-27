using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AerodynamicObject : MonoBehaviour
{
    public float airDensity = 1.225f;
    public float diameter = 0.22f;
    public float dragCoefficient = 0.47f;
    public float angularDragScale = 0.01f;

    private Rigidbody rb;
    private float area;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        diameter = transform.localScale.x;
        float radius = diameter / 2f;
        area = Mathf.PI * radius * radius;
    }

    void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;
        Vector3 angularVelocity = rb.angularVelocity;
        float speed = velocity.magnitude;
        if (speed < Mathf.Epsilon)
        {
            return;
        }

        // drag force
        // Formula is Fd = 1/2 * rho * v^2 * A * Cd
        // Direction is opposite to velocity
        float dragForceMagnitude = 0.5f * airDensity * (speed * speed) * area * dragCoefficient;
        Vector3 dragForce = -velocity.normalized * dragForceMagnitude;
        rb.AddForce(dragForce);

        // magnus force
        // Formula is Fm = 1/2 * rho * v^2 * A * Cl
        // Direction cross product (spinAxis x velocity)
        float surfaceSpeed = angularVelocity.magnitude * (diameter / 2f);
        float spinRatio = surfaceSpeed / speed;
        float liftCoefficient = GetLiftCoefficient(spinRatio);

        float magnusForceMagnitude = 0.5f * airDensity * (speed * speed) * area * liftCoefficient;
        Vector3 magnusDirection = Vector3.Cross(angularVelocity, velocity).normalized;

        if (!float.IsNaN(magnusDirection.x))
        {
            Vector3 magnusForce = magnusDirection * magnusForceMagnitude;
            rb.AddForce(magnusForce);
        }

        // angular decay
        // simple model: torque opposite to angular velocity -k * w
        rb.AddTorque(-angularVelocity * angularDragScale);
    }

    float GetLiftCoefficient(float spinRatio)
    {
        if (spinRatio <= 0) return 0f;
        // Curve fit: Cl rises and caps at roughly 0.35 for high spin ratios
        // This is physically more accurate than a simple linear multiplier
        // 0.35 is a typical max Cl for a sphere before turbulence chaos ensues
        return Mathf.Min(0.35f, spinRatio);
    }
}