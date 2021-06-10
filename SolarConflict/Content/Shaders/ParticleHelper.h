// Particle texture and sampler.
texture Texture;

sampler Sampler = sampler_state
{
    Texture = (Texture);
    
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Point;
    
    AddressU = Wrap;
    AddressV = Clamp;
};


// Vertex shader input structure describes the start position and
// velocity of the particle, and the time at which it was created,
// along with some random values that affect its size and rotation.
struct VertexShaderInput
{
    float4 Position : SV_Position0;
    float2 Corner : TEXCOORD0;
    float3 Velocity : NORMAL0;
    float4 Random : COLOR0;
    float Time : TEXCOORD1;
	float2 Parameters : TEXCOORD2;
};


// Vertex shader output structure specifies the position and color of the particle.
struct VertexShaderOutput
{
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float2 TextureCoordinate : TEXCOORD0;
    float2 Phase  : TEXCOORD1;
	float Temperature: TEXCOORD2;
};


// Parameters describing how the particles animate.
float Duration;
float DurationRandomness;
float3 Gravity;
float EndVelocity;
float4 MinColor;
float4 MaxColor;

// Camera parameters.
float4x4 View;
float4x4 Projection;
float2 ViewportScale;

float CurrentTime;

// These float2 parameters describe the min and max of a range.
// The actual value is chosen differently for each particle,
// interpolating between x and y by some random amount.
float2 RotateSpeed;
float2 StartSize;
float2 EndSize;

// Vertex shader helper for computing the position of a particle.
float4 ComputeParticlePosition(float3 position, float3 velocity,
                               float age, float normalizedAge)
{
    float startVelocity = length(velocity);

    // Work out how fast the particle should be moving at the end of its life,
    // by applying a constant scaling factor to its starting velocity.
    float endVelocity = startVelocity * EndVelocity;
    
    // Our particles have constant acceleration, so given a starting velocity
    // S and ending velocity E, at time T their velocity should be S + (E-S)*T.
    // The particle position is the sum of this velocity over the range 0 to T.
    // To compute the position directly, we must integrate the velocity
    // equation. Integrating S + (E-S)*T for T produces S*T + (E-S)*T*T/2.

    float velocityIntegral = startVelocity * normalizedAge +
                             (endVelocity - startVelocity) * normalizedAge *
                                                             normalizedAge / 2;
     
    position += normalize(velocity) * velocityIntegral * Duration;
    
    // Apply the gravitational force.
    position += Gravity * age * normalizedAge;
    
    // Apply the camera view and projection transforms.
    return mul(mul(float4(position, 1), View), Projection);
}

// Vertex shader helper for computing the size of a particle.
float ComputeParticleSize(float randomValue, float normalizedAge, float scaleMulti)
{
    // Apply a random factor to make each particle a slightly different size.
    float startSize = lerp(StartSize.x * scaleMulti, StartSize.y * scaleMulti, randomValue);
    float endSize = lerp(EndSize.x * scaleMulti, EndSize.y * scaleMulti, randomValue);
    
    // Compute the actual size based on the age of the particle.
    float size = lerp(startSize , endSize , normalizedAge);

    // Project the size into screen coordinates.
    return size * Projection._m11 * View._m11;
}

float EasyInOut(float normAge, float sValue = 0, float dValue = 1, float duration = 1)
{
	normAge /= duration/2;
	if (normAge < 1) return dValue/2*(normAge*normAge*normAge) + sValue;
	normAge -= 2;
	return dValue/2*(normAge*normAge*normAge + 2) + sValue;
}

// Vertex shader helper for computing the color of a particle.
float4 ComputeParticleColor(float4 projectedPosition, float normalizedAge)
{

    float4 color = float4(1,1,1,1);
	color.a = 1 - EasyInOut(normalizedAge);
    return color;
}

float4 ComputeParticleColorSpikeIn(float4 projectedPosition, float normalizedAge)
{

    float4 color = float4(1,1,1,1);
	color.a = normalizedAge * (1-normalizedAge) * (1-normalizedAge)  * 6.7;
    return color;
}

float3 ComputeTemperature(float normalizedAge)
{
	float3 color = lerp(float3(1,1,1), float3(0,0,0), normalizedAge);
	return color;
}

// Vertex shader helper for computing the rotation of a particle.
float2x2 ComputeParticleRotation(float randomValue, float age)
{    
    // Apply a random factor to make each particle rotate at a different speed.
    float rotateSpeed = lerp(RotateSpeed.x, RotateSpeed.y, randomValue);
    
    float rotation = rotateSpeed * age + randomValue * 6;

    // Compute a 2x2 rotation matrix.
    float c = cos(rotation);
    float s = sin(rotation);
    
    return float2x2(c, -s, s, c);
}