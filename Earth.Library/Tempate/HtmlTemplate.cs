using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Earth.Library.Template
{

    public delegate bool TagProcessDelegate(DataRow dr, string columnName, ref string result);
    public delegate bool EntityTagProcessDelegate(object entity, string columnName, ref string result);

    public class HtmlTemplate
    {
        #region
        private static Dictionary<string, TemplateFile> tempateFiles = new Dictionary<string, TemplateFile>();

        private StringBuilder sb;

        private Block currentBlock;

        private Stack<Block> stack = new Stack<Block>();
        #endregion

        #region
        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty(currentBlock.Html))
                {
                    Flush();
                }
                return currentBlock.Html;
            }
        }
        #endregion

        #region
        public HtmlTemplate(string filename)
        {
            filename = filename.ToLower();

            TemplateFile tempateFile;
            if (tempateFiles.ContainsKey(filename))
            {
                tempateFile = tempateFiles[filename];
            }
            else
            {
                tempateFile = new TemplateFile(filename);
            }

            sb = new StringBuilder(tempateFile.Text);
            currentBlock = new Block(sb);
        }
        #endregion

        #region
        public void OpenBlock(string blockName)
        {
            stack.Push(currentBlock);

            currentBlock = new Block(blockName, sb);
            sb = currentBlock.StringBuilder;
        }

        public void CloseBlock()
        {
            Block block = stack.Pop();
            block.StringBuilder.Replace(currentBlock.Text, currentBlock.Html);
            currentBlock = block;
            sb = currentBlock.StringBuilder;
            //currentBlock.iii();
        }

        public void Flush()
        {
            currentBlock.Append(sb.ToString());
            sb = new StringBuilder(currentBlock.InnerText);
            currentBlock.StringBuilder = sb;

        }

        public void Append(string text)
        {
            currentBlock.Append(text);
        }

        #endregion

        #region
        public void ReplaceVar(string tagName, object value)
        {
            string v = "";
            if (value != null)
            {
                v = value.ToString();
            }
            sb.Replace("%%" + tagName + "%%", v);
            sb.Replace("%{" + tagName + "}", v);
            sb.Replace("$" + tagName, v);
        }

        public void ReplaceVar(DataRow dr)
        {
            ReplaceVar(dr, null);
        }

        public void ReplaceVar(DataRow dr, string blockName)
        {
            ReplaceVar(dr, blockName, null);
        }

        public void ReplaceVar(DataRow dr, string blockName, TagProcessDelegate tagProcess)
        {
            foreach (DataColumn col in dr.Table.Columns)
            {
                string tagName = col.ColumnName;
                string newValue = "";
                if (tagProcess != null && tagProcess(dr, tagName, ref newValue))
                {
                }
                else
                {
                    newValue = dr[tagName].ToString();
                }
                ReplaceVar(tagName, newValue);
            }
        }

        public void ReplaceVar(DataTable dt, string blockName)
        {
            ReplaceVar(dt, blockName, null);
        }

        public void ReplaceVar(DataTable dt, string blockName, TagProcessDelegate tagProcess)
        {
            OpenBlock(blockName);

            foreach (DataRow dr in dt.Rows)
            {
                ReplaceVar(dr, "", tagProcess);
                Flush();
            }

            CloseBlock();
        }

        public void ReplaceVar(DataTable dt, string blockName, int columnCount)
        {
            ReplaceVar(dt, blockName, columnCount, null, null, null);
        }

        public void ReplaceVar(DataTable dt, string blockName, int columnCount, TagProcessDelegate tagProcess)
        {
            ReplaceVar(dt, blockName, columnCount, tagProcess, null, null);
        }

        public void ReplaceVar(DataTable dt, string blockName, int columnCount, TagProcessDelegate tagProcess, string rowBeginTag, string emptyCellTag)
        {
            if (string.IsNullOrEmpty(rowBeginTag))
                rowBeginTag = "<tr>";

            if (string.IsNullOrEmpty(emptyCellTag))
                emptyCellTag = "<td>&nbsp;</td>";

            int n = 0;
            OpenBlock("ListByBoardNews");
            for (int h = 0; h < 999; h++)
            {
                Append(rowBeginTag);

                for (int L = 0; L < columnCount; L++)
                {
                    if (n < dt.Rows.Count)
                    {
                        ReplaceVar(dt.Rows[n]);
                        Flush();
                        n++;
                    }
                    else
                    {
                        Append(emptyCellTag);
                    }
                }

                Append("</tr>");
                if (n == dt.Rows.Count)
                    break;
            }
            CloseBlock();
        }



        #endregion

        public override string ToString()
        {
            return this.Text;
        }
    }
}