extends CharacterBody2D


@onready var sprite_2d: Sprite2D = $sprite_2d


const SPEED: float = 300.0
const JUMP_VELOCITY: float = -400.0


var flip_h: bool = false


func _physics_process(delta: float) -> void:
	if not is_on_floor():
		velocity += get_gravity() * delta

	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	var direction: float = Input.get_axis("move_left", "move_right")
	if direction:
		velocity.x = direction * SPEED

		flip_h = direction < 0
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

	sprite_2d.flip_h = flip_h

	if move_and_slide():
		pass
