using System.Collections.Generic;

namespace HotelAdmin
{
    [System.Serializable]
    public class Hotel
    {
        public string Name;
        public string Adress;
        public string Phone;
        public int Floors;
        public List<HotelNumber> Numbers;
    }
}