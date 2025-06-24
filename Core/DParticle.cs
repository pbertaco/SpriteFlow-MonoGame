namespace Dragon;

public class DParticle
{
    public Vector2 position;
    public float speedX;
    public float speedY;
    public float alpha;
    public float scale;
    public float birthTime;
    public float lifetime;

    public void update(float dt, float xAcceleration, float yAcceleration, float particleAlphaSpeed, float particleScaleSpeed)
    {
        speedX += xAcceleration * dt;
        speedY += yAcceleration * dt;
        position.X += speedX * dt;
        position.Y += speedY * dt;
        alpha += particleAlphaSpeed * dt;
        scale += particleScaleSpeed * dt;
    }
}
