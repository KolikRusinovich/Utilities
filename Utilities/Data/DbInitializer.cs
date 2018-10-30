using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Models;

namespace Utilities.Data
{
    public class DbInitializer
    {
        public static void Initialize(UtilitiesContext db)
        {
            db.Database.EnsureCreated();

            // Проверка занесены ли виды топлива
            if (db.Rates.Any())
            {
                return;   // База данных инициализирована
            }

            int rates_number = 35;
            int tenants_number = 35;
            int readings_number = 300;
            int payments_number = 300;
            string rateType;
            string name;
            string surname;
            string patronymic;
            string voc = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            int value;
            int apartmentNumber;
            int numberOfPeople;
            int totalArea;
            DateTime date;

            Random randObj = new Random(1);

            string[] rate_voc = { "Электроэнергия", "Теплоэнергия", "Водоснабжение"};//словарь названий счётчиков
            int count_rate_voc = rate_voc.GetLength(0);
            for (int rateID = 1; rateID <= rates_number; rateID++)
            {
                //создаем объект Random, генерирующий случайные числа
                
                rateType = rate_voc[randObj.Next(count_rate_voc)] + rateID.ToString();
                value = randObj.Next(1,4);
                DateTime today = DateTime.Now.Date;
                date = today.AddDays(-rateID);
                db.Rates.Add(new Rate { Type = rateType, Value = value, DateOfIntroduction = date });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();
           
            for (int tenantID = 1; tenantID <= tenants_number; tenantID++)
            {
                name = GenRandomString(voc, 10);
                surname = GenRandomString(voc, 15);
                patronymic = GenRandomString(voc, 15);
                apartmentNumber = tenantID;
                numberOfPeople = randObj.Next(1,5);
                totalArea = randObj.Next(60,123);
                db.Tenants.Add(new Tenant { NameOfTenant = name, Surname = surname, Patronymic = patronymic, ApartmentNumber = apartmentNumber,
                    NumberOfPeople = numberOfPeople, TotalArea = totalArea
                });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();

            //Заполнение таблицы показаний
            for (int readingID = 1; readingID <= readings_number; readingID++)
            {
                int tenantID = randObj.Next(1, tenants_number - 1);
                apartmentNumber = tenantID;
                int rateID = randObj.Next(1, rates_number - 1);
                int counterNumber = randObj.Next(10000,32000);
                int indications = randObj.Next(10, 20);
                int inc_exp = randObj.Next(200) - 100;
                DateTime today = DateTime.Now.Date;
                DateTime readingDate = today.AddDays(-readingID);
                db.Readings.Add(new Reading { TenantId = tenantID, Apartmentnumber = apartmentNumber, RateId = rateID, CounterNumber = counterNumber,
                    Indications = indications, DateOfReading = readingDate
                });
            }
            db.SaveChanges();

            for (int paymentID = 1; paymentID <= payments_number; paymentID++)
            {
                int tenantID = randObj.Next(1, tenants_number - 1);
                int rateID = randObj.Next(1, rates_number - 1);
                int sum = randObj.Next(3, 10);
                DateTime today = DateTime.Now.Date;
                DateTime paymentDate = today.AddDays(-paymentID);
                db.Payments.Add(new Payment
                {
                    TenantId = tenantID,
                    RateId = rateID,
                    Sum = sum,
                    DateOfPayment = paymentDate
                });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();
        }

        static string GenRandomString(string Alphabet, int Length)
        {
            Random rnd = new Random();
            //объект StringBuilder с заранее заданным размером буфера под результирующую строку
            StringBuilder sb = new StringBuilder(Length - 1);
            //переменную для хранения случайной позиции символа из строки Alphabet
            int Position = 0;
            string ret = "";
            for (int i = 0; i < Length; i++)
            {
                //получаем случайное число от 0 до последнего
                //символа в строке Alphabet
                Position = rnd.Next(0, Alphabet.Length - 1);
                //добавляем выбранный символ в объект
                //StringBuilder
                ret = ret + Alphabet[Position];
            }
            return ret;
        }
    }
}
