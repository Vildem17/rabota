using System;
using System.Transactions;
using Newtonsoft.Json;

namespace HotelAdmin
{
    [System.Serializable]
    public class HotelNumber
    {
        public int Free, Places, Price;
        public string Type;
        internal TypeNumberHotel typeNumber;
        
        [JsonConstructor]
        public HotelNumber(string Type,int Free,int Places,string Condition,int Price)
        {
            TypeNumberHotel.TryParse(Type,out typeNumber);
            this.Free = Free;
            this.Places = Places;
            this.Price = Price;
            this.Type = Type;
        }

        internal string GetTypeNumberForLocalization()
        {
            switch (typeNumber)
            {
                case TypeNumberHotel.eco : return "Эконом";
                case TypeNumberHotel.standart : return "Стандарт";
                case TypeNumberHotel.lux : return "Люкс";
            }
            
            return String.Empty;
        }
    }

    internal enum TypeNumberHotel
    {
        eco,
        standart,
        lux
    }
}