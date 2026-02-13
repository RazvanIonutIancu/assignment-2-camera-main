using Godot;
using System;

public partial class Camera : Node3D
{
    [Export]
    Node3D CameraOffset;

    [Export]
    Node3D CameraTilt;

    [Export]
    Node3D CameraYaw;

    [Export]
    Node3D CameraZoom;

    [Export]
    CharacterBody3D Player;

    [Export]
    Node3D CameraPosition;



    [Export]
    public RayCast3D LeftRay1;
    [Export]
    public RayCast3D LeftRay2;
    [Export]
    public RayCast3D LeftRay3;

    [Export]
    public RayCast3D CameraRay;

    [Export]
    public RayCast3D RightRay1;
    [Export]
    public RayCast3D RightRay2;
    [Export]
    public RayCast3D RightRay3;




    float rotationLimit = Mathf.DegToRad(5.0f);
    float rotationY;
    float rotationYSpeed = 0.3f;
    float rotationMax = 0.1f;
    float maxZoom = 15.0f;
    float minZoom = 1.0f;
    float cameraOffset = 0f;


    public override void _PhysicsProcess(double delta)
    {
        CameraPosition.Position = Player.Position;
        CameraYaw.Rotation = new Vector3(0f, Player.Rotation.Y, 0f);






        // **DEBUG**
        DebugDraw3D.DrawSphere(LeftRay3.GetCollisionPoint(), 0.5f, Colors.Yellow);



        if (LeftRay3.IsColliding() || LeftRay2.IsColliding() || LeftRay1.IsColliding())
        {

            if (LeftRay1.IsColliding())
            {
                rotationMax = 0.2f;
            }
            else if (LeftRay2.IsColliding())
            {
                rotationMax = 0.15f;
            }
            else if (LeftRay3.IsColliding())
            {
                rotationMax = 0.1f;
            }


            // BS
            //rotationY -= Player.Rotation.Y * (float)delta * rotationYSpeed;
            //if (rotationY < 0f) rotationY = 0f;
            //else if (rotationY > rotationMax) rotationY = rotationMax;


            rotationY = Mathf.Lerp(rotationY, rotationMax, 0.01f);

            CameraYaw.Rotation += new Vector3(0f, rotationY, 0f);





            cameraOffset = Mathf.Lerp(cameraOffset, 1.0f, 0.01f);
            CameraOffset.Position = new Vector3(cameraOffset, 0f, 0f);
        }
        else
        {
            rotationMax = 0f;

            rotationY = Mathf.Lerp(rotationY, rotationMax, 0.01f);

            CameraYaw.Rotation += new Vector3(0f, rotationY, 0f);



            cameraOffset = Mathf.Lerp(cameraOffset, 0.0f, 0.01f);
            CameraOffset.Position = new Vector3(cameraOffset, 0f, 0f);

        }



        if (RightRay3.IsColliding() || RightRay2.IsColliding() || RightRay1.IsColliding())
        {

            if (RightRay1.IsColliding())
            {
                rotationMax = -0.2f;
            }
            else if (RightRay2.IsColliding())
            {
                rotationMax = -0.15f;
            }
            else if (RightRay3.IsColliding())
            {
                rotationMax = -0.1f;
            }


            rotationY = Mathf.Lerp(rotationY, rotationMax, 0.01f);

            CameraYaw.Rotation += new Vector3(0f, rotationY, 0f);




            cameraOffset = Mathf.Lerp(cameraOffset, -1.0f, 0.01f);
            CameraOffset.Position = new Vector3(cameraOffset, 0f, 0f);


        }
        else
        {
            rotationMax = 0f;

            rotationY = Mathf.Lerp(rotationY, rotationMax, 0.01f);

            CameraYaw.Rotation += new Vector3(0f, rotationY, 0f);



            cameraOffset = Mathf.Lerp(cameraOffset, 0.0f, 0.01f);
            CameraOffset.Position = new Vector3(cameraOffset, 0f, 0f);

        }




        if(CameraRay.IsColliding())
        {

            //float zoom = Mathf.Lerp(CameraZoom.Position.Z, Player.Position.Z - CameraRay.GetCollisionPoint().Z, 0.1f);

            float zoom = Mathf.Lerp(CameraZoom.Position.Z, CameraRay.GlobalPosition.DistanceTo(CameraRay.GetCollisionPoint()), 0.5f);


            CameraZoom.Position = new Vector3(0f, 0f, zoom);
        }
        else
        {
            float zoom = Mathf.Lerp(CameraZoom.Position.Z, maxZoom, 0.5f);

            CameraZoom.Position = new Vector3(0f, 0f, zoom);

        }


















        DebugDraw3D.DrawSphere(CameraPosition.GlobalPosition, 0.5f, Colors.Red);
        DebugDraw3D.DrawSphere(CameraOffset.GlobalPosition, 0.5f, Colors.Green);






    }



}
