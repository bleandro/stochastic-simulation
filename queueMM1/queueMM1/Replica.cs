using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace queueMM1
{
    public class Replica
    {
        #region Properties
        private List<Arrival> listArrivals;
        private decimal avgTimeBetweenArrivals = 0;
        private decimal avgServiceTime = 0;
        private decimal avgQueuingTime = 0;
        private decimal avgSystemTime = 0;
        #endregion

        #region Fields
        public List<Arrival> ListArrivals { get => listArrivals; set => listArrivals = value; }
        public decimal AvgTimeBetweenArrivals { get => avgTimeBetweenArrivals; set => avgTimeBetweenArrivals = value; }
        public decimal AvgServiceTime { get => avgServiceTime; set => avgServiceTime = value; }
        public decimal AvgQueuingTime { get => avgQueuingTime; set => avgQueuingTime = value; }
        public decimal AvgSystemTime { get => avgSystemTime; set => avgSystemTime = value; }
        #endregion
    }
}
