extends CharacterBody2D


@onready var animated_sprite_2d: AnimatedSprite2D = $animated_sprite_2d


const SPEED: float = 300.0
const JUMP_VELOCITY: float = -400.0


var flip_h: bool = false


func _physics_process(delta: float) -> void:
	handle_gravity(delta)
	handle_jumping()
	handle_movement()
	handle_animation()


func handle_gravity(delta: float) -> void:
	if not is_on_floor():
		velocity += get_gravity() * delta


func handle_jumping() -> void:
	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = JUMP_VELOCITY


func handle_movement() -> void:
	var direction: float = Input.get_axis("move_left", "move_right")
	if direction:
		velocity.x = direction * SPEED

		flip_h = direction < 0
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

	if move_and_slide():
		pass


func handle_animation() -> void:
	animated_sprite_2d.flip_h = flip_h
