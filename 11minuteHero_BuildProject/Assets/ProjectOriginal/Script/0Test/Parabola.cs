using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    
    public Transform Target;
    public float firingAngle = 45.0f;
    public float velocity;
    public float gravity = 9.8f;
    public Transform obj;

    private Vector3 xDir = Vector3.left + Vector3.forward;
    private Vector3 yDir = Vector3.right + Vector3.forward;
    //public Transform Projectile;
    //private Transform myTransform;

    //void Awake()
    //{
    //    myTransform = transform;
    //    Debug.Log(Mathf.Sin(45 * Mathf.Deg2Rad));
    //    Debug.Log(Mathf.Cos(45 * Mathf.Deg2Rad));
    //    Debug.Log(Mathf.Tan(45 * Mathf.Deg2Rad));
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SimulateProjectile());
        }
        obj.Rotate(transform.forward * 360 * Time.deltaTime);
    }
    IEnumerator SimulateProjectile()
    {
        // Short delay added before Projectile is thrown
        yield return new WaitForSeconds(1.5f);

        // Calculate the velocity needed to throw the object to the target at specified angle.

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        Debug.Log(Vx);
        Debug.Log(Vy);
        float elapse_time = 0;

        while (elapse_time < 4)
        {
            transform.position += xDir * 1 * Time.deltaTime;
            transform.position += yDir * (5f - (gravity * elapse_time)) * Time.deltaTime;
            //transform.Translate((2.5f - (gravity * elapse_time)) * Time.deltaTime, 0,(2.5f - (gravity * elapse_time)) * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }
    //IEnumerator SimulateProjectile()
    //{
    //    // Short delay added before Projectile is thrown
    //    yield return new WaitForSeconds(1.5f);

    //    // Move projectile to the position of throwing object + add some offset if needed.
    //    Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

    //    // Calculate distance to target
    //    float target_Distance = Vector3.Distance(Projectile.position, Target.position);

    //    // Calculate the velocity needed to throw the object to the target at specified angle.
    //    float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

    //    // Extract the X  Y componenent of the velocity
    //    float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
    //    float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

    //    // Calculate flight time.
    //    float flightDuration = target_Distance / Vx;

    //    Debug.Log(flightDuration);

    //    // Rotate projectile to face the target.
    //    Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);

    //    float elapse_time = 0;

    //    while (elapse_time < flightDuration)
    //    {
    //        Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

    //        elapse_time += Time.deltaTime;

    //        yield return null;
    //    }
    //}
}
