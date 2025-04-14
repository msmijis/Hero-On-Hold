using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	player player; // reference to player
	
	[Export]
	private int speed = 20;
	private int damage = 10;
	private int aps = 2;
	
	private float attack_speed;
	private float time_until_attack;
	bool within_attack_range = false;

	Node currentScene;
	private AnimatedSprite2D _enemySprite;
	private Vector2 currentVelocity;
	private String direction = "down";

	public override void _Ready() {
		var currentScene = GetTree().CurrentScene;
		string sceneName = currentScene.Name;
		player = (player)GetTree().Root.GetNode(sceneName).GetNode("Player");
		_enemySprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		attack_speed = 1/aps;
		time_until_attack = attack_speed;
		
		// connect signals
		/*
		var accessRange = GetNode<Area2D>("Attack Range");
		accessRange.BodyEntered += OnAccessRangeBodyEnter;
		accessRange.BodyExited += OnAccessRangeBodyExit;
		*/
	}
	
		// public void Attack() {
		// player.GetNode<Health>("Health").Damage(damage);
	// }

	public override void _PhysicsProcess(double delta) {
		// base._PhysicsProcess(delta);
		if (player != null) {
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			currentVelocity = direction * speed;
			Velocity = currentVelocity;
		}
		
		MoveAndSlide();

		updateAnimation();
	}
	
	private void updateAnimation() {
	if (currentVelocity.Length() == 0) { //if currentVelocity's magnitude is zero...
		_enemySprite.Stop(); //first frame of walk animation only will show
		return;
	}

	string new_direction;
	// Compare absolute values to decide which component is dominant.
	if (Mathf.Abs(currentVelocity.X) > Mathf.Abs(currentVelocity.Y)) {
		new_direction = currentVelocity.X < 0 ? "left" : "right";
	} else {
		new_direction = currentVelocity.Y < 0 ? "up" : "down";
	}
	
	if (direction != new_direction) { //prevents frames from reseting
		direction = new_direction;
		_enemySprite.Animation = "walk_" + direction; //resets frame count to 0 and changes animation to correct direction
	}

	_enemySprite.Play();
	}
}
/*
	public void OnAttackRangeBodyEnter(Node2D body) {
		if (body.IsInGroup("player")) {
			GD.Print("player in range");
			within_attack_range = true;
		}
	}
	
	public void OnAttackRangeBodyExit(Node2D body) {
		if (body.IsInGroup("player")) {
			within_attack_range = false;
			time_until_attack = attack_speed;
		}
	}
	*/
