namespace JumperGame.components
{
    public class SimpleMovementComponent
    {
        public float Speed { get; set; } = 50.0f;
        public bool MoveLeft { get; set; } = true;
        public float DirectionChangeInterval { get; set; } = 2.0f; 
        public float DirectionChangeTimer { get; set; } = 0.0f; 
    }
}