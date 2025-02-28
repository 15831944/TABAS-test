﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using ExcelWork.Panes;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace ExcelWork
{
    public partial class ThisWorkbook
    {

        public ProcessPane processActionPane;

        private void ThisWorkbook_Startup(object sender, System.EventArgs e)
        {
            
        }


        private void ThisWorkbook_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisWorkbook_Startup);
            this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
        }

        #endregion


        #region Task Pane
        public void ShowTaskPane()
        {

            this.ActionsPane.Clear();
            processActionPane = new ProcessPane();
            this.ActionsPane.Controls.Add(processActionPane);
            this.ActionsPane.Controls[0].Text = "Design Process";
            this.ActionsPane.Controls[0].Name = "Tank Process";
            this.Application.CommandBars["Tank Process"].Position = Office.MsoBarPosition.msoBarRight;
            this.Application.CommandBars["Tank Process"].accName = "aaa";




        }
        #endregion
    }
}
