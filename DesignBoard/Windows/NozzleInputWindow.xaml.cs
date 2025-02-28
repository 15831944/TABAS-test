﻿using DesignBoard.Commons;
using DesignBoard.Models;
using DesignBoard.Services;
using DesignBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesignBoard.Windows
{
    /// <summary>
    /// NozzleInputWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NozzleInputWindow : Window
    {

        #region One Click
        private long _oneClickFirsttime = (long)0;

        public bool One_Click()
        {
            bool flag;
            long ticks = DateTime.Now.Ticks;
            if (ticks - this._oneClickFirsttime >= (long)4000000)
            {
                this._oneClickFirsttime = ticks;
                flag = true;
            }
            else
            {
                this._oneClickFirsttime = ticks;
                flag = false;
            }
            return flag;
        }
        #endregion

        public NozzleInputWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }


        private void btnClose_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void btnApply_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Nozzle Data Applied.", "Information");
            Close();
        }

        private void btnReCheck_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
            {
                NozzleInputWindowViewModel selView = this.DataContext as NozzleInputWindowViewModel;
                selView.ReCheck();
            }


            
        }

        public void SetData(ObservableCollection<NozzleModel> NozzleList)
        {
            NozzleInputWindowViewModel selView = this.DataContext as NozzleInputWindowViewModel;
            selView.NozzleList = NozzleList;
        }



        private void btnNozzle_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
            {
                FileService newFileService = new FileService();
                FileModel selFile = newFileService.GetFile(OpenFile_Type.NozzleData);
                NozzleInputWindowViewModel selView = this.DataContext as NozzleInputWindowViewModel;
                selView.CreateData();
            }

        }
    }
}
