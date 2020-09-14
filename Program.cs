using System;
using System.Collections.Generic;

namespace EpamTest
{
	class Program
    {
        public static int firsStep = 224 / 2;

		public static string[] array = new string[225];

		static void Main(string[] args)
        {
			List<int> firstPlayerSteps = new List<int>();
			List<int> secondPlayerSteps = new List<int>();
			List<int> PlayerOneFirstAndLast = new List<int>();
			List<int> PlayerTwoFirstAndLast = new List<int>();
			PlayerOneFirstAndLast.Add(0);
			PlayerOneFirstAndLast.Add(0);
			PlayerTwoFirstAndLast.Add(0);
			PlayerTwoFirstAndLast.Add(0);

			initialArray(array);

			User player1 = new User("X", "0", firstPlayerSteps, secondPlayerSteps,0, PlayerOneFirstAndLast, PlayerTwoFirstAndLast);
			User player2 = new User("0", "X", secondPlayerSteps, firstPlayerSteps,0, PlayerTwoFirstAndLast, PlayerOneFirstAndLast);
			
			//FIRST STEP
			player1.firstStep();
			player2.firstStep();

			//ATTACK
			while(player1.biggest != 5 || player2.biggest != 5)
            {
				if(player2.biggest == 3)
                {
					player1.defend(player2.choice);
                }
                else
                {
					player1.attack();
				}
				if(player1.biggest == 5)
                {
					Console.WriteLine("Player 1 won!");
					Console.WriteLine();
					showArray(array);
					Console.WriteLine();
					return;
                }

				if (player1.biggest == 3)
                {
					player2.defend(player1.choice);
                }
                else
                {
					player2.attack();
				}
				if (player2.biggest == 5)
				{
					Console.WriteLine("Player 2 won!");
					Console.WriteLine();
					showArray(array);
					Console.WriteLine();
					return;
				}
			}
			
		}

		static void showArray(string[] Array)
		{
			for (int i = 0; i < Array.Length; i++)
			{
				if (i % 15 == 0)
				{
					Console.WriteLine();
					Console.Write(Array[i]);
				}
				else
				{
					Console.Write(" ");
					Console.Write(Array[i]);
				}
			}
		}
		static void initialArray(string[] Array)
		{
			for (int i = 0; i < Array.Length; i++)
			{
				Array[i] = "-";
			}
		}

		public class User
		{
			string playerSign;
			string opponentSign;

			public int biggest { get; set; }

			public bool defender = false;

			List<int> mySteps;
			List<int> opponentSteps;
			List<int> myFirstAndLast;
			List<int> opFirstAndLast;

			public HashSet<int> S = new HashSet<int>();

			//Choice - 1 = Horizontal
			//Choice - 2 = Vertical
			//Choice - 3 = DiagonalRight
			//Choice - 4 = DiagonalLeft
			public int choice = 0;

			public int firstPos = 0;
			public int lastPos = 0;


			public User(): this("","",null,null,0, null, null)
            {

            }
			public User(string sign):this("","",null,null,0, null, null)
            {
				playerSign = sign;
            }

			public User(
				string sign, 
				string opponent, 
				List<int> me, 
				List<int> oppon, 
				int biggestSeq, 
				List<int> _myFirstAndLast, 
				List<int> _opponentFirstAndLast)
			{
				playerSign = sign;
				opponentSign = opponent;
				mySteps = me;
				opponentSteps = oppon;
				biggest = biggestSeq;
				myFirstAndLast = _myFirstAndLast;
				opFirstAndLast = _opponentFirstAndLast;
			}

			public void firstStep()
            {
				if (array[firsStep] == "-") 
				{
					array[firsStep] = playerSign;
					firstPos = firsStep;
					mySteps.Add(firstPos);
                }
                else
                {
					defender = true;
					Random r = new Random();
					firstPos = r.Next(50, 150);
					while(firstPos == firsStep)
                    {
						firstPos = r.Next(50, 150);
					}
					array[firstPos] = playerSign;
					mySteps.Add(firstPos);
				}
				
			}   

