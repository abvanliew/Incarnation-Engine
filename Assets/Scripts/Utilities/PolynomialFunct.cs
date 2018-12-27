using System;

public struct PolynomialFunct
{
	public double[] coeffcients;

	public PolynomialFunct( double[] newCoEff )
	{
		coeffcients = newCoEff;
	}

	public double solve( double x )
	{
		double total = 0D;

		if (coeffcients.Length > 0)
		{
			for( int i = 0; i < coeffcients.Length; i++ ) 
			{
				total += coeffcients[i] * Math.Pow( x, i );
			}
		}

		return total;
	}
}