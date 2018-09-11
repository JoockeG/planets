using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Updatesite : System.Web.UI.Page
{
    string cs = "Data Source=MARTIN-PC;Initial Catalog = Tidslinje; Integrated Security = True";
    int datum;
    string datumtemp;
    string existerandetitel;
    string titel;
    string beskrivning;
    string land;
    string temp;
    protected void Page_Load(object sender, EventArgs e)
    {
        temp = (string)(Session["titel"]);
        titel = temp.Trim();
        existerandetitel = titel;
        datumtemp = (string)(Session["datum"]);
        datum = Convert.ToInt32(datumtemp);
        temp = (string)(Session["beskrivning"]);
        beskrivning = temp.Trim();
        temp = (string)(Session["land"]);
        land = temp.Trim();

        Label1.Text = "Vald händelse: " + titel;


    }

    protected void uppdaterabtn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(datumin.Text) && string.IsNullOrWhiteSpace(titelin.Text) && string.IsNullOrWhiteSpace(beskrin.Text) && string.IsNullOrWhiteSpace(landin.Text))
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kontrollera att något av de fyra fälten är ifyllda innan du uppdaterar händelsen')", true);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(datumin.Text))
            {
                if (datumin.Text.Length == 8)
                {
                    if (Int32.TryParse(datumin.Text, out datum))
                    {
                        if (!string.IsNullOrWhiteSpace(titelin.Text))
                        {
                            titel = titelin.Text;
                        }

                        if (!string.IsNullOrWhiteSpace(beskrin.Text))
                        {
                            beskrivning = beskrin.Text;
                        }

                        if (!string.IsNullOrWhiteSpace(landin.Text))
                        {
                            land = landin.Text;
                        }

                        try
                        {
                            SqlConnection conn2 = new SqlConnection(cs);
                            SqlCommand cmd2 = new SqlCommand("UPDATE ww2tabell SET datum = @datum, titel = @titel, beskrivning = @beskr, land = @land WHERE titel = @existerandetitel", conn2);
                            cmd2.Parameters.AddWithValue("@datum", datum);
                            cmd2.Parameters.AddWithValue("@titel", titel);
                            cmd2.Parameters.AddWithValue("@beskr", beskrivning);
                            cmd2.Parameters.AddWithValue("@land", land);
                            cmd2.Parameters.AddWithValue("@existerandetitel", existerandetitel);
                            conn2.Open();
                            cmd2.ExecuteNonQuery();
                            conn2.Close();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Händelsen har uppdaterats i databasen');", true);
                            titelin.Text = "";
                            landin.Text = "";
                            beskrin.Text = "";
                            datumin.Text = "";
                        }
                        catch (SqlException)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Något gick fel mot databasen, kontrollera att du valt en händelse att uppdatera. Se även till att du inte överskrider maxgränsen för tecken för respektive värde. Max antal tecken för Titel: 50, Beskrivning: 500, Land: 50');", true);
                        }
                    }

                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kontrollera att datumet endast består av siffror enligt formatet ÅÅÅÅMMDD')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Kontrollera att datumet är 8 siffror långt enligt formatet ÅÅÅÅMMDD')", true);
                }

            }

            else
            {
                if (!string.IsNullOrWhiteSpace(titelin.Text))
                {
                    titel = titelin.Text;
                }

                if (!string.IsNullOrWhiteSpace(beskrin.Text))
                {
                    beskrivning = beskrin.Text;
                }

                if (!string.IsNullOrWhiteSpace(landin.Text))
                {
                    land = landin.Text;
                }

                try
                {
                    SqlConnection conn2 = new SqlConnection(cs);
                    SqlCommand cmd2 = new SqlCommand("UPDATE ww2tabell SET datum = @datum, titel = @titel, beskrivning = @beskr, land = @land WHERE titel = @existerandetitel", conn2);
                    cmd2.Parameters.AddWithValue("@datum", datum);
                    cmd2.Parameters.AddWithValue("@titel", titel);
                    cmd2.Parameters.AddWithValue("@beskr", beskrivning);
                    cmd2.Parameters.AddWithValue("@land", land);
                    cmd2.Parameters.AddWithValue("@existerandetitel", existerandetitel);
                    conn2.Open();
                    cmd2.ExecuteNonQuery();
                    conn2.Close();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Händelsen har uppdaterats i databasen');", true);
                    titelin.Text = "";
                    landin.Text = "";
                    beskrin.Text = "";
                    datumin.Text = "";
                }
                catch (SqlException)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Något gick fel mot databasen, kontrollera att du valt en händelse att uppdatera. Se även till att du inte överskrider maxgränsen för tecken för respektive värde. Max antal tecken för Titel: 50, Beskrivning: 500, Land: 50');", true);
                }
            }
        }

    }



    protected void tillbakabtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("Editsite.aspx");
    }
}