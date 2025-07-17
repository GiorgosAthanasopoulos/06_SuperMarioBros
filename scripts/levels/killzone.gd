extends Area2D

@onready var kill_respawn_timer: Timer = $"Kill Respawn Timer"


func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("Player"):
		body.queue_free()
		kill_respawn_timer.start()


func _on_kill_respawn_timer_timeout() -> void:
	var error: Error = get_tree().reload_current_scene()
	if error != OK:
		print("Error occurred while trying to reload level after player hitting killzone: ", error_string(error))
