using EPRIN_Klient_V3;
using Microsoft.VisualBasic.Logging;
using System;
using System.Data;
using System.Drawing.Text;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;



namespace EPRIN_Klient_V3
{
    public partial class Form1 : Form
    {
        Thread serverThread;

        int iDtoEdit;
        string nameFormInput = "";
        string surnameFormInput = "";
        string ageFormInput = "";

        public bool clientIsRunning = true;
        public bool endapplication = false;

        public bool rowAdded = false;
        public bool iEdited = false;

        public int selectedRowIndex;
        public int rowsCount;

        readonly List<int> IDlist = new();

        public static event Action<DataGridView> DataGridLoader;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //p�id�n� poslucha�e pro ud�lost DataLoader
            DataGridLoader += LoadDataGridView;
        }
        private void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //p�i kliknut� na bu�ku se ozna�� cel� ��dek a z jednotliv�ch bu��k se vytvo�� objekt selectedPerson podle t��dy Person
            //a do glob�ln�ch prom�nn�ch se vypln� p��slu�n� data
            try
            {
                if (DataGridView1.DataSource != null)
                {
                    var selectedPerson = DataGridView1.SelectedRows[0].DataBoundItem as Person;

                    NameTextBox.Text = selectedPerson.Name.ToString();
                    SurnameTextBox.Text = selectedPerson.Surname.ToString();
                    AgeTextBox.Text = selectedPerson.Age.ToString();
                    iDtoEdit = selectedPerson.ID;
                    selectedRowIndex = DataGridView1.CurrentCell.RowIndex;
                    rowsCount = DataGridView1.Rows.Count;
                }
                else
                {
                    MessageBox.Show("NO DATA");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("NO DATA");  
            }

        }
        private void LoadDataGridView(DataGridView dataGridView)
        {
            //funkce kter� se vol� ud�lost� DataGridLoader, vytvo�� list objekt� People a jestli �e nebude pr�zdn� tak se vypln� DataGridView
            var people = Person.ReadDataFromServer();
            if (people.Count != 0)
            {
                dataGridView.DataSource = people;
            }
            else
            {
                dataGridView.DataSource = null;
            }
        }
        private void LogoutButton_Click(object sender, EventArgs e)
        {
            //tla��tko odhl�en� zkontroluje zda je p�ipojen� aktivn�
            //po�le na server zpr�vu "logout" kter� se podle toho zachov� a sma�e ho ze slovn�ku connectedClients
            //clientIsRunning flag se nastav� na false a stopne se t�m while loop kter� je ve funkci SignInButton_Click
            //a n�sledn� zastav� stream a clienta

            if (Server.client != null && Server.client.Connected)
            {
                byte[] response = Encoding.UTF8.GetBytes("logout");
                Server.stream.Write(response, 0, response.Length);
                Thread.Sleep(1000);

                clientIsRunning = false;

                Server.stream.Close();
                Server.client.Close();

            }
        }
        private void SignInButton_Click(object sender, EventArgs e)
        {
            IpErrorLabel.Visible = true;
            PortErrorLabel.Visible = true;

            bool ipIsValid = false;
            bool portIsValid = false;

            IPAddress address = null;
            clientIsRunning = true;

            string ipaddressFormInput = IPAddressTextBox.Text;
            string portFormInput = PortTextBox.Text;

            //zkontroluje se zda vstup nen� pr�zdn� a zda je ve form�tu IP adresy
            if (String.IsNullOrEmpty(ipaddressFormInput))
            {

                IpErrorLabel.Text = "Type IP Address";
            }
            else if (!(ipaddressFormInput != null && ipaddressFormInput.Count(c => c == '.') == 3
                                                  && IPAddress.TryParse(ipaddressFormInput, out address)))
            {
                IpErrorLabel.Text = "Wrong form of ip address";
            }
            else
            {
                IpErrorLabel.Visible = false;
                ipIsValid = true;
            }

            //zkontroluje zda vstup nen� pr�dzn� a zda je ��slo v intervalu a p�etypuje na int
            if (Int32.TryParse(portFormInput, out int port) && (port > 0 && port < 65536))
            {
                PortErrorLabel.Visible = false;
                portIsValid = true;
            }
            else if (String.IsNullOrEmpty(portFormInput))
            {
                PortErrorLabel.Text = "Type port";
            }
            else
            {
                PortErrorLabel.Text = "Wrong form of port (1-65535)";
            }

            //pokud kontrola vstup� prob�hla v po��dku, spust� se funkce s p�edan�mi parametry
            if (ipIsValid && portIsValid)
            {
                StartClient(address, port);
            }

        }
        private void StartClient(IPAddress address, int port)
        {
            //vl�kno kter� spust� client na ur�it� ip a portu
            //server mus� b�et na jin�m vl�kn� jinak by se blokovalo hlavn� vl�kno
            //a program by zamrznul
            serverThread = new Thread(() =>
            {
                Server.client = new TcpClient();
                try
                {
                    Server.client.Connect(address, port);

                    //vytvo�en� pole pro p��jm�n� dat ze serveru
                    byte[] receiveBytes = new byte[4096];

                    //invoke.((methodInvoker)delegate se mus� pou��t aby bylo mo�n� p�istupovat z jin�ho vl�kna k prom�nn�m z hlavn�ho vl�kna
                    SignInPanel.Invoke((MethodInvoker)delegate
                    {
                        LogoutButton.Visible = true;
                        SignInPanel.Hide();
                        MainPanel.Show();
                    });

                    Server.stream = Server.client.GetStream();

                    //flag clientIsRunning kter� se p�i kliknut� na Connect nastav� na true, spust� kontrolovan� while loop a �ek� na data
                    //po p�e�ten� dat p�evede pole bajt� na string do StringBuilderu ve t��d� Person builder zap�e p�ijat� data

                    while (clientIsRunning)
                    {
                        int bytesRead = Server.stream.Read(receiveBytes, 0, receiveBytes.Length);
                        Server.content = Encoding.UTF8.GetString(receiveBytes, 0, bytesRead);

                        if (Server.content == "")
                        {
                            clientIsRunning = false;

                            Server.stream.Close();
                            Server.client.Close();

                            SignInPanel.Invoke((MethodInvoker)delegate
                            {
                                SignInPanel.Show();
                                MainPanel.Hide();
                                LogoutButton.Visible = false;
                                DataGridView1.DataSource = null;
                            });

                            Person.builder.Clear();
                            //Tento messagebox se vyp�e kdy� se n�hle ukon�� server, n�kdy ale i kdy� se u�ivatel odhl�s� s�m
                            MessageBox.Show("User has been logged out\r\nServer has crashed or shut down");
                        }

                        Person.builder.Clear();
                        Person.builder.Append(Server.content);

                        //invoke.((methodInvoker)delegate se mus� pou��t aby bylo mo�n� p�istupovat z jin�ho vl�kna k prom�nn�m z hlavn�ho vl�kna

                        DataGridView1.Invoke((MethodInvoker)delegate
                        {

                            //pot� na�te data pomoc� ud�losti DataGridLoader
                            DataGridLoader?.Invoke(DataGridView1);

                            // LoadDataGridView(DataGridView1);
                            //funkce rowSelect ozna�� ��dek na z�klad� toho, zda n�co p�idal/smazal/upravil konkr�tn� klient a nebo jin�
                            //poku� jin� klient ne� ten, kter� m�me spu�t�n�, ��dek z�stane stejn�
                            //pokud n�co uprav� n� klient, nastav� se flag iEdited na true a provede se ELSE tak viz. funkce rowSelect
                            if (!iEdited)
                            {
                                RowSelect(selectedRowIndex);
                            }
                            else
                            {
                                if (rowAdded)
                                {
                                    RowSelect(rowsCount);
                                }
                                else
                                {
                                    RowSelect(selectedRowIndex);
                                }
                                rowAdded = false;
                                iEdited = false;
                            }
                        });
                    }
                }
                catch (InvalidOperationException)
                {
                }
                //k t�to vyj�mce dojde, kdy� zadan� ip a port neodpov�d� spu�t�n�mu serveru, nebo je server off
                catch (SocketException)
                {

                    MessageBox.Show("Connection error\r\n Server not found");
                }
                //k t�to vyj�mce dojde kdy� se u�ivatel s�m odhl�s�, nebo klikne na k��ek a zav�e aplikaci
                catch (IOException)
                {
                    try
                    {
                        SignInPanel.Invoke((MethodInvoker)delegate
                        {
                            SignInPanel.Show();
                            MainPanel.Hide();
                            LogoutButton.Visible = false;
                            DataGridView1.DataSource = null;
                        });
                    }
                    //k t�to vyj�mce dojde kdy� je u�ivatel p�ihl�en a k��kem zav�e aplikaci
                    catch (Exception)
                    {
                        MessageBox.Show("Application has ended");
                    }
                    MessageBox.Show("User has logged out");
                }
            });
            serverThread.Start();
        }

