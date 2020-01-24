using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace HotelAdmin
{
    internal static class DataController
    {
        
       static Dictionary<Type, int> typeDict = new Dictionary<Type, int>
        {
            {typeof(Hotel),0},
            {typeof(Person),1},
        };
        
        internal static List<Hotel> GetAllHotelsList()
        {
            return LoadFileAndParse<Hotel>();
        }


        internal static List<T> LoadFileAndParse<T>()
        {
            string content = String.Empty;
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            switch (typeDict[typeof(T)])
            {
                case 0:
                {
                   content = File.ReadAllText($"{dir}/HotelsData.txt");
                } break;
            }
            
            return JsonConvert.DeserializeObject<List<T>>(content);
        }

        internal static void UpdateInformationHotels(List<Hotel> hotels)
        {
            FileStream fs;
            string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/HotelsData.txt";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                fs = File.Create(filePath);
            }
            else
            {
                fs = File.Create(filePath);
            }
            
            if(fs == null)
            fs = File.OpenWrite(filePath);

           string json = JsonConvert.SerializeObject(hotels);
           var bytes = Encoding.UTF8.GetBytes(json);
           fs.Write(bytes,0,bytes.Length);
           fs.Close();
        }
        
        internal static void SerializeAndSaveToFileDataPersons(Person newPerson)
        {
           string content = String.Empty;
           string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}/Persons.txt";
           List<Person> persons = new List<Person>();
           bool fileExist = File.Exists(filePath);
           if (fileExist)
           {
               try
               {
                   content = File.ReadAllText(filePath);
                   persons = JsonConvert.DeserializeObject<List<Person>>(content);
               }
               catch
               {
                   content = String.Empty;
                   persons = new List<Person>();
               }
           }
           
           persons.Add(newPerson);

           FileStream fs;
           content = JsonConvert.SerializeObject(persons);
           if (!fileExist)
           {
               fs = File.Create(filePath);
           }
           else
           {
               fs = File.OpenWrite(filePath);
           }

           var bytes = Encoding.UTF8.GetBytes(content);
           fs.Write(bytes,0,bytes.Length);
           fs.Close();
        }
    }
}