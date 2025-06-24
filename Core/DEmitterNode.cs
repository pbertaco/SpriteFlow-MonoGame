namespace Dragon;

public class DEmitterNode : DSpriteNode
{
    List<DParticle> particles;

    // Determining When Particles Are Created
    public float particleBirthRate; // The rate at which new particles are created.
    public int numParticlesToEmit; // The number of particles the emitter should emit before stopping.
    // ParticleRenderOrder particleRenderOrder; // The order in which the emitter’s particles are rendered.

    // Defining Which Node Emits Particles
    public DNode targetNode; // The target node which renders the emitter’s particles.

    // Determining a Particle Lifetime
    public float particleLifetime; // The average lifetime of a particle, in seconds.
    public float particleLifetimeRange; // The range of allowed random values for a particle’s lifetime.

    // Determining a Particle’s Initial Position
    public Vector2 particlePosition; // The average starting position for a particle.
    public Vector2 particlePositionRange; // The range of allowed random values for a particle’s position.
    public float particleZPosition; // The average starting depth of a particle.

    // Determining a Particle’s Velocity and Acceleration
    public float particleSpeed; // The average initial speed of a new particle in points per second.
    public float particleSpeedRange; // The range of allowed random values for a particle’s initial speed.
    public float emissionAngle; // The average initial direction of a particle, expressed as an angle in radians.
    public float emissionAngleRange; // The range of allowed random values for a particle’s initial direction, expressed as an angle in radians.
    public float xAcceleration; // The acceleration to apply to a particle’s horizontal velocity.
    public float yAcceleration; // The acceleration to apply to a particle’s vertical velocity.

    // Determining a Particle’s Rotation
    public float particleRotation; //The average initial rotation of a particle, expressed as an angle in radians.
    public float particleRotationRange; // The range of allowed random values for a particle’s initial rotation, expressed as an angle in radians.
    public float particleRotationSpeed; // The speed at which a particle rotates, expressed in radians per second.

    // Determining a Particle’s Scale Factor
    // particleScaleSequence: KeyframeSequence? // The sequence used to specify the scale factor of a particle over its lifetime.
    public float particleScale = 1; // The average initial scale factor of a particle.
    public float particleScaleRange; // The range of allowed random values for a particle’s initial scale.
    public float particleScaleSpeed; // The rate at which a particle’s scale factor changes per second.

    // Setting a Particle’s Texture and Size
    public Texture2D particleTexture { get => texture; set => texture = value; } // The texture to use to render a particle.
    public Vector2 particleSize; // The starting size of each particle.

    // Configuring Particle Color
    // particleColorSequence: KeyframeSequence? // The sequence used to specify the color components of a particle over its lifetime.
    public Color particleColor { get => color; set => color = value; } // The average initial color for a particle.
    public float particleColorAlphaRange; // The range of allowed random values for the alpha component of a particle’s initial color.
    public float particleColorBlueRange; // The range of allowed random values for the blue component of a particle’s initial color.
    public float particleColorGreenRange; // The range of allowed random values for the green component of a particle’s initial color.
    public float particleColorRedRange; // The range of allowed random values for the red component of a particle’s initial color.
    public float particleColorAlphaSpeed; // The rate at which the alpha component of a particle’s color changes per second.
    public float particleColorBlueSpeed; // The rate at which the blue component of a particle’s color changes per second.
    public float particleColorGreenSpeed; // The rate at which the green component of a particle’s color changes per second.
    public float particleColorRedSpeed; // The rate at which the red component of a particle’s color changes per second.

    // Determining How the Particle Texture Is Blended with the Particle Color
    // particleColorBlendFactorSequence: KeyframeSequence? // The sequence used to specify the color blend factor of a particle over its lifetime.
    public float particleColorBlendFactor; // The average starting value for the color blend factor.
    public float particleColorBlendFactorRange; // The range of allowed random values for a particle’s starting color blend factor.
    public float particleColorBlendFactorSpeed; // The rate at which the color blend factor changes per second.

    // Blending Particles with the Framebuffer
    public BlendState particleBlendMode { get => blendState; set => blendState = value; } // The blending mode used to blend particles into the framebuffer.

    // particleAlphaSequence: KeyframeSequence? // The sequence used to specify the alpha value of a particle over its lifetime.
    public float particleAlpha = 1; // The average starting alpha value for a particle.
    public float particleAlphaRange; // The range of allowed random values for a particle’s starting alpha value.
    public float particleAlphaSpeed; // The rate at which the alpha value of a particle changes per second.

    // Adding an Action to Particles
    // particleAction: Action? // Specifies an action executed by new particles.

    // Applying Physics Fields to the Particles
    // fieldBitMask: UInt32 //A mask that defines which categories of physics fields can exert forces on the particles.

    float particleCounter;

    public DEmitterNode(int numParticlesToEmit) : base("spark")
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

                DParticle particle = new DParticle();
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
