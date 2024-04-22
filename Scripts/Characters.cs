using Godot;

public partial class Characters : Node3D
{
    private float m_currentRotationTarget = 0;
    private float m_rotationTimer;

    private int m_currentTargetIndex = 0;

    [Export]
    public AnimationTree[] m_animTrees;
    [Export]
    public Button m_selectButton;

    [Export]
    public float m_timeToRotate;

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

        m_animTrees[m_currentTargetIndex].Set("parameters/conditions/selected", false);
        m_animTrees[m_currentTargetIndex].Set("parameters/conditions/deselected", true);

        m_selectButton.ButtonPressed = false;

        m_currentTargetIndex -= 1;
        if (m_currentTargetIndex < 0) { m_currentTargetIndex = m_animTrees.Length - 1; }
    }

    // Signal from RightButton UI element
    private void OnRightButtonPressed()
    {
        m_currentRotationTarget -= Mathf.Pi / 2;
        m_rotationTimer = 0;

        m_animTrees[m_currentTargetIndex].Set("parameters/conditions/selected", false);
        m_animTrees[m_currentTargetIndex].Set("parameters/conditions/deselected", true);

        m_selectButton.ButtonPressed = false;

        m_currentTargetIndex += 1;
        if (m_currentTargetIndex >= m_animTrees.Length) { m_currentTargetIndex = 0; }
    }

    // Signal from SelectButton UI element
    private void OnSelectButtonToggle(bool toggledOn)
    {
        // I hate this but I'm not sure how to solve it
        m_animTrees[m_currentTargetIndex].Set("parameters/conditions/selected", toggledOn);
        m_animTrees[m_currentTargetIndex].Set("parameters/conditions/deselected", !toggledOn);
    }

    // Signal from WoahButton UI element
    private void OnWoahButtonToggle(bool toggledOn)
    {
        RenderingServer.GlobalShaderParameterSet("waveEffect", toggledOn);
    }
}
