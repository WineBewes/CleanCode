using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
u
using AutoMapper;

namespace Examples
{
	abstract class KleinVoorbeeldjeWaaromUnitTestNuttigKanZijn<TSource, TDestination> : IAutoMapMediator<TSource, TDestination>
	{
		//ZOEK DE FOUT

		private static readonly ICollection<string> DerivedTypesCache = new List<string>();

		public TDestination Map(TSource source)
		{
			if (MapDoesNotExist)
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

		protected virtual bool MapDoesNotExist
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
