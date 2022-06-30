using System;
using System.Collections.Generic;
using System.Text;

namespace AutoComplete
{
    class Program
    {
        static void Main()
        {
            var a = new AutoCompleter();
            var list = new List<FullName>() {};
            list.Add(new FullName("", "Ангелина", ""));
            list.Add(new FullName("     ", "Андрей ", ""));


            list.Add(new FullName("Ходайкин      "," Миша      ","    Петрович"));

            a.AddToSearch(list);
            var cw = a.Search("Ан");
            foreach (var item in cw)
            {
                Console.WriteLine(item);
            }
        }
    }
}
