using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MASFileViewer
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rootPath = ConfigurationManager.AppSettings["RootFolder"];
            string filePathViewer = Request.Url.AbsolutePath;
            if (!string.IsNullOrEmpty(rootPath))
            {
                filePathViewer = rootPath;
            }
            _fileViewer.AltRelFilePath = filePathViewer;
        }
    }
}