        public void RowSelect(int selectedRowIndex)
        {
            //poku� se data p�idaj�, ozna�� se posledn� ��dek
            //poku� se ��dek uprav�, z�stane pot� ozna�en�
            //pokud se ��dek odstran� tak se zvol� n�sleduj�c� ��dek
            //pokud se odstran� posledn� ��dek ze seznamu, zvol� se p�edchoz�
            //a jestli se odstran� upln� posledn� ��dek tak nic
            DataGridView1.ClearSelection();

            if (selectedRowIndex >= 0 && selectedRowIndex < DataGridView1.Rows.Count)
            {
                DataGridView1.Rows[selectedRowIndex].Selected = true;
            }
            try
            {
                DataGridView1.FirstDisplayedScrollingRowIndex = selectedRowIndex;
            }
            catch (ArgumentOutOfRangeException)
            {
                if (rowsCount != 1)
                {
                    try
                    {
                        DataGridView1.Rows[selectedRowIndex - 1].Selected = true;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        private void AddButton_Click(object sender, EventArgs e)
        {

            AgeErrorLabel.Text = "";

            nameFormInput = NameTextBox.Text;
            surnameFormInput = SurnameTextBox.Text;
            ageFormInput = AgeTextBox.Text;

            bool ageIsOK = false;
            bool nameIsOk = false;
            bool surnameIsOk = false;

            //zkontroluje zda nen� vstup pr�zdn� a jestli je ��slo  a p�etypuje na int
            if (!(Int32.TryParse(ageFormInput, out int value)))
            {
                AgeErrorLabel.Text = "Cell 'Age' must be number";
            }
            else if (String.IsNullOrEmpty(ageFormInput))
            {
                AgeErrorLabel.Text = "Cell 'Age' cannot be empty";
            }
            else
            {
                ageIsOK = true;
                ageFormInput = value.ToString();
            }

            //zkontroluje zda vstup nen� pr�zdn� a zda neobsahuje ��sla
            if (String.IsNullOrEmpty(nameFormInput))
            {
                NameErrorLabel.Text = "Cell cannot be empty";
            }
            else if (nameFormInput.Any(c => Char.IsDigit(c)))
            {
                NameErrorLabel.Text = "Cell cannot contains numbers";
            }
            else
            {
                nameIsOk = true;
            }

            //zkontroluje zda vstup nen� pr�zdn� a zda neobsahuje ��sla
            if (String.IsNullOrEmpty(surnameFormInput))
            {
                SurnameErrorLabel.Text = "Cell cannot be empty";
            }
            else if (surnameFormInput.Any(c => Char.IsDigit(c)))
            {
                SurnameErrorLabel.Text = "Cell cannot contains numbers";
            }
            else
            {
                surnameIsOk = true;
            }

            //jestli v�e prob�hne v po��dku, IDgenerator vr�t� vygenerovan� ID k z�znamu a spust� funkci AddRecord a pak vypr�zdn� textboxy
            if (ageIsOK & nameIsOk & surnameIsOk)
            {
                int newID = IDGenerator();

                Person.AddRecord(newID, nameFormInput, surnameFormInput, value);
                iEdited = true;
                rowAdded = true;
                ClearInputs();
               
            }
        }
        private void EditButton_Click(object sender, EventArgs e)
        {
            AgeErrorLabel.Text = "";

            nameFormInput = NameTextBox.Text;
            surnameFormInput = SurnameTextBox.Text;
            ageFormInput = AgeTextBox.Text;

            bool ageIsOK = false;
            bool nameIsOk = false;
            bool surnameIsOk = false;

            //zkontroluje zda nen� vstup pr�zdn� a jestli je ��slo  a p�etypuje na int
            if (!(Int32.TryParse(ageFormInput, out int value)))
            {
                AgeErrorLabel.Text = "Cell 'Age' must be number";
            }
            else if (String.IsNullOrEmpty(ageFormInput))
            {
                AgeErrorLabel.Text = "Cell 'Age' cannot be empty";
            }
            else
            {
                ageIsOK = true;
                ageFormInput = value.ToString();
            }


            //zkontroluje zda vstup nen� pr�zdn� a zda neobsahuje ��sla
            if (String.IsNullOrEmpty(nameFormInput))
            {
                NameErrorLabel.Text = "Cell cannot be empty";
            }
            else if (nameFormInput.Any(c => Char.IsDigit(c)))
            {
                NameErrorLabel.Text = "Cell cannot contains numbers";
            }
            else
            {
                nameIsOk = true;
            }

            //zkontroluje zda vstup nen� pr�zdn� a zda neobsahuje ��sla
            if (String.IsNullOrEmpty(surnameFormInput))
            {
                SurnameErrorLabel.Text = "Cell cannot be empty";
            }
            else if (surnameFormInput.Any(c => Char.IsDigit(c)))
            {
                SurnameErrorLabel.Text = "Cell cannot contains numbers";
            }
            else
            {
                surnameIsOk = true;
            }

            //jestli v�e prob�hne v po��dku, iDtoEdit z�sk� z kliknut na ��dek v datagridu a spust� funkci UpdateRecord a pak vypr�zdn� textboxy
            if (ageIsOK & nameIsOk & surnameIsOk)
            {
                Person.UpdateRecord(iDtoEdit, nameFormInput, surnameFormInput, value);
                iEdited = true;
                ClearInputs();
                
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            //ov��� zda je klikl� na ��dek, iDtoEdit p�evezme z kliknut� na ��dek a vyhodi� messagebox kde pak povede funkci DeleteRecord
            //vyma�e ID z IDlistu a vy�ist� textboxy
            if (iDtoEdit != 0)
            {
                DialogResult dialogResult = MessageBox.Show(
                 "Do you really want to delete the record with ID " + iDtoEdit + "?",
                 "Warning",
                 MessageBoxButtons.YesNo
                  );
                if (dialogResult == DialogResult.Yes)
                {
                    Person.DeleteRecord(iDtoEdit);
                    IDlist.Remove(iDtoEdit);
                    iEdited = true;
                    ClearInputs();
                }
            }
        }
        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            NameErrorLabel.Text = "";
        }
        private void SurnameTextBox_TextChanged(object sender, EventArgs e)
        {
            SurnameErrorLabel.Text = "";
        }
        private void AgeTextBox_TextChanged(object sender, EventArgs e)
        {
            AgeErrorLabel.Text = "";
        }
        private void IPAddressTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            IpErrorLabel.Visible = false;
        }
        private void PortTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            PortErrorLabel.Visible = false;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //zjisti� zda je p�ipojen� k serveru st�le aktivn�, po�le na server zpr�vu "logout" a ukon�� spojen� a server z�roven ukon�� spojen� s klientem
            if (Server.client != null && Server.client.Connected)
            {
                byte[] response = Encoding.UTF8.GetBytes("logout");
                Server.stream.Write(response, 0, response.Length);

                clientIsRunning = false;

                Server.stream.Close();
                Server.client.Close();
            }
        }
        private void ClearInputs()
        {
            NameTextBox.Text = "";
            SurnameTextBox.Text = "";
            AgeTextBox.Text = "";
        }
        public int IDGenerator()
        {
            //funkce vygeneruje ��slo v intervalu a zkontroluje zda ji� nen� p�id�no v listu IDlist, ulo�� ho tam a vr�t� int
            Random rand = new();
            var people = Person.ReadDataFromServer();
            int newID;

            foreach (var item in people)
            {
                IDlist.Add(item.ID);
            }
            do
            {
                newID = rand.Next(10000, 19999);
            } while (IDlist.Contains(newID));
            IDlist.Clear();
            return newID;
        }
    }
}





