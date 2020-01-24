using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAdmin
{
    internal static class DialogController
    {
        
        static List<Hotel> lastLoadedHotels = new List<Hotel>();
        internal static void FirstLaunch()
        {
            //Обновляем лист отелей и кэшируем его.
            UpdateHotelList();
            
            
            Console.WriteLine("Добро пожаловать в панель управления отелем.");
            Console.WriteLine("Примечание: Выберете отель, затем номер, в который вы хотите заселить человека.");
            Console.WriteLine("----Меню---");
            //Тут показываем список отелей.
            ShowAllHotelsForMainMenu();
            
            Console.WriteLine("Выберете нужный отель и введите цифру.");

            int selectedNumber = -1;
            while (selectedNumber == -1)
            {
                Console.WriteLine("Выбрать отель -> ");
                try
                {
                    selectedNumber = Convert.ToInt32(Console.ReadLine());
                    ShowInfoHotel(selectedNumber);
                }
                catch
                {
                    Console.WriteLine("Некоректные данные, попробуйте ещё раз!");
                    selectedNumber = -1;
                }
            }
            
        }


        internal static void ShowInfoHotel(int selectedHotel) //Показываем информацию об отеле.
        {
            int firstNumber = selectedHotel;
            
            if(selectedHotel > lastLoadedHotels.Count)
             throw new Exception("Вышли за пределы доступных отелей.");

            selectedHotel = selectedHotel == 0 ? selectedHotel : selectedHotel - 1;

            Hotel hotel = lastLoadedHotels[selectedHotel];
            
            Console.Clear();
            
            Console.WriteLine($"Вы выбрали отель под номером -> {firstNumber}, что бы вернуться введите -1");
            Console.WriteLine($"Информация об отеле.");
            
            StringBuilder info = new StringBuilder();
            StringBuilder numbers = new StringBuilder();
            info.AppendLine($"Название отеля: {hotel.Name}");
            info.AppendLine($"Адрес: {hotel.Adress}");
            info.AppendLine($"Номер телефона: {hotel.Phone}");
            info.AppendLine($"Этажей: {hotel.Floors}");
            
            //Подсчитываем свободные номера и создаём список.
            int allFreeNumbers = 0;
            int i = 0;
            foreach (var number in hotel.Numbers)
            {
                string typeNumberLoc = number.GetTypeNumberForLocalization();
                
                if(typeNumberLoc == String.Empty)
                    continue;
                
                if(number.Free == 0)
                    continue;
                
                allFreeNumbers += number.Free;
                numbers.AppendLine($"Номер ({i}): {typeNumberLoc}, свободных: {number.Free}");
                i++;
            }
            
            info.AppendLine($"Свободных номеров всего: {allFreeNumbers}");
            info.Append(numbers);
            Console.WriteLine(info);
            
            
            //Получаем ответ от пользователя.
            int selectedAction = -2;
            while (selectedAction == -2)
            {
                try
                {
                    Console.WriteLine("Выберите номер ->");
                    selectedAction = Convert.ToInt32(Console.ReadLine());
                    MoveToNumbersOrMenu(selectedAction,hotel, selectedHotel);
                }
                catch
                {
                    Console.WriteLine("Вы ввели не допустимое значение. Что бы выйти в меню, введите -1");
                    selectedAction = -2;
                }
            }
           
        }


        //Переход к нужному типу меню.
        private static void MoveToNumbersOrMenu(int selectedNumber,Hotel hotel, int selectedHotel)
        {
            if(selectedNumber > hotel.Numbers.Count)
                throw new Exception("Вышли за пределы доступных номеров.");

            if (selectedNumber == -1)
            {
                Console.Clear();
                FirstLaunch();
            }

            selectedNumber = selectedNumber == 0 ? selectedNumber : selectedNumber - 1;


            if (hotel.Numbers[selectedNumber].Free == 0)
            {
                throw new Exception("Все номера заняты.");
            }
            //Показываем информацию о номере отеля.
            ShowInfoHotelNumber(hotel.Numbers[selectedNumber],hotel,selectedHotel);
        }


        internal static void ShowInfoHotelNumber(HotelNumber hotelNumber, Hotel hotel, int selectedHotel)
        {
            Console.Clear();
            
            Console.WriteLine($"Вы выбрали номер в отеле типа -> {hotelNumber.GetTypeNumberForLocalization()}, что бы вернуться назад введите n");
            Console.WriteLine($"Информация об номере в отеле {hotel.Name}.");
            
            StringBuilder info = new StringBuilder();
            info.AppendLine($"Свободных номеров данного типа: {hotelNumber.Free}");
            info.AppendLine($"Вместимость : {hotelNumber.Places}");
            info.AppendLine($"Цена за сутки: {hotelNumber.Price} рублей.");
            
           
            
            string selectedAction = String.Empty;

            while (selectedAction == String.Empty)
            {
                Console.WriteLine("Что бы заселить человека введетие (Y), что бы вернуться назад введите (N).");
                Console.WriteLine("-> ");
                try
                {
                    string result = Console.ReadLine();
                    selectedAction = result;
                    MoveToRegOrNumbers(result, selectedHotel,hotel,hotelNumber);
                }
                catch
                {
                    Console.WriteLine("Вы ввели не корректные данные. Попробуйте ещё раз.");
                    selectedAction = String.Empty;
                }
            }

        }

        private static void MoveToRegOrNumbers(string selectedAction, int selectedHotel, Hotel hotel, HotelNumber hotelNumber)
        {
            if (selectedAction == "y" || selectedAction == "Y")
            {

                string fiog, phoneg, passportg;
                Console.WriteLine("Введите Ф.И.О жильца -> ");
                fiog = Console.ReadLine();
                Console.WriteLine("Введите номер телефона жильца -> ");
                phoneg = Console.ReadLine();
                Console.WriteLine("Введите серию и номер паспорта жильца -> ");
                passportg = Console.ReadLine();


                int days = -1;
                while (days == -1)
                {
                    try
                    {
                        Console.WriteLine("Введите сколько дней будет проживать жилец -> ");
                        days = Convert.ToInt32(Console.ReadLine());
                        if (days <= 0)
                        {
                            throw new Exception("Не верное колличество дней.");
                        }
                    }
                    catch
                    {
                        days = -1;
                        Console.WriteLine("Некорректо введены данные, введите число! ");
                    }
                }

                int finalPrice = hotelNumber.Price * days;

                Person person = new Person()
                {
                    fio =  fiog,
                    phone = phoneg,
                    passport = passportg,
                    hotelName = hotel.Name,
                    typeNumber = hotelNumber.GetTypeNumberForLocalization(),
                    FinalPrice = finalPrice
                };
                
                DataController.SerializeAndSaveToFileDataPersons(person);
                hotelNumber.Free--;
                DataController.UpdateInformationHotels(lastLoadedHotels);
                Console.Clear();
               
                Console.WriteLine($"Вы успешно заселили {fiog} в отель {hotel.Name} с уровнем комфортности  {hotelNumber.GetTypeNumberForLocalization()}. Итого: {finalPrice}");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                //Перезапускаем цикл работы программы.
                FirstLaunch();
            }else if (selectedAction == "n" || selectedAction == "N")
            {
                Console.Clear();
                ShowInfoHotel(selectedHotel);
            }
            else
            {
                throw new Exception("Не корректная информация.");
            }
        }
        
        
        internal static void ShowAllHotelsForMainMenu()
        {
            int i = 0;
            foreach (Hotel hotel in lastLoadedHotels)
            {
                Console.WriteLine($"Гостиница ({i}): {hotel.Name}");
                i++;
            }
        }
        

        private static void UpdateHotelList()
        {
            lastLoadedHotels = DataController.GetAllHotelsList();
        }
    }
}