using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    class Machine
    {
        private const int productNum = 3; //сколько различных продуктов
        private int[] initialProductsNum = { 4, 3, 10 }; //начальное количество каждого типа продуктов. Через переменные делать плохо, вдруг захочется еще что то добавить
        private int[] costs = { 50, 10, 30 }; //соответствующие стоимости
        private List<int> products; //список, содержащий количество продуктов
        private List<String> codes; //коды продуктов
        private List<int> coins; //список доступных монет
        private Dictionary<int, int> coinsCnt; //сколько монет поступило в оборот астомата для выдачи сдачи
        private int order; //номер заказа
        private int usersMoney = 0;
        public Machine()
        {
            //инициализация списков начальными данными
            products = new List <int>();
            for (int i = 0; i < productNum; ++i) 
                products.Add(initialProductsNum[i]);
            codes = new List<string> { "01", "02", "03" };
            coins = new List<int> { 1, 2, 5, 10 };
            coinsCnt = new Dictionary<int, int>();
            for (int i = 0; i < coins.Count; ++i)
                coinsCnt[coins[i]] = 0;
        }
        public void showMenu()
        {
            Console.Clear();
            Console.WriteLine("Hey! Enter a code to make an order (your balance is " + usersMoney + ")");
            Console.WriteLine(codes[0] + " - Cakes (" + costs[0] + " rubles), in stock " + products[0]);
            Console.WriteLine(codes[1] + " - Cookies (" + costs[1] + " rubles), in stock " + products[1]);
            Console.WriteLine(codes[2] + " - Waffles (" + costs[2] + " rubles), in stock " + products[2]);
        }

        public bool getOrder()
        {
            //ф-ция обрабатывает код продукта, в случае ошибки пользователь вернется в начальное меню
            String orderCode = Console.ReadLine();
            if (!codes.Contains(orderCode)) //если введенного кода нет в списке, то ошибка
            {      
                Console.WriteLine("Wrong code value. Press any key and try again");
                Console.ReadKey();
                return false;
            }
            order = codes.IndexOf(orderCode); //иначе получаем индекс заказа
            if (products[order] == 0) //а если заказа нет, то опять же пользователь попадет в меню
            {
                Console.WriteLine("This type of products is over. You can choose something else. Press any key.");
                Console.ReadKey();
                return false;
            }
            return true;
        }

        public bool getCoins()
        {
            //ф-ция обрабатывает ввод монет, проверяя правильность номинала
            Console.WriteLine("Please enter coin and press Enter (by value, i.e. 1 2 5 or 10). To continue enter 'O', to exit to main menu enter 'X'");
            while (true)
            {
                String str = Console.ReadLine();
                if (str == "X") //если выход, то пользователь попадет в меню
                    return false;
                if (str == "O")
                {
                    if (usersMoney >= costs[order]) //если денег достаточно отдаем продукт, уменьшаем его количество и идем дальше
                    {
                        Console.WriteLine("Please, take your order");
                        products[order]--;
                        usersMoney -= costs[order];
                        return true;
                    }
                    else
                        Console.WriteLine("Sorry, not enough money, add some coins"); //иначе просим добавить монет
                }
                else
                {
                    int currentCoin;
                    bool isNum = Int32.TryParse(str, out currentCoin);
                    if (isNum && coins.Contains(currentCoin)) //если пользователь ввел правильную монету (и вообще монету?) то увеличиваем его баланс
                    {
                        coinsCnt[currentCoin]++; //увеличиваем количество текущего номинала внутри банкомата
                        usersMoney += currentCoin; //деньги внутри банкомата
                        User.money -= currentCoin; //деньги снаружи, то есть у пользователя
                        if (User.money < 0)
                            return false;
                        Console.WriteLine("Current amount of money is " + usersMoney);
                    }
                    else
                        Console.WriteLine("Incorrect type of coin. Please Try again");
                }
            }
        }

        public int returnChange ()
        {
            // ф-ция рассчитывает сдачу минимальным образом
            Console.WriteLine("To get your change enter 'C', to make one more order press any other key");
            //в данной задаче монеты хорошие, поэтому простой жадный алгоритм
            String str = Console.ReadLine();
            if (str != "C")
                return -1;
            Console.WriteLine ("Your change is");
            int cnt = 0, change = usersMoney;
            if (change == 0)
            {
                Console.WriteLine("0");
                return 0;
            }
            for (int i = coins.Count-1; i >= 0; i--) //на каждой итерации берем максимально возможное значение
            {
                cnt = usersMoney / coins[i]; //считаем сколько нужно монет данного типа
                cnt = Math.Min(coinsCnt[coins[i]], cnt); //берем столько сколько надо, учитывая их количество в автомате
                usersMoney -= cnt * coins[i]; //смотрим остаток
                coinsCnt[coins[i]] -= cnt; //уменьшаем количество взятых монет
                if (cnt != 0)
                    Console.WriteLine(cnt + " coins the value of " + coins[i]);
                if (usersMoney == 0)
                    break;
            }
            if (usersMoney != 0)
            {
                Console.WriteLine("No money to give a change");
                return 0;
            }
            return change;
        }

    }
}
