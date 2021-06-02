public interface IMovementController
{
    void EnableMovement();
    void DisableMovement();
    bool FacingRight { get; }
    void Flip();
}
