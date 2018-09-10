using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Asteroids
{
    public partial class Form1 : Form
    {
        DataTable dt; // Variabel för datatable
        string namnvarde; // variabel för valt värde
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\\Asteroiddatabase.mdf;Integrated Security=True"; // ConnectionString deklareras
        DataSet ds; // variabel för dataset
        SqlDataAdapter dataadapter; // variabler för adapter och connection
        SqlConnection connection;
        string sql = "SELECT * FROM Asteroids"; //sql-sträng deklareras som hämtar ut all data från tabellen "Asteroider"

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionString); // Här skapas ett nytt objekt av klassen "SqlConnection" som används för att upprätthålla en kontakt till en SQL-databas. Här används connectionstring som inparameter som definierades i början av sekvensen. Man anger alltså databasens värden och sökväg som inparameter till konstruktorn.
            dataadapter = new SqlDataAdapter(sql, connectionString); // Här skapas ett nytt objekt av klassen "SqlDataAdapter", som används för att sköta kontakt mot en databas. Denna används då man vill manipulera en SQLdatabas (uppdatera värden) samt hämta ut värden från databasen till programmet och lagra i ett DataSet. Här anges tidigare definierat SQL-kommando samt connectionstring som inparametrar för att peka på vilken databas det gäller samt vilken SQL-sats som ska användas (detta kan anges på olika sätt för denna klass, exempelvis behöver man inte ange vilken connectionstring och SQL-sats som ska användas vid konstruktorn utan kan göra detta i senare skede). 
            ds = new DataSet(); // Här skapas ett objekt av klassen "DataSet", som används för att lagra/representera data från exempelvis en databas tillfälligt i RAM (under körning av programmet). DataSet lagrar ett komplett set av data inkluderande tabeller, dess ordninig, primärnycklar, främmande nycklar, relationer mellan tabeller etc. DataSet kan helt enkelt användas för att lagra "en hel databas" i en tillfällig variabel som programmet kan komma åt på en direkt nivå och manipulera.
            connection.Open(); // Här kallas metoden Open för objektet av klassen SqlConnection, som innebär att databasanslutningen upprättas (och fortsätter tills .Close kallas i sekvensen).
            dataadapter.Fill(ds); // Här kallas metoden "Fill" för objektet av klassen SqlDataAdapter, som fyller angivet DataSet med data från den databas som anges i dataadapterns ena inparameter högre upp (connectionstring). Det DataSet som ska fyllas anges som inparameter, vilket vi ser i koden där "ds" anges som inparameter efter metoden ".Fill". Detta är den enda inparameter som är ett måste för denna metod, men i detta fall har även inparametern "Planeter" angetts på andra parameterposition. Detta syftar till ett DataTable, alltså en tabell I DATASETET som man pekar på. I detta fall menas alltså att SqlAdaptern ska använda sin angivna connectionstring samt sqlstring, för att fylla datasetet "ds" OCH I DETTA DATASET SKA DATAN HAMNA I DATATABLE SOM HETER "Planeter". Detta görs för att senare i koden kunna referera till ett specifikt datatable i Dataset och kunna få ut det i DataGridView-kontrollen.
            connection.Close(); // Här stängs databasanslutningen ner och avslutas.
            dt = ds.Tables[0];
            dataGridView1.DataSource = ds.Tables[0]; // Här anges att dataGridView-kontrollens "DataSource" - alltså från vilken datakälla som data i GridViewen ska visas. Här anges alltså att dess datakälla ska vara ds(DataSetet) och mer specifikt tabellen "Planeter" (som angavs vid metoden ".Fill" tidigare). ds.Tables[xxx] pekar alltså på ett specifikt DataTable i DataSetet "ds". Nu kan DataGridView visa all data från DataSetet "ds" och mer specifikt tabellen "Planeter". Eftersom DataSet kan hålla ALLA egenskaper från databasen så har den koll på primärnycklar och det är därför som rätt planetnamn knyts till rätt årslängd och dygnslängd i DatagridView. Hade vi inte angett "ds.Tables[Planeter]" utan bara "DataSource = ds" hade DataGridView inte kunnat skriva ut något, då den måste ha en specifik DataTable som indata, den kan inte bearbeta ett helt dataset utan behöver det mer specifikt angett.
            // Nu har vi alltså fyllt Dataset[0] med tabellen Asteroider (alla attribut)
        }

        private void dataGridView1_Cellclick(object sender, DataGridViewCellEventArgs e)
        {
                namnvarde = dataGridView1.Rows[e.RowIndex].Cells["Namn"].Value.ToString(); // Denna kod tar reda på vilken cell som är markerad i DataGridView. strängen "namnvärde" knyts till ett Asteroidnamn. "RowIndex" tar reda på vilken rad man befinner sig på (alltså som är markerad med musen). Mer specifikt blir det sedan "Cells" och till slutt hämtas värdet ut på Kolumn "Namn". Alltså står markören på "Ceres"-raden så är det strängen "Ceres" (alltså namn-kolumnens värde) som knyts till variabeln "namnvarde".
                foreach (DataRow row in dt.Rows) // loopar igenom varje rad i tabellen "dt".
                {
                    if (row["Namn"].ToString() == namnvarde) //om kolumnen "namns" värde är detsamma som variabeln "namnvarde" ovan så skrivs dess attribut ut i textrutorna i programmet, annars inte. Detta görs för att den rad som är markerad i DataGridViewen ska skrivas ut i boxarna.
                    {
                        DataRow rw = row;
                        namnbox.Text = rw["Namn"].ToString();
                        omloppstidbox.Text = rw["Omloppstid"].ToString();  // Här representerar rw["Omloppstid"] raden "rw" som loopen befinner sig på, samt dess kolumn vid namn "Omloppstid". Den kolumnens värde är det som detta resresenterar.
                        upptackarebox.Text = rw["Upptäckare"].ToString();
                    }
                }
        }

        private void uppdaterabtn_Click(object sender, EventArgs e)
        {
            if((string.IsNullOrEmpty(namnbox.Text)) || (string.IsNullOrEmpty(omloppstidbox.Text)) || (string.IsNullOrEmpty(upptackarebox.Text))) // Kontroll för att se att alla tre fält är ifyllda när uppdatering görs. För att hantera undantag. Om de inte är det så utförs inte resten av koden mot databasen etc.
            {
                MessageBox.Show("Fyll i alla tre fält för att kunna uppdatera en asteroid i databasen.");
            }
            else
            {
                bool temp = false;          // mer felhantering´, denna gång för att kontrollera om "Namn", alltså primärnyckeln finns i tabellen. Om det inte finns så går det ju inte att uppdatera ett värde. Detta är alltså också undantagshantering. Samma sak gäller här som ovan. Om detta inte stämmer fortsätter inte koden att exekveras.
                foreach (DataRow rad in dt.Rows)
                {
                    if (rad["Namn"].ToString() == namnbox.Text) // Loopen kollar varje rad för att jämföra "Namn" med det namn i textboxen som ska uppdateras.
                    {
                        temp = false;
                        break;      // hittar den ett passande värde så bryts loopen då det inte är någon idé att söka igenom resterande rader, eftersom en match redan hittats.
                    }
                    else
                    {
                        temp = true;
                    }
                }
                if (temp == true) // Inga matchningar i loopen leder till detta - undantagshantering.
                {
                    MessageBox.Show("Angivet namn finns inte i databasen. Det går därför inte att uppdatera värden för detta namn.");
                }
                else // Om allt stämmer (inga undantag) så exekveras resten av knappens kod.
                {

                    try
                    {
                        SqlCommand sqlupdate = new SqlCommand("UPDATE Asteroider SET Omloppstid = @Omloppstid, Upptäckare = @Upptäckare WHERE Namn = @Namn", connection); // Ett sqlkommando för uppdatering  med tre parametrar.
                        sqlupdate.Parameters.AddWithValue("@Namn", namnbox.Text); // parametrarna knyts till textboxarnas värden.
                        sqlupdate.Parameters.AddWithValue("@Omloppstid", Convert.ToDecimal(omloppstidbox.Text));
                        sqlupdate.Parameters.AddWithValue("@Upptäckare", upptackarebox.Text);
                        connection.Open();// anslutningen mot databasen öppnas.
                        sqlupdate.ExecuteNonQuery(); // Här körs executenonquery eftersom vi kör en uppdatering - alltså inga rader ska returneras.
                        ds.Clear(); // datasetet rensas eftersom vi nu ska fylla det på nytt, när värden har uppdaterats. Notera att detta endast sker EFTER ExequteNonQuery i sekvensen. Alltså skulle ett SQLundantag ske (exempelvis primärnyckel-violation) så hoppas sekvensen från raden ovan direkt till catch - därmed exekveras aldrig denna rad. Så Detta görs endast om SQL-queryn går igenom.
                        dataadapter.Fill(ds); //datasetet fylls på nytt eftersom det nu finns nya uppdaterade värden i databasen
                        connection.Close(); // Här stängs databasanslutningen ner och avslutas.
                    }
                    catch (SqlException exception)     // Detta är catch-blocket som slår in om några undantag/exceptions skulle uppstå under kodkörningen i try-blocket. Här anges även en exceptionhandler (alltså en undantagshanterare). Det finns flera olika sådana här, exempelvis "sqlexception" eller "InvalidOperationExcepteion", eller "ArgumentNullException" o.s.v. Dessa anges inom parentesen som följer nyckelordet "catch" och specificerar helt enkelt vilken typ av fel/undantag som hanteras. Man väljer denna utifrån vilka fel man förväntar sig kan uppstå i try-blocket. Man kan även ha flera catch-block med olika exceptionhandlers för olika fel, alla kopplade till samma try-block. Då väljs den som stämmer överens med undantaget som sker och därmed triggrar "catch". I detta fallet räknar vi främst med att problem ska uppstå med SQL och skrivningen till databasen. "SqlException" är en undantagshantering som reagerar när SQL-databas returnerar felmeddelanden så som att en primärnyckel av ett visst värde redan finns, eller att fel datatyp har angetts i ett fält. Om ingen undantagshanterare hade angivits så hade programmet avbrutits enligt en standardrutin i c#. (alltså om man bara skrivit catch med tomma parenteser).
                    {
                        MessageBox.Show(exception.Message); // Denna kod är det som sker då SqlException triggrar. Här väljer vi att använda undantagshanteraren som döps till "exception" ovan (alltså SqlException-objektet), och använda detta för att kalla på dess metod "Message". Metoden "Message" ger ut ett felmeddelande som beskriver det aktuella undantaget som har skett. Alltså om ett existerande PK-värde skulle försöka läggas in i databasen så visas meddelandet "VIOLATION OF PRIMARY KEY.....". och liknande.
                        connection.Close();
                    }
                    catch (FormatException exception2)     // Catchblock även för "FormatException" som hanterar felaktiga indata osv. Exempelvis om man skulle skriva bokstäver istället för siffror i fältet för "Omloppstid".
                    {
                        MessageBox.Show(exception2.Message); 
                        connection.Close();                 // Connection.Close läggs efter båda undantagen, eftersom om ett undantag sker så hinner inte kontakten avslutas i sekvensen ovan.
                    }
                }
            }
        }

        private void registrerabtn_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(namnbox.Text)) || (string.IsNullOrEmpty(omloppstidbox.Text)) || (string.IsNullOrEmpty(upptackarebox.Text))) // Samma felhantering som för knappen uppdatera, alltså alla tre fält måste vara ifyllda.
            {
                MessageBox.Show("Fyll i alla tre fält för att kunna registrera en ny asteroid i databasen.");
            }
            else
            {
                try
                {
                    SqlCommand sqlinsert = new SqlCommand("INSERT INTO Asteroider VALUES (@Namn, @Omloppstid, @Upptäckare)", connection); // SQLkommando för INSERT med tre parametrar.
                    sqlinsert.Parameters.AddWithValue("@Namn", namnbox.Text); // parametrar åt kommandot ovan.
                    sqlinsert.Parameters.AddWithValue("@Omloppstid", Convert.ToDecimal(omloppstidbox.Text));
                    sqlinsert.Parameters.AddWithValue("@Upptäckare", upptackarebox.Text);
                    connection.Open();// Här kallas metoden Open för objektet av klassen SqlConnection, som innebär att databasanslutningen upprättas (och fortsätter tills .Close kallas i sekvensen).
                    sqlinsert.ExecuteNonQuery(); // ExecuteNonQuery kör vi eftersom det är en INSERT och inga rader ska returneras 
                    ds.Clear(); // precis i samma ordning som ovan rensar vi datasetet för ny data (endast om inget undantag skett för att inte lämna en tom DataViewGrid under körning)
                    dataadapter.Fill(ds); //DataSet fylls på nytt efter INSERT har gjorts.
                    connection.Close(); // Här stängs databasanslutningen ner och avslutas.
                }
                catch (SqlException exception)     // Detta är catch-blocket som slår in om några undantag/exceptions skulle uppstå under kodkörningen i try-blocket. Här anges även en exceptionhandler (alltså en undantagshanterare). Det finns flera olika sådana här, exempelvis "sqlexception" eller "InvalidOperationExcepteion", eller "ArgumentNullException" o.s.v. Dessa anges inom parentesen som följer nyckelordet "catch" och specificerar helt enkelt vilken typ av fel/undantag som hanteras. Man väljer denna utifrån vilka fel man förväntar sig kan uppstå i try-blocket. Man kan även ha flera catch-block med olika exceptionhandlers för olika fel, alla kopplade till samma try-block. Då väljs den som stämmer överens med undantaget som sker och därmed triggrar "catch". I detta fallet räknar vi främst med att problem ska uppstå med SQL och skrivningen till databasen. "SqlException" är en undantagshantering som reagerar när SQL-databas returnerar felmeddelanden så som att en primärnyckel av ett visst värde redan finns, eller att fel datatyp har angetts i ett fält. Om ingen undantagshanterare hade angivits så hade programmet avbrutits enligt en standardrutin i c#. (alltså om man bara skrivit catch med tomma parenteser).
                {
                    MessageBox.Show(exception.Message); // Denna kod är det som sker då SqlException triggrar. Här väljer vi att använda undantagshanteraren som döps till "exception" ovan (alltså SqlException-objektet), och använda detta för att kalla på dess metod "Message". Metoden "Message" ger ut ett felmeddelande som beskriver det aktuella undantaget som har skett. Alltså om ett existerande PK-värde skulle försöka läggas in i databasen så visas meddelandet "VIOLATION OF PRIMARY KEY.....". och liknande.
                    connection.Close(); // Här stängs databasanslutningen ner och avslutas.
                }
                catch (FormatException exception2)     // catch-block förfelaktig indata även här.
                {
                    MessageBox.Show(exception2.Message); 
                    connection.Close();
                }
            }
        }

        private void rensabtn_Click(object sender, EventArgs e)
        {
            namnbox.Clear();  // Här rensas alla textBoxar från data, detta görs när användaren trycker på "Rensa"-knappen.
            omloppstidbox.Clear();
            upptackarebox.Clear();
        }


        private void info_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Program: Asteroider" + '\n' +        // Kod för att visa information om labben vid knapptryckning.
                           					   "Datum: 9 Mars 2017");
        }

        private void stang_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