			public void attack()
			{
				
				if (choice == 0)
                {
					choice = 1;
                }
                else if(mySteps.Count >= 2 && checkFutureSteps(mySteps, choice, S) == false)
                {
					chooseChoice(S);
					Console.WriteLine("WE ARE IN CICLE!!");
					attack();
				}
				//else if (mySteps.Count >= 2 && checkFutureSteps(mySteps, choice, S) == true)
				//{
				//	chooseChoice(S);
				//}

				//Собственно Сам Ход
				if(choice != 0)
                {
					if(mySteps.Count > 1)
                    {
						setFirstAndLast();
                    }
					if(lastPos != 0)
                    {
						if (array[lastPos + choice] != opponentSign && array[lastPos + choice] != playerSign && (lastPos + choice) % 15 != 0)
						{
							array[lastPos + choice] = playerSign;
							lastPos = lastPos + choice;
							mySteps.Add(lastPos);
						}
						else if (array[firstPos - choice] != opponentSign && array[firstPos - choice] != playerSign && (firstPos - choice) % 15 != 0)
						{
							array[firstPos - choice] = playerSign;
							firstPos = firstPos - choice;
							mySteps.Add(firstPos);
						}
						else
						{
							Console.Write("Something happend while attaked!!!");
							Console.ReadLine();
						}
						biggest = checkDiagonalVerticalHorizontal(mySteps, mySteps.Count, S, choice);
					}
                    else
                    {
						if (array[firstPos + choice] != opponentSign && (firstPos + choice)%15 != 0)
						{
							array[firstPos + choice] = playerSign;
							lastPos = firstPos + choice;
							mySteps.Add(lastPos);
						}
						else
						{
							array[firstPos - choice] = playerSign;
							lastPos = firstPos - choice;
							mySteps.Add(lastPos);
						}
					}
					
                }

			}

			public void defend(int opChoice)
            {
				if(array[opFirstAndLast[0] - opChoice] == "-") 
				{
					array[opFirstAndLast[0] - opChoice] = playerSign;
				} else if(array[opFirstAndLast[1] + opChoice] == "-")
                {
					array[opFirstAndLast[1] + opChoice] = playerSign;
				}
			}

			public int returnBiggest()
            {
				return this.biggest;
            }

			public void setFirstAndLast()
            {
				firstPos = mySteps[0];
				lastPos = mySteps[mySteps.Count - 1];
				myFirstAndLast[0] = firstPos;
				myFirstAndLast[1] = lastPos;

			}

			public void chooseChoice(HashSet<int> S)
            {
				int horizontal = checkDiagonalVerticalHorizontal(mySteps, mySteps.Count, S, 1);
				int vertical = checkDiagonalVerticalHorizontal(mySteps, mySteps.Count, S, 15);
				int diagonalR = checkDiagonalVerticalHorizontal(mySteps, mySteps.Count, S, 16);
				int diagonalL = checkDiagonalVerticalHorizontal(mySteps, mySteps.Count, S, 14);

				biggest = Math.Max(Math.Max(Math.Max(horizontal, vertical), diagonalR), diagonalL);

				if(biggest == horizontal)
                {
					if (checkFutureSteps(mySteps, 1, S) == true)
						choice = 1;
				}
				else if (biggest == vertical)
				{
					if (checkFutureSteps(mySteps, 15, S) == true)
						choice = 15;
				}
				else if (biggest == diagonalR)
				{
					if (checkFutureSteps(mySteps, 15, S) == true)
						choice = 15;
				}
				else if (biggest == diagonalL)
				{
					if (checkFutureSteps(mySteps, 15, S) == true)
						choice = 15;
				}
				else
				{
					choice = 1;
				}
			}

			public bool checkFutureSteps(List<int> arr, int step, HashSet<int> S)
            {
				bool ans = false;

				for (int i = 0; i < arr.Count; ++i)
				{
					S.Add(arr[i]);
				}

				for (int i = 0; i < arr.Count; i += step)
				{
					if (!S.Contains(arr[i] - 1))
					{
						//Проверка на доступность будущих ходов
						int position = arr[i]-5*step;
						int count=0;
						for (int p = 0; p < 10; p++)
						{
							if(array[position] == playerSign || array[position] == "-")
                            {
								count++;

                                if (count == 5)
                                {
									return true;
                                }
                            }
                            else
                            {
								if (count == 5)
								{
									return true;
								}

								count = 0;
							}
							position += step;
						}
					}
				}

				return ans;
            }

			public int checkDiagonalVerticalHorizontal(List<int> arr, int n, HashSet<int> S, int step)
			{
				int ans = 0;
				S.Clear();

				for (int i = 0; i < n; ++i)
				{
					S.Add(arr[i]);
				}

				for (int i = 0; i < n; i+=step)
				{
					if (!S.Contains(arr[i] - 1))
					{
						int j = arr[i];
						while (S.Contains(j))
						{
							j+=step;
						}
						if (ans < (j - arr[i])/step)
						{
							ans = (j - arr[i])/step;
						}
					}
				}
				return ans;
			}
		}
	}
}