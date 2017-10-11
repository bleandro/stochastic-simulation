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

			int SEED;
			Console.WriteLine("Valor inicial ou semente:");
			SEED = Convert.ToInt32(Console.ReadLine());

			RandomNumberGenerator random = new RandomNumberGenerator(SEED);

			int numberOfClients;
			Console.WriteLine("Quantidade de testes a serem realiza:");
			numberOfClients = Convert.ToInt32(Console.ReadLine());

			//Definitions
			decimal timeBetweenArrivals = 0;
			decimal serviceTime = 0;
			decimal timeCurrent = 0;
			decimal initialTime = 0;
			decimal finalTime = 0;
			decimal queuingTime = 0;
			decimal systemTime = 0;
			decimal numberOfPeopleInQueue = 0;
			decimal freeTime = 0;

			decimal[] lastFinalTimes = new decimal[10000];

			for (int iCont = 0; iCont < numberOfClients; iCont++)
			{
				timeBetweenArrivals = random.Next();
				serviceTime = random.Next();

				if (timeBetweenArrivals <= 0.52M)
					timeBetweenArrivals = 1;
				else if (timeBetweenArrivals <= 0.8M)
					timeBetweenArrivals = 3;
				else if (timeBetweenArrivals <= 0.9M)
					timeBetweenArrivals = 5;
				else if (timeBetweenArrivals <= 0.96M)
					timeBetweenArrivals = 7;
				else if (timeBetweenArrivals <= 1.0M)
					timeBetweenArrivals = 9;

				if (serviceTime <= 0.5M)
					serviceTime = 1.25M;
				else if (serviceTime <= 0.82M)
					serviceTime = 3.75M;
				else if (serviceTime <= 0.9M)
					serviceTime = 6.25M;
				else if (serviceTime <= 0.94M)
					serviceTime = 8.75M;
				else if (serviceTime <= 1.0M)
					serviceTime = 11.25M;

				timeCurrent += timeBetweenArrivals;

				queuingTime = 0;
				if (timeCurrent < finalTime)
					queuingTime = (finalTime - timeCurrent);

				if (queuingTime > 0)
					numberOfPeopleInQueue += 1;

				initialTime = timeCurrent + queuingTime;
				freeTime = initialTime - finalTime;
				finalTime = initialTime + serviceTime;
				systemTime = queuingTime + serviceTime;

				file.WriteLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
					timeBetweenArrivals, serviceTime, timeCurrent, initialTime, finalTime, queuingTime, systemTime, numberOfPeopleInQueue, freeTime
					));

				Console.WriteLine(
					String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
					timeBetweenArrivals, serviceTime, timeCurrent, initialTime, finalTime, queuingTime, systemTime, numberOfPeopleInQueue, freeTime
					)
					);

				for (int jCont = 0; jCont < 10000; jCont++)
				{
					if ((lastFinalTimes[jCont] > 0) && (lastFinalTimes[jCont] < timeCurrent))
					{
						lastFinalTimes[jCont] = 0;
						numberOfPeopleInQueue -= 1;
					}
				}

				if (queuingTime > 0)
					lastFinalTimes[iCont] = finalTime;
			}

			Console.ReadKey();
		}
	}
}
