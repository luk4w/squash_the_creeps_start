using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public int BounceImpulse { get; set; } = 20;

	[Export]
	public int JumpImpulse { get; set; } = 20;

	[Export]
	public int Speed { get; set; } = 14;

	[Export]
	public int FallAcceleration { get; set; } = 75;

	private Vector3 _targetVelocity = Vector3.Zero;

	// Função que trabalha com física, não atualiza o mais rapido possivel, trabalha com um valor fixo de atualizações por segundo
	public override void _PhysicsProcess(double delta)
	{
		// Input direction
		var direction = Vector3.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}

		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}

		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 1.0f;
		}

		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1.0f;
		}

		// Evita o movimento mais rapido do player na diagonal (modulo do vetor resultante da direção maior que 1)
		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			GetNode<Node3D>("Pivot").LookAt(Position + direction, Vector3.Up);
		}

		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		// Se estiver no ar, aplicar um "efeito" de gravidade
		if (!IsOnFloor())
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}

		// Mecânica do pulo
		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			_targetVelocity.Y = JumpImpulse;
		}

		// Mover o player
		Velocity = _targetVelocity;
		// Move o personagem suavemente (Se atingir uma parede no meio de um movimento, a engine tentará suavizar essa ação)
		MoveAndSlide();


		// Itera por todas as colisões que ocorreram neste frame
		for (int index = 0; index < GetSlideCollisionCount(); index++)
		{
			// Obtem uma das colisões com o jogador.
			KinematicCollision3D collision = GetSlideCollision(index);

			// Verifica se a colisão foi com um inimigo
			if (collision.GetCollider() is Mob mob)
			{
				// Verifica se atingiu o inimigo de cima
				if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
				{
					// Se sim, esmaga o inimigo e pula
					mob.Squash();
					_targetVelocity.Y = BounceImpulse;
					// Evita chamadas duplicadas adicionais
					break;
				}
			}
		}

	}

}
