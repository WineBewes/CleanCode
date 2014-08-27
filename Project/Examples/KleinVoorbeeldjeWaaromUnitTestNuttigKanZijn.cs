using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Examples
{
	/// <summary>
	/// Deze klasse is een mediator voor AutoMapper
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TDestination"></typeparam>
	abstract class KleinVoorbeeldjeWaaromUnitTestNuttigKanZijn<TSource, TDestination> : IAutoMapMediator<TSource, TDestination>
	{
		//ZOEK DE FOUT DIE RESHARPER NIET KAN VINDEN

		private static readonly ICollection<string> DerivedTypesCache = new List<string>();

		public TDestination Map(TSource source)
		{
			if (!DoesMapExist)
			{
				CreateMap();
			}

			return Mapper.Map<TSource, TDestination>(source);
		}

		protected virtual void CreateMap()
		{
			Mapper.CreateMap<TSource, TDestination>();
			DerivedTypesCache.Add(ToString());
		}

		protected virtual bool DoesMapExist
		{
			get
			{
				//om niet altijd naar AutoMapper te hoeven gaan voor elke keer dat method Map wordt gecalled
				if (DerivedTypesCache.Contains(ToString()))
				{
					return true;
				}

				return Mapper.FindTypeMapFor(typeof(TSource), typeof(TDestination)) == null;
			}
		}
	}

	interface IAutoMapMediator<in TSource, out TDestination>
	{
		TDestination Map(TSource source);
	}
}
