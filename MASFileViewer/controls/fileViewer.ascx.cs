using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MASFileViewer.controls
{
    public partial class fileViewer : System.Web.UI.UserControl
    {
        private string _root;
        private string _altRelFilePath;
        char[] delimiterChars = { ' ', ',', ';', ':', '\t' };
        //string[] _goodFileTypes={ ".gif", ".pdf", ".png", ".jpg", ".jpeg", ".doc", ".docx", ".xls", ".xlsx", ".psd", ".eps", ".zip", ".ai", ".ppt", ".pptx" };
        List<string> _goodFileTypes = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string fileTypes = ConfigurationManager.AppSettings["FileTypes"];
            if (!string.IsNullOrEmpty(fileTypes))
            {
                _goodFileTypes = fileTypes.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            CreateTree();
        }

        private void CreateTree()
        {
            _root = String.IsNullOrEmpty(_altRelFilePath) ? Server.MapPath(Request.Url.AbsolutePath) : Server.MapPath(_altRelFilePath);
            _tree.Nodes.Clear();
            var rootDirectory = String.IsNullOrEmpty(_altRelFilePath) ? new DirectoryInfo(Path.GetDirectoryName(_root)) : new DirectoryInfo(_root);
            TreeNode t = CreateDirectoryNode(rootDirectory);
            if (t != null)
            {
                _tree.Nodes.Add(t);
            }
            else
            {
                _noFiles.Visible = true;
                _files.Visible = false;
            }
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            TreeNode directoryNode = null;

            if (directoryInfo.Exists && CanViewFiles(directoryInfo))
            {
                directoryNode = new TreeNode(directoryInfo.Name) { Value = directoryInfo.FullName };

                directoryNode.NavigateUrl = "";
                directoryNode.SelectAction = TreeNodeSelectAction.Expand;

                foreach (var directory in directoryInfo.GetDirectories())
                {
                    TreeNode t = CreateDirectoryNode(directory);
                    if (t != null)
                    {
                        directoryNode.ChildNodes.Add(t);
                    }
                }

                foreach (var file in directoryInfo.GetFiles().Where(a => _goodFileTypes.Contains(a.Extension.ToLower())))
                {
                    var f = new TreeNode(file.Name);
                    f.ToolTip = "See Syntax Highlighting";
                    //f.Text = f.Text + FileLength(file.Length);
                    //f.Target = "_blank";
                    switch (file.Extension.ToLower())
                    {
                        case ".doc":
                        case ".docx":
                            f.ImageUrl = "/images/treeview/file.png";
                            f.NavigateUrl = RelativePath(file.FullName);
                            break;
                        case ".pdf":
                            f.ImageUrl = "/images/treeview/pdf.png";
                            f.NavigateUrl = RelativePath(file.FullName);
                            break;
                        case ".psd":
                            f.ImageUrl = "/images/treeview/psd.png";
                            f.NavigateUrl = RelativePath(file.FullName);
                            break;
                        case ".xlsx":
                        case ".xls":
                            f.ImageUrl = "/images/treeview/xls.png";
                            f.NavigateUrl = RelativePath(file.FullName);
                            break;
                        case ".zip":
                            f.ImageUrl = "/images/treeview/zip.png";
                            f.NavigateUrl = RelativePath(file.FullName);
                            break;
                        case ".gif":
                            f.ImageUrl = "/images/treeview/gif40.png";
                            f.NavigateUrl = RelativePath(file.FullName);
                            break;
                        case ".png":
                            f.ImageUrl = "/images/treeview/png.png";
                            f.NavigateUrl = RelativePath(file.FullName);
                            break;
                        case ".jpg":
                            f.ImageUrl = "/images/treeview/jpg40.png";
                            f.NavigateUrl = RelativePath(file.FullName);
                            break;
                        case ".cs":
                            f.ImageUrl = "/images/treeview/cs.gif";
                            f.NavigateUrl = RedirectToView(file.FullName);
                            break;
                        case ".c++":
                            f.ImageUrl = "/images/treeview/cpp.png";
                            f.NavigateUrl = RedirectToView(file.FullName);
                            break;
                        case ".h":
                        case ".cc":
                        case ".c":
                        case ".C":
                        case ".cp":
                        case ".cpp":
                        case ".hh":
                            f.ImageUrl = "/images/treeview/c.png";
                            f.NavigateUrl = RedirectToView(file.FullName);
                            break;
                        default:
                            f.ImageUrl = "/images/treeview/" + ConfigurationManager.AppSettings["DefaultImageIcon"];
                            break;
                    }

                    directoryNode.ChildNodes.Add(f);
                }

                directoryNode = directoryNode.ChildNodes.Count > 0 ? directoryNode : null;
            }

            return directoryNode;
        }

        private string RelativePath(string fullName)
        {
            string relativePath = "/" + fullName.Replace(Server.MapPath("~/"), String.Empty).Replace(@"\", "/");
            return relativePath;
        }

        private string RedirectToView(string fullName)
        {
            string filePath = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(fullName ?? ""));
            string viewPath = "/view.aspx/?id=" + filePath;
            return viewPath;
        }

        private bool CanViewFiles(DirectoryInfo dir)
        {
            var config = WebConfigurationManager.OpenWebConfiguration(RelativePath(dir.FullName));
            if (config != null && config.HasFile)
            {
                var section = config.GetSection("system.web/authorization") as AuthorizationSection;
                if (section != null)
                {
                    foreach (AuthorizationRule rule in section.Rules)
                    {
                        if (rule.Action == AuthorizationRuleAction.Allow)
                        {
                            foreach (string role in rule.Roles)
                            {
                                if (HttpContext.Current.User.IsInRole(role))
                                {
                                    return true;
                                }
                            }

                            foreach (string user in rule.Users)
                            {
                                if (HttpContext.Current.User.Identity.Name == user)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                }
            }
            return true;
        }

        private string FileLength(long fileSize)
        {
            string startSpan = "<span style=\"font-size:.8em;margin-left: 10px;\">";
            string endSpan = "</span>";
            return fileSize <= 999999 ? String.Format(" {0}{1}{2}{3}", startSpan, (Math.Round((fileSize / 1024f), 2)).ToString(), " KB", endSpan) :
                String.Format(" {0}{1}{2}{3}", startSpan, (Math.Round((fileSize / 1024000f), 2)).ToString(), " MB", endSpan);
        }

        public string AltRelFilePath
        {
            get { return _altRelFilePath; }
            set { _altRelFilePath = value; }
        }

        protected void _tree_SelectedNodeChanged(object sender, EventArgs e)
        {
            string filename = Server.MapPath(_tree.SelectedValue);

            System.IO.FileInfo file = new System.IO.FileInfo(filename);
        }
    }
}