﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ExcelAddIn.ExcelServices;
using ExcelAddIn.Commons;
using ExcelAddIn.Windows;

namespace ExcelAddIn.Panes
{
    /// <summary>
    /// ProcessPaneWPF.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProcessPaneWPF : UserControl
    {
        public ProcessPaneWPF()
        {
            InitializeComponent();
            ChangeProcess(0);
            ChangeCurrentDesignType();
        }



        // Main Index
        private void Ellipse_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            Ellipse currentItem = sender as Ellipse;
            int currentIndex = Grid.GetColumn(currentItem.Parent as Grid);

            ChangeProcess(currentIndex);
            ChangeSheet(currentIndex);
        }

        // Sub Index
        private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border currentItem = sender as Border;
            int currentSubIndex= Grid.GetColumn(currentItem);
            int currentIndex = Grid.GetColumn(currentItem.Parent as Grid);

            ChangeSheet(currentIndex, currentSubIndex);
        }

        public void ChangeProcess(int currentIndex)
        {
            SolidColorBrush activeColor = new SolidColorBrush(Color.FromRgb(110, 128, 147));
            SolidColorBrush deactiveColor = new SolidColorBrush(Color.FromRgb(202, 209, 216));
            SolidColorBrush currentColor = new SolidColorBrush(Color.FromRgb(26, 202, 142));

            // Item
            foreach (Grid eachItem in gridProcessItem.Children.OfType<Grid>())
            {
                int colValue = Grid.GetColumn(eachItem);
                if (currentIndex == colValue)
                {
                    foreach (Ellipse eachEllipse in eachItem.Children.OfType<Ellipse>())
                        if (eachEllipse.Width == 10)
                            eachEllipse.Visibility = Visibility.Visible;
                        else
                            eachEllipse.Fill = currentColor;

                    foreach (Polygon eachPolygon in eachItem.Children.OfType<Polygon>())
                        eachPolygon.Visibility = Visibility.Hidden;
                }
                else if (currentIndex > colValue)
                {
                    foreach (Ellipse eachEllipse in eachItem.Children.OfType<Ellipse>())
                        if (eachEllipse.Width == 10)
                            eachEllipse.Visibility = Visibility.Hidden;
                        else
                            eachEllipse.Fill = activeColor;

                    foreach (Polygon eachPolygon in eachItem.Children.OfType<Polygon>())
                        eachPolygon.Visibility = Visibility.Visible;
                }
                else if (currentIndex < colValue)
                {
                    foreach (Ellipse eachEllipse in eachItem.Children.OfType<Ellipse>())
                        if (eachEllipse.Width == 10)
                            eachEllipse.Visibility = Visibility.Visible;
                        else
                            eachEllipse.Fill = deactiveColor;

                    foreach (Polygon eachPolygon in eachItem.Children.OfType<Polygon>())
                        eachPolygon.Visibility = Visibility.Hidden;
                }
            }

            // Line
            foreach (Rectangle eachItem in gridProcessLine.Children.OfType<Rectangle>())
            {
                int colValue = Grid.GetColumn(eachItem);
                if (currentIndex >= colValue)
                {
                    eachItem.Fill = activeColor;
                }
                else if (currentIndex < colValue)
                {
                    eachItem.Fill = deactiveColor;
                }
            }

            // Line pad
            foreach (Polygon eachItem in gridProcessLine.Children.OfType<Polygon>())
            {
                int colValue = Grid.GetColumn(eachItem);
                if (currentIndex >= colValue)
                {
                    eachItem.Fill = activeColor;
                }
                else if (currentIndex < colValue)
                {
                    eachItem.Fill = deactiveColor;
                }
            }

            // Text
            foreach (TextBlock eachItem in gridProcessText.Children.OfType<TextBlock>())
            {
                int colValue = Grid.GetColumn(eachItem);
                if (currentIndex >= colValue)
                {
                    eachItem.Foreground = activeColor;
                }
                else if (currentIndex < colValue)
                {
                    eachItem.Foreground = deactiveColor;
                }
            }


            // Item : Sub
            foreach (Grid eachItem in gridProcessItemSub.Children.OfType<Grid>())
            {
                int colValue = Grid.GetColumn(eachItem);
                if (currentIndex == colValue)
                {
                    eachItem.Visibility = Visibility.Visible;
                }
                else
                {
                    eachItem.Visibility = Visibility.Collapsed;
                }

            }
        }

        public void ChangeSheet(int currentIndex,int currentSubIndex=999)
        {
            switch (currentIndex)
            {
                case 0:
                    if (currentSubIndex == 999)
                    {
                        ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_GENERAL);
                    }
                    else
                    {
                        switch (currentSubIndex)
                        {
                            case 0:
                                ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_GENERAL);
                                break;
                            case 1:
                                ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_WELDING);
                                break;
                        }
                    }

                    break;
                case 1:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_ROOF);
                    break;
                case 2:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_SHELL);
                    break;
                case 3:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_BOTTOM);
                    break;
                case 4:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_STRUCTURE);
                    break;
                case 5:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_NOZZLE);
                    break;
                case 6:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_ACCESS);
                    break;
                case 7:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_APPURTENANCES);
                    break;

                default:
                    break;
            }
            
        }


        private void sese_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(Globals.Ribbons.Count().ToString());
        }

        private void btnDesignType_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TypePlanWindow newType = new TypePlanWindow();
            var ss = new IntPtr(CommonAddin.GetAddinApplicationHWND());

            newType.ShowDialog();
            ROOF_TYPE selButton = newType.selectButton;
            if (selButton != ROOF_TYPE.NotSet)
            {
                // Desing Type
                ChangeDesignType(selButton);

                // Excel Value
                ExcelService.ChangeRoofType(selButton);

                // Process Panel Value
                if (Globals.ThisAddIn.customProcessPane != null)
                {
                    int selIndex = 0;
                    Globals.ThisAddIn.customProcessPane.elementHost1WPF.ChangeProcess(selIndex);
                    Globals.ThisAddIn.customProcessPane.elementHost1WPF.ChangeSheet(selIndex);
                }
                else
                {
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_GENERAL);
                    ExcelService.SetInformationWindowArea(false, false);
                }


            }
        }



        #region Current Status
        private void ChangeDesignType(ROOF_TYPE selType)
        {
            switch (selType)
            {
                case ROOF_TYPE.CRT:
                    textDesignType01.Text = "CRT";
                    textDesignType02.Visibility = Visibility.Collapsed;
                    currentImage.Source = new BitmapImage(new Uri("/ExcelAddin;component/Resources/icon_CRT.png", UriKind.Relative));
                    break;
                case ROOF_TYPE.DRT:
                    textDesignType01.Text = "DRT";
                    textDesignType02.Visibility = Visibility.Collapsed;
                    currentImage.Source = new BitmapImage(new Uri("/ExcelAddin;component/Resources/icon_DRT.png", UriKind.Relative));
                    break;
                case ROOF_TYPE.IFRT:
                    textDesignType01.Text = "IFRT";
                    textDesignType02.Visibility = Visibility.Collapsed;
                    currentImage.Source = new BitmapImage(new Uri("/ExcelAddin;component/Resources/icon_IFRT_single_deck.png", UriKind.Relative));
                    break;
                case ROOF_TYPE.EFRTSingle:
                    textDesignType01.Text = "EFRT";
                    textDesignType02.Text = "Single Deck";
                    textDesignType02.Visibility = Visibility.Visible;
                    currentImage.Source = new BitmapImage(new Uri("/ExcelAddin;component/Resources/icon_EFRT_single_deck.png", UriKind.Relative));
                    break;
                case ROOF_TYPE.EFRTDouble:
                    textDesignType01.Text = "EFRT";
                    textDesignType02.Text = "Double Deck";
                    textDesignType02.Visibility = Visibility.Visible;
                    currentImage.Source = new BitmapImage(new Uri("/ExcelAddin;component/Resources/icon_EFRT_double_deck.png", UriKind.Relative));
                    break;
                default:
                    textDesignType01.Text = "TANK";
                    textDesignType02.Text = "OPEN";
                    break;
            }
        }

        private void ChangeCurrentDesignType()
        {
            ROOF_TYPE currentType= ExcelService.GetSheetRoofType();
            ChangeDesignType(currentType);
        }

        #endregion
    }
}
