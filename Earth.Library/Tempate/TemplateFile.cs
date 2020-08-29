using System.IO;

namespace Earth.Library.Template
{
    class TemplateFile
    {
        #region
        internal string Filename
        {
            get;
            set;
        }

        private string _text;

        public string Text
        {
            get
            {
                if (_text == null)
                {
                    string fullpath;
                    if (Filename.Contains("\\"))
                    {
                        fullpath = this.Filename;
                    }
                    else
                    {
                        fullpath = System.Web.HttpContext.Current.Server.MapPath(Filename);
                    }
                    string pPath = System.IO.Path.GetDirectoryName(fullpath);
                    string filename = System.IO.Path.GetFileName(fullpath);
                    _text = System.IO.File.ReadAllText(fullpath);
                    FileSystemWatcher fsw = new FileSystemWatcher(pPath, filename);
                    fsw.Changed += new FileSystemEventHandler(fsw_Changed);
                    fsw.EnableRaisingEvents = true;
                }
                return _text;
            }
        }
        #endregion

        #region
        public TemplateFile(string filename)
        {
            Filename = filename;
        }
        #endregion

        #region
        private void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            _text = null;
        }
        #endregion
    }
}