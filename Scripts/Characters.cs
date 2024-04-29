using Godot;

public partial class Characters : Node3D
{
    private float m_rotationOrigin = 0;
    private float m_rotationTarget = 0;
    private float m_rotationTimer;

    private float m_waveEffectTimer;
    private bool m_waveEffectActive = false;

    private int m_currentTargetIndex = 0;

    [Export]
    public AnimationTree[] m_animTrees;
    [Export]
    public Button m_selectButton;

    [Export]
    public float m_timeToRotate;
    [Export]
    public float m_waveEffectActivationSpeed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        m_rotationTimer = m_timeToRotate;
        m_waveEffectTimer = m_waveEffectActivationSpeed;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (m_rotationTimer < m_timeToRotate)
        {
            float lerpedRot = Mathf.Lerp(m_rotationOrigin, m_rotationTarget, 1 - Mathf.Pow(1 - (m_rotationTimer / m_timeToRotate), 8));
            //RotateY(lerpedRot - Rotation.Y);
            Rotation = new(Rotation.X, lerpedRot, Rotation.Z);

            m_rotationTimer += (float)delta;
        }

        if (m_waveEffectTimer < m_waveEffectActivationSpeed)
        {
            // Always a lerp between 0 and 1
            float lerpedWaveIntensity = m_waveEffectTimer / m_waveEffectActivationSpeed;
            if (!m_waveEffectActive) { lerpedWaveIntensity = 1 - lerpedWaveIntensity; }

            RenderingServer.GlobalShaderParameterSet("waveFadeIn", lerpedWaveIntensity);

            m_waveEffectTimer += (float)delta;
        }
	}

    // Signal from LeftButton UI element
    private void OnLeftButtonPressed()
    {
        m_rotationOrigin = Rotation.Y;
        m_rotationTarget += Mathf.Pi / 2;
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
        m_rotationOrigin = Rotation.Y;
        m_rotationTarget -= Mathf.Pi / 2;
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
        m_waveEffectActive = toggledOn;
        m_waveEffectTimer = 0;
    }
}
