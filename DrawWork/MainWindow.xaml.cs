﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DrawWork.ViewModels;
using DrawWork.DrawBuilders;
using DrawWork.DrawServices;
using DrawWork.CommandModels;
using DrawWork.FileServices;
using DrawWork.Windows;
using Microsoft.Win32;
using devDept.Eyeshot.Translators;
using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using DrawWork.ImportServices;
using DrawWork.DrawStyleServices;
using ExcelDataLib.ExcelServices;
using System.Collections.ObjectModel;
using ExcelDataLib.ExcelModels;
using System.Diagnostics;
using DrawWork.DesignServices;
using AssemblyLib.AssemblyModels;
using DrawLogicLib.DrawLogicFileServices;
using DrawLogicLib.Models;
using DrawWork.CommandServices;
using DrawWork.AssemblyServices;
using DrawWork.Commons;
using DrawWork.DrawSacleServices;
using DrawWork.DWGFileServices;
using DrawSettingLib.SettingServices;
using StrengthCalLib.CalServices;
using EPDataLib.ExcelServices;
using DrawWork.DrawDetailServices;

namespace DrawWork
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DrawSettingService drawSetting;

        private Drawings tempPaper;

        private BlockKeyedCollection tempImportBlocks;
        public MainWindow()
        {
            InitializeComponent();

            this.testModel.Unlock("UF21-7LEVP-EN12E-8L83-F1NY");
            this.testModel.ActionMode = devDept.Eyeshot.actionType.SelectByPick;

            //this.testModel.Renderer = rendererType.OpenGL;

            BackgroundSettings cc = new BackgroundSettings();
            this.testModel.ActiveViewport.Background.TopColor= new SolidColorBrush(Color.FromRgb(59, 68, 83));
            //this.testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Rendered;
            this.testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Wireframe;
            //this.testModel.Wireframe.SilhouettesDrawingMode = silhouettesDrawingType.Always;
            //this.testModel.ActiveViewport.SmallSize
            //this.testModel.ActiveViewport.SmallSizeRatio = 0.001;

            drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);

            //logicFile.Text = @"C:\Users\tree\Desktop\CAD\tabas\Sample_DrawLogic.txt";
            logicFile.Text = Properties.Settings.Default.logicPath;
            ExcelFile.Text = Properties.Settings.Default.excelPath;
            dwgFile.Text = @"C:\Users\tree\Desktop\CAD\tabas\Block_Sample.dwg";


            // Custom Setting
            inputScale.Text = "90";
            cbRoofType.Text = "ROOF";
            tbPWidth.Text = "2048";
            tbPLength.Text = "9144";
            tbOD.Text = "32000";

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            drawSetting.CreateModelSpaceSample(testModel);
            //MessageBox.Show("완료");
        }



        // Create
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;
            selView.CreateCommandService(testModel);

            string logicFilePath = logicFile.Text;

            if (logicFilePath != "")
            {
                selView.commandData.commandList.Clear();
                TextFileService newFileService = new TextFileService();
                string[] newComData = newFileService.GetTextFileArray(logicFile.Text);
                //selView.commandData.commandList = new List<CommandLineModel>();
                foreach (string eachText in newComData)
                    selView.commandData.commandList.Add(new CommandLineModel(eachText));
            }


            double selScale = Convert.ToDouble(inputScale.Text);
            LogicBuilder testBuilder = selView.GetLogicBuilder(selScale);

            // remove old block
            testModel.Entities.Clear();
            testModel.StartWork(testBuilder);
            MessageBox.Show("완료");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;

            AssemblyWindow cc = new AssemblyWindow();
            cc.SetAssembly(selView.TankData);
            cc.Show();
        }

        private void btnCreateDwg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //NewSave();
            OldSave();
        }
        private void OldSave()
        {
            var exportFileDialog = new SaveFileDialog();
            exportFileDialog.Filter = "CAD drawings(*.dwg)| *.dwg|" + "Drawing Exchange Format (*.dxf)|*.dxf";
            exportFileDialog.AddExtension = true;
            exportFileDialog.Title = "Export";
            exportFileDialog.CheckPathExists = true;
            var result = exportFileDialog.ShowDialog();
            if (result == true)
            {
                WriteAutodeskParams wap = new WriteAutodeskParams(testModel, tempPaper, false, false);
                WriteFileAsync wa = new WriteAutodesk(wap, exportFileDialog.FileName);

                testModel.StartWork(wa);


            }
        }
        private void NewSave()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CAD drawings (*.dwg)|*.dwg";
            saveFileDialog.AddExtension = true;
            saveFileDialog.CheckPathExists = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                WriteFileAsync wfa = null;
                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                    case 2:
