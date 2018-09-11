using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Welcomesite : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "typewriter", "typewriter();", true);
        Response.AddHeader("REFRESH", "7;URL=Default.aspx");
    }
}