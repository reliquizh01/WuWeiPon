using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public Rigidbody2D weaponRigidBody;

    public WeaponBehavior weaponBehaviorRef;
    public ConstantForce2D constantForce2d;

    private void Start()
    {
        
    }

    public void Update()
    {

    }

    public void AddRotationalForce(DirectionEnum direction,float force = 175.0f)
    {
        constantForce2d.enabled = true;
        
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
                weaponRigidBody.AddForce(new Vector2(force, 0f));
                break;
            case DirectionEnum.Down:
                weaponRigidBody.AddForce(new Vector2(-force, 0f));
                break;
            case DirectionEnum.Left:
                weaponRigidBody.AddForce(new Vector2(0f, force));
                break;
            case DirectionEnum.Right:
                weaponRigidBody.AddForce(new Vector2(0f, -force));
                break;
            default:
                break;
        }
    }
}