using System.Collections;
using System;
using System.Runtime.InteropServices;

public static class PandaRandom {
	
	// Using a global identifier to seed the generator
	static Random rand = new Random(Guid.NewGuid().GetHashCode());
	static float randomFactor = 0.1f;
	
	public static float RandomBlood(float liters)
	{
		return liters + (liters * randomFactor * (float)RandomDoubleBetween(-1,1));//* rand.
	}
	
	public static double RandomDoubleBetween(float min, float max)
	{
		return rand.NextDouble() * ( max - min ) + min;
	}
	
	public static int NextInt(int minval, int maxval)
	{
		return rand.Next(minval, maxval);	
	}

    public static float NextFloat (float minval, float maxval)
    {
        return (float) RandomDoubleBetween(minval, maxval);
    }
}
