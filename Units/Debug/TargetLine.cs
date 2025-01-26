using System;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class TargetLine : MeshInstance3D {

    private Vector3 start;
    private Vector3 end;
    private Vector4 color = new Vector4(255, 0, 0, 255);

    public ImmediateMesh mesh;

    public override void _Ready()
    {
        base._Ready();

        mesh = new ImmediateMesh();
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (!Game.ShowTargets) {
            Visible = false;
            return;
        }
        Visible = true;

        mesh.ClearSurfaces();
        mesh.SurfaceBegin(Mesh.PrimitiveType.Lines);
        mesh.SurfaceSetColor(Color.Color8((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W));
        mesh.SurfaceAddVertex(start);
        mesh.SurfaceSetColor(Color.Color8((byte)color.X, (byte)color.Y, (byte)color.Z, (byte)color.W));
        mesh.SurfaceAddVertex(end);
        mesh.SurfaceEnd();
    }

    public void setStart(Vector3 point) {
        start = point;
    }

    public void SetEnd(Vector3 point) {
        end = point;
    }

    public void setColour(Vector4 colour) {
        color = colour;
    }

}