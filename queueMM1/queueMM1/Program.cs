using System;
using System.Collections.Generic;

namespace queueMM1
{
	public class Program
	{
        /*
         * Programa principal
         * Lê as informações e realiza os cálculos, simulando a fila
         */

        public static void Main()
        {
            #region Inputs

            //Armazena o valor que será usado como a semente inicial
            int SEED;
            Console.WriteLine("Valor inicial ou semente:");
            SEED = Convert.ToInt32(Console.ReadLine());

            //Armazena o número de réplicas que serão geradas
            int numberOfReplicas;
            Console.WriteLine("Quantidade de replicas a serem realizadas:");
            numberOfReplicas = Convert.ToInt32(Console.ReadLine());

            //Armazena o número de clientes (testes) que serão gerados para cada réplica
            int numberOfClients;
            Console.WriteLine("Quantidade de testes a serem realizados:");
            numberOfClients = Convert.ToInt32(Console.ReadLine());
            #endregion

            #region Lists
            List<Arrival> listQueuing = new List<Arrival>(); //Lista que armazena quantidade de clientes na fila
            List<Arrival> listArrivals = new List<Arrival>(); //Lista que armazena cada chegada de cliente
            List<Replica> listReplicas = new List<Replica>(); //Lista que armazena os dados de cada réplica
            #endregion

            #region Round
            listReplicas.Clear();

            #region Replica
            for (int wCont = 0; wCont < numberOfReplicas; wCont++)
            {
                //Limpa listas para começar nova réplica
                listArrivals.Clear();
                listQueuing.Clear();

                //Objeto para geração de números pseudo-aleatórios (iniciado com a semente)
                RandomNumberGenerator random = new RandomNumberGenerator(SEED);

                //Incremente semente para gerar números diferentes a cada réplica
                SEED += 1;

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

                    //Gera tempo entre chegadas de acordo com número aleatório gerado
                    timeBetweenArrivals = GetRandomTimeBetweeArrival(random.Next());

                    //Gera tempo de serviço de acordo com número aleatório gerado
                    serviceTime = GetRandomServiceTime(random.Next());

                    //Calcula tempo atual incrementando de acordo com os tempos entre as chegadas
                    timeCurrent += timeBetweenArrivals;

                    #region Cálculos
                    queuingTime = 0;
                    if (timeCurrent < finalTime)
                        queuingTime = (finalTime - timeCurrent);

                    // Se houver tempo de fila, precisamos adicionar um cliente à fila
                    if (queuingTime > 0)
                        listQueuing.Add(newArrival);

                    initialTime = timeCurrent + queuingTime;
                    freeTime = initialTime - finalTime;

                    //Calcula o "novo" tempo final
                    finalTime = initialTime + serviceTime;

                    #endregion

                    #region Criando um novo objeto para armazenar tempos de chegadas

                    //Objeto que armazena os dados de chegadas
                    newArrival.TimeBetweenArrivals = timeBetweenArrivals;
                    newArrival.ServiceTime = serviceTime;
                    newArrival.TimeCurrent = timeCurrent;
                    newArrival.QueuingTime = queuingTime;
                    newArrival.NumberOfPeopleInQueue = listQueuing.Count;
                    newArrival.FreeTime = freeTime;
                    newArrival.FinalTime = finalTime;
                    listArrivals.Add(newArrival);

                    #endregion
                                        
                    #region Decrementa pessoas na fila
                    for (int jCont = listQueuing.Count - 1; jCont >= 0; jCont--)
                    {
                        if (listQueuing[jCont].FinalTime < timeCurrent)
                            listQueuing.RemoveAt(jCont);
                    }
                    #endregion
                }

                #region Calcula estatísticas
                decimal avgQueuingTime = 0;
                decimal avgSystemTime = 0;
                decimal avgTimeBetweenArrivals = 0;
                decimal avgServiceTime = 0;

                //Calcula média para cada tempo da fila
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

                #region Criando objeto para armazenar dados de cada réplica

                Replica newReplica = new Replica();
                newReplica.AvgQueuingTime = avgQueuingTime;
                newReplica.AvgSystemTime = avgSystemTime;
                newReplica.AvgTimeBetweenArrivals = avgTimeBetweenArrivals;
                newReplica.AvgServiceTime = avgServiceTime;
                listReplicas.Add(newReplica);
                
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
                //Média das médias Sum(M)
                avgQueuingTimeAverage += replica.AvgQueuingTime;
                avgServiceTimeAverage += replica.AvgServiceTime;
                avgSystemTimeAverage += replica.AvgSystemTime;
                avgTimeBetweenArrivalsAverage += replica.AvgTimeBetweenArrivals;

                //Quadrado das médias Sum(M²)
                avgQueuingTimeAverageSquare += (replica.AvgQueuingTime * replica.AvgQueuingTime);
                avgServiceTimeAverageSquare += replica.AvgServiceTime * replica.AvgServiceTime;
                avgSystemTimeAverageSquare += replica.AvgSystemTime * replica.AvgSystemTime;
                avgTimeBetweenArrivalsAverageSquare += replica.AvgTimeBetweenArrivals * replica.AvgTimeBetweenArrivals;
            }

