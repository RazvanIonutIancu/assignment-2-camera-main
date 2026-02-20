using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Player : CharacterBody3D
{

	[Export]
    public float Speed = 10.0f;

    [Export]
    public float JumpVelocity = 4.5f;

	[Export]
	public float TurnSpeed = 3.0f;

	[Export]
	Node3D camera;

	public bool isConnectedToCamera = true;



    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		//Vector2 inputDir = Input.GetVector("turn_left", "turn_right", "move_forward", "move_backward");
		//Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		//if (direction != Vector3.Zero)
		//{
		//	velocity.X = direction.X * Speed;
		//	velocity.Z = direction.Z * Speed;
		//}


		float movementInput = Input.GetAxis("move_forward", "move_backward");

		Vector3 forward = Transform.Basis.Z;
		Vector3 direction = forward * movementInput;

		if(Mathf.Abs(movementInput) > 0.01f)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}


        // CUSTOM MOVEMENT
        float turnInput = Input.GetAxis("turn_left", "turn_right");

        if (Input.IsActionPressed("turn_left"))
		{
			RotateY(-turnInput * TurnSpeed * (float)delta);
        }
        if (Input.IsActionPressed("turn_right"))
        {
            RotateY(-turnInput * TurnSpeed * (float)delta);
        }

		//if(Input.IsActionJustPressed("move_forward"))
		//{
		//	velocity += -Transform.Basis.Z * Speed;
		//      }
		//      if (Input.IsActionJustPressed("move_backward"))
		//      {
		//          velocity += Transform.Basis.Z * Speed;
		//      }

		//if(Input.IsActionPressed("move_forward") || Input.IsActionPressed("move_backward"))
		//      {
		//          if (Input.IsActionPressed("move_forward") && velocity.Z <= Speed)
		//          {
		//              velocity += -Transform.Basis.Z * Speed;
		//          }
		//          if (Input.IsActionPressed("move_backward") && velocity.Z >= -Speed)
		//          {
		//              velocity += Transform.Basis.Z * Speed;
		//          }

		//      }
		//else
		//{
		//	velocity = Transform.Basis.Z * 0;
		//}



		















		Velocity = velocity;
		MoveAndSlide();
	}

	private void OnBodyEntered(Node3D body)
    {
        if (body is Player)
        {
            isConnectedToCamera = false;
        }
    }


    private void OnBodyExited(Node3D body)
    {
        if (body is Player)
        {
            isConnectedToCamera = true;
        }
    }


}
