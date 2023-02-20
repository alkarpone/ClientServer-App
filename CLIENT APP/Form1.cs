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
            //pøidání posluchaèe pro událost DataLoader
            DataGridLoader += LoadDataGridView;
        }
        private void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //pøi kliknutí na buòku se oznaèí celý øádek a z jednotlivých buòìk se vytvoøí objekt selectedPerson podle tøídy Person
            //a do globálních promìnných se vyplní pøíslušná data
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
            //funkce která se volá událostí DataGridLoader, vytvoøí list objektù People a jestli že nebude prázdný tak se vyplní DataGridView
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
            //tlaèítko odhlášení zkontroluje zda je pøipojení aktivní
            //pošle na server zprávu "logout" který se podle toho zachová a smaže ho ze slovníku connectedClients
            //clientIsRunning flag se nastaví na false a stopne se tím while loop který je ve funkci SignInButton_Click
            //a následnì zastaví stream a clienta

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

            //zkontroluje se zda vstup není prázdný a zda je ve formátu IP adresy
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

            //zkontroluje zda vstup není prádzný a zda je èíslo v intervalu a pøetypuje na int
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

            //pokud kontrola vstupù probìhla v poøádku, spustí se funkce s pøedanými parametry
            if (ipIsValid && portIsValid)
            {
                StartClient(address, port);
            }

        }
        private void StartClient(IPAddress address, int port)
        {
            //vlákno které spustí client na urèité ip a portu
            //server musí bìžet na jiném vláknì jinak by se blokovalo hlavní vlákno
            //a program by zamrznul
            serverThread = new Thread(() =>
            {
                Server.client = new TcpClient();
                try
                {
                    Server.client.Connect(address, port);

                    //vytvoøené pole pro pøíjmání dat ze serveru
                    byte[] receiveBytes = new byte[4096];

                    //invoke.((methodInvoker)delegate se musí použít aby bylo možné pøistupovat z jiného vlákna k promìnným z hlavního vlákna
                    SignInPanel.Invoke((MethodInvoker)delegate
                    {
                        LogoutButton.Visible = true;
                        SignInPanel.Hide();
                        MainPanel.Show();
                    });

                    Server.stream = Server.client.GetStream();

                    //flag clientIsRunning který se pøi kliknutí na Connect nastaví na true, spustí kontrolovaný while loop a èeká na data
                    //po pøeètení dat pøevede pole bajtù na string do StringBuilderu ve tøídì Person builder zapíše pøijatá data

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
                            //Tento messagebox se vypíše když se náhle ukonèí server, nìkdy ale i když se uživatel odhlásí sám
                            MessageBox.Show("User has been logged out\r\nServer has crashed or shut down");
                        }

                        Person.builder.Clear();
                        Person.builder.Append(Server.content);

                        //invoke.((methodInvoker)delegate se musí použít aby bylo možné pøistupovat z jiného vlákna k promìnným z hlavního vlákna

                        DataGridView1.Invoke((MethodInvoker)delegate
                        {

                            //poté naète data pomocí události DataGridLoader
                            DataGridLoader?.Invoke(DataGridView1);

                            // LoadDataGridView(DataGridView1);
                            //funkce rowSelect oznaèí øádek na základì toho, zda nìco pøidal/smazal/upravil konkrétní klient a nebo jiný
                            //pokuï jiný klient než ten, který máme spuštìný, øádek zùstane stejný
                            //pokud nìco upraví náš klient, nastaví se flag iEdited na true a provede se ELSE tak viz. funkce rowSelect
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
                //k této vyjímce dojde, když zadaná ip a port neodpovídá spuštìnému serveru, nebo je server off
                catch (SocketException)
                {

                    MessageBox.Show("Connection error\r\n Server not found");
                }
                //k této vyjímce dojde když se uživatel sám odhlásí, nebo klikne na køížek a zavøe aplikaci
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
                    //k této vyjímce dojde když je uživatel pøihlášen a køížkem zavøe aplikaci
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
            //pokuï se data pøidají, oznaèí se poslední øádek
            //pokuï se øádek upraví, zùstane poté oznaèený
            //pokud se øádek odstraní tak se zvolí následující øádek
            //pokud se odstraní poslední øádek ze seznamu, zvolí se pøedchozí
            //a jestli se odstraní uplnì poslední øádek tak nic
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

            //zkontroluje zda není vstup prázdný a jestli je èíslo  a pøetypuje na int
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

            //zkontroluje zda vstup není prázdný a zda neobsahuje èísla
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

            //zkontroluje zda vstup není prázdný a zda neobsahuje èísla
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

            //jestli vše probìhne v poøádku, IDgenerator vrátí vygenerované ID k záznamu a spustí funkci AddRecord a pak vyprázdní textboxy
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

            //zkontroluje zda není vstup prázdný a jestli je èíslo  a pøetypuje na int
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


            //zkontroluje zda vstup není prázdný a zda neobsahuje èísla
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

            //zkontroluje zda vstup není prázdný a zda neobsahuje èísla
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

            //jestli vše probìhne v poøádku, iDtoEdit získá z kliknut na øádek v datagridu a spustí funkci UpdateRecord a pak vyprázdní textboxy
            if (ageIsOK & nameIsOk & surnameIsOk)
            {
                Person.UpdateRecord(iDtoEdit, nameFormInput, surnameFormInput, value);
                iEdited = true;
                ClearInputs();
                
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            //ovìøí zda je kliklé na øádek, iDtoEdit pøevezme z kliknutí na øádek a vyhodií messagebox kde pak povede funkci DeleteRecord
            //vymaže ID z IDlistu a vyèistí textboxy
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
            //zjistií zda je pøipojení k serveru stále aktivní, pošle na server zprávu "logout" a ukonèí spojení a server zároven ukonèí spojení s klientem
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
            //funkce vygeneruje èíslo v intervalu a zkontroluje zda již není pøidáno v listu IDlist, uloží ho tam a vrátí int
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