#if SETUP
                        wfa = _helper.GetWriteAutodesk(testModel, null, false, saveFileDialog.FileName);
#else
                        wfa = new WriteAutodesk(new WriteAutodeskParams(testModel), saveFileDialog.FileName);
#endif
                        break;
                    case 3:
#if SETUP
                        //wfa = _helper.GetWritePDF(model1, saveFileDialog.FileName);
#else
                        //var writePdfParams = new WritePdfParams(testModel, new Size(842, 595), new Rect(20, 40, 802, 495)) { ViewBorderWidth = 0, TransparentBackground = true };
                        //wfa = new MyWritePDF(writePdfParams, saveFileDialog.FileName);
#endif
                        break;
                }

                testModel.StartWork(wfa);
            }
        }

        private void btnLoadDWG_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            var importFileDialog = new OpenFileDialog();
            importFileDialog.Filter= "All compatible file types (*.*)|*.step;*.stp;*.iges;*.igs;*.stl;*.obj;*.dwg;*.dxf|Standard for the Exchange of Product Data (*.stp; *.step)|*.stp; *.step|Initial Graphics Exchange Specification (*.igs; *.iges)|*.igs; *.iges|WaveFront OBJ (*.obj)|*.obj|Stereolithography (*.stl)|*.stl|CAD drawings (*.dwg)|*.dwg|Drawing Exchange Format (*.dxf)|*.dxf";
            importFileDialog.AddExtension = true;
            importFileDialog.Title = "Import";
            importFileDialog.CheckFileExists = true;
            importFileDialog.CheckPathExists = true;
            var result = importFileDialog.ShowDialog();
            if (result == true)
            {
                //testModel.Clear();

                BackgroundSettings temp = new BackgroundSettings();
                temp.TopColor = new SolidColorBrush(Color.FromRgb(225,225,225));
                testModel.Viewports[0].Background = temp;

                //ReadFileAsync rfa = GetReader(importFileDialog.FileName);
                //if (rfa != null)
                //    testModel.StartWork(rfa);

                ReadAutodesk ra = new ReadAutodesk(importFileDialog.FileName);
                if (ra != null) 
                {
                    testModel.StartWork(ra);
                    testModel.Refresh();
                }

            }

            this.testModel.ActiveViewport.Background.TopColor = new SolidColorBrush(Color.FromRgb(59, 68, 83));


        }




        private ReadFileAsync GetReader(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName);

            if (ext != null)
            {
                ext = ext.TrimStart('.').ToLower();

                switch (ext)
                {
                    case "dwg":
                    case "dxf":

                        ReadAutodesk ra = new ReadAutodesk(fileName);
                        ra.SkipLayouts = false;
                        return ra;

                    case "stl":
                        return new ReadSTL(fileName);
                    case "obj":
                        return new ReadOBJ(fileName);

                }
            }

            return null;
        }

        private void testModel_WorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            if (e.WorkUnit is ReadFileAsync)
            {
                ReadFileAsync rfa = (ReadFileAsync)e.WorkUnit;

                //ReadFile rf = e.WorkUnit as ReadFile;

                //ReadFile rf = e.WorkUnit as ReadFile;
                //if (rf != null)
                //if(rfa.Entities !=null)

                // ReadFile rf = new ReadFile(rfa.Stream);

                if (e.WorkUnit is ReadFileAsyncWithBlocks)
                {
                    ReadFileAsyncWithBlocks readFileWithBlocks = (ReadFileAsyncWithBlocks)e.WorkUnit;

                    ImportBlockService importBlockS = new ImportBlockService();
                    importBlockS.CreateBlock(readFileWithBlocks, testModel);

                    //devDept.Eyeshot.Block ddd= readFileWithBlocks.Blocks["block_Sample"]);
                    //testModel.Blocks.Add(readFileWithBlocks.Blocks["ssblock"]);

                    //var dd= testModel.Blocks["ssblock"];

                    //foreach(Entity eachEntity in dd.Entities)
                    //{
                    //eachEntity is LinearPath
                    //eachEntity.LayerName = "DashDot1";
                    //eachEntity.LineTypeName = "";
                    //eachEntity.LineTypeMethod = colorMethodType.byLayer;
                    //eachEntity.LayerName = "DashDot";
                    //eachEntity.LineTypeName = "DashDot";
                    //}
                    MessageBox.Show("Block Import 완료");
                }



                //var br3 = new BlockReference(10, 100, 10, "ssblock",testModel.RootBlock.Units,testModel.Blocks,0);

                // 블럭 삽입 방법
                //if (false)
                //{
                //    var br3 = new BlockReference(-1000, 5000, 0, "LADDER-1", 0);
                //    br3.Scale(1);
                //    testModel.Entities.Add(br3, "LayerDimension");
                //}

                //testModel.Entities.Add(br3, "DashDot");

                //                rfa.AddToScene(testModel);
            }
        }



        // Environment
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            EnvironmentWindow newWin = new EnvironmentWindow();
            EnvironmentWindowViewModel newWinView = newWin.DataContext as EnvironmentWindowViewModel;
            newWinView.SetModelEnvironment(testModel);
            newWinView.CreateEnvironment();
            newWin.Show();

        }


        // Preview
        private void btnPreview_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PreviewWindow cc = new PreviewWindow();
            cc.Owner = this;
            PreviewWindowViewModel previewView = cc.DataContext as PreviewWindowViewModel;
            previewView.previewService.SetModelObject(this.testModel);
            previewView.previewService.SetDrawingsObject(cc.testDraw);

            previewView.viewPortSet.Scale = inputScale.Text;

            tempPaper = cc.testDraw;
            cc.Show();
        }

        private void btnCreateExcel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;
            ObservableCollection<ExcelWorkSheetModel> newSheetList = new ObservableCollection<ExcelWorkSheetModel>();

            Stopwatch stopWatch = new Stopwatch(); 
            stopWatch.Start();

            ExcelApplicationService eDataService = new ExcelApplicationService(-1);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            MessageBox.Show(elapsedTime);

            stopWatch.Start();
            newSheetList = eDataService.GetSheetListAll("TankDesign_0428.xlsm");
            stopWatch.Stop();
            TimeSpan ts1 = stopWatch.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts1.Hours, ts1.Minutes, ts1.Seconds, ts1.Milliseconds / 10);
            MessageBox.Show(elapsedTime1);

            stopWatch.Start();
            eDataService.GetSheetData(selView.TankData, newSheetList);

            stopWatch.Stop();
            TimeSpan ts2 = stopWatch.Elapsed;
            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts2.Hours, ts2.Minutes, ts2.Seconds, ts2.Milliseconds / 10);
            MessageBox.Show(elapsedTime2);


            DesignService designS = new DesignService();
            designS.CreateDesignCRTModel(selView.TankData);

        }

        private void btnTitleBlock_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TitleBlockWindow cc = new TitleBlockWindow();
            cc.Owner = this;
            cc.ShowDialog();
        }

        // Test New
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Sample RoofBottom Arrange
            SingletonData.RoofBottomArrange.Clear();
            SingletonData.RoofBottomArrange.Add(cbRoofType.Text);
            SingletonData.RoofBottomArrange.Add(tbOD.Text);
            SingletonData.RoofBottomArrange.Add(tbPWidth.Text);
            SingletonData.RoofBottomArrange.Add(tbPLength.Text);



            DWGFileService dwgService = new DWGFileService();
            string dwgFilePath= dwgService.FileFilecopy();

            // Singleton Data : Reset
            SingletonData.Clear();

            // Assembly
            AssemblyDataService assemblyService = new AssemblyDataService();
            //AssemblyModel newTankData = assemblyService.CreateMappingData(ExcelFile.Text);

            // New Excel Read
            AssemblyModel newTankData = assemblyService.CreateMappingDataNew(ExcelFile.Text);


            // Logic
            DrawLogicDBService newLogic = new DrawLogicDBService();
            DrawLogicModel newLogicData = newLogic.GetLogicCommand(DrawLogicLib.Commons.LogicFile_Type.GA);

            // Ligic File
            TextFileService newFileService = new TextFileService();
            string[] newComData = newFileService.GetTextFileArray(logicFile.Text);

            // Virtual Design
            DesignService designS = new DesignService();
            designS.CreateDesignCRTModel(newTankData);

            // 시트 값을 가져와야 함 : Assembly.Create Mapping Data

            // 버추얼 디자인의 True False

            // Logic을 수동으로 연결

            // 형상 그리는 것은 완료
            List<string> newAll = new List<string>();
            newAll.AddRange(newLogicData.UsingList);
            newAll.AddRange(newLogicData.ReferencePointList);
            foreach (DrawCommandModel eachCommand in newLogicData.CommandList)
                newAll.AddRange(eachCommand.Command);

            //newAll.ToArray()

            DrawScaleService scaleService = new DrawScaleService();

            double autoScale = 1;
            if (inputScale.Text == "")
            {
                autoScale = scaleService.GetAIScale(newTankData);
                inputScale.Text = autoScale.ToString();
            }
            else
            {
                autoScale = Convert.ToDouble(inputScale.Text);
            }


            // Scale Setting
            ModelAreaService areaService = new ModelAreaService();
            PaperAreaService paperAreaService = new PaperAreaService();
            DrawDetailRoofBottomService roofBottomService = new DrawDetailRoofBottomService(newTankData, testModel);

            SingletonData.GAArea = areaService.GetModelAreaData();
            double bottomRoofOD = roofBottomService.GetBottomRoofOD();
            string annularStr = newTankData.BottomInput[0].AnnularPlate;
            string topAngelType = newTankData.RoofCompressionRing[0].CompressionRingType;
            string tankType = newTankData.GeneralDesignData[0].RoofType;
            string structureType = newTankData.StructureCRTInput[0].SupportingType;
            double layerCount = newTankData.StructureCRTColumnInput.Count;
            if (SingletonData.TankType == TANK_TYPE.DRT)
            {
                structureType = newTankData.StructureDRTInput[0].SupportingType;
                layerCount = 0;
            }
            double roofOD = roofBottomService.GetRoofOD();

            SingletonData.PaperArea.AreaList = paperAreaService.GetPaperAreaData(tankType, bottomRoofOD, annularStr, topAngelType,
                                                                                structureType, layerCount, roofOD);

            // Virtual Design
            DrawDetailVisibleService detailService = new DrawDetailVisibleService(newTankData, testModel);
            detailService.SetDetailVisibleALL(SingletonData.PaperArea.AreaList);


            tempImportBlocks = testModel.Blocks;

            // Delete
            testModel.Entities.Clear();
            testModel.Purge();
            testModel.Invalidate();


            // Create Setting
            DrawSettingService drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);

            if (testModel.Blocks.Count == 1)
            {
                if (tempImportBlocks != null)
                {
                    testModel.Blocks = tempImportBlocks;
                }
            }

            string testName = "ORIENTATION";
            //string testName = "GA";
            IntergrationService newInterService = new IntergrationService(testName, newTankData, testModel);
            //IntergrationService newInterService = new IntergrationService("GA", newTankData, testModel);

            LogicBuilder outBuilder = null;
            if (newInterService.CreateLogic(Convert.ToDouble(autoScale), newComData,out outBuilder))
            {
                MessageBox.Show("완료");
            }
            else
            {
                //MessageBox.Show("오류");
            }

            testModel.Entities.Regen();
            testModel.SetView(viewType.Top);

            // fits the model in the viewport
            testModel.ZoomFit();
            testModel.Refresh();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.logicPath = logicFile.Text;
            Properties.Settings.Default.excelPath=ExcelFile.Text;
            Properties.Settings.Default.Save();

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            this.testModel.Entities.RegenAllCurved(0.005);
            this.testModel.Refresh();

        }

        private void btnCal_Click(object sender, RoutedEventArgs e)
        {
            
            bool stopValue = false;
            if (stopValue)
            {
                CalImportService calService = new CalImportService();
                string fileName = @"C:\Users\tree\Desktop\CAD\TABAS\20210719 GA보완\calSample.doc";
                //calService.Read(fileName);

                string testName = @"C:\Users\tree\Desktop\CAD\TABAS\20210719 GA보완\TABAS_20210614+1.xlsm";
                //testName = @"C:\Users\tree\Desktop\CAD\TABAS\20210719 GA보완\test.xlsm";
                //testName = @"C:\Users\tree\Desktop\CAD\TABAS\20210719 GA보완\test2.xlsm";
                testName = @"C:\Users\tree\Desktop\CAD\TABAS\20210719 GA보완\newtest.xlsm";
                testName = @"C:\Users\tree\Desktop\CAD\TABAS\20210719 GA보완\TABAS_20210614+11111.xlsm";
                EPService excelService = new EPService();
                if (excelService.SetSaveData(testName))
                {
                    MessageBox.Show("성공");
                }
                else
                {
                    MessageBox.Show("비성공");
                }

            }


            if (true)
            {
                string selFile = @"C:\Users\tree\Desktop\CAD\TABAS\20210719 GA보완\calSampleadj.doc";
                CalImportService calService = new CalImportService();
                calService.ReadDOC(selFile);
            }
        }
    }
}
