using System;
using System.Collections.Generic;

namespace NumberGuesser
{
    class Program
    {
        static void Main(string[] args)
        {
            String userName;                                        //Имя пользователя
            LinkedList<int> userAnswers = new LinkedList<int>();    //Его ответы

            int counter = 0;                                        
            int curAnswer;                                          

            Random randomizer = new Random();                       //Генератор числа и "унижающих человеческое достоинство" ответов
            int number = randomizer.Next() % 101;                   //Искомое число
            
            Console.WriteLine("What's your name, stranger?");       
            userName = Console.ReadLine();

            String[] systemAnswers = new String[] { "\nOne more pathetic attempt, " + userName + ", one more...\n",
                "\nAre you even trying, " + userName + "?\n",
                "\n" + userName + ", I find your mental abilities so depressing...\n",
                "\nAnd you still have not guessed, " + userName + ". Why am I not surprised?\n",
                "\nMaybe you should play another game? You know, more simple one.\n",
                "\nGosh... " + userName + ", are you really that stupid or just pretending?\n",
                "\nShame on you, " +  userName + "! And on me 'cause I still write back to you...\n"};    
            
            DateTime startTime = DateTime.Now;                       //Запуск таймера

            Console.WriteLine("So, " + userName + ", try to guess a number. Try to the bitter end or go away in shame (press 'q')");
            while (true)
            {
                //Считывание ответа пользователя
                var str = Console.ReadLine();

                //Попытка распарсить в int                
                if (int.TryParse(str, out curAnswer))
                {                  
                    userAnswers.AddLast(curAnswer);

                    //Если пользователь угадал число
                    if (curAnswer == number)                            
                    {                                              
                        Console.WriteLine(String.Format("\nMy congratulations, " + userName + "! You win in {0} minut(es) after {1} attempt(s)",       
                            (DateTime.Now - startTime).Minutes, userAnswers.Count));
                        //Вывод всех попыток и знаков сравнения
                        Console.WriteLine("Game log:");
                        foreach (int answer in userAnswers)                                  
                        {
                            Console.WriteLine(answer + ((answer > number) ? (" >") : ((answer < number) ? (" <") : (" ="))));  
                        }
                        break;                         
                    }

                    if (curAnswer > number)                
                    {
                        Console.WriteLine("less");
                    }
                    else
                    {
                        Console.WriteLine("more");
                    }

                    counter = (counter + 1) % 4;    
                    if (counter == 0)               
                    {
                        Console.WriteLine(systemAnswers[randomizer.Next() % systemAnswers.Length]);
                    }
                }
                //Если пользователь ввел не число
                else
                {
                    //Пользователь решил позорно смыться
                    if (str == "q")     
                    {
                        Console.WriteLine("I didn't think that it's so easy to beat you, " + userName + "...");
                        break;
                    }
                    //Если пользователь ввел что-то помимо числа и команды на выход
                    else
                    {
                        Console.WriteLine("Khm, " + userName + ", I really have to remind you that I only understand numbers and command 'q'?");   
                    }
                }
            }
            Console.ReadKey();     
        }
    }
}