            // Calculando médias
            avgQueuingTimeAverage = avgQueuingTimeAverage / numberOfReplicas;
            avgServiceTimeAverage = avgServiceTimeAverage / numberOfReplicas;
            avgSystemTimeAverage = avgSystemTimeAverage / numberOfReplicas;
            avgTimeBetweenArrivalsAverage = avgTimeBetweenArrivalsAverage / numberOfReplicas;
            
            // (Sum(M)²) / n
            decimal varQueuingTime = (avgQueuingTimeAverage * avgQueuingTimeAverage) / numberOfReplicas;
            decimal varServiceTime = (avgServiceTimeAverage * avgServiceTimeAverage) / numberOfReplicas;
            decimal varSystemTime = (avgSystemTimeAverage * avgSystemTimeAverage) / numberOfReplicas;
            decimal varTimeBetweenArrivals = (avgTimeBetweenArrivalsAverage * avgTimeBetweenArrivalsAverage) / numberOfReplicas;

            // Calculando variâncias
            varQueuingTime = ((avgQueuingTimeAverageSquare /numberOfReplicas) - varQueuingTime) / (numberOfReplicas - 1);
            varServiceTime = ((avgServiceTimeAverageSquare / numberOfReplicas) - varServiceTime) / (numberOfReplicas - 1);
            varSystemTime = ((avgSystemTimeAverageSquare / numberOfReplicas) - varSystemTime) / (numberOfReplicas - 1);
            varTimeBetweenArrivals = ((avgTimeBetweenArrivalsAverageSquare / numberOfReplicas) - varTimeBetweenArrivals) / (numberOfReplicas - 1);


            #region Cálculo para o intervalo de confiança

            //Constante de T-student para o intervalo de confiança com precisão de 95% e 99%, respectivamente
            const decimal T95 = 1.96M;
            const decimal T99 = 1.28M;

            //Raiz(N)
            decimal sqrtN = Convert.ToDecimal(Math.Sqrt((double)numberOfReplicas));

            //Raiz(var)
            decimal sqrtVarQueuingTime = Convert.ToDecimal(Math.Sqrt((double)varQueuingTime));
            decimal sqrtVarServiceTime = Convert.ToDecimal(Math.Sqrt((double)varServiceTime));
            decimal sqrtVarSystemTime = Convert.ToDecimal(Math.Sqrt((double)varSystemTime));
            decimal sqrtVarTimeBetweenArrivals = Convert.ToDecimal(Math.Sqrt((double)varTimeBetweenArrivals));

            //Calculando os intervalos de confiança
            decimal intervalQueuingTime95 = Math.Abs(avgQueuingTimeAverage + (T95 * sqrtVarQueuingTime / sqrtN));
            decimal intervalQueuingTime99 = Math.Abs(avgQueuingTimeAverage + (T99 * sqrtVarQueuingTime / sqrtN));

            decimal intervalServiceTime95 = Math.Abs(avgServiceTimeAverage + (T95 * sqrtVarServiceTime / sqrtN));
            decimal intervalServiceTime99 = Math.Abs(avgServiceTimeAverage + (T99 * sqrtVarServiceTime / sqrtN));

            decimal intervalSystemTime95 = Math.Abs(avgSystemTimeAverage + (T95 * sqrtVarSystemTime / sqrtN));
            decimal intervalSystemTime99 = Math.Abs(avgSystemTimeAverage + (T99 * sqrtVarSystemTime / sqrtN));

            decimal intervalTimeBetweenArrivals95 = Math.Abs(avgTimeBetweenArrivalsAverage + (T95 * (sqrtVarTimeBetweenArrivals / sqrtN)));
            decimal intervalTimeBetweenArrivals99 = Math.Abs(avgTimeBetweenArrivalsAverage + (T99 * (sqrtVarTimeBetweenArrivals / sqrtN)));

            #endregion

            Console.Write("\n\nReplicas: \n\n");

            //Escreve na tela os resultados obtidos
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

            //Escreve na tela os resultados obtidos
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
            //Gera o tempo entre as chegadas baseado em um número aleatório

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
            //Gera o tempo de serviço baseado em um número aleatório

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
