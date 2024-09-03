using System;
using System.Collections.Generic;

namespace vhodnoykontrol
{
    static class Data
    {
        public static List<Info> Info = new List<Info>
        {
            new Info(10153, "Чупашева А.И.", "01.10.2015"),
            new Info(20174, "Иванов А.В.", "10.01.2017"),
            new Info(30191, "Кривцов О.П.", "05.02.2019"),
            new Info(40140, "Янаева Э.С.", "15.12.2014")
        };
    }

    internal class Info
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Date { get; set; }

        public Info(int id, string fio, string date)
        {
            Id = id;
            FIO = fio;
            Date = date;
        }
    }
}