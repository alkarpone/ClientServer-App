using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Net.Http;

namespace SERVER_V2
{
    public partial class Form1 : Form
    {
        Thread serverThread;
        TcpListener server;

        bool serverIsRunning = false;
        string serverContent = "";

        readonly object serverContentLock = new();
        readonly object fileContentLock = new();

        List<int> clientlist = new(); //list do kterého se ukládají vygenerované ID ke klientùm
        Dictionary<string, TcpClient> connectedClients = new(); //slovník do kterého se ukládá clientID a konkrétní objekt client

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //pokud se zavøe køížkem aplikace a server stále bìží, dojde k jeho zastavení a zavøení všech aktivních klientù
            if (serverIsRunning)
            {
                serverIsRunning = false;
                server.Stop();
            }
            foreach (TcpClient c in connectedClients.Values)
            {
                if (c.Connected)
                {
                    c.Close();
                }
            }
        }
        private void IpAddressTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            wrongIpLabel.Visible = false;
        }
        private void PortTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            wrongPortLabel.Visible = false;
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            wrongIpLabel.Visible = true;
            wrongPortLabel.Visible = true;
            bool ipIsValid = false;
            bool portIsValid = false;

            string ipaddressFormInput = IpAddressTextBox.Text;
            string portFormInput = PortTextBox.Text;
            IPAddress? ipaddress = null;

            //ovìøení zda vstup není prázdný a jestli má správný formát IP adresy
            if (String.IsNullOrEmpty(ipaddressFormInput))
            {

                wrongIpLabel.Text = "Type IP Address";
            }
            else if (!(ipaddressFormInput != null && ipaddressFormInput.Count(c => c == '.') == 3
                                                  && IPAddress.TryParse(ipaddressFormInput, out ipaddress)))
            {
                wrongIpLabel.Text = "Wrong form of ip address";
            }
            else
            {
                ipIsValid = true;
                wrongIpLabel.Visible = false;
            }

            //ovìøení zda vstup není prázdný a že je èíslo v intervalu, poté se pøetypuje na int
            if (Int32.TryParse(portFormInput, out int port) && (port > 0 && port < 65536))
            {
                portIsValid = true;
                wrongPortLabel.Visible = false;
            }
            else if (String.IsNullOrEmpty(portFormInput))
            {
                wrongPortLabel.Text = "Type port";
            }
            else
            {
                wrongPortLabel.Text = "Wrong form of port (1-65535)";
            }

            //pokud je vše Ok, server se spustí
            if (ipIsValid && portIsValid)
            {

                
               
                StartButton.Enabled = false;
                StopButton.Enabled = true;

                serverPanel.BackColor = Color.Green;
                StartMulti(ipaddress, port);


            }
        }
        private void StopButton_Click(object sender, EventArgs e)
        {
            StopButton.Enabled = false;
            StartButton.Enabled = true;

            serverPanel.BackColor = Color.Red;
            serverIsRunning = false;
            //pokud dojde ke stopnutí serveru tlaèítkem, dojde k odhlášení všech klientù a server se zastaví
            //je možné ho opìtovnì spustit
            //protože connectecClients je sdílená promìnná, mùže k nìmu pøistupovat více vláken a proto je potøeba ho zamknout
            //aby nedocházelo je kolizím a chybám aplikace

            lock (connectedClients)
            {
                foreach (var clientend in connectedClients.Values)
                {
                    try
                    {
                        clientend.GetStream().Close();
                        clientend.Close();
                    }
                    catch (ObjectDisposedException)
                    {

                        break;
                    }

                }
                connectedClients.Clear();
            }
            server.Stop();
            //globální promìnná se nastaví na "end" 
            serverContent = "end";
            AddToLog("\r\nServer is closed\r\n_____________________________\r\n", serverinfo);
        }
        private int ClientIdGenerator()
        {
            //funkce, která generuje náhodné èísla v rozhsahu a pøidìluje je klientùm 
            //zároveò kontroluje zda už takové ID neexistuje
            Random rand = new();
            int clientId;
            do
            {
                clientId = rand.Next(5000, 5999);
            } while (clientlist.Contains(clientId));
            clientlist.Add(clientId);
            return clientId;
        }
        private void StartMulti(IPAddress ipaddress, int port)
        {
            //tato funkce obsahuje vlákno serverThread, server musí bìžet na jimém vláknì jinak by se blokovalo hlavní vlákno
            //a program by zamrznul

            serverIsRunning = true;
            serverThread = new Thread(() =>
            {
                try
                {
                    server = new TcpListener(ipaddress, port);
                    server.Start();
                    AddToLog($"\r\nServer is running on {ipaddress}:{port}\r\nWaiting for connection..\r\n_____________________________\r\n", serverinfo);
                }
                catch (Exception ex)
                {
                    //pokud IP adresa není platná, napø. 1.1.1.1, dojde k této vyjímce kdy se server zastaví 
                    //invoke.((methodInvoker)delegate se musí použít aby bylo možné pøistupovat z jiného vlákna k promìnným z hlavního vlákna

                    StopButton.Invoke((MethodInvoker)delegate
                    {
                        StopButton.Enabled = false;
                        StartButton.Enabled = true;

                        serverPanel.BackColor = Color.Red;
                        serverIsRunning = false;
                    });

                    MessageBox.Show(ex.Message);
                }

                while (serverIsRunning)
                {
                    //pokud spuštìní serveru probìhne v poøádku, spustí se tento kontrolovaný while loop, který bude pøijmat nové klienty
                    //a každý klient bude mít svoje vlákno
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();

                        string clientId = ClientIdGenerator().ToString();

                        Thread clientThread = new(() => HandleClient(clientId, client));
                        clientThread.Start();
                        
                    }
                    catch (SocketException)
                    {
                        if (!serverIsRunning)
                        {
                            break;
                        }
                        throw;
                    }
                }
            });
            serverThread.Start();
            
        }
        private void HandleClient(string clientId, TcpClient client)
        {
            //funkce na zpracování klienta, každý klient pobìží v jiném vláknì
            byte[] bytes = new byte[4096];
            string data;
            try
            {
                NetworkStream stream = client.GetStream();

                AddToLog($"\r\nClient '{clientId}' has connected\r\n_____________________________\r\n", serverinfo);
                //protože connectecClients je sdílená promìnná, mùže k nìmu pøistupovat více vláken a proto je potøeba ho zamknout
                //aby nedocházelo je kolizím a chybám aplikace
                lock (connectedClients)
                {
                    connectedClients.Add(clientId, client);
                }
                //po spuštìní aplikace serveru je serverContent prázdný, nebo po zastavení serveru tlaèítkem STOP je "end"
                if (serverContent == "" | serverContent == "end")
                {
                    //ZAMEZENÍ ZTRÁTY
                    //pokud je aplikace spuštìna poprvé, server neobsahuje žádná data,
                    //vytvoøí soubor backup.csv v adresáøi, kde je EXE, do kterého se uloží data po každém zápisu od klienta
                    //pokud se tedy server zastaví nebo dojde k výpadku, je tu záloha

                    CreateFileIfDoesntExist("backup.csv");
                    string backupFile;

                    //pokud je server spuštìn opìtovì pomocí START STOP, naète data ze souboru backup.csv
                    //zkontroluje obsah dat a pošle data klientovi a serverContent už tyto data bude obsahovat,
                    //pokud by se dva kilenti pøipojili v totožnou chvíli, operace je zamklá pomocí lock aby nedocházelo ke kolizím 

                    //---podaøilo se mi nasimulovat jen tak že jsem ruènì smazal buòku v excelu za bìhu programu,
                    //jinak mi k této události nikdy nedošlo, ale pro jistotu

                    //pokud dojde k neoèekávanému chování aplikace nebo špatnému zápisu dat
                    //a obsah dat poškozený = nebude ve formátu x,x,x,x nebo bude nìjaká buòka prázdná 
                    //vytvoøí se nový soubor v adresáøi kde je EXE,s poškozenými daty a zkontrolování
                    //a nová data se budou opìt ukládat do backup.csv

                    lock (fileContentLock)
                    {
                        //kdyby byl soubor obrovský, naèítání probíhá asynchronnì aby se neblokovalo vlákno programu
                        backupFile = LoadFileAsync("backup.csv").Result;
                    }
                    if (VerifyFileContent(backupFile))
                    {
                        SendDataAllClients(backupFile);
                        serverContent = backupFile;
                    }
                    else
                    {
                        AddToLog("\r\nBackup file is corrupted :( , cannot be uploaded\r\n_____________________________\r\n", serverinfo);

                        CreateFileIfDoesntExist("corruptedbackup.csv");
                        //soubor se ukláda asynchronnì aby se neblokovalo vlákno programu
                        _ = SaveBackupFileAsync("corruptedbackup.csv", backupFile);
                    }
                }
                else
                {
                    //dalšímu klientovi se odešlou data už z této promìnné, aby se zamezilo opìtovnému naèítání dat ze souboru a nedocházelo ke kolizím
                    SendDataAllClients(serverContent);
                }
                //po odeslání dat skoèí do kontrolovaného while loopu, jakmile se pøijmou nìjáká data, pokraèuje se v programu




                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.UTF8.GetString(bytes, 0, i);
                    //pokud klient odstraní poslední záznam, odešle "nothing" jakože je prázdný
                    //a poté se data pøepíšou na "deleteall" a nakonci se serverContent nastaví na prázdný string "" 
                    if (data == "nothing")
                    {
                        data = "deleteall";
                    }
                    //pokud se jeden z klientù odhlásí, pošle "logout" a cyklus foreach projde všechny pøipojené klienty
                    //a na základì vyhledávání v slovníku connectedClients odstraní kontrétního klienta a zapíše se do serverlogu
                    //že se urèitý klient odhlásil
                    //poté se data nastaví na "continue" jako aby program pokraèoval, a v bloku Finally dojde k uzavøení spojení
                    else if (data == "logout")
                    {
                        foreach (TcpClient c in connectedClients.Values)
                        {
                            NetworkStream s = c.GetStream();
                            if (c.Connected)
                            {
                                if (connectedClients.Any(x => x.Key == clientId && x.Value == c))
                                {
                                    lock (connectedClients)
                                    {
                                        connectedClients.Remove(clientId);
                                        clientlist.Remove(Int32.Parse(clientId));
                                        AddToLog($"\r\nClient '{clientId}' was logged out\r\n_____________________________\r\n", serverinfo);
                                    }
                                    break;
                                }
                            }
                        }
                        data = "continue";
                    }
                    //jestli klient pøidá smaže nebo upraví data, zapíše se do serverlogu
                    //data se odešlou odešlou všem, a uloží se záloha dat
                    else
                    {
                        AddToLog($"\r\nReceive from CLIENT ID:{clientId}\r\n{data}\n_____________________________\r\n", receivelog);
                    }
                    lock (serverContentLock)
                    {
                        //tento IF nastane když se data budou rovnat "deleteall" a poté se klientùm pošle zpráva "deleteall"
                        //a na základnì toho se tam provedou funkce
                        if (data != "continue")
                        {
                            serverContent = data;
                        }
                        //tento IF nastane v pøípadech kdy se rovná serverContent "deleteall" nebo data ve formátu x,x,x,x
                        //a odešle se všem klientùm na základì foreach cyklu který projde všechny pøipojené klienty v connectedClients a data jim pošle

                        //jestliže serverContent bude "deleteall" klientùm pošle zpráva "deleteall"
                        //a na základnì toho se uz klientù provedou funkce

                        //v obou pøípadech se uloží asynchronnì záloha dat, když budou data prázdná uloží se prádzný soubor
                        if (serverContent != "end")
                        {
                            SendDataAllClients(serverContent);
                            if (data == "deleteall")
                            {
                                serverContent = "";
                            }
                            _ = SaveBackupFileAsync("backup.csv", serverContent);

                        }
                    }
                }
            }
            //pokud se s klientem ztratí spojení z nìjakého dùvodu, kdy odhlášení nebo vypnutí aplikace neproivede sám
            //zapíše se do serverlogu a odstraní ho z listu, a v bloku Finally dojde k uzavøení spojení
            catch (Exception)
            {
                try
                {
                    connectedClients.Remove(clientId);
                    clientlist.Remove(Int32.Parse(clientId));
                    var test = connectedClients.Values;
                    AddToLog($"\r\nConnection to the'{clientId}'has been lost\r\n_____________________________\r\n", serverinfo);
                }
                catch (ObjectDisposedException)
                {
                }
            }
            finally
            {
                client.Close();
            }
        }
        private static void AddToLog(string message, TextBox textbox)
        {
            //invoke.((methodInvoker)delegate se musí použít aby bylo možné pøistupovat z jiného vlákna k promìnným z hlavního vlákna

            //funkce pro zapsání do jednoho z tøí serverlogù, urèi se podle parametrù
            textbox.Invoke((MethodInvoker)delegate
            {
                DateTime dateTimeNow = DateTime.Now;
                textbox.Text += dateTimeNow + message;
                textbox.SelectionStart = textbox.Text.Length;
                textbox.ScrollToCaret();

            });
        }
        private static void CreateFileIfDoesntExist(string filename)
        {
            if (!File.Exists(filename))
            {
                using FileStream fs = new(filename, FileMode.CreateNew);
            }

        }
        private void SendDataAllClients(string serverContent)
        {

            foreach (TcpClient c in connectedClients.Values)
            {
                byte[] response = Encoding.UTF8.GetBytes(serverContent);
                if (c.Connected)
                {
                    NetworkStream s = c.GetStream();
                    s.Write(response, 0, response.Length);
                }
                else
                {
                    //pokud by se objevili klienti v connectedClients, kteøí nejsou pøipojení tak se odstraní
                    lock (connectedClients)
                    {
                        connectedClients.Remove(connectedClients.FirstOrDefault(x => x.Value == c).Key);
                    }
                }
            }
            AddToLog($"\r\nSend to everyone\r\n{serverContent}\r\n_____________________________\r\n", sendlog);
        }
        private static async Task SaveBackupFileAsync(string fileName, string fileContent)
        {
            string[] lines = fileContent.Split("\r\n");

            using StreamWriter sw = new(fileName);
            foreach (var line in lines)
            {
                if (line != "")
                {
                    await sw.WriteLineAsync(line);
                }
            }
        }
        private static async Task<string> LoadFileAsync(string filename)
        {
            string fileContent;
            using (StreamReader sr = new(filename))
            {
                fileContent = await sr.ReadToEndAsync();
            }
            return fileContent;
        }
        private static bool VerifyFileContent(string fileContent)
        {
            //pokud obsah souboru obsahuje data která nejsou ve tvaru x,x,x,x - vrátí false
            string[] lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var row in lines)
            {
                if (row.Count(c => c == ',') != 3)
                {
                    return false;
                }
                //pokud je nìjaká buòka prázdná - vrátí false
                string[] cells = row.Split(",");
                foreach (var cell in cells)
                {
                    if (cell == "")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
