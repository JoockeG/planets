using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data.SqlClient;

namespace Planetregister
{

    public partial class Form1 : Form
    {
        List<string> resultat = new List<string>(); // Här skapas en ny lista att lagra resultaten från databasen i.
        List<string> checklista = new List<string>(); // Här skapas en lista som ska användas för att checka av existerande planeter vid insättning mot databasen på ett effektivt sätt.
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; AttachDbFilename='|DataDirectory|\\Planetdb.mdf';Integrated Security=True"; //Connectionstring till databasen
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);// Här skapas ett nytt objekt av klassen "SqlConnection" som används för att upprätthålla en kontakt till en SQL-databas. Här används connectionstring som inparameter som definierades i början av sekvensen. Man anger alltså databasens värden och sökväg som inparameter till konstruktorn.
            richTextBox1.Text = "Namn" + '\t' + "Årslängd" + '\t' + "Dygnslängd"; // Här skrivs rubriker ut i resultatBoxen, samt en tom rad under dessa för användarvänlighetens skull.
            richTextBox1.Text += '\n';
            SqlCommand selectsql = new SqlCommand("SELECT * FROM Planeter", connection); // Här skapas ett objekt av klassen "SqlCommand" som representerar ett SQL-uttryck som kan köras mot en angiven databas. Som inparametrar anges delvis SQL-uttrycket som objektet ska representera i form av en sträng, men även "connection" alltså objektet av klassen "SqlConnection" som tidigare skapats som representerar vilken databas som SQL-frågan ska hänvisas till och köras mot.
            connection.Open();// Här kallas metoden Open för objektet av klassen SqlConnection, som innebär att databasanslutningen upprättas (och fortsätter tills .Close kallas i sekvensen).
            SqlDataReader rdr = selectsql.ExecuteReader(); // En variabel skapas här som representerar ett "SqlDataReader"-objekt. Denna variabel knyts sedan till SqlCommand-objektet "selectsql" som skapades ovan. Här skapas helt enkelt ett datareader objekt med hjälp av metoden "Execute Reader" som körs för objektet "selectsql".

            while (rdr.Read())  // En while-loop som i varje iteration kallar på metoden "Read" för objektet "rdr" (alltså DataReader-objektet). "Read"-metoden läser rad för rad enligt tidigare angiven databaskälla/SQL-fråga, fram tills den sista raden. Därför går den att använda inuti en while-loop som här, det innebär att sålänge metoden returnerar värden (alltså att det finns en nästa rad i databastabellen) så går iterationen vidare.
            {
                checklista.Add(rdr["Planetnamn"].ToString());           // Adderar varje planetnamnt till checklistan (den kommer användas senare i programmet vid import)
                string namn = rdr["Planetnamn"].ToString();           // Här knyts varje värde från databastabellen till en strängvariabel. "rdr["Namn"]" hänvisar till kolumnen "Namn" i rdr-objektet på nuvarande rad (där Read-metoden i just denna iteration befinner sig.
                string arslangd = rdr["Arslangd"].ToString();
                string dygnslangd = rdr["Dygnslangd"].ToString();
                string result = namn + '\t' + arslangd + '\t' + dygnslangd;  // Här knyts alla tre värdena ihop i en sträng tillsammans med tab-steg mellan varje värde för en snyggare utskrivning i resultatboxen.
                resultat.Add(result);                                        // den sammalagda strängen lagras i en lista. För varje inläst rad från databasen så sparas alltså först varje kolumn i en varsin sträng, sen skrivs de tre värdena ihop i en sträng tillsammans med tab-steg, och denna resultatsträngen sparas sedan i listan över resultatsträngar som sen kommer skrivas ut.
            }
            rdr.Close(); // Stänger SqlReaderObjektet för att anslutningen ska kunna naänvdas till annat (spear ingen roll i detta specifika fall)
            connection.Close(); // Här stängs databasanslutningen ner och avslutas. Detta görs när while-loopen är färdig och därmed alla tabellrader från databasen inlästa.

