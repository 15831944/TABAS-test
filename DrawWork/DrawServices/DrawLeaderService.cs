﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;
using MColor = System.Windows.Media.Color;

using AssemblyLib.AssemblyModels;

using DrawWork.ValueServices;
using DrawWork.DrawModels;
using DrawWork.Commons;
using DrawWork.DrawCustomObjectModels;
using DrawWork.DrawStyleServices;
using DrawWork.CutomModels;
using System.Collections.ObjectModel;


namespace DrawWork.DrawServices
{
    public class DrawLeaderService
    {
        private Model singleModel;

        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService workingPointService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawPublicFunctionService publicFunService;

        private DrawNozzleBlockService nozzleBlock;
        private DrawImportBlockService blockImportService;

        private DrawService drawService;


        private DrawLeaderPublicService leaderDataService;

        public DrawLeaderService(AssemblyModel selAssembly, Object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;

            valueService = new ValueService();
            workingPointService = new DrawWorkingPointService(selAssembly);
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            publicFunService = new DrawPublicFunctionService();

            nozzleBlock = new DrawNozzleBlockService(selAssembly);
            blockImportService = new DrawImportBlockService(selAssembly,(Model)selModel);

            drawService = new DrawService(selAssembly);

            leaderDataService = new DrawLeaderPublicService(selAssembly);
        }


        public DrawEntityModel GetLeaderList(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {



            DrawEntityModel returnEntity = new DrawEntityModel();

            List<Entity> leaderLine = new List<Entity>();
            List<Entity> leaderText = new List<Entity>();
            List<Entity> leaderArrow = new List<Entity>();


            // Roof
            DrawEntityModel leader_Basic = GetLeader_Basic(ref refPoint, ref curPoint, scaleValue);
            returnEntity.AddDrawEntity(leader_Basic);

            DrawEntityModel leader_SingletonData = GetLeader_SignletonData(ref refPoint, ref curPoint, scaleValue);
            returnEntity.AddDrawEntity(leader_SingletonData);



            return returnEntity;
        }

        private DrawEntityModel GetLeader_Basic( ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();


            string newPosition = "";
            string newLeaderType = "";
            string newLength = "";
            string newFontSize = "";
            string newLayerHeight = "7";


            string layerName = layerService.LayerDimension;

            DrawEntityModel returnEntity = new DrawEntityModel();

            double tankHeight= valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight); 
            double tankNominalID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankNominalIDHalf = tankNominalID / 2;
            double bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);

            double scaleLength = 0.061728 * tankNominalID; //32400일때 리더 길이 2000
            newLength = scaleLength.ToString();

            List<Entity> leaderLine = new List<Entity>();
            List<Entity> leaderText = new List<Entity>();
            List<Entity> leaderArrow = new List<Entity>();
            foreach (LeaderListCRTInputModel eachLeader in assemblyData.LeaderListCRTInput)
            {
                string eachPart = eachLeader.Part.ToLower();

                List<string> newText = leaderDataService.GetLeaderLineText(eachLeader);
                List<string> newTextSub = leaderDataService.GetLeaderEmptyLineText(eachLeader);
                if (newText.Count == 0)
                    newText = newTextSub;

                DrawEntityModel eachLeaderList = new DrawEntityModel();
                if (eachPart == "roof")
                {
                    newText = new List<string>();
                    newText.Add("t" + assemblyData.RoofCompressionRing[0].RoofPlateThickness + " ROOF PLATE");

                    double radiusValue = valueService.GetDoubleValue(eachLeader.R)/100 * tankNominalIDHalf;
                    CDPoint currentPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, radiusValue, ref refPoint, ref curPoint);
                    if (eachLeader.LR == "L")
                    {
                        newPosition = "topleft";
                    }
                    else
                    {
                        newPosition = "topright";
                        currentPoint.X += radiusValue * 2;
                    }
                    if(SingletonData.TankType==TANK_TYPE.CRT || SingletonData.TankType == TANK_TYPE.DRT || SingletonData.TankType == TANK_TYPE.IFRT)
                        eachLeaderList = drawService.Draw_Leader(currentPoint, newLength, newPosition, "", "", newText, newTextSub, singleModel, scaleValue, layerName);

                }
                else if (eachPart == "shell")
                {   
                    // 없음
                    //eachLeaderList = drawService.Draw_Leader(currentPoint, newLength, newPosition, "", "", newText, newTextSub, singleModel, scaleValue, layerName);
                }
                else if (eachPart == "bottom")
                {
                    newText = new List<string>();
                    newText.Add("t" + assemblyData.BottomInput[0].BottomPlateThickness + " BOTTOM PLATE");

                    double radiusValue = valueService.GetDoubleValue(eachLeader.R) / 100 * tankNominalIDHalf;
                    CDPoint currentPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomDown, radiusValue, ref refPoint, ref curPoint);

                    newLength = scaleLength.ToString();
                    if (eachLeader.LR == "L")
                    {
                        newPosition = "bottomleft";
                    }
                    else
                    {
                        newPosition = "bottomleft";
                        currentPoint.Y = refPoint.Y - bottomThickness;
                        currentPoint.X += radiusValue * 2;
                    }
                    eachLeaderList = drawService.Draw_Leader(currentPoint, newLength, newPosition, "", "", newText, newTextSub, singleModel, scaleValue, layerName);
                }

                returnEntity.AddDrawEntity(eachLeaderList);
            }


            //returnEntity = drawService.Draw_Leader(newPoint1, newLength, newPosition, newFontSize, newLayerHeight, newText, newTextSub, selModel as Model, scaleValue, layerName);
            


            return returnEntity;
        }

        private DrawEntityModel GetLeader_SignletonData(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();


            string newPosition = "";
            string newLeaderType = "";
            string newLength = "";
            string newFontSize = "";
            string newLayerHeight = "7";


            string layerName = layerService.LayerDimension;

            DrawEntityModel returnEntity = new DrawEntityModel();

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankNominalID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankNominalIDHalf = tankNominalID / 2;
            double bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);

            double scaleFactor = 0.061728;
            double scaleLength = scaleFactor * tankNominalID; //32400일때 리더 길이 2000
            newLength = scaleLength.ToString();



            foreach(LeaderPointModel eachPoint in SingletonData.LeaderPublicList)
            {
                
                List<string> newText = eachPoint.lineTextList; ;
                List<string> newTextSub = eachPoint.emptyTextList;
                if (newText.Count == 0)
                    newText = newTextSub;
                CDPoint currentPoint = eachPoint.leaderPoint;
                newPosition = eachPoint.Position;
                newLength = (scaleLength + (eachPoint.lineLength / 100 * scaleLength)).ToString();

                DrawEntityModel eachLeaderList = drawService.Draw_Leader(currentPoint, newLength, newPosition, "", "", newText, newTextSub, singleModel, scaleValue, layerName);
                returnEntity.AddDrawEntity(eachLeaderList);
            }


            return returnEntity;
        }


    }
}
