using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    class StartClass
    {
        public StartClass()
        {
            Machine machine = new Machine();
            char exit = '\0';
            while (exit != 'X')
            {
                if (User.money <= 0)
                {
                    Console.WriteLine("Sorry, you ain't got no money. Press any key to exit");
                    Console.ReadKey();
                    return;
                }
                machine.showMenu();
                if (machine.getOrder())
                {
                    if (machine.getCoins())
                    {
                        int change = machine.returnChange();
                        if (change != -1)
                        {
                            User.money += change;
                            Console.WriteLine("Enter X to exit the game. To continue press any other key");
                            exit = Console.ReadKey().KeyChar;
                        }
                    }
                }
            }
        }
    }
}
