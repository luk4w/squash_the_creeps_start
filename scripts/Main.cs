using Godot;
using System;

public partial class Main : Node3D
{
	[Export]
	public PackedScene MobScene { get; set; }

	// Nós também especificamos este nome de função em PascalCase na janela de conexão do editor
	private void OnMobTimerTimeout()
	{
		// Cria uma nova instância da cena para Mob
		Mob mob = MobScene.Instantiate<Mob>();

		// Escolhe uma localização aleatória no SpawnPath
		var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// Obtem a posição do jogador.
		Vector3 playerPosition = GetNode<Player>("Player").Position;
		// Inicializa o mob com a posição de spawn e a posição do jogador
		mob.Initialize(mobSpawnLocation.Position, playerPosition);

		// Gera o mob em main.tscn
		AddChild(mob);
	}

}
