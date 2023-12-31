using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public Rigidbody2D weaponRigidBody;
    public ConstantForce2D constantForce2d;

    private void Start()
    {
        
    }

    public void Update()
    {

    }

    public void AddTorqueForce(DirectionEnum direction, float torqueForce = 100.0f)
    {
        switch (direction)
        {
            case DirectionEnum.Up:
                break;
            case DirectionEnum.Down:
                break;
            case DirectionEnum.Left:
                weaponRigidBody.AddTorque(-torqueForce);
                break;
            case DirectionEnum.Right:
                weaponRigidBody.AddTorque(torqueForce);
                break;
            default:
                break;
        }
    }

    public void AddConstantRotationForce(DirectionEnum direction,float force = 200.0f)
    {
        switch (direction)
        {
            case DirectionEnum.Up:
                break;
            case DirectionEnum.Down:
                break;
            case DirectionEnum.Left:
                constantForce2d.torque = -force;
                break;
            case DirectionEnum.Right:
                constantForce2d.torque = force;
                break;
            default:
                break;
        }
    }
    public void AddForce(DirectionEnum direction,float force = 175.0f)
    {
        switch (direction)
        {
            case DirectionEnum.Up:
                weaponRigidBody.AddForce(new Vector2(0f, force));
                break;
            case DirectionEnum.Down:
                weaponRigidBody.AddForce(new Vector2(0f, -force));
                break;
            case DirectionEnum.Left:
                weaponRigidBody.AddForce(new Vector2(-force, 0f));
                break;
            case DirectionEnum.Right:
                weaponRigidBody.AddForce(new Vector2(force, 0f));
                break;
            default:
                break;
        }
    }
    
    public void ResetTorque()
    {
        constantForce2d.torque = (constantForce2d.torque > 0) ? -constantForce2d.torque: constantForce2d.torque;
        constantForce2d.force = Vector2.zero;
        constantForce2d.enabled = false;
        weaponRigidBody.velocity = Vector2.zero;
    }
}