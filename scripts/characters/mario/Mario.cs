using Godot;

public partial class Mario : CharacterBody2D
{
    [Export]
    public float Speed = 200;
    public float SprintSpeed = 400;

    [Export]
    public float JumpVelocity = -9.8f * 25;
    [Export]
    public float Gravity = 9.8f * 30;

    [Export]
    public AnimationTree animationTree;
    [Export]
    public Sprite2D sprite;
    [Export]
    public string AnimationTreeNodePath = "AnimationTree", SpriteNodePath = "Sprite2D";

    [Export]
    public string MoveUpAction = "move_up", MoveDownAction = "move_down", MoveLeftAction = "move_left",
                  MoveRightAction = "move_right", JumpAction = "jump", SprintAction = "sprint";

    [Export]
    public string AnimatrionTreeFallingParameter = "parameters/is_falling", AnimationTreeJumpingParameter = "parameters/is_jumping",
    AnimationTreeRunningParameter = "parameters/is_running", AnimationTreeWalkingParameter = "parameters/is_walking",
    AnimationTreePlaybackParameter = "parameters/playback";

    [Export]
    public string FallingAnimatiom = "drop_right", JumpingAnimation = "jump_right", WalkingAnimation = "walk_right",
    RunningAnimation = "run_right", IdleAnimation = "idle_right";
    [Export]
    public string ReloadSceneAction = "ui_cancel";

    private bool lastDirectionLeft = false;

    private AnimationNodeStateMachinePlayback stateMachine;

    public override void _Ready()
    {
        animationTree ??= GetNode<AnimationTree>(AnimationTreeNodePath);
        sprite ??= GetNode<Sprite2D>(SpriteNodePath);
        stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get(AnimationTreePlaybackParameter);
    }

    public override void _PhysicsProcess(double delta)
    {
        PhysicsTick(delta);
        HandleAnimationTree();
        HandleSpriteRotation();

        if (Input.IsActionJustPressed(ReloadSceneAction))
            GetTree().ReloadCurrentScene();
    }

    private void PhysicsTick(double delta)
    {
        if (!IsOnFloor()) // gravity
            Velocity = Velocity with { Y = Velocity.Y + Gravity * (float)delta };
        if (IsOnFloor() && Input.IsActionJustPressed(JumpAction)) // jump
            Velocity = Velocity with { Y = JumpVelocity };

        // left/right movement
        float inputDirection = Input.GetActionStrength(MoveRightAction) - Input.GetActionStrength(MoveLeftAction);
        float speed = Input.IsActionPressed(SprintAction) ? SprintSpeed : Speed;
        Velocity = Velocity with { X = inputDirection * speed };

        MoveAndSlide();
    }

    private void HandleAnimationTree()
    {
        bool is_falling = Velocity.Y > 0 && !IsOnFloor();
        bool is_jumping = Velocity.Y < 0 && !IsOnFloor();
        bool is_running = Input.IsActionPressed(SprintAction) && IsOnFloor();
        bool is_walking = !is_running && IsOnFloor() && (Input.IsActionPressed(MoveLeftAction) || Input.IsActionPressed(MoveRightAction));

        animationTree.Set(AnimatrionTreeFallingParameter, is_falling);
        animationTree.Set(AnimationTreeJumpingParameter, is_jumping);
        animationTree.Set(AnimationTreeRunningParameter, is_running);
        animationTree.Set(AnimationTreeWalkingParameter, is_walking);

        if (is_falling)
            stateMachine.Travel(FallingAnimatiom);
        else if (is_jumping)
            stateMachine.Travel(JumpingAnimation);
        else if (is_running)
            stateMachine.Travel(RunningAnimation);
        else if (is_walking)
            stateMachine.Travel(WalkingAnimation);
        else
            stateMachine.Travel(IdleAnimation);
    }

    private void HandleSpriteRotation()
    {
        float inputDirection = Input.GetActionStrength(MoveRightAction) - Input.GetActionStrength(MoveLeftAction);

        if (inputDirection != 0)
            lastDirectionLeft = inputDirection < 0;

        sprite.FlipH = lastDirectionLeft;
    }
}
