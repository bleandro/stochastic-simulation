using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace queueMM1
{
	public class Arrival
	{
        /*
         * Classe que armazena as Chegadas
         * E todas as informações de cada Chegada
         */


		#region Fields
		private decimal timeBetweenArrivals;
		private decimal serviceTime;
		private decimal timeCurrent;
		private decimal finalTime;
		private decimal queuingTime;
        private decimal freeTime;
        private int numberOfPeopleInQueue;
		#endregion

		#region Properties
		public decimal TimeBetweenArrivals { get => timeBetweenArrivals; set => timeBetweenArrivals = value; }
		public decimal ServiceTime { get => serviceTime; set => serviceTime = value; }
		public decimal TimeCurrent { get => timeCurrent; set => timeCurrent = value; }
		public decimal FinalTime { get => finalTime; set => finalTime = value; }
		public decimal QueuingTime { get => queuingTime; set => queuingTime = value; }
        public decimal FreeTime { get => freeTime; set => freeTime = value; }
        public int NumberOfPeopleInQueue { get => numberOfPeopleInQueue; set => numberOfPeopleInQueue = value; }

        public decimal InitialTime
        {
            get
            {
                return timeCurrent + queuingTime;
            }
        }

        public decimal SystemTime
        {
            get
            {
                return queuingTime + serviceTime;
            }
        }

        #endregion
    }
}
