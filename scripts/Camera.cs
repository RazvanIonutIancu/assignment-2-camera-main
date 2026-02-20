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
    public Player player;

    [Export]
    CharacterBody3D NPC;

    [Export]
    Node3D CameraPosition;

    [Export]
    Timer FollowTimer;





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



    // Camera
    float rotationLimit = Mathf.DegToRad(5.0f);
    float rotationY = 0f;
    float rotationYSpeed = 0.3f;
    //float rotationMax = 0.1f;
    float maxZoom = 15.0f;
    float minZoom = 1.0f;
    float npcZoom = 10.0f;
    float cameraOffset = 0f;
    float cameraSpeed = 0.05f;
    float maxCameraTilt = Mathf.DegToRad(-1);
    float defaultCameraTilt = Mathf.DegToRad(-30);
    float currentCameraTilt;
    float minCameraTilt = Mathf.DegToRad(-60);

    float cameraLeftRotationBase = 0f;
    float cameraRightRotationBase = 0f;
    float cameraRotationBase = 0f;





    public override void _PhysicsProcess(double delta)
    {
        // This makes the player either follow the player or the NPC. Could be set to other NPCs too, but I don't need that for this project.
        if (player.isConnectedToCamera)
        {
            CameraPosition.Position = CameraPosition.Position.Slerp(Player.Position, 5.0f * (float)delta);


            float zoom = Mathf.Lerp(CameraZoom.Position.Z, maxZoom, 0.1f);
            CameraZoom.Position = new Vector3(0f, 0f, zoom);
        }
        else
        {
            CameraPosition.Position = CameraPosition.Position.Slerp(NPC.Position, 5.0f * (float)delta);

            float zoom = Mathf.Lerp(CameraZoom.Position.Z, npcZoom, 0.1f);
            CameraZoom.Position = new Vector3(0f, 0f, zoom);
        }


        // CameraYaw.Rotation = new Vector3(0f, Player.Rotation.Y, 0f);


        // Camera joystick controls
        // Tilt
        float cameraVerticalInput = Input.GetAxis("camera_down", "camera_up");

        if (Mathf.Abs(cameraVerticalInput) > 0.01f)
        {
            CameraTilt.Rotation += new Vector3(cameraVerticalInput * cameraSpeed, 0f, 0f);
            FollowTimer.Start();

            if (CameraTilt.Rotation.X > maxCameraTilt)
            {
                CameraTilt.Rotation = new Vector3(maxCameraTilt, 0f, 0f);
            }
            if (CameraTilt.Rotation.X < minCameraTilt)
            {
                CameraTilt.Rotation = new Vector3(minCameraTilt, 0f, 0f);
            }

        }
        else if((Mathf.Abs(player.Velocity.X) > 0.1f || Mathf.Abs(player.Velocity.Z) > 0.1f) && FollowTimer.IsStopped())
        {
            currentCameraTilt = Mathf.LerpAngle(CameraTilt.Rotation.X, defaultCameraTilt, cameraSpeed);

            CameraTilt.Rotation = new Vector3(currentCameraTilt, 0f, 0f);
        }



        // Yaw
        float cameraHorizontalInput = Input.GetAxis("camera_left", "camera_right");

        if (Mathf.Abs(cameraHorizontalInput) > 0.01f)
        {
            CameraYaw.Rotation += new Vector3(0f, cameraHorizontalInput * cameraSpeed, 0f);
            FollowTimer.Start();
        }
        else if ((Mathf.Abs(player.Velocity.X) > 0.1f || Mathf.Abs(player.Velocity.Z) > 0.1f) && FollowTimer.IsStopped())
        {
            rotationY = Mathf.LerpAngle(CameraYaw.Rotation.Y, Player.Rotation.Y + cameraRotationBase, cameraSpeed);

            CameraYaw.Rotation = new Vector3(0f, rotationY, 0f);

        }









        // Raycasts 

        if ((LeftRay3.IsColliding() || LeftRay2.IsColliding() || LeftRay1.IsColliding()) && player.isConnectedToCamera && (Mathf.Abs(cameraHorizontalInput) < 0.01f) && FollowTimer.IsStopped())
        {

            if (LeftRay1.IsColliding())
            {
                cameraLeftRotationBase = 0.2f;
                GD.Print("LR 1 is colliding");
            }
            else if (LeftRay2.IsColliding())
            {
                cameraLeftRotationBase = 0.15f;
                GD.Print("LR 2 is colliding");
            }
            else if (LeftRay3.IsColliding())
            {
                cameraLeftRotationBase = 0.1f;
                GD.Print("LR 3 is colliding");
            }


            // BS
            //rotationY -= Player.Rotation.Y * (float)delta * rotationYSpeed;
            //if (rotationY < 0f) rotationY = 0f;
            //else if (rotationY > rotationMax) rotationY = rotationMax;


            //rotationY = Mathf.Lerp(rotationY, rotationMax, 0.01f);

            //CameraYaw.Rotation += new Vector3(0f, rotationY, 0f);





            cameraOffset = Mathf.Lerp(cameraOffset, 1.0f, 0.01f);
            CameraOffset.Position = new Vector3(cameraOffset, 0f, 0f);
        }
        else if (player.isConnectedToCamera && (Mathf.Abs(cameraHorizontalInput) < 0.01f) && FollowTimer.IsStopped())
        {
            cameraLeftRotationBase = 0f;

            //rotationY = Mathf.Lerp(rotationY, rotationMax, 0.01f);

            //CameraYaw.Rotation += new Vector3(0f, rotationY, 0f);



            cameraOffset = Mathf.Lerp(cameraOffset, 0.0f, 0.01f);
            CameraOffset.Position = new Vector3(cameraOffset, 0f, 0f);

        }



        if ((RightRay3.IsColliding() || RightRay2.IsColliding() || RightRay1.IsColliding()) && player.isConnectedToCamera && (Mathf.Abs(cameraHorizontalInput) < 0.01f) && FollowTimer.IsStopped())
        {

            if (RightRay1.IsColliding())
            {
                cameraRightRotationBase = -0.2f;
            }
            else if (RightRay2.IsColliding())
            {
                cameraRightRotationBase = -0.15f;
            }
            else if (RightRay3.IsColliding())
            {
                cameraRightRotationBase = -0.1f;
            }


            //rotationY = Mathf.Lerp(rotationY, rotationMax, 0.01f);

            //CameraYaw.Rotation += new Vector3(0f, rotationY, 0f);




            cameraOffset = Mathf.Lerp(cameraOffset, -1.0f, 0.01f);
            CameraOffset.Position = new Vector3(cameraOffset, 0f, 0f);


        }
        else if (player.isConnectedToCamera && (Mathf.Abs(cameraHorizontalInput) < 0.01f) && FollowTimer.IsStopped())
        {
            cameraRightRotationBase = 0f;

            //rotationY = Mathf.Lerp(rotationY, rotationMax, 0.01f);

            //CameraYaw.Rotation += new Vector3(0f, rotationY, 0f);



            cameraOffset = Mathf.Lerp(cameraOffset, 0.0f, 0.01f);
            CameraOffset.Position = new Vector3(cameraOffset, 0f, 0f);

        }

        cameraRotationBase = cameraRightRotationBase + cameraLeftRotationBase;


        if (CameraRay.IsColliding() && player.isConnectedToCamera)
        {

            //float zoom = Mathf.Lerp(CameraZoom.Position.Z, Player.Position.Z - CameraRay.GetCollisionPoint().Z, 0.1f);

            float zoom = Mathf.Lerp(CameraZoom.Position.Z, CameraRay.GlobalPosition.DistanceTo(CameraRay.GetCollisionPoint()), 0.5f);


            CameraZoom.Position = new Vector3(0f, 0f, zoom);
        }
        else if (player.isConnectedToCamera)
        {
            float zoom = Mathf.Lerp(CameraZoom.Position.Z, maxZoom, 0.1f);

            CameraZoom.Position = new Vector3(0f, 0f, zoom);

        }

















        // **DEBUG**
        DebugDraw3D.DrawSphere(LeftRay3.GetCollisionPoint(), 0.5f, Colors.Yellow);

        DebugDraw3D.DrawSphere(CameraPosition.GlobalPosition, 0.5f, Colors.Red);

        DebugDraw3D.DrawSphere(CameraOffset.GlobalPosition, 0.5f, Colors.Green);

        GD.Print(cameraRotationBase);





    }


}


