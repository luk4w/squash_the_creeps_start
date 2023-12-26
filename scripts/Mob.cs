using Godot;
using System;

public partial class Mob : CharacterBody3D
{
    [Export]
    public int MinSpeed { get; set; } = 10;

    [Export]
    public int MaxSpeed { get; set; } = 18;

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    // Função que será chamada a partir de main.tscn
    public void Initialize(Vector3 startPosition, Vector3 playerPosition)
    {
        // Posiciona o mob na startPosition e rotacionan em direção a playerPosition, para que ele olhe para o jogador
        LookAtFromPosition(startPosition, playerPosition, Vector3.Up);

        // Rotaciona o mob aleatoriamente dentro de uma faixa de -45 e +45 graus para nao ir reto no jogador
        RotateY((float)GD.RandRange(-Mathf.Pi / 4.0, Mathf.Pi / 4.0));

        // Calcula a velocidade aleatória frontal
        int randomSpeed = GD.RandRange(MinSpeed, MaxSpeed);
        Velocity = Vector3.Forward * randomSpeed;

        // Rotaciona o vetor de velocidade com base na rotação Y do inimigo para mover na direção em que o inimigo está olhando
        Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
    }

    // Destrói a instancia do mob chamada caso ele saia do campo visivel
    private void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }
}