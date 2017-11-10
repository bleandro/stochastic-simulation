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

            int numberOfReplicas;
            Console.WriteLine("Quantidade de replicas a serem realizadas:");
            numberOfReplicas = Convert.ToInt32(Console.ReadLine());

            int numberOfClients;
            Console.WriteLine("Quantidade de testes a serem realizados:");
            numberOfClients = Convert.ToInt32(Console.ReadLine());
            #endregion

            #region Lists
            List<Arrival> listQueuing = new List<Arrival>();
            List<Arrival> listArrivals = new List<Arrival>();
            List<Replica> listReplicas = new List<Replica>();
            #endregion

            #region Round
            listReplicas.Clear();

            #region Replica
            for (int wCont = 0; wCont < numberOfReplicas; wCont++)
            {
                listArrivals.Clear();
                listQueuing.Clear();

                RandomNumberGenerator random = new RandomNumberGenerator(SEED);
                SEED += 2;

                #region Process
                decimal timeBetweenArrivals = 0;
                decimal serviceTime = 0;
                decimal timeCurrent = 0;
                decimal initialTime = 0;
                decimal finalTime = 0;
                decimal queuingTime = 0;
                decimal freeTime = 0;

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
                    /*
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
                    */
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
                decimal avgQueuingTime = 0;
                decimal avgSystemTime = 0;
                decimal avgTimeBetweenArrivals = 0;
                decimal avgServiceTime = 0;
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
                        "\n\n\n" +
                        "Média tempo de fila: {0}\n" +
                        "Média tempo de sistema: {1}\n" +
                        "Média tempo entre chegadas: {2}\n" +
                        "Média tempo de serviço: {3}"
                        , newReplica.AvgQueuingTime, newReplica.AvgSystemTime, newReplica.AvgTimeBetweenArrivals, newReplica.AvgServiceTime
                    ));
                #endregion

            }
            #endregion
            
            decimal avgQueuingTimeAverage = 0;
            decimal avgServiceTimeAverage = 0;
            decimal avgSystemTimeAverage = 0;
            decimal avgTimeBetweenArrivalsAverage = 0;

            decimal avgQueuingTimeAverageSquare = 0;
            decimal avgServiceTimeAverageSquare = 0;
            decimal avgSystemTimeAverageSquare = 0;
            decimal avgTimeBetweenArrivalsAverageSquare = 0;

            foreach (Replica replica in listReplicas)
            {
                //Average of Averages Sum(M)
                avgQueuingTimeAverage += replica.AvgQueuingTime;
                avgServiceTimeAverage += replica.AvgServiceTime;
                avgSystemTimeAverage += replica.AvgSystemTime;
                avgTimeBetweenArrivalsAverage += replica.AvgTimeBetweenArrivals;

                //Square Averages Sum(M²)
                avgQueuingTimeAverageSquare += (replica.AvgQueuingTime * replica.AvgQueuingTime);
                avgServiceTimeAverageSquare += replica.AvgServiceTime * replica.AvgServiceTime;
                avgSystemTimeAverageSquare += replica.AvgSystemTime * replica.AvgSystemTime;
                avgTimeBetweenArrivalsAverageSquare += replica.AvgTimeBetweenArrivals * replica.AvgTimeBetweenArrivals;
            }

            // Calculating Average
            avgQueuingTimeAverage = avgQueuingTimeAverage / numberOfReplicas;
            avgServiceTimeAverage = avgServiceTimeAverage / numberOfReplicas;
            avgSystemTimeAverage = avgSystemTimeAverage / numberOfReplicas;
            avgTimeBetweenArrivalsAverage = avgTimeBetweenArrivalsAverage / numberOfReplicas;
            
            // (Sum(M)²) / n
            decimal varQueuingTime = (avgQueuingTimeAverage * avgQueuingTimeAverage) / numberOfReplicas;
            decimal varServiceTime = (avgServiceTimeAverage * avgServiceTimeAverage) / numberOfReplicas;
            decimal varSystemTime = (avgSystemTimeAverage * avgSystemTimeAverage) / numberOfReplicas;
            decimal varTimeBetweenArrivals = (avgTimeBetweenArrivalsAverage * avgTimeBetweenArrivalsAverage) / numberOfReplicas;

            // Calculating Var
            varQueuingTime = ((avgQueuingTimeAverageSquare /numberOfReplicas) - varQueuingTime) / (numberOfReplicas - 1);
            varServiceTime = ((avgServiceTimeAverageSquare / numberOfReplicas) - varServiceTime) / (numberOfReplicas - 1);
            varSystemTime = ((avgSystemTimeAverageSquare / numberOfReplicas) - varSystemTime) / (numberOfReplicas - 1);
            varTimeBetweenArrivals = ((avgTimeBetweenArrivalsAverageSquare / numberOfReplicas) - varTimeBetweenArrivals) / (numberOfReplicas - 1);

            const decimal T95 = 1.96M;
            const decimal T99 = 1.28M;

            decimal sqrtN = Convert.ToDecimal(Math.Sqrt((double)numberOfReplicas));

            decimal sqrtVarQueuingTime = Convert.ToDecimal(Math.Sqrt((double)varQueuingTime));
            decimal sqrtVarServiceTime = Convert.ToDecimal(Math.Sqrt((double)varServiceTime));
            decimal sqrtVarSystemTime = Convert.ToDecimal(Math.Sqrt((double)varSystemTime));
            decimal sqrtVarTimeBetweenArrivals = Convert.ToDecimal(Math.Sqrt((double)varTimeBetweenArrivals));

            decimal intervalQueuingTime95 = Math.Abs(avgQueuingTimeAverage + (T95 * sqrtVarQueuingTime / sqrtN));
            decimal intervalQueuingTime99 = Math.Abs(avgQueuingTimeAverage + (T99 * sqrtVarQueuingTime / sqrtN));

            decimal intervalServiceTime95 = Math.Abs(avgServiceTimeAverage + (T95 * sqrtVarServiceTime / sqrtN));
            decimal intervalServiceTime99 = Math.Abs(avgServiceTimeAverage + (T99 * sqrtVarServiceTime / sqrtN));

            decimal intervalSystemTime95 = Math.Abs(avgSystemTimeAverage + (T95 * sqrtVarSystemTime / sqrtN));
            decimal intervalSystemTime99 = Math.Abs(avgSystemTimeAverage + (T99 * sqrtVarSystemTime / sqrtN));

            decimal intervalTimeBetweenArrivals95 = Math.Abs(avgTimeBetweenArrivalsAverage + (T95 * (sqrtVarTimeBetweenArrivals / sqrtN)));
            decimal intervalTimeBetweenArrivals99 = Math.Abs(avgTimeBetweenArrivalsAverage + (T99 * (sqrtVarTimeBetweenArrivals / sqrtN)));

            Console.Write("\n\nReplicas: \n\n");

            Console.WriteLine(
                String.Format(
                    "Media das medias tempo de fila: {0}\n" +
                    "Media das medias tempo de serviço: {1}\n" +
                    "Media das medias tempo de sistema: {2}\n" +
                    "Media das medias tempo entre chegadas: {3}\n" +
                    "\n" +
                    "Variancia tempo de fila: {4}\n" +
                    "Variancia tempo de serviço: {5}\n" +
                    "Variancia tempo de sistema: {6}\n" +
                    "Variancia tempo entre chegadas: {7}\n"
                    ,
                    avgQueuingTimeAverage,
                    avgServiceTimeAverage,
                    avgSystemTimeAverage,
                    avgTimeBetweenArrivalsAverage,

                    varQueuingTime,
                    varServiceTime,
                    varSystemTime,
                    varTimeBetweenArrivals
            ));

            Console.WriteLine(
                $"Intervalo 95 tempo de fila: +-{intervalQueuingTime95}\n" +
                $"Intervalo 95 tempo de serviço: +-{intervalServiceTime95}\n" +
                $"Intervalo 95 tempo de sistema: +-{intervalSystemTime95}\n" +
                $"Intervalo 95 tempo entre chegadas: +-{intervalTimeBetweenArrivals95}\n\n" +

                $"Intervalo 99 tempo de fila: +-{intervalQueuingTime99}\n" +
                $"Intervalo 99 tempo de serviço: +-{intervalServiceTime99}\n" +
                $"Intervalo 99 tempo de sistema: +-{intervalSystemTime99}\n" +
                $"Intervalo 99 tempo entre chegadas: +-{intervalTimeBetweenArrivals99}\n"
            );

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