            foreach (string s in resultat)   // Nu är alla rader inlästa, och sparade i listan. Därför skriver vi nu ut alla strängar från listan i resultatboxen. Här skrivs alltså alla värden ut, som fanns i databastabellen vid uppstart av programmet.
            {
                richTextBox1.Text += '\n' + s;
            }
            
        }
        

        private void infobtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show( "Program: Planetregister" + '\n' +
											   "Datum: 13 Mars 2017");   // Kod för att visa information om programmet vid knapptryckning.
        }

        private void rensabtn_Click(object sender, EventArgs e)
        {
            planetnamnin.Clear();     // Här rensas alla textBoxar från data, detta görs när användaren trycker på "Rensa"-knappen.
            dygnslangdin.Clear();
            arslangdin.Clear();
        }

        private void registrerabtn_Click(object sender, EventArgs e)
        {

            try  // Allt i registreringen testas för undantag, för att kunna hantera eventuella problem vid skrivning till databasen.
            {
                SqlConnection connection = new SqlConnection(connectionString);// Här skapas ett nytt objekt av klassen "SqlConnection" som används för att upprätthålla en kontakt till en SQL-databas. Här används connectionstring som inparameter som definierades i början av sekvensen. Man anger alltså databasens värden och sökväg som inparameter till konstruktorn.
                SqlCommand insertsql = new SqlCommand("INSERT INTO Planeter(Planetnamn, Arslangd, Dygnslangd) VALUES(@Planetnamn, @Arslangd, @Dygnslangd)", connection); // Här skapas ett objekt av klassen "SqlCommand" som representerar ett SQL-uttryck som kan köras mot en angiven databas. Som inparametrar anges delvis SQL-uttrycket som objektet ska representera i form av en sträng, men även "connection" alltså objektet av klassen "SqlConnection" som tidigare skapats som representerar vilken databas som SQL-frågan ska hänvisas till och köras mot. I detta fall är detta en SQL-syntax för att göra en insättning i databasen, alltså en registrering av en planet. Här används "insert into" samt tre parametrar som håller ingående värden från textboxar.
                SqlCommand selectsql = new SqlCommand("SELECT * FROM Planeter WHERE Planetnamn = @Planetnamn", connection); // Här skapas ett objekt av klassen "SqlCommand" som representerar ett SQL-uttryck som kan köras mot en angiven databas. Som inparametrar anges delvis SQL-uttrycket som objektet ska representera i form av en sträng, men även "connection" alltså objektet av klassen "SqlConnection" som tidigare skapats som representerar vilken databas som SQL-frågan ska hänvisas till och köras mot. I detta fall är det en select-sats som visar alla data från en viss rad, där Namn-kolumnen (alltså PK) stämmer överens med angiven inparameter som definieras som "@Namn".
                insertsql.Parameters.AddWithValue("@Planetnamn", planetnamnin.Text); // Här anges tre parametrar, en för varje attribut. Dessa används i ovan SqlCommands för att representera textBoxarnas värden (alltså det som angivits av användaren). Detta är ett smidigt sätt jämfört med att skriva in "planetnamnin.Text" o.s.v i strängen ovan, då det lätt blir kladdigt. Dessutom är det ett bra sätt att motverka s.k. SQL-injektion. Detta kallas paramaterisering. Det som händer i kodraden är egentligen att SqlCommandot "insertsql" som vi kallar det, kallar på egenskapen "Parameters" och sedan metoden "AddWithValue", där sedan ett namn på parametern -i detta fall @Namn- samt ett värde -i detta fall en textbox- anges som inparametrar till metoden för att visa vad som ska representeras och hur det ska representeras av parametern.
                insertsql.Parameters.AddWithValue("@Arslangd", arslangdin.Text);
                insertsql.Parameters.AddWithValue("@Dygnslangd", dygnslangdin.Text);
                selectsql.Parameters.AddWithValue("@Planetnamn", planetnamnin.Text);  //Här anges en parameter även för "selectsql" för att kunna använda det angivna selectkommandot ovan.
                checklista.Add(planetnamnin.Text); // Adderar likaså här det nya planetnamnet till checklista som används senare vid importen
                connection.Open();// Här kallas metoden Open för objektet av klassen SqlConnection, som innebär att databasanslutningen upprättas (och fortsätter tills .Close kallas i sekvensen).
                insertsql.ExecuteNonQuery(); //Här kallas metoden "ExecuteNonQuery" för SqlComman "insertsql". Detta är en metod som används då man inte vill ha några rader/värden tillbaka från databasen, alltså t.ex. när man kör "Update" eller "Insert", eller "Delete". Denna metod returnerar endast antalet rader påverkade, inget annat. Här exekveras alltså helt enkelt SqlCommand som tidigare skapades enligt namnet "insertsql" mot databasen. Här sätts en ny planet in i databastabellen (om allt stämmer).
                SqlDataReader rdr = selectsql.ExecuteReader(); // En variabel skapas här som representerar ett "SqlDataReader"-objekt. Denna variabel knyts sedan till SqlCommand-objektet "selectsql" som skapades ovan. Här skapas helt enkelt ett datareader objekt med hjälp av metoden "Execute Reader" som körs för objektet "selectsql".
                while (rdr.Read())  // Här körs metoden "Read" för sqlreader-objektet. I detta fall vet vi att det endast är en specifik rad vi letar efter, eftersom SQL-kommandot är angivet med en "Where" som specificerar en primärnyckel. Det kommer alltså resultera i en radläsning.
                {
                    string namn = rdr["Planetnamn"].ToString();
                    string arslangd = rdr["Arslangd"].ToString();
                    string dygnslangd = rdr["Dygnslangd"].ToString();
                    richTextBox1.Text += '\n' + namn + '\t' + arslangd + '\t' + dygnslangd; // Här skrivs resultaten av den funna raden ut i resultatboxen.
                                                                                            // Det som händer i ovan rader är helt enkelt att en SELECT körs mot databasen med en WHERE som specificerar planetnamnet till det som står i textboxen (alltså det som angivits av användaren). Här tar man alltså fram den planet som användaren precis har registrerat i databasen, och skriver ut den i resultatboxen. Den planet som sätts in, söks sedan upp med hjälp av sitt angivna planetnamn, och läggs till i resultatboxen. Så fort man lägger till en ny planet i databasen så letas den alltså upp och skrivs ut.

                    MessageBox.Show("Planeten " + namn + " har registrerats."); // till sist i loopen så öppnas en MessageBox för i programmet för att bekräfta att planeten registrerats.
                }
                rdr.Close(); // Stänger SqlReaderObjektet för att anslutningen ska kunna naänvdas till annat (spear ingen roll i detta specifika fall)
                connection.Close(); // Här stängs databasanslutningen ner och avslutas.
            }

            catch (SqlException exception)     // Detta är catch-blocket som slår in om några undantag/exceptions skulle uppstå under kodkörningen i try-blocket. Här anges även en exceptionhandler (alltså en undantagshanterare). Det finns flera olika sådana här, exempelvis "sqlexception" eller "InvalidOperationExcepteion", eller "ArgumentNullException" o.s.v. Dessa anges inom parentesen som följer nyckelordet "catch" och specificerar helt enkelt vilken typ av fel/undantag som hanteras. Man väljer denna utifrån vilka fel man förväntar sig kan uppstå i try-blocket. Man kan även ha flera catch-block med olika exceptionhandlers för olika fel, alla kopplade till samma try-block. Då väljs den som stämmer överens med undantaget som sker och därmed triggrar "catch". I detta fallet räknar vi främst med att problem ska uppstå med SQL och skrivningen till databasen. "SqlException" är en undantagshantering som reagerar när SQL-databas returnerar felmeddelanden så som att en primärnyckel av ett visst värde redan finns, eller att fel datatyp har angetts i ett fält. Om ingen undantagshanterare hade angivits så hade programmet avbrutits enligt en standardrutin i c#. (alltså om man bara skrivit catch med tomma parenteser).
            {
                MessageBox.Show(exception.Message); // Denna kod är det som sker då SqlException triggrar. Här väljer vi att använda undantagshanteraren som döps till "exception" ovan (alltså SqlException-objektet), och använda detta för att kalla på dess metod "Message". Metoden "Message" ger ut ett felmeddelande som beskriver det aktuella undantaget som har skett. Alltså om ett existerande PK-värde skulle försöka läggas in i databasen så visas meddelandet "VIOLATION OF PRIMARY KEY.....". och liknande.
            }
        }

        private void exportbtn_Click(object sender, EventArgs e)
        {

            int raknare = 0; // en intvariabel som senare används för att skriva ut antalet planeter i en messagebox.
            SqlConnection connection = new SqlConnection(connectionString); // Ett nytt connectionobjekt skapas med den connectionstring som deklarerades högre upp.
            SqlCommand selectsql = new SqlCommand("SELECT * FROM Planeter", connection); // Nytt SQLkommando för att hämta ut alla data ur tabellen.
            SqlDataAdapter adapter = new SqlDataAdapter(selectsql); // DataAtapter att använda för att fylla DataSet med data från databasen.
            DataSet ds = new DataSet(); // DataSet instansieras här. I labb 4.3 använde vi inget DataSet utan SqlDataReader istället, så det finns inget DataSet sedan tidigare i denna öabb. Därför skapas ett här.
            adapter.Fill(ds, "Planeter"); // dataadaptern används för att fylla Datasetet "ds" med ett DataTable vi väljer att kalla för "Planeter" 

            XmlDocument dok = new XmlDocument(); // Ett nytt objekt av klassen XmlDocument skapas som används för att representera ett specifikt XML-dokument.

            XmlElement planet = dok.CreateElement("Planet"); // Här använder vi vårt XmlDocument ("dok") för att kalla på metoden "CreateElement" som skapar ett element i XML-dokumentet enligt specificerat namn (i detta fall anger vi namnet Planet, vilket vi även kallar Elementvariabeln). Variabeln "planet" representerar alltså elementet "Planet" i Textdokumentet som är förälder till de tre planetattributen om man ser till själva hierarkin i textdokumentet.

            XmlElement planetnamn = dok.CreateElement("Planetnamn"); // Här använder vi vårt XmlDocument igen för att skapa tre element till.  Dessa döper vi till de tre strängarna vi ser i parenteserna (detta är de namn som elementen/taggarna kommer att ges i själva XML-textdokumentet. Vi väljer att ge de tre variablerna samma namn för att repsresentera dessa element i koden.
            XmlElement dygnslangd = dok.CreateElement("Dygnslangd");
            XmlElement arslangd = dok.CreateElement("Arslangd");

            // Än så länge har vi endast angivit ett antal element på samma logiska/hierarkiska nivå. Nu ska vi bygga upp själva assosiationerna mellan dessa/strukturen. Detta görs med metoden "AppendChild" som anger ett element som "barn" för ett annat element.

            planet.AppendChild(planetnamn); // Här anger vi elementet "planetnamn" som barn för elementet "planet". Likaså med de två nedan.
            planet.AppendChild(dygnslangd);
            planet.AppendChild(arslangd);

            dok.AppendChild(planet);    // Här anger vi elementet "planet" som barn för "dok" som alltså är själva XmlDocumentet. "planet"-elemenet blir alltså den översta föräldern i hierarkin (den som befinner sig direkt under roten som är taggen/elementet "<XML>").


            StreamWriter filskrivare = File.CreateText("exportfil.xml"); // Här instansierar vi en StreamWriter för att skriva ut text till en fil. VI behöver inte Serialisera filskrivningen eller dylikt, eftersom det är ren text som ska skrivas till ett Textdokument (som att spara text till filer). Vi anger att det ska skapas ett nytt textdokument vid namn "exportfil.xml" (denna läggs som standard av Visual Studio i projektmappen och vi behöver därför inte ange specifik sökväg). Eftersom vi kallar på CreateFile så skapas den på nytt varje gång och skriver alltså över den existerande filen.
            XmlTextWriter xmlw = new XmlTextWriter(filskrivare); // Här instansierar vi en XmlTextWriter som är en klass som används i StreamWriter-sammanhang när man arbetar med specifikt XML. Den genererar och sköter en filström som är optimal och effektiv för att fungera när man sparar ner just XML-text. Vi knyter den till vårt StreamWriter genom att ange den som inparameter.
            xmlw.Formatting = Formatting.Indented; // Här sätter vi vår XmlTextWriter's (kallad "xmlw")  "Formatting"-egenskap till "Formatting.Indented". Detta görs för att indents, alltså inhopp i textfilen/XML-filen ska göras vid varje ny child-nivå. Man specificerar helt enkelt formateringsegenskaperna för XmlTextWriter till att använda Indented för att möjliggöra att barnelement skrivs med inhopp i XML-filen.
            xmlw.WriteStartDocument(); // Vi kallar på metoden WriteStartDocument som på egen hand skriver in den översta raden i XML-dokumentet som behövs för att det ska gå att tolka av program. Detta syftar allatså till raden: "<?xml version="1.0" encoding="utf-8"?>". Värdena i denna rad tas automatiskt reda på av programmet när man kallar på just denna metod, och det behöver fskrivas in i XML-filen för funktionaliteten ska stämma.
            xmlw.WriteStartElement("XML"); //Vi sätter den första taggen "starttaggen" eller "startelementet" till just strängen "XML" vilket är standard i dessa sammanhang. Detta leder alltså till att den andra raden i XML-filen skrivs till "<XML>".
                                           
            // Efter de två standardraderna kan vi nu börja skriva vår data till filen.

            foreach (DataRow rad in ds.Tables["Planeter"].Rows) // Foreachloop som går igenom varje DataRow i vårt DataSet, och mer specifikt DataTable "Planeter" som vi fyllde med data tidigare i sekvensen.
            {
                planetnamn.InnerText = rad["Planetnamn"].ToString(); //För varje DataRow skrivs de tre attributen (så som rad["Planetnamn"]) till ett element i XML-filen, notera att vi även skriver "InnerText" vilket behövs för att det ska hamna rätt i dokumentet.
                dygnslangd.InnerText = rad["Dygnslangd"].ToString();
                arslangd.InnerText = rad["Arslangd"].ToString();

                raknare++; // int-variabeln "raknare" inkrementeras med 1 varje gång loopen itererar, denna används sedan för att skriva ut antal i en messageBox.

                dok.WriteContentTo(xmlw); // Metoden WriteContentTo för vårt XmlDocument kallas på, med vår XmlTextWriter som inparameter. Detta för att spara ner de angivna värdena till XML-skrivaren.
            }
            xmlw.WriteEndElement(); // Här använder vi vår XmlTextWriter och kallar på metoden WriteEndElement för att skriva och stänga ner elementen i XML-filen.
            filskrivare.Close(); // Stänger ner vår StreamWriter.

            MessageBox.Show(raknare + " st planeter har exporterats till XML-filen 'exportfil.xml'"); // Messagebox skriver ut en text om exporten.
        }


        private void importbtn_Click(object sender, EventArgs e)
        {
                richTextBox1.Text = "Namn" + '\t' + "Årslängd" + '\t' + "Dygnslängd" + '\n'; // Textboxen för planeterna rensas från tidigare värden (de värden som läses in från databasen vid programstart) Detta för att undvika dubbletter. Skriver ut Rubrikerna för de olika värdena samt nya rader för synlighets skull.
                int raknare = 0; // en intvariabel som senare används för att skriva ut antalet planeter i en messagebox.
                SqlConnection connection = new SqlConnection(connectionString); // Ett nytt connectionobjekt skapas med den connectionstring som deklarerades högre upp.
                SqlCommand selectsql = new SqlCommand("SELECT * FROM Planeter", connection); // Nytt SQLkommando för att hämta ut alla data ur tabellen.
                OpenFileDialog dialog = new OpenFileDialog();  // Kod för att starta "OpenFile"-dialogbox
                dialog.Filter = "xml filer (*.xml)|*.xml";     // Kod för att specificera "OpenFile"-dialogbox
                dialog.Title = "Välj fil för import";          // Kod för att specificera "OpenFile"-dialogbox
                dialog.InitialDirectory = @".";               // Kod för att specificera "OpenFile"-dialogbox
                if (dialog.ShowDialog() == DialogResult.OK)  // if-sats för att kontrollera att använderen trycker på "OK"-knappen i dialogboxen (alltså när man valt en fil i utforskaren och trycker på OK-knappen. Då exekverar koden nedan som läser in XMLfilen.
                {
                    XmlDocument inXml = new XmlDocument();  // Instansierar ett nytt XmlDocument som representerar ett XML-dokument.
                    inXml.Load(dialog.FileName); // Använder metoden Load för att läsa in den angivna filen. Här hänvisas filen till "dialog.Filename" för att öppna den fil som valdes i dialogboxen. Hade man velat skriva in en fil direkt i koden (hårdprogrammera en fast fil) hade denna kodrad istället kunnat vara: "inXml.Load("exportfil.xml")" för att peka på filen "exportfil.xml".
                    XmlNode rooten = inXml.DocumentElement; // Skapar ett XmlNode-objekt (som representerar en specifik nod/element/tagg i WML-dokumentet). Denna representerar XMLDocumentets "DocumentElement" vilket är det första elementet (rotelementet, den föversta föräldern) i dokumentet. Detta representerar roten, alltså <XML>-taggen (från start till slut - alltså från <XML> till och med </XML>).
                    XmlNode planet = rooten.FirstChild; // En till nod som representerar det första barnet sett ur rootens synvinkel. Detta blir alltså taggen "planet". variabeln "planet" representerar alltså den hierarkiska nivån <Planet> till och med </Planet> i XML-dokumentet. Eftersom det troligtvis finns flea Planeter lagrade i dokumentet blir det flera läsningar eftersom de är på smma hierarkiska nivå (inhopp/indent i dokumentet). Därför går det att använda en whileloop som vi gör nedan i detta sammanhang.
                    string temp; // En sträng som används för att temporärt hålla värden inuti loopen.
                    string result = null; // En sträng som används för att lagra varje rad av planetdata (per yttre loop).
                    string namn = "";   //Tre strängar att senare kunna knyta värden från XMLfilen till och använda som parametrar.
                    string arslangd = "";
                    string dygnslangd = "";
                    

                    while (planet != null) // En whileloop som itererar sålänge det finns en nästa/kommande planet-tagg i XML-dokumentet.
                    {
                         XmlNode planetvarde = planet.FirstChild; // en ny tredje nod som representerar nästa hierarkiska nivå (nästa indent/insteg i dokumentet). Denna representerar alltså de insteg som är under "planet"-taggarna, vilket då förstås blir de tre attributen planetnamn, arslangd samt dygnslangd.

                         bool existerandeplanet = false; // variabel som används senare för att bedöma om en planet existerar i databasen
                         namn = planet.SelectSingleNode("Planetnamn").InnerText.ToString(); // Här använder vi klassen Xpath och metoden "SelectSingleNode" som hämtar ut värdet av en specifik nod's InnerText i detta fall. För varje yytre iteration, alltså varje planet, så hämtar vi ut de tre värdena och knyter till variabler som används för parametrar till INSERTkommandot.
                         arslangd = planet.SelectSingleNode("Arslangd").InnerText.ToString();
                         dygnslangd = planet.SelectSingleNode("Dygnslangd").InnerText.ToString();
                         SqlCommand insertsql = new SqlCommand("INSERT INTO Planeter(Planetnamn, Arslangd, Dygnslangd) VALUES(@Planetnamn, @Arslangd, @Dygnslangd)", connection); // Här definierar vi vårt SQL-kommando för insättning v planeter, med tre parametrar.
                         insertsql.Parameters.AddWithValue("@Planetnamn", namn); // Här definierar vi de tre parametrar som används i sqlkommandot, med de värden vi fick ut genom SelectSingleNode. Notera att både SqlCommand-objektet samt dessa tre parametrar definieras för varje yttre iteration. Detta för att de ska förändras i takt med att XML-taggarna gör det.
                         insertsql.Parameters.AddWithValue("@Arslangd", arslangd);
                         insertsql.Parameters.AddWithValue("@Dygnslangd", dygnslangd);

                         foreach (string s in checklista)//En forloop som kontrollerar om en planet redan finns i databasen. Här använder vi en lista med strängar som håller alla planeter som finns från början, eller registreras under körning. För varje yttre iteration (varje Planet i XML-filen) jämför vi dess namn mot namnen i checklistan över existerande planeter. Skulle en match finnas så bryts loopen och en variabel sätts till true (variabeln "existerandeplanet). Skulle hela loopen köra igenom utan matchningan så innebär detta att planeten INTE finns i databasen och därmed ska en INSERT Göras. Allat detta är felhantering gentemot databasen, för att inte programmet ska försöka sätta in dubbla värden och bryta mot primärnyckelregler.
                         {
                            if(s == namn)
                            {
                                existerandeplanet = true;
                                break;
                            }
                            else
                            {
                                existerandeplanet = false;
                            }
                         }

                         if(existerandeplanet)
                         {
                         }
                         else // Om "existerandeplanet" är falsk - alltså nuvarande planet finns INTE i databasen, så ska en insert göras. 
                         {
                            raknare++; // för varje yttre iteration (varje planet) så inkrementeras "raknare" för att räkna antalet inlästa planeter.
                            connection.Open();
                            insertsql.ExecuteNonQuery(); // Detta innebär att vi öppnar en anslutning mot databasen och kör vårt "insertsql"-kommando med ExecuteNonQuery eftersom vi inte ska ha några rader tillbaka.
                            connection.Close();
                         }
                          // Här är den INRE LOOPEN färdig, och en hel planet har alltså nu gåtts igenom i XML-filen. Om den fanns i databasen har den inte lagts till, fanns den inte så har den nu lagts till.
                         planet = planet.NextSibling; // Vi kallar metoden "NextSibling" för den yttre loopen (en nivå upp i hierarkin) alltså för elementet "planet" som hoppar ner till nästa planet. Har vi fyra planeter i XML-dokumentet kommer det alltså ske fyra loopar och efter den fjärde loopen kommer NextSibling att returnera "null" (eftersom det är slut på planettaggar i XML-dokumentet) och då slutar loopen (den yttre).
                         // Detta används endast om man skriver ut kod direkt från XML i programmet vilket vi inte gör. richTextBox1.Text += result; // När de tre inre iterationerna är klara (alltså de tre planetattributen har skrivits till strängen "result", och nästa yttre loop ska påbörjas så skrivs resutat ut i richTextBox  
                    }
                    //Här är den YTTRE LOOPEN färdig och nu har ALLA planeter gåtts igenom. De som inte fanns i databasen har nu lagts till. Nedan så läser vi in ALL data från tabellen PLANETER i vår databas. VI läser alltså inte bara in nya värden utan ALLT från databasen för att visa att nya värden har lagrats i databasen.

                    MessageBox.Show(raknare + " st nya planeter har importerats från XML-filen " + dialog.FileName); // En messagebox skriver ut hur många planeter som har importerats från XML-filen.
                    checklista.Clear(); // rensar checklistan för att fylla den på nytt när vi hämtar alla planeter från databasen nedan.
                    connection.Open(); // Öppnar anslutning
                    SqlDataReader rdr = selectsql.ExecuteReader(); //SqlDataReader exekveras
                    while (rdr.Read())        // Läser så länge det finns en kommande rad i tabellen
                    {
                        checklista.Add(rdr["Planetnamn"].ToString());
                        string namn1 = rdr["Planetnamn"].ToString();       // FÖr varje rad hämtas de tre värdena och knyts till strängvariabler
                        string arslangd1 = rdr["Arslangd"].ToString();
                        string dygnslangd1 = rdr["Dygnslangd"].ToString();
                        richTextBox1.Text += '\n' + namn1 + '\t' + arslangd1 + '\t' + dygnslangd1; // De tre värdena skrivs ut i richTextBox.
                    }
                    rdr.Close();    // Efter all läsning skett så stängs först läsaren sen anslutningen.
                    connection.Close();
                }
         }
        }
    }