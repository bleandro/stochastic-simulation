using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace queueMM1
{
	public class Program
	{
		public static void Main()
		{
			try
			{
				if (File.Exists("planilha.csv"))
					File.Delete("planilha.csv");
			}
			catch (Exception) { }
			StreamWriter file = new StreamWriter("planilha.csv");

            #region Inputs
            int SEED;
			Console.WriteLine("Valor inicial ou semente:");
			SEED = Convert.ToInt32(Console.ReadLine());

			RandomNumberGenerator random = new RandomNumberGenerator(SEED);

			int numberOfClients;
			Console.WriteLine("Quantidade de testes a serem realiza:");
			numberOfClients = Convert.ToInt32(Console.ReadLine());
            #endregion
        
            #region Definitions
            decimal timeBetweenArrivals = 0;
			decimal serviceTime = 0;
			decimal timeCurrent = 0;
			decimal initialTime = 0;
			decimal finalTime = 0;
			decimal queuingTime = 0;
			decimal freeTime = 0;

            decimal avgTimeBetweenArrivals = 0;
            decimal avgServiceTime = 0;
            decimal avgQueuingTime = 0;
            decimal avgSystemTime = 0;

            List<Arrival> listQueuing = new List<Arrival>();
            List<Arrival> listArrivals = new List<Arrival>();

            List<Replica> listReplicas = new List<Replica>();
            #endregion

            #region Process 

            for (int iCont = 0; iCont < numberOfClients; iCont++)
			{
                Arrival newArrival = new Arrival();

                timeBetweenArrivals = GetRandomTimeBetweeArrival(random.Next());
                serviceTime = GetRandomServiceTime(random.Next());

				timeCurrent += timeBetweenArrivals;

                #region Calculate
                queuingTime = 0;
				if (timeCurrent < finalTime)
					queuingTime = (finalTime - timeCurrent);

                // If there's queuingTime, We need to add a person to the Queue
                if (queuingTime > 0)
                    listQueuing.Add(newArrival);

                initialTime = timeCurrent + queuingTime;
                freeTime = initialTime - finalTime;

                //Calculate new finalTime
                finalTime = initialTime + serviceTime;

                #endregion

                #region Creating Arrival Object

                newArrival.TimeBetweenArrivals = timeBetweenArrivals;
                newArrival.ServiceTime = serviceTime;
                newArrival.TimeCurrent = timeCurrent;
                newArrival.QueuingTime = queuingTime;
                newArrival.NumberOfPeopleInQueue = listQueuing.Count;
                newArrival.FreeTime = freeTime;
                newArrival.FinalTime = finalTime;
                listArrivals.Add(newArrival);

                #endregion

                #region Output

                //Output to a file
                file.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
					newArrival.TimeBetweenArrivals, 
                    newArrival.ServiceTime,
                    newArrival.TimeCurrent,
                    newArrival.InitialTime,
                    newArrival.FinalTime,
                    newArrival.QueuingTime,
                    newArrival.SystemTime,
                    newArrival.NumberOfPeopleInQueue,
                    newArrival.FreeTime
				));

                //Output to Console
				Console.WriteLine(
					String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                    newArrival.TimeBetweenArrivals,
                    newArrival.ServiceTime,
                    newArrival.TimeCurrent,
                    newArrival.InitialTime,
                    newArrival.FinalTime,
                    newArrival.QueuingTime,
                    newArrival.SystemTime,
                    newArrival.NumberOfPeopleInQueue,
                    newArrival.FreeTime
                ));

                #endregion

                #region Decrease People in Queue
                for (int jCont = listQueuing.Count - 1; jCont >= 0; jCont--)
                {
                    if (listQueuing[jCont].FinalTime < timeCurrent)
                        listQueuing.RemoveAt(jCont);
                }
                #endregion

			}

            #region Calculate statistics
            foreach (Arrival arrival in listArrivals)
            {
                avgQueuingTime += arrival.QueuingTime;
                avgSystemTime += arrival.SystemTime;
                avgTimeBetweenArrivals += arrival.TimeBetweenArrivals;
                avgServiceTime += arrival.ServiceTime;
            }
            avgQueuingTime = avgQueuingTime / numberOfClients;
            avgSystemTime = avgSystemTime / numberOfClients;
            avgTimeBetweenArrivals = avgTimeBetweenArrivals / numberOfClients;
            avgServiceTime = avgServiceTime / numberOfClients;
            #endregion

            #region Creating Replica Object
            Replica newReplica = new Replica();
            newReplica.AvgQueuingTime = avgQueuingTime;
            newReplica.AvgSystemTime = avgSystemTime;
            newReplica.AvgTimeBetweenArrivals = avgTimeBetweenArrivals;
            newReplica.AvgServiceTime = avgServiceTime;
            listReplicas.Add(newReplica);


            Console.WriteLine(
                String.Format(
                    "Média tempo de fila: {0}\n" +
                    "Média tempo de sistema: {1}\n" +
                    "Média tempo entre chegadas: {2}\n" +
                    "Média tempo de serviço: {3}"
                    , newReplica.AvgQueuingTime, newReplica.AvgSystemTime, newReplica.AvgTimeBetweenArrivals, newReplica.AvgServiceTime
                ));
            #endregion

            #endregion


            Console.ReadKey();
		}

        private static decimal GetRandomTimeBetweeArrival(decimal timeBetweenArrivals)
        {
            if (timeBetweenArrivals <= 0.52M)
                return 1;
            else if (timeBetweenArrivals <= 0.8M)
                return 3;
            else if (timeBetweenArrivals <= 0.9M)
                return 5;
            else if (timeBetweenArrivals <= 0.96M)
                return 7;

            //else if (timeBetweenArrivals <= 1.0M)
            return 9;
        }

        private static decimal GetRandomServiceTime(decimal serviceTime)
        {
            if (serviceTime <= 0.5M)
                return 1.25M;
            else if (serviceTime <= 0.82M)
                return 3.75M;
            else if (serviceTime <= 0.9M)
                return 6.25M;
            else if (serviceTime <= 0.94M)
                return 8.75M;

            //else if (serviceTime <= 1.0M)
            return 11.25M;
        }
    }
}
