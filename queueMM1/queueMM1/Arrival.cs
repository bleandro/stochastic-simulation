using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace queueMM1
{
	public class Arrival
	{
		#region Fields
		private decimal timeBetweenArrivals;
		private decimal serviceTime;
		private decimal timeCurrent;
		private decimal initialTime;
		private decimal finalTime;
		private decimal queuingTime;
		private decimal systemTime;
		private decimal numberOfPeopleInQueue;
		private decimal freeTime;
		#endregion

		#region Properties
		public decimal TimeBetweenArrivals { get => timeBetweenArrivals; set => timeBetweenArrivals = value; }
		public decimal ServiceTime { get => serviceTime; set => serviceTime = value; }
		public decimal TimeCurrent { get => timeCurrent; set => timeCurrent = value; }
		public decimal InitialTime { get => initialTime; set => initialTime = value; }
		public decimal FinalTime { get => finalTime; set => finalTime = value; }
		public decimal QueuingTime { get => queuingTime; set => queuingTime = value; }
		public decimal SystemTime { get => systemTime; set => systemTime = value; }
		public decimal NumberOfPeopleInQueue { get => numberOfPeopleInQueue; set => numberOfPeopleInQueue = value; }
		public decimal FreeTime { get => freeTime; set => freeTime = value; }
		#endregion
	}
}
