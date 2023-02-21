
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace EPRIN_Klient_V3
{
    //třda Person má 4 globální proměnné  ID,Name,Surname,Age
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public static StringBuilder builder = new();

        public static List<Person> ReadDataFromServer()
        {
            //tato funkce zpracuje data přečtená ze serveru, která se uloží do StringBuilderu a následně se předá do Server.content
            //funkce vrací list a v něm objekty Person
            List<Person> people = new();

            Server.content = builder.ToString();

            if (Server.content != "" && Server.content != "deleteall")
            {
                string[] lines = Server.content.Split("\r\n");

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] line = lines[i].Split(',');

                    if (lines[i] != "")
                    {
                        Person record = new()
                        {
                            ID = Convert.ToInt32(line[0]),
                            Name = line[1].ToString(),
                            Surname = line[2].ToString(),
                            Age = Convert.ToInt32(line[3])
                        };
                        people.Add(record);
                    }
                }
            }
            return people;
        }
        public static void AddRecord(int id, string name, string surname, int age)
        {
            //funkce která podle vložených parametrů z formuláře, zapíše na server data v konkrétním formátu
            //jeslti že už nějaké data na serveru existují, zapíše za ně na konec všech záznamů
            //a na konec se zavolá událost DataGridLoader
            //půl sekundy se program zastaví aby stihly data dorazit na server a zpět

            string responseData = $"{id},{name},{surname},{age}\r\n";

            if (Server.content.Length != 0 && Server.content != "deleteall")
            {
                responseData = Server.content + responseData;
            }
            byte[] response = Encoding.UTF8.GetBytes(responseData);
            Server.stream.Write(response, 0, response.Length);
            MessageBox.Show("Record with ID: " + id + " added");

            Thread.Sleep(500);

        }
        public static void UpdateRecord(int id, string name, string surname, int age)
        {
            //funkce která podle parametrů upraví konkrétní záznam podle ID
            //v listu people se vyhledá záznam který odpovídá parametru ID  a tento konkrétní objekt person se upraví
            //následně se pomocí StringBuilderu naplní responseData a celé odešle na server 
            //a nakonec DataGridLoader přečte data a vyplni DataGrid
            //pokud nějaký jiný klient odstraní záznam ve chvíli, kdy se ho tento klient chystá upravovat, vypíše Person not found

            var people = ReadDataFromServer();
            StringBuilder responseData = new();
            Person? person = people.Find(p => p.ID == id);
            if (person != null)
            {
                person.Name = name;
                person.Surname = surname;
                person.Age = age;

                foreach (var item in people)
                {
                    responseData.Append($"{item.ID},{item.Name},{item.Surname},{item.Age}\r\n");
                }
                people.Clear();

                byte[] response = Encoding.UTF8.GetBytes(responseData.ToString());

                Server.stream.Write(response, 0, response.Length);
                MessageBox.Show("Record with ID " + id + " edited");

                Thread.Sleep(500);

            }
            else
            {
                MessageBox.Show("Person not found");
            }

        }

        public static void DeleteRecord(int id)
        {
            //funkce která v listu people najde záznam odpovídající parametru ID a odstraní ho
            //poté pomocí StringBuilderu naplní responseData a odešle
            //pokud bude odstraněn poslední záznam, na servere se pošle zpráva "nothing" který se podle toho zachová
            //pokud nějaký jiný klient odstraní záznam ve chvíli, kdy se ho tento klient chystá smazat, vypíše Person not found

            var people = ReadDataFromServer();
            StringBuilder responseData = new();
            Person? person = people.Find(p => p.ID == id);
            if (person != null)
            {
                people.RemoveAll(p => p.ID == id);

                foreach (var item in people)
                {
                    responseData.Append($"{item.ID},{item.Name},{item.Surname},{item.Age}\r\n");
                }
                people.Clear();

                var data = responseData.ToString();
                if (data == "")
                {
                    data = "nothing";
                    builder.Clear();
                }
                byte[] response = Encoding.UTF8.GetBytes(data);

                Server.stream.Write(response, 0, response.Length);
                MessageBox.Show("Record with ID: " + id + " deleted");

                Thread.Sleep(500);

            }
            else
            {
                MessageBox.Show("Person not found");
            }

        }

        
    }
}


