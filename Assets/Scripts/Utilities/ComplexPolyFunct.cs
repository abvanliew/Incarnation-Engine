using System;

public struct ComplexPolyFunct
{
	public double[] coeffcients;
	public double[] powers;
		
	public ComplexPolyFunct( double[] newCoeff, double[] newPow )
	{
		coeffcients = newCoeff;
		powers = newPow;
	}
		
	public double solve( double x )
	{
		double total = 0d;
		double curCoeff;
		double curPow;
		int forLength;

		if( coeffcients.Length > powers.Length )
			forLength = coeffcients.Length;
		else
			forLength = powers.Length;

		if( forLength > 0 ) 
		{
			for( int i = 0; i < forLength; i++ ) 
			{
				curCoeff = ( i >= coeffcients.Length ) ? 1 : coeffcients[i];
				curPow = ( i >= powers.Length ) ? 0 : powers[i];

				total += curCoeff * Math.Pow( x, curPow );
			}
		}
			
		return total;
	}
}