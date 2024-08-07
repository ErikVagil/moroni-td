using Godot;
using System;

public partial class Arrow : PathFollow2D
{
	[Export]
	public float speed = 1500f;
	[Export]
	public int damage = 1;

	private Lamanite target = null;

    public override void _Process(double delta) {
		Progress += speed * (float)delta;
		if (!IsInstanceValid(target)) {
			QueueFree();
		}
		if (ProgressRatio >= 0.8f) {
			target.UpdateHealth(-damage);
			QueueFree();
		}
    }

	public void SetTarget(Lamanite target) {
		this.target = target;
	}
}
