using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace queueMM1
{
	public class RandomNumberGenerator
	{
		private const decimal PMOD = 2147483647;

		public int ISEED { get; set; }

		public RandomNumberGenerator(int _ISEED)
		{
			this.ISEED = _ISEED;
		}

		public decimal Next()
		{
			decimal RMOD, DMAX;
			int IMOD;

			RMOD = this.ISEED;
			DMAX = 1 / PMOD;
			RMOD = RMOD * 16807;
			IMOD = Convert.ToInt32(Math.Floor(RMOD * DMAX));

			RMOD = RMOD - (PMOD * IMOD);

			//New value of ISEED
			this.ISEED = Convert.ToInt32(Math.Floor(RMOD));

			return (RMOD * DMAX);
		}
	}
}
