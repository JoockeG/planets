using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page
{
    List<Handelse> handelser = new List<Handelse>();
    DataSet ds;
    DataTable dt;
    int temp;

    protected void Page_Load(object sender, EventArgs e)
    {

        string cs = "Data Source=MARTIN-PC; Initial Catalog = Tidslinje; Integrated Security = True";
            SqlConnection conn = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("SELECT * FROM ww2tabell", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            conn.Open();
            da.Fill(ds);
            conn.Close();
            dt = ds.Tables[0];
            string titeltemp = "a";
            string beskrivningtemp = "a";
            string landtemp = "a";
            int datumtemp = 0;

      
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    datumtemp = Convert.ToInt32(row["datum"]);
                    titeltemp = row["titel"].ToString();
                    beskrivningtemp = row["beskrivning"].ToString();
                    landtemp = row["land"].ToString();
                }
                handelser.Add(new Handelse(titeltemp, beskrivningtemp, landtemp, datumtemp));
            }
      

            handelser.Sort((x, y) => x.Datum.CompareTo(y.Datum));


            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "click();", true);
       
      
    }

    public class Handelse
    {
        string titel;
        string beskrivning;
        string land;
        int datum;

        public Handelse(string titelin, string beskrivningin, string landin, int datumin)
        {
            Titel = titelin;
            Beskrivning = beskrivningin;
            Land = landin;
            Datum = datumin;
        }

        public string Titel
        {
            get { return titel; }
            set { titel = value; }
        }
        public string Beskrivning
        {
            get { return beskrivning; }
            set { beskrivning = value; }
        }
        public string Land
        {
            get { return land; }
            set { land = value; }
        }
        public int Datum
        {
            get { return datum; }
            set { datum = value; }
        }
    }




    protected void Button1_Click(object sender, EventArgs e)
    {
        temp = Convert.ToInt32(Label1.Text);
        if (temp >= handelser.Count)
        {
        }
        else
        {
            string datum = handelser[temp].Datum.ToString();
            string datumny = datum[0].ToString() + datum[1].ToString() + datum[2].ToString() + datum[3].ToString() + '-' + datum[4].ToString() + datum[5].ToString() + '-' + datum[6].ToString() + datum[7].ToString();
            HiddenField1.Value = datumny;
            HiddenField2.Value = handelser[temp].Titel +  " i " + handelser[temp].Land + "- " + handelser[temp].Beskrivning;
            int count = 0;
            count++;
            ViewState["count"] = Convert.ToInt32(ViewState["count"]) + count;
            Label1.Text = ViewState["count"].ToString();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "myFunction", "myFunction();", true);
        }
        
    }

}