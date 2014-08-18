using System;
using System.Collections.Generic;

namespace Examples
{
	public class ReSharperEdges
	{
		public void Kijk_Altijd_Na_Of_ReSharper_Beter_Leesbare_Code_Oplevert()
		{
			//ReSharper stelt "collection initializer voor : OK
			var bestaandeActies = new List<string>();
			bestaandeActies.Add("actieA");
			bestaandeActies.Add("actieB");
			bestaandeActies.Add("actieC");

			var behandeldeActies = new List<string> { "actieA", "actieC" };

			//ReSharper stelt voor : "Convert part of body into LINQ-expression" : OK ???
			foreach (var bestaandeActie in bestaandeActies)
			{
				if (!behandeldeActies.Contains(bestaandeActie))
				{
					throw new Exception(string.Format("Bestaande actie {0} wordt niet behandeld.", bestaandeActie));
				}
			}
		}
	}
}
