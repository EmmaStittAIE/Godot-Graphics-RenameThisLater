using Godot;

public partial class Characters : Node3D
{
    private float m_currentRotationTarget;
    private float m_rotationTimer;

    public float m_timeToRotate = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        m_rotationTimer = m_timeToRotate;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (m_rotationTimer < m_timeToRotate)
        {
            float lerpedRot = Mathf.LerpAngle(Rotation.Y, m_currentRotationTarget, m_rotationTimer / m_timeToRotate);
            RotateY(lerpedRot - Rotation.Y);

            m_rotationTimer += (float)delta;
        }
	}

    // Signal from LeftButton UI element
    private void OnLeftButtonPressed()
    {
        m_currentRotationTarget += Mathf.Pi / 2;
        m_rotationTimer = 0;
    }

    // Signal from RightButton UI element
    private void OnRightButtonPressed()
    {
        m_currentRotationTarget -= Mathf.Pi / 2;
        m_rotationTimer = 0;
    }

    // Signal from SelectButton UI element
    private void OnSelectButtonPressed()
    {
        // Replace with function body.
    }
}
