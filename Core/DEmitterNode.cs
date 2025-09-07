namespace Dragon;

public class DEmitterNode : DSpriteNode
{
    List<DParticle> particles;


    public float particleBirthRate;
    public int numParticlesToEmit;



    public DNode targetNode;


    public float particleLifetime;
    public float particleLifetimeRange;


    public Vector2 particlePosition;
    public Vector2 particlePositionRange;
    public float particleZPosition;


    public float particleSpeed;
    public float particleSpeedRange;
    public float emissionAngle;
    public float emissionAngleRange;
    public float xAcceleration;
    public float yAcceleration;


    public float particleRotation;
    public float particleRotationRange;
    public float particleRotationSpeed;



    public float particleScale = 1;
    public float particleScaleRange;
    public float particleScaleSpeed;


    public Texture2D particleTexture { get => texture; set => texture = value; }
    public Vector2 particleSize;



    public Color particleColor { get => color; set => color = value; }
    public float particleColorAlphaRange;
    public float particleColorBlueRange;
    public float particleColorGreenRange;
    public float particleColorRedRange;
    public float particleColorAlphaSpeed;
    public float particleColorBlueSpeed;
    public float particleColorGreenSpeed;
    public float particleColorRedSpeed;



    public float particleColorBlendFactor;
    public float particleColorBlendFactorRange;
    public float particleColorBlendFactorSpeed;


    public BlendState particleBlendMode { get => blendState; set => blendState = value; }


    public float particleAlpha = 1;
    public float particleAlphaRange;
    public float particleAlphaSpeed;







    float particleCounter;

    public DEmitterNode(int numParticlesToEmit) : base("missingTexture2D")
    {
        this.numParticlesToEmit = numParticlesToEmit;
        particles = new List<DParticle>(numParticlesToEmit);
    }

    public override void update()
    {
        float currentTime = DGame.currentTime;
        float elapsedTime = DGame.elapsedTime;

        if (numParticlesToEmit != 0)
        {
            particleCounter += particleBirthRate * elapsedTime;

            while ((int)particleCounter > 0 && numParticlesToEmit > 0)
            {
                numParticlesToEmit--;
                particleCounter--;

                DParticle particle = new();
                particle.birthTime = currentTime;

                Vector2 randomPosition = new(random.NextFloat() * particlePositionRange.X, random.NextFloat() * particlePositionRange.Y);
                particle.position = (particlePositionRange / 2) + randomPosition;

                float randomAngle = random.NextFloat() * emissionAngleRange;
                float randomSpeed = random.NextFloat() * particleSpeedRange;
                particle.speedX = (float)Math.Sin(emissionAngle - (emissionAngleRange / 2) + randomAngle) * (particleSpeed - (particleSpeedRange / 2) + randomSpeed);
                particle.speedY = -(float)Math.Cos(emissionAngle - (emissionAngleRange / 2) + randomAngle) * (particleSpeed - (particleSpeedRange / 2) + randomSpeed);

                float randomAlpha = random.NextFloat() * particleAlphaRange;
                particle.alpha = particleAlpha - (particleAlphaRange / 2) + randomAlpha;

                float randomScale = random.NextFloat() * particleScaleRange;
                particle.scale = particleScale - (particleScaleRange / 2) + randomScale;

                float randomLifeTime = random.NextFloat() * particleLifetimeRange;
                particle.lifetime = particleLifetime - (particleLifetimeRange / 2) + randomLifeTime;

                particles.Add(particle);
            }
        }

        for (int i = particles.Count - 1; i >= 0; i--)
        {
            DParticle particle = particles[i];

            if (currentTime - particle.birthTime > particle.lifetime)
            {
                particles.RemoveAt(i);
            }
            else
            {
                particle.update(elapsedTime, xAcceleration, yAcceleration, particleAlphaSpeed, particleScaleSpeed);
            }
        }
    }

    public override void draw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        if (hidden || currentAlpha <= 0.01f)
        {
            return;
        }

        beforeDraw(currentPosition, currentRotation, currentScale, currentAlpha);

        foreach (DParticle particle in particles)
        {
            Vector2 drawPosition = this.drawPosition + (particle.position * currentScale);
            Vector2 drawScale = this.drawScale * particle.scale;
            float drawAlpha = this.drawAlpha * particle.alpha;
            DGame.current.spriteBatch.Draw(texture, drawPosition, sourceRectangle, color * drawAlpha, drawRotation, origin, drawScale * textureScale, spriteEffects, layerDepth);
        }

        drawChildren();
    }
}
