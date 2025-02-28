﻿using devDept.Eyeshot;
using devDept.Geometry;
using devDept.Graphics;
using DrawSample.DrawService;
using System;
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

using MColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;
using devDept.Eyeshot.Entities;
using DrawSample.Commons;

namespace DrawSample
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        bool inspectVertex=true;

        bool markValue = true;

        double shiftFactor = 10;

        public Entity currentEntity = null;
        private DrawSettingService drawSetting;


        public List<Brep> flangeList = null;
        public List<Brep> nozzleList = null;

        public MainWindow()
        {
            InitializeComponent();

            flangeList = new List<Brep>();
            nozzleList = new List<Brep>();


            this.testModel.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");
            this.testModel.ActionMode = devDept.Eyeshot.actionType.SelectByPick;

            this.testModel.Renderer = rendererType.OpenGL;

            this.testModel.ActiveViewport.Background.TopColor = new SolidColorBrush(MColor.FromRgb(59, 68, 83));
            
            //this.testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Wireframe;
            this.testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Rendered;
            //this.testModel.Wireframe.SilhouettesDrawingMode = silhouettesDrawingType.Always;

            //testModel.AssemblySelectionMode = devDept.Eyeshot.Environment.assemblySelectionType.Branch;

            drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);

        }

        private void btnDraw_Click(object sender, RoutedEventArgs e)
        {
            drawSetting.CreateModelSpaceSample(testModel);
        }

        private void testModel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Checks that we are not using left mouse button for ZPR
            if (testModel.ActionMode == actionType.SelectByPick && e.ChangedButton != System.Windows.Input.MouseButton.Middle)
            {

                Point3D closest;

                if (inspectVertex)
                {
                    
                    //if (testModel.FindClosestVertex(RenderContextUtility.ConvertPoint(testModel.GetMousePosition(e)), 50, out closest) != -1)

                        //testModel.Labels.Add(new devDept.Eyeshot.Labels.LeaderAndText(closest, closest.ToString(), new System.Drawing.Font("Tahoma", 8.25f),Color.Yellow, new Vector2D(0, 50)));
                    foreach(Entity eachEntity in testModel.Entities)
                    {
                        if (eachEntity.Selected)
                        {
                            currentEntity = eachEntity;
                        }
                    }

                }

                testModel.Invalidate();

            }
        }

        private void btnDetection_Click(object sender, RoutedEventArgs e)
        {
            double totalValue = 0;
            double outlineValue = 0;
            double shapeValue = 0;
            drawSetting.ExecuteDetection2(out totalValue, out outlineValue, out shapeValue,testModel);
            drawSetting.CreateMarkValue(markValue,testModel);



            tbIntersect.Text = totalValue.ToString();
            tbIntersectPoint.Text = (totalValue * 2).ToString();
            tbOutline.Text = outlineValue.ToString();
            tbShape.Text = shapeValue.ToString();

        }

        private void btnMark_Click(object sender, RoutedEventArgs e)
        {
            markValue = !markValue;
            drawSetting.CreateMarkValue(markValue, testModel);
        }


        #region Button : Move
        private void shiftLeft_Click(object sender, RoutedEventArgs e)
        {
            MoveShape(MOVE_TYPE.LEFT);
        }

        private void shiftRight_Click(object sender, RoutedEventArgs e)
        {
            MoveShape(MOVE_TYPE.RIGHT);
        }

        private void shiftBottom_Click(object sender, RoutedEventArgs e)
        {
            MoveShape(MOVE_TYPE.BOTTOM);
        }

        private void shiftTop_Click(object sender, RoutedEventArgs e)
        {
            MoveShape(MOVE_TYPE.TOP);
        }


        private void MoveShape(MOVE_TYPE selType)
        {
            double moveFactor = Convert.ToDouble(tbMoveFactor.Text);

            

            if (currentEntity != null)
            {
                if (currentEntity is Circle)
                {
                    foreach (Entity eachEntity in testModel.Entities)
                    {
                        if (eachEntity is Circle)
                            if (eachEntity.Selected)
                            {
                                Circle eachCircle = ((Circle)eachEntity);
                                switch (selType)
                                {
                                    case MOVE_TYPE.TOP:
                                        eachCircle.Center.Y = eachCircle.Center.Y + moveFactor;
                                        break;
                                    case MOVE_TYPE.BOTTOM:
                                        eachCircle.Center.Y = eachCircle.Center.Y - moveFactor;
                                        break;
                                    case MOVE_TYPE.LEFT:
                                        eachCircle.Center.X = eachCircle.Center.X - moveFactor;
                                        break;
                                    case MOVE_TYPE.RIGHT:
                                        eachCircle.Center.X = eachCircle.Center.X + moveFactor;
                                        break;
                                }
                                eachCircle.Radius = eachCircle.Radius;
                                testModel.Entities.Regen();

                                testModel.Refresh();
                                testModel.Invalidate();
                                break;

                            }
                    }

                }

                if(currentEntity is Brep)
                {
                    double moveAngle = 2;
                    double moveVertical = 50;
                    double shellRadius = 2700;
                    Vector3D trValue = new Vector3D();
                    Brep eachBrep = ((Brep)currentEntity);

                    Brep eachBrepNozzle = null;
                    for(int i=0;i<flangeList.Count;i++) 
                    {
                        Brep eachFlange = flangeList[i];
                        if (eachBrep.Equals(eachFlange))
                        {
                            eachBrepNozzle = nozzleList[i];
                            break;
                        }
                    }

                    switch (selType)
                    {
                        case MOVE_TYPE.TOP:

                            trValue = new Vector3D(0, 0, moveVertical);
                            eachBrep.Translate(trValue);
                            if(eachBrepNozzle !=null)
                                eachBrepNozzle.Translate(trValue);
                            break;
                        case MOVE_TYPE.BOTTOM:
                            trValue = new Vector3D(0, 0, -moveVertical);
                            eachBrep.Translate(trValue);
                            if (eachBrepNozzle != null)
                                eachBrepNozzle.Translate(trValue);
                            break;
                        case MOVE_TYPE.LEFT:
                            eachBrep.Rotate(Utility.DegToRad(-moveAngle), Vector3D.AxisZ, new Point3D(0, 0, 0));
                            if (eachBrepNozzle != null)
                                eachBrepNozzle.Rotate(Utility.DegToRad(-moveAngle), Vector3D.AxisZ, new Point3D(0, 0, 0));
                            break;
                        case MOVE_TYPE.RIGHT:
                            eachBrep.Rotate(Utility.DegToRad(moveAngle), Vector3D.AxisZ, new Point3D(0, 0, 0));
                            if (eachBrepNozzle != null)
                                eachBrepNozzle.Rotate(Utility.DegToRad(moveAngle), Vector3D.AxisZ, new Point3D(0, 0, 0));
                            break;
                    }
                    testModel.Entities.Regen();

                    testModel.Refresh();
                    testModel.Invalidate();
                }
            }
        }
        #endregion


        private void btnPlate_Click(object sender, RoutedEventArgs e)
        {

            drawSetting.CreateArrangePlate(testModel, tbDiameter.Text);
        }

        private void btnRegen_Click(object sender, RoutedEventArgs e)
        {
            testModel.Entities.RegenAllCurved(0.05);

            testModel.Refresh();

        }

        private void btn3D_Click(object sender, RoutedEventArgs e)
        {
            drawSetting.CreateThreeD(testModel, out nozzleList,out flangeList);
            
        }

        private void btn3DTest_Click(object sender, RoutedEventArgs e)
        {
            drawSetting.CreateThreeD2(testModel);
        }
    }
}
