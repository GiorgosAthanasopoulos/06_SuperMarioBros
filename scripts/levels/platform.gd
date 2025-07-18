extends CharacterBody2D


@onready var nav_agent: NavigationAgent2D = $navigation_agent_2d
@onready var start: Marker2D = $marker_2d_start
@onready var end: Marker2D = $marker_2d_end


var current_target: Vector2
var speed : float = 50


func _ready() -> void:
	current_target = end.global_position
	nav_agent.target_position = current_target


func _physics_process(_delta: float) -> void:
	if nav_agent.is_navigation_finished():
		current_target = start.global_position if current_target == end.global_position else end.global_position
		nav_agent.target_position = current_target

	var direction: Vector2 = (nav_agent.get_next_path_position() - global_position).normalized()
	velocity = direction * speed
	if move_and_slide():
		pass
