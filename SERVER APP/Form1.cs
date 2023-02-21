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

        List<int> clientlist = new(); //list do kter�ho se ukl�daj� vygenerovan� ID ke klient�m
        Dictionary<string, TcpClient> connectedClients = new(); //slovn�k do kter�ho se ukl�d� clientID a konkr�tn� objekt client

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //pokud se zav�e k��kem aplikace a server st�le b��, dojde k jeho zastaven� a zav�en� v�ech aktivn�ch klient�
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

            //ov��en� zda vstup nen� pr�zdn� a jestli m� spr�vn� form�t IP adresy
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

            //ov��en� zda vstup nen� pr�zdn� a �e je ��slo v intervalu, pot� se p�etypuje na int
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

            //pokud je v�e Ok, server se spust�
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
            //pokud dojde ke stopnut� serveru tla��tkem, dojde k odhl�en� v�ech klient� a server se zastav�
            //je mo�n� ho op�tovn� spustit
            //proto�e connectecClients je sd�len� prom�nn�, m��e k n�mu p�istupovat v�ce vl�ken a proto je pot�eba ho zamknout
            //aby nedoch�zelo je koliz�m a chyb�m aplikace

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
            //glob�ln� prom�nn� se nastav� na "end" 
            serverContent = "end";
            AddToLog("\r\nServer is closed\r\n_____________________________\r\n", serverinfo);
        }
        private int ClientIdGenerator()
        {
            //funkce, kter� generuje n�hodn� ��sla v rozhsahu a p�id�luje je klient�m 
            //z�rove� kontroluje zda u� takov� ID neexistuje
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
            //tato funkce obsahuje vl�kno serverThread, server mus� b�et na jim�m vl�kn� jinak by se blokovalo hlavn� vl�kno
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
                    //pokud IP adresa nen� platn�, nap�. 1.1.1.1, dojde k t�to vyj�mce kdy se server zastav� 
                    //invoke.((methodInvoker)delegate se mus� pou��t aby bylo mo�n� p�istupovat z jin�ho vl�kna k prom�nn�m z hlavn�ho vl�kna

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
                    //pokud spu�t�n� serveru prob�hne v po��dku, spust� se tento kontrolovan� while loop, kter� bude p�ijmat nov� klienty
                    //a ka�d� klient bude m�t svoje vl�kno
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
            //funkce na zpracov�n� klienta, ka�d� klient pob�� v jin�m vl�kn�
            byte[] bytes = new byte[4096];
            string data;
            try
            {
                NetworkStream stream = client.GetStream();

                AddToLog($"\r\nClient '{clientId}' has connected\r\n_____________________________\r\n", serverinfo);
                //proto�e connectecClients je sd�len� prom�nn�, m��e k n�mu p�istupovat v�ce vl�ken a proto je pot�eba ho zamknout
                //aby nedoch�zelo je koliz�m a chyb�m aplikace
                lock (connectedClients)
                {
                    connectedClients.Add(clientId, client);
                }
                //po spu�t�n� aplikace serveru je serverContent pr�zdn�, nebo po zastaven� serveru tla��tkem STOP je "end"
                if (serverContent == "" | serverContent == "end")
                {
                    //ZAMEZEN� ZTR�TY
                    //pokud je aplikace spu�t�na poprv�, server neobsahuje ��dn� data,
                    //vytvo�� soubor backup.csv v adres��i, kde je EXE, do kter�ho se ulo�� data po ka�d�m z�pisu od klienta
                    //pokud se tedy server zastav� nebo dojde k v�padku, je tu z�loha

                    CreateFileIfDoesntExist("backup.csv");
                    string backupFile;

                    //pokud je server spu�t�n op�tov� pomoc� START STOP, na�te data ze souboru backup.csv
                    //zkontroluje obsah dat a po�le data klientovi a serverContent u� tyto data bude obsahovat,
                    //pokud by se dva kilenti p�ipojili v toto�nou chv�li, operace je zamkl� pomoc� lock aby nedoch�zelo ke koliz�m 

                    //---poda�ilo se mi nasimulovat jen tak �e jsem ru�n� smazal bu�ku v excelu za b�hu programu,
                    //jinak mi k t�to ud�losti nikdy nedo�lo, ale pro jistotu

                    //pokud dojde k neo�ek�van�mu chov�n� aplikace nebo �patn�mu z�pisu dat
                    //a obsah dat po�kozen� = nebude ve form�tu x,x,x,x nebo bude n�jak� bu�ka pr�zdn� 
                    //vytvo�� se nov� soubor v adres��i kde je EXE,s po�kozen�mi daty a zkontrolov�n�
                    //a nov� data se budou op�t ukl�dat do backup.csv

                    lock (fileContentLock)
                    {
                        //kdyby byl soubor obrovsk�, na��t�n� prob�h� asynchronn� aby se neblokovalo vl�kno programu
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
                        //soubor se ukl�da asynchronn� aby se neblokovalo vl�kno programu
                        _ = SaveBackupFileAsync("corruptedbackup.csv", backupFile);
                    }
                }
                else
                {
                    //dal��mu klientovi se ode�lou data u� z t�to prom�nn�, aby se zamezilo op�tovn�mu na��t�n� dat ze souboru a nedoch�zelo ke koliz�m
                    SendDataAllClients(serverContent);
                }
                //po odesl�n� dat sko�� do kontrolovan�ho while loopu, jakmile se p�ijmou n�j�k� data, pokra�uje se v programu




                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.UTF8.GetString(bytes, 0, i);
                    //pokud klient odstran� posledn� z�znam, ode�le "nothing" jako�e je pr�zdn�
                    //a pot� se data p�ep�ou na "deleteall" a nakonci se serverContent nastav� na pr�zdn� string "" 
                    if (data == "nothing")
                    {
                        data = "deleteall";
                    }
                    //pokud se jeden z klient� odhl�s�, po�le "logout" a cyklus foreach projde v�echny p�ipojen� klienty
                    //a na z�klad� vyhled�v�n� v slovn�ku connectedClients odstran� kontr�tn�ho klienta a zap�e se do serverlogu
                    //�e se ur�it� klient odhl�sil
                    //pot� se data nastav� na "continue" jako aby program pokra�oval, a v bloku Finally dojde k uzav�en� spojen�
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
                    //jestli klient p�id� sma�e nebo uprav� data, zap�e se do serverlogu
                    //data se ode�lou ode�lou v�em, a ulo�� se z�loha dat
                    else
                    {
                        AddToLog($"\r\nReceive from CLIENT ID:{clientId}\r\n{data}\n_____________________________\r\n", receivelog);
                    }
                    lock (serverContentLock)
                    {
                        //tento IF nastane kdy� se data budou rovnat "deleteall" a pot� se klient�m po�le zpr�va "deleteall"
                        //a na z�kladn� toho se tam provedou funkce
                        if (data != "continue")
                        {
                            serverContent = data;
                        }
                        //tento IF nastane v p��padech kdy se rovn� serverContent "deleteall" nebo data ve form�tu x,x,x,x
                        //a ode�le se v�em klient�m na z�klad� foreach cyklu kter� projde v�echny p�ipojen� klienty v connectedClients a data jim po�le

                        //jestli�e serverContent bude "deleteall" klient�m po�le zpr�va "deleteall"
                        //a na z�kladn� toho se uz klient� provedou funkce

                        //v obou p��padech se ulo�� asynchronn� z�loha dat, kdy� budou data pr�zdn� ulo�� se pr�dzn� soubor
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
            //pokud se s klientem ztrat� spojen� z n�jak�ho d�vodu, kdy odhl�en� nebo vypnut� aplikace neproivede s�m
            //zap�e se do serverlogu a odstran� ho z listu, a v bloku Finally dojde k uzav�en� spojen�
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
            //invoke.((methodInvoker)delegate se mus� pou��t aby bylo mo�n� p�istupovat z jin�ho vl�kna k prom�nn�m z hlavn�ho vl�kna

            //funkce pro zaps�n� do jednoho z t�� serverlog�, ur�i se podle parametr�
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
                    //pokud by se objevili klienti v connectedClients, kte�� nejsou p�ipojen� tak se odstran�
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
            //pokud obsah souboru obsahuje data kter� nejsou ve tvaru x,x,x,x - vr�t� false
            string[] lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var row in lines)
            {
                if (row.Count(c => c == ',') != 3)
                {
                    return false;
                }
                //pokud je n�jak� bu�ka pr�zdn� - vr�t� false
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
