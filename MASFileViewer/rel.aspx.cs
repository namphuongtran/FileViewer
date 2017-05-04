using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MASFileViewer
{
    public partial class rel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string altRelFilePath = ConfigurationManager.AppSettings["AltRelFilePath"];
            if (!string.IsNullOrEmpty(altRelFilePath))
            {
                _fileViewer.AltRelFilePath = altRelFilePath;
            }
        }
    }
}