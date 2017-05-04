using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MASFileViewer
{
    public partial class view : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string id = Request.QueryString["id"];
                    string fullFileName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(HttpUtility.UrlDecode(id)));
                    FileInfo file = new FileInfo(fullFileName);
                    if (file.Exists)
                    {
                        var data = File.ReadAllText(fullFileName);
                        lblContent.Text = "<pre><code>" + data.ToString() + "</code></pre>";
                    }
                    else
                    {
                        lblContent.Text = "No file existed!";
                    }
                }
            }
        }
    }
}