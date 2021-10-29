using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
using DrawSettingLib.SettingServices;
using DrawShapeLib.DrawServices;
using DrawWork.AssemblyServices;
using DrawWork.Commons;
using DrawWork.DrawCommonServices;
using DrawWork.DrawModels;
using DrawWork.DrawServices;
using DrawWork.DrawShapes;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailStructureService
    {

        private Model singleModel;
        private AssemblyModel assemblyData;
        private AssemblyCommonDataService assemblyComData;

        private DrawPublicService drawCommon;

        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;
        private DrawShapeServices shapeService;


        private DrawCommonService drawComService;
        private DrawReferenceBlockService refBlockService;
        private DrawWorkingPointService workingPointService;

        private DrawDetailStructureShareService drawShareService;


        public PaperAreaService areaService;

        public DrawBreakSymbols breakService { get; set; }

        // Structure
        private StructureService structureService;
        private StructureCRTColumnModel structureCRTColumnModel;


        public DrawDetailStructureService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;
            assemblyData = selAssembly;
            assemblyComData = new AssemblyCommonDataService(selAssembly);

            drawCommon = new DrawPublicService(selAssembly);
            refBlockService = new DrawReferenceBlockService(selAssembly);
            workingPointService = new DrawWorkingPointService(selAssembly);

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();


            editingService = new DrawEditingService();
            shapeService = new DrawShapeServices();

            drawComService = new DrawCommonService();
            drawShareService = new DrawDetailStructureShareService(selAssembly,selModel) ;

            areaService = new PaperAreaService();

            breakService = new DrawBreakSymbols();

            structureService = new StructureService();
            structureCRTColumnModel = new StructureCRTColumnModel();
        }

        public DrawEntityModel DrawDetailRoofStructureMain(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double selScaleValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();

            // Structure Model : CRT
            CreateStructureModel();

            // Structure
            foreach (PaperAreaModel eachModel in SingletonData.PaperArea.AreaList)
            {
                //PaperAreaModel eachModel = areaService.GetPaperAreaModel(eachModel.Name, eachModel.SubName, SingletonData.PaperArea.AreaList);
                // RefPoint
                Point3D referencePoint = new Point3D(eachModel.ReferencePoint.X, eachModel.ReferencePoint.Y);
                //SingletonData.RefPoint = referencePoint;
                // Scale
                double scaleValue = GetStructureCustomScale(eachModel);

                // Structure
                if (eachModel.SubName== PAPERSUB_TYPE.RoofStructureOrientation)               
                {
                    //drawList.AddDrawEntity(DrawDetailRoofStructureOrientation(referencePoint, selModel, scaleValue, eachModel));
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RoofStructureAssembly)
                {
                    //drawList.AddDrawEntity(DrawDetailRoofStructureAssembly(referencePoint, selModel, scaleValue,eachModel));
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RafterSideClipDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.CenterRingDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RafterCenterClipDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.PurlinDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.SectionAA)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RibPlateDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }


                // Structure Ext
                else if (eachModel.SubName == PAPERSUB_TYPE.CenterRingRafterDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailB)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.SectionCC)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.ViewC)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RafterAndReinfPadCrossDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }


                // Structure Column
                else if (eachModel.SubName == PAPERSUB_TYPE.CenterColumnDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.C2SideColumnDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.C3SideColumnDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailG)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }

                else if (eachModel.SubName == PAPERSUB_TYPE.DrainDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }











                else if (eachModel.SubName == PAPERSUB_TYPE.SectionBB)
                {
                    //drawList.AddDrawEntity(DrawDetailCenterColumn_BB(referencePoint, selModel, scaleValue, eachModel));
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailC)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailE)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailF)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.STRSectionDD)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }




                else if (eachModel.SubName == PAPERSUB_TYPE.SectionEE)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailD)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.GirderBracketDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Rafter)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }





                else if (eachModel.SubName == PAPERSUB_TYPE.Girder)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailA)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.BracketDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }


                else if (eachModel.SubName == PAPERSUB_TYPE.Table1)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Table2)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Table3)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Table4)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }


            }

            return drawList;
        }



        public void CreateStructureModel()
        {

            #region Structure Data
            // Input : Rafter , Column, Girder
            List<StructureCRTRafterInputModel> rafterInputList = assemblyData.StructureCRTRafterInput.ToList();
            List<StructureCRTColumnInputModel> columnInputList = assemblyData.StructureCRTColumnInput.ToList();
            List<StructureCRTGirderInputModel> girderInputList = assemblyData.StructureCRTGirderInput.ToList();

            // Output : Rafter -> Column Rafter
            List<NColumnRafterModel> rafterOutputList = new List<NColumnRafterModel>();
            foreach (StructureCRTRafterInputModel eachRafter in rafterInputList)
                rafterOutputList.Add(GetRafterModel(eachRafter.Size));
            
            // Output : Rafter -> H Beam
            List<HBeamModel> girderHbeamList = new List<HBeamModel>();
            foreach (StructureCRTGirderInputModel eachGirder in girderInputList)
                girderHbeamList.Add(GetHBeamModel(eachGirder.Size));

            List<NColumnGirderModel> girderOutputList = new List<NColumnGirderModel>();
            foreach (StructureCRTGirderInputModel eachGirder in girderInputList)
                girderOutputList.Add(GetNewColumnGirderModel(eachGirder.Size));
            
            // Output : Column -> First Column = Cetner Top Support
            List<NColumnCenterTopSupportModel> columnCenterTopSupportList = new List<NColumnCenterTopSupportModel>();
            for(int rafterIndex=0;rafterIndex<rafterInputList.Count;rafterIndex++) 
            {
                StructureCRTRafterInputModel eachRafter = rafterInputList[rafterIndex];
                StructureCRTColumnInputModel eachColumn = columnInputList[rafterIndex];
                columnCenterTopSupportList.Add(GetNewColumnCenterTopSupportModel(eachRafter.Size, eachColumn.Size));
            }

            // Output : Column -> First Column = Center Top Support : Pipe
            List<PipeModel> columnPipeList = new List<PipeModel>();
            foreach (StructureCRTColumnInputModel eachColumn in columnInputList)
                columnPipeList.Add(GetPipeModel(eachColumn.Size));
            
            List<NColumnBaseSupportModel> columnBaseSupportList = new List<NColumnBaseSupportModel>();
            foreach (StructureCRTColumnInputModel eachColumn in columnInputList)
                columnBaseSupportList.Add(GetColumnBaseSupportModel(eachColumn.Size));

            // Pipe Schedule 적용 안됨
            List<NColumnSideTopSupportModel> columnSideTopSupportList = new List<NColumnSideTopSupportModel>();
            foreach(StructureCRTColumnInputModel eachColumn in columnInputList)
                columnSideTopSupportList.Add(GetNewColumnSideTopSupportModel(eachColumn.Size));


            // Input : Centering
            List<StructureCRTCenteringInputModel> centeringInputList = assemblyData.StructureCRTCenterRingInput.ToList();




            #endregion


            // Tank : Basic Information
            double selTankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double selTankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);

            // Tank : Roof
            double selRoofOD = drawCommon.GetRoofOD();
            double selRoofSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);

            // Tank : Bottom
            double selBottomThk = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            double selBottoSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);
            double selAnnularInnerWidth = 0;
            if (drawCommon.isAnnular())
                selAnnularInnerWidth = (selTankID - valueService.GetDoubleValue(assemblyData.BottomInput[0].OD)) / 2;
            
            // Tnak : Shell Reduce : k Type Shell  안쪽으로 들어 옴
            double selShellReduce = GetShellReduceByTopAngle();


            // Create Structure
            switch (assemblyComData.StructureSupprtType)
            {
                case StructureSupporting_Type.RafterOnlyExternal:
                case StructureSupporting_Type.RafterOnlyInternal:
                    structureCRTColumnModel = structureService.CreateStructureCRTColumn(

                        rafterInputList, columnInputList, girderInputList,

                        girderHbeamList, columnCenterTopSupportList,
                        columnSideTopSupportList,
                        girderOutputList,
                        columnBaseSupportList,
                        columnPipeList,
                        rafterOutputList,

                        selTankID,
                        selTankHeight,
                        selAnnularInnerWidth,
                        selRoofOD,
                        selBottomThk,
                        selRoofSlope,
                        selBottoSlope,
                        selShellReduce);
                    break;
                case StructureSupporting_Type.RafterWColumn:
                case StructureSupporting_Type.RafterWColumnGirder:
                    structureCRTColumnModel = structureService.CreateStructureCRTCentering(
                        centeringInputList,
                        rafterInputList, columnInputList, girderInputList,

                        girderHbeamList, columnCenterTopSupportList,
                        columnSideTopSupportList,
                        girderOutputList,
                        columnBaseSupportList,
                        columnPipeList,
                        rafterOutputList,

                        selTankID,
                        selTankHeight,
                        selAnnularInnerWidth,
                        selRoofOD,
                        selBottomThk,
                        selRoofSlope,
                        selBottoSlope,
                        selShellReduce);
                    break;
            }



        }



        private double GetShellReduceByTopAngle()
        {
            double returnValue = 0;
            if (drawCommon.GetCurrentTopAngleType() == TopAngle_Type.k)
            {
                if (assemblyData.ShellOutput.Count >= 2)
                {
                    int maxCourse = assemblyData.ShellOutput.Count - 1;
                    double lastThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                    double lastThkBefore = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse - 1].Thickness);
                    returnValue = (lastThk - lastThkBefore) / 2;
                }
            }

            return returnValue;
        }


        #region Detail Sample
        private double GetStructureCustomScale(PaperAreaModel selModel)
        {
            double returnValue = 1;

            // Custom Scale
            switch (selModel.SubName)
            {
                case PAPERSUB_TYPE.RoofStructureAssembly:
                    //selModel.ScaleValue=1
                    break;
            }

            // AutoScale
            if (selModel.ScaleValue == 0)
                selModel.ScaleValue = 1;
            returnValue = selModel.ScaleValue;
            return returnValue;
        }

        private DrawEntityModel DrawDetailAssembly(Point3D refPoint, object selModel, double scaleValue,PaperAreaModel selPaperModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            List<Entity> newList = new List<Entity>();



            double testRecSize = 80 * scaleValue;
            double textHeight = 10 * scaleValue;

            Point3D recPoint = GetSumPoint(referencePoint, -testRecSize/2, -testRecSize/2);
            Line testLine01 = new Line(GetSumPoint(recPoint, 0, 0), GetSumPoint(recPoint, 0, testRecSize));
            Line testLine02 = new Line(GetSumPoint(recPoint, 0, testRecSize), GetSumPoint(recPoint, testRecSize, testRecSize));
            Line testLine03 = new Line(GetSumPoint(recPoint, testRecSize, testRecSize), GetSumPoint(recPoint, testRecSize,0));
            Line testLine04 = new Line(GetSumPoint(recPoint, testRecSize, 0), GetSumPoint(recPoint, 0, 0));


            Text text01 = new Text(GetSumPoint(referencePoint, 0, 0), selPaperModel.SubName.ToString(), textHeight);
            text01.Alignment = Text.alignmentType.MiddleCenter;


            newList.Add(testLine01);
            newList.Add(testLine02);
            newList.Add(testLine03);
            newList.Add(testLine04);

            newList.Add(text01);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(newList);

            return drawList;
        }
        #endregion





        #region One Size
        public DrawEntityModel DrawDetailCenterColumn_BB(Point3D refPoint, object selModel, double scaleValue, PaperAreaModel selPaperModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            List<Entity> newList = new List<Entity>();


            //newList.AddRange(drawShareService.GetCenterColumn_BB(referencePoint, scaleValue));

            newList.AddRange(drawShareService.GetCenterColumn_Detail_C(GetSumPoint(referencePoint,1000,0), scaleValue));
            newList.AddRange(drawShareService.GetCenterColumn_Detail_D(GetSumPoint(referencePoint, 2000, 0), scaleValue)); // 작성중
            newList.AddRange(drawShareService.GetCenterColumn_EE(GetSumPoint(referencePoint, 3000, 0), scaleValue));
            newList.AddRange(drawShareService.GetCenterColumn_Drain_Detail(GetSumPoint(referencePoint, 4000, 0), scaleValue));
            newList.AddRange(drawShareService.GetCenterColumn_BB_Edit(GetSumPoint(referencePoint, 5000, 0), scaleValue));
            

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(newList);

            return drawList;
        }
        #endregion



        // Roof Structure : Side View
        public DrawEntityModel DrawDetailRoofStructureAssembly(Point3D refPoint, object selModel, double scaleValue, PaperAreaModel selPaperModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();

            switch(assemblyComData.TankType)
            {
                case TANK_TYPE.CRT:
                    switch (assemblyComData.StructureSupprtType)
                    {
                        case StructureSupporting_Type.RafterOnlyInternal:
                        case StructureSupporting_Type.RafterOnlyExternal:
                            drawList.AddDrawEntity(DrawDetailRoofStructureColumnAssembly(refPoint, selModel, scaleValue, selPaperModel));
                            break;
                        case StructureSupporting_Type.RafterWColumn:
                        case StructureSupporting_Type.RafterWColumnGirder:
                            drawList.AddDrawEntity(DrawDetailRoofStructureCenteringAssembly(refPoint, selModel, scaleValue, selPaperModel));
                            break;
                    }
                    break;
                case TANK_TYPE.DRT:
                    break;
            }

            return drawList;
        }


        public DrawEntityModel DrawDetailRoofStructureColumnAssembly(Point3D refPoint, object selModel, double scaleValue, PaperAreaModel selPaperModel)
        {
            // List
            DrawEntityModel drawList = new DrawEntityModel();

            // Reference Point
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            Point3D shellBottomPoint = GetSumPoint(referencePoint, 0, 0);

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            //double roofSlope= valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);
            double bottomThk = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            double shellLastCourseThk = assemblyComData.ShellLastCourseThk;

            List<Entity> topAngleList = new List<Entity>();
            List<Entity> shellList = new List<Entity>();
            List<Entity> bottomList = new List<Entity>();
            List<Entity> roofList = new List<Entity>();

            List<Entity> clipList = new List<Entity>();
            List<Entity> rafterList = new List<Entity>();
            List<Entity> columnList = new List<Entity>();
            List<Entity> columnCenterLineList = new List<Entity>();
            List<Entity> girderList = new List<Entity>();
            List<Entity> rafterClipList = new List<Entity>();

            List<Entity> centerLineList = new List<Entity>();


            List<Entity> etcList = new List<Entity>();

            double shellHeight = 0;
            double bottomCenterOpposite = 0;
            double roofCenterOpposite = 0;

            double shellHeightPercentage = 100;

            double shellSideHeight = 0;
            double centerSideHeight = 0;

            Point3D roofCenterLowerPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopDown, referencePoint); 
            Point3D roofCenterUpperPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopUp, referencePoint); 
            Point3D roofSideLowerPoint= workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint); 
            Point3D roofSideUpperPoint= workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofUp, referencePoint);


            Point3D bottomCenterLowerPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterBottomDown, referencePoint);
            Point3D bottomCenterUpperPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterBottomUp, referencePoint);
            Point3D bottomSideLowerPoint= GetSumPoint(shellBottomPoint, 0, 0);
            Point3D bottomSideUpperPoint = GetSumPoint(shellBottomPoint, 0, 0);





            List<Point3D> rafterOutputPointList = new List<Point3D>();

            // CenterLine
            double exLength = 2;
            double exCenterLength = 6;

            centerLineList.AddRange( editingService.GetCenterLine(GetSumPoint(roofCenterUpperPoint, 0,0), GetSumPoint(bottomCenterLowerPoint, 0, 0), exCenterLength, scaleValue));

            // Shell
            shellList.AddRange(GetShellAssembly(GetSumPoint(referencePoint,0,0))) ;
            // Bottom
            bottomList.AddRange(GetBottomAssembly(GetSumPoint(referencePoint, 0, 0)));
            // TopAngle
            topAngleList.AddRange(GetTopAngleAssembly(GetSumPoint(referencePoint, 0, 0)));
            // Roof
            roofList.AddRange(GetRoofAssembly(GetSumPoint(referencePoint, 0, 0)));





            // Shell Side Clip
            Point3D roofStartPoint=  workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);
            Point3D clipShellSidePoint = GetClipShellSidePoint(referencePoint);
            StructureRafterModel rafterLastModel = structureService.GetShortRafterInLayer(structureCRTColumnModel.LayerList[structureCRTColumnModel.LayerList.Count - 1].RafterList);
            NRafterSupportClipShellSideModel rafterClipShellSide = GetNewRafterSupportClipShellSideModel(rafterLastModel.Size);
            clipList.AddRange(drawShareService.RafterSideClipDetail(clipShellSidePoint, roofStartPoint, singleModel, scaleValue, rafterClipShellSide,shellLastCourseThk).GetDrawEntity());


            // Rafter, Column, Girder
            double centerFromCL = 0;

            // Column Point 
            Point3D currentColumnSideGirderPoint = GetSumPoint(roofCenterLowerPoint, 0, 0);
            Point3D innerColumnSideGirderPoint = GetSumPoint(roofCenterLowerPoint, 0, 0);

            for (int layerIndex = 0; layerIndex < structureCRTColumnModel.LayerList.Count; layerIndex++)
            {
                StructureLayerModel currentLayer = structureCRTColumnModel.LayerList[layerIndex];

                NColumnBaseSupportModel currentColumnBaseSupport = structureCRTColumnModel.strData.newColumnBaseSupportList[layerIndex];
                NColumnCenterTopSupportModel currentColumnCenterSupport = structureCRTColumnModel.strData.newColumnCenterTopSupportList[layerIndex];
                NColumnSideTopSupportModel currentColumnSideSupport = structureCRTColumnModel.strData.newColumnSideTopSupportList[layerIndex];
                PipeModel currentColumnPipe = structureCRTColumnModel.strData.newColumnPipeList[layerIndex];
                AngleSizeModel currentColumnBaseAngle = GetAngleModel(currentColumnBaseSupport.J1);
                HBeamModel currentGirderHBeam = structureCRTColumnModel.strData.newGirderHBeamList[layerIndex];

                double columnRadius = currentLayer.Radius;
                Point3D bottomUppderRadiusPoint= workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.AdjCenterBottomUp,columnRadius, referencePoint);

                centerFromCL = currentLayer.Radius;
                //rafterList.Add(new Line(GetSumPoint(roofCenterLowerPoint, -centerFromCL, -3000), GetSumPoint(roofCenterLowerPoint, -centerFromCL, 1000))); // 검증 라인

                // Center
                if (layerIndex == 0)
                {
                    if (currentLayer.ColumnList.Count > 0) 
                    {
                        StructureColumnModel centerColumn = currentLayer.ColumnList[0];

                        // Column : Center Top Support => ref : Rafter Size
                        StructureLayerModel outerLayer = structureCRTColumnModel.LayerList[layerIndex + 1];
                        StructureRafterModel outerRafter = structureService.GetLongRafterInLayer(outerLayer.RafterList);
                        double nCenterColumnPlateThk = valueService.GetDoubleValue(currentColumnCenterSupport.C1);

                        // Column
                        Point3D columnCenterTopPoint = GetSumPoint(bottomUppderRadiusPoint, 0, centerColumn.Height);
                        columnList.AddRange(drawShareService.DrawColumnCenterTopSupport(columnCenterTopPoint, singleModel, scaleValue, currentColumnCenterSupport, currentColumnPipe).GetDrawEntity());

                        // Column Clip
                        if (currentLayer.GirderList.Count > 0)
                        {
                            StructureGirderModel eachGirder = currentLayer.GirderList[0];
                            StructureClipModel eachClip = eachGirder.ClipList[0];
                            double columnClipDistance = valueService.GetDoubleValue(currentColumnCenterSupport.B);
                            Point3D columnCenterLeftClipPoint = GetSumPoint(bottomUppderRadiusPoint, -columnClipDistance, centerColumn.Height);
                            Point3D columnCenterRightClipPoint = GetSumPoint(bottomUppderRadiusPoint, columnClipDistance, centerColumn.Height);
                            columnList.AddRange(GetClipAssembly(GetSumPoint(columnCenterLeftClipPoint, 0, 0), eachClip.ClipWidth, eachClip.ClipHeight, scaleValue));
                            columnList.AddRange(GetClipAssembly(GetSumPoint(columnCenterRightClipPoint, 0, 0), eachClip.ClipWidth, eachClip.ClipHeight, scaleValue));


                            // Column : Base
                            Point3D columnSideBasePoint = GetSumPoint(bottomUppderRadiusPoint, 0, 0);
                            columnList.AddRange(drawShareService.DrawColumnBaseSupport_Side_WideView(columnSideBasePoint, singleModel, scaleValue, centerColumn.Height,
                                bottomThk, nCenterColumnPlateThk, currentColumnBaseSupport, currentColumnBaseAngle, currentColumnPipe, bottomSlope, true).GetDrawEntity());

                            //columnList.Add(new Line(GetSumPoint(columnSideBasePoint, 0, 0), GetSumPoint(columnSideBasePoint, 100, 1000)));
                        }

                    }

                }

                // Side
                else
                {

                    double columnSideGirderHeight = valueService.GetDoubleValue(currentGirderHBeam.A);


                    // Column : Side Top Support
                    if (currentLayer.ColumnList.Count > 0)
                    {
                        StructureColumnModel sideColumn = currentLayer.ColumnList[0];
                        StructureRafterModel eachRafter = structureService.GetShortRafterInLayer(currentLayer.RafterList);

                        double nCenterColumnPlateThk = valueService.GetDoubleValue(currentColumnSideSupport.A1);


                        // Girder : Top
                        Point3D columnSideGirderPoint = GetSumPoint(bottomUppderRadiusPoint, 0, sideColumn.Height + columnSideGirderHeight);
                        girderList.AddRange(GetGirderFrontAssembly(GetSumPoint(columnSideGirderPoint, 0, 0), currentGirderHBeam, scaleValue));

                        // Column : Top
                        Point3D columnSideTopPoint = GetSumPoint(bottomUppderRadiusPoint, 0, sideColumn.Height);
                        columnList.AddRange(drawShareService.DrawColumnSideTopSupport(columnSideTopPoint, singleModel, scaleValue, currentColumnSideSupport, currentColumnPipe).GetDrawEntity());

                        // Column : Base
                        Point3D columnSideBasePoint = GetSumPoint(bottomUppderRadiusPoint, 0, 0);
                        columnList.AddRange(drawShareService.DrawColumnBaseSupport_Side_WideView(columnSideBasePoint, singleModel, scaleValue, sideColumn.Height,
                            bottomThk, nCenterColumnPlateThk, currentColumnBaseSupport, currentColumnBaseAngle, currentColumnPipe, bottomSlope, false).GetDrawEntity());

                        // Column : CenterLine
                        columnCenterLineList.AddRange(editingService.GetCenterLine(GetSumPoint(bottomUppderRadiusPoint,0, 0),
                                                                                   GetSumPoint(bottomUppderRadiusPoint, 0, sideColumn.Height),exLength , scaleValue));

                        // Current : Column Point
                        currentColumnSideGirderPoint = GetSumPoint(columnSideGirderPoint, 0, 0);
                        //columnList.Add(new Line(GetSumPoint(columnSideBasePoint, 0, 0), GetSumPoint(columnSideBasePoint, 100, 1000)));
                    }

                    // Rafter : Current -> Inner
                    if (currentLayer.RafterList.Count > 0)
                    {

                        // Rafter
                        StructureRafterModel eachRafter = structureService.GetShortRafterInLayer(currentLayer.RafterList);
                        if (layerIndex == 1)
                            eachRafter = structureService.GetLongRafterInLayer(currentLayer.RafterList);

                        NColumnRafterModel nColumnRafter = GetRafterModel(eachRafter.Size);
                        double sideRafterStartX = -eachRafter.InnerRealRadius;
                        double sideRafterStartY = -valueService.GetOppositeByAdjacent(assemblyComData.RoofSlopeRadian, eachRafter.InnerRealRadius);

                        // Rafter : Hole 관련
                        bool rafterRightToCenter = false;
                        if (layerIndex == 1)
                            rafterRightToCenter = true;

                        double rafterLeftHoleQty = valueService.GetDoubleValue(nColumnRafter.BoltHoleQtyOnColumnSide); 
                        double rafterRightHoleQty = valueService.GetDoubleValue(nColumnRafter.BoltHoleQtyOnColumnSide);
                        if (layerIndex == 1)
                            rafterRightHoleQty = valueService.GetDoubleValue(nColumnRafter.BoltHoleQtyOnCenterSide);
                        if(layerIndex== structureCRTColumnModel.LayerList.Count-1)
                            rafterLeftHoleQty = valueService.GetDoubleValue(nColumnRafter.BoltHoleQtyOnShellSide);

                        rafterList.AddRange(GetRafterAssembly(out rafterOutputPointList, 
                                                GetSumPoint(roofCenterLowerPoint, sideRafterStartX, sideRafterStartY), 
                                                eachRafter.Length, assemblyComData.RoofSlopeRadian, 1, 1, null, nColumnRafter,
                                                rafterLeftHoleQty,rafterRightHoleQty, shellLastCourseThk, rafterRightToCenter,                                               
                                                scaleValue));



                        // Rafter : Inner Clip
                        StructureLayerModel innerLayer= structureCRTColumnModel.LayerList[layerIndex-1];
                        StructureGirderModel innerGirder = structureService.GetGirderByRafterAngle(innerLayer.GirderList, eachRafter.AngleFromCenter);
                        StructureGirderModel currentGirder = structureService.GetGirderByRafterAngle(currentLayer.GirderList, eachRafter.AngleFromCenter);
                        StructureClipModel innerGirderClip = structureService.GetClipByRafterAngle(innerGirder.ClipList, eachRafter.AngleFromCenter, 1);
                        StructureClipModel currentGirderClip = structureService.GetClipByRafterAngle(currentGirder.ClipList, eachRafter.AngleFromCenter, 0);


                        if (layerIndex == 1)
                        {
                            // Outer Clip 만
                            Point3D outerClipPoint = new Point3D(roofCenterLowerPoint.X - eachRafter.OuterClipRadiusFromCenter, currentColumnSideGirderPoint.Y);
                            rafterClipList.AddRange(GetClipAssembly(GetSumPoint(outerClipPoint, 0, 0), currentGirderClip.ClipWidth, currentGirderClip.ClipHeight, scaleValue));
                        }
                        else if(layerIndex == structureCRTColumnModel.LayerList.Count-1)
                        {
                            // Inner Clip 만
                            
                            Point3D innerClipPoint = new Point3D(roofCenterLowerPoint.X - eachRafter.InnerClipRadiusFromCenter, innerColumnSideGirderPoint.Y);
                            rafterClipList.AddRange(GetClipAssembly(GetSumPoint(innerClipPoint, 0, 0), innerGirderClip.ClipWidth, innerGirderClip.ClipHeight, scaleValue));
                        }
                        else
                        {
                            // Outer , Inner Clip
                            // Outer Clip 만
                            Point3D innerClipPoint = new Point3D(roofCenterLowerPoint.X - eachRafter.InnerClipRadiusFromCenter, innerColumnSideGirderPoint.Y);
                            rafterClipList.AddRange(GetClipAssembly(GetSumPoint(innerClipPoint, 0, 0), innerGirderClip.ClipWidth, innerGirderClip.ClipHeight, scaleValue));
                            Point3D outerClipPoint = new Point3D(roofCenterLowerPoint.X - eachRafter.OuterClipRadiusFromCenter, currentColumnSideGirderPoint.Y);
                            rafterClipList.AddRange(GetClipAssembly(GetSumPoint(outerClipPoint, 0, 0), currentGirderClip.ClipWidth, currentGirderClip.ClipHeight, scaleValue));
                        }

                        //rafterList.Add(new Line(GetSumPoint(roofCenterLowerPoint, sideRafterStartX, 0), GetSumPoint(roofCenterLowerPoint, sideRafterStartX, -1000))); // 검증 라인
                    }

                    // Inner : Column Point
                    innerColumnSideGirderPoint = GetSumPoint(currentColumnSideGirderPoint, 0, 0);

                }

                // Column : Base
                //DrawEntityModel structureBaseDraw = DDSServiceShare.DrawColumnBaseSupport(refPoint, singleModel, scaleValue, baseModel);

            }

            // 
            // 1
            //etcList.AddRange(drawComService.GetColumnCenterTopSupport_TopView(GetSumPoint(referencePoint, 0, 0), scaleValue));
            // 2
            //etcList.AddRange(drawComService.GetColumnCenterTopSupport(GetSumPoint(referencePoint, 1000, 0), scaleValue));
            // 3





            //etcList.AddRange(drawShareService.Rafter(GetSumPoint(referencePoint, 0, tankHeight), scaleValue,));


            styleService.SetLayerListEntity(ref columnCenterLineList, layerService.LayerCenterLine);
            drawList.centerlineList.AddRange(columnCenterLineList);

            drawList.outlineList.AddRange(columnList);

            //styleService.SetLayerListEntity(ref rafterList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(rafterList);

            //styleService.SetLayerListEntity(ref clipList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(clipList);

            //styleService.SetLayerListEntity(ref girderList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(girderList);

            drawList.outlineList.AddRange(rafterClipList);

            styleService.SetLayerListEntity(ref topAngleList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(topAngleList);

            styleService.SetLayerListEntity(ref topAngleList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(topAngleList);

            styleService.SetLayerListEntity(ref shellList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(shellList);

            styleService.SetLayerListEntity(ref bottomList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(bottomList);

            styleService.SetLayerListEntity(ref roofList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(roofList);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            drawList.centerlineList.AddRange(centerLineList);

            drawList.outlineList.AddRange(etcList);

            return drawList;
        }

        public DrawEntityModel DrawDetailRoofStructureCenteringAssembly(Point3D refPoint, object selModel, double scaleValue, PaperAreaModel selPaperModel)
        {
            // List
            DrawEntityModel drawList = new DrawEntityModel();

            // Reference Point
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            Point3D shellBottomPoint = GetSumPoint(referencePoint, 0, 0);

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            //double roofSlope= valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);
            double bottomThk = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            double shellLastCourseThk = assemblyComData.ShellLastCourseThk;

            List<Entity> topAngleList = new List<Entity>();
            List<Entity> shellList = new List<Entity>();
            List<Entity> bottomList = new List<Entity>();
            List<Entity> roofList = new List<Entity>();

            List<Entity> clipList = new List<Entity>();
            List<Entity> rafterList = new List<Entity>();
            List<Entity> columnList = new List<Entity>();
            List<Entity> columnCenterLineList = new List<Entity>();
            List<Entity> girderList = new List<Entity>();
            List<Entity> rafterClipList = new List<Entity>();

            List<Entity> centerLineList = new List<Entity>();


            List<Entity> etcList = new List<Entity>();

            double shellHeight = 0;
            double bottomCenterOpposite = 0;
            double roofCenterOpposite = 0;

            double shellHeightPercentage = 100;

            double shellSideHeight = 0;
            double centerSideHeight = 0;

            Point3D roofCenterLowerPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopDown, referencePoint);
            Point3D roofCenterUpperPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopUp, referencePoint);
            Point3D roofSideLowerPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);
            Point3D roofSideUpperPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofUp, referencePoint);


            Point3D bottomCenterLowerPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterBottomDown, referencePoint);
            Point3D bottomCenterUpperPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterBottomUp, referencePoint);
            Point3D bottomSideLowerPoint = GetSumPoint(shellBottomPoint, 0, 0);
            Point3D bottomSideUpperPoint = GetSumPoint(shellBottomPoint, 0, 0);





            List<Point3D> rafterOutputPointList = new List<Point3D>();

            // CenterLine
            double exLength = 2;
            double exCenterLength = 6;

            centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(roofCenterUpperPoint, 0, 0), GetSumPoint(bottomCenterLowerPoint, 0, 0), exCenterLength, scaleValue));

            // Shell
            shellList.AddRange(GetShellAssembly(GetSumPoint(referencePoint, 0, 0)));
            // Bottom
            bottomList.AddRange(GetBottomAssembly(GetSumPoint(referencePoint, 0, 0)));
            // TopAngle
            topAngleList.AddRange(GetTopAngleAssembly(GetSumPoint(referencePoint, 0, 0)));
            // Roof
            roofList.AddRange(GetRoofAssembly(GetSumPoint(referencePoint, 0, 0)));





            // Shell Side Clip
            Point3D roofStartPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);
            Point3D clipShellSidePoint = GetClipShellSidePoint(referencePoint);
            StructureRafterModel rafterLastModel = structureService.GetShortRafterInLayer(structureCRTColumnModel.LayerList[structureCRTColumnModel.LayerList.Count - 1].RafterList);
            NRafterSupportClipShellSideModel rafterClipShellSide = GetNewRafterSupportClipShellSideModel(rafterLastModel.Size);
            clipList.AddRange(drawShareService.RafterSideClipDetail(clipShellSidePoint, roofStartPoint, singleModel, scaleValue, rafterClipShellSide, shellLastCourseThk).GetDrawEntity());


            // Rafter, Column, Girder
            double centerFromCL = 0;

            // Column Point 
            Point3D currentColumnSideGirderPoint = GetSumPoint(roofCenterLowerPoint, 0, 0);
            Point3D innerColumnSideGirderPoint = GetSumPoint(roofCenterLowerPoint, 0, 0);

            for (int layerIndex = 0; layerIndex < structureCRTColumnModel.LayerList.Count; layerIndex++)
            {
                StructureLayerModel currentLayer = structureCRTColumnModel.LayerList[layerIndex];

                NColumnBaseSupportModel currentColumnBaseSupport = structureCRTColumnModel.strData.newColumnBaseSupportList[layerIndex];
                NColumnCenterTopSupportModel currentColumnCenterSupport = structureCRTColumnModel.strData.newColumnCenterTopSupportList[layerIndex];
                NColumnSideTopSupportModel currentColumnSideSupport = structureCRTColumnModel.strData.newColumnSideTopSupportList[layerIndex];
                PipeModel currentColumnPipe = structureCRTColumnModel.strData.newColumnPipeList[layerIndex];
                AngleSizeModel currentColumnBaseAngle = GetAngleModel(currentColumnBaseSupport.J1);
                HBeamModel currentGirderHBeam = structureCRTColumnModel.strData.newGirderHBeamList[layerIndex];

                double columnRadius = currentLayer.Radius;
                Point3D bottomUppderRadiusPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.AdjCenterBottomUp, columnRadius, referencePoint);

                centerFromCL = currentLayer.Radius;
                //rafterList.Add(new Line(GetSumPoint(roofCenterLowerPoint, -centerFromCL, -3000), GetSumPoint(roofCenterLowerPoint, -centerFromCL, 1000))); // 검증 라인

                // Center
                if (layerIndex == 0)
                {
                    if (currentLayer.ColumnList.Count > 0)
                    {
                        StructureColumnModel centerColumn = currentLayer.ColumnList[0];

                        // Column : Center Top Support => ref : Rafter Size
                        StructureLayerModel outerLayer = structureCRTColumnModel.LayerList[layerIndex + 1];
                        StructureRafterModel outerRafter = structureService.GetLongRafterInLayer(outerLayer.RafterList);
                        double nCenterColumnPlateThk = valueService.GetDoubleValue(currentColumnCenterSupport.C1);

                        // Column
                        Point3D columnCenterTopPoint = GetSumPoint(bottomUppderRadiusPoint, 0, centerColumn.Height);
                        columnList.AddRange(drawShareService.DrawColumnCenterTopSupport(columnCenterTopPoint, singleModel, scaleValue, currentColumnCenterSupport, currentColumnPipe).GetDrawEntity());

                        // Column Clip
                        if (currentLayer.GirderList.Count > 0)
                        {
                            StructureGirderModel eachGirder = currentLayer.GirderList[0];
                            StructureClipModel eachClip = eachGirder.ClipList[0];
                            double columnClipDistance = valueService.GetDoubleValue(currentColumnCenterSupport.B);
                            Point3D columnCenterLeftClipPoint = GetSumPoint(bottomUppderRadiusPoint, -columnClipDistance, centerColumn.Height);
                            Point3D columnCenterRightClipPoint = GetSumPoint(bottomUppderRadiusPoint, columnClipDistance, centerColumn.Height);
                            columnList.AddRange(GetClipAssembly(GetSumPoint(columnCenterLeftClipPoint, 0, 0), eachClip.ClipWidth, eachClip.ClipHeight, scaleValue));
                            columnList.AddRange(GetClipAssembly(GetSumPoint(columnCenterRightClipPoint, 0, 0), eachClip.ClipWidth, eachClip.ClipHeight, scaleValue));


                            // Column : Base
                            Point3D columnSideBasePoint = GetSumPoint(bottomUppderRadiusPoint, 0, 0);
                            columnList.AddRange(drawShareService.DrawColumnBaseSupport_Side_WideView(columnSideBasePoint, singleModel, scaleValue, centerColumn.Height,
                                bottomThk, nCenterColumnPlateThk, currentColumnBaseSupport, currentColumnBaseAngle, currentColumnPipe, bottomSlope, true).GetDrawEntity());

                            //columnList.Add(new Line(GetSumPoint(columnSideBasePoint, 0, 0), GetSumPoint(columnSideBasePoint, 100, 1000)));
                        }

                    }

                }

                // Side
                else
                {

                    double columnSideGirderHeight = valueService.GetDoubleValue(currentGirderHBeam.A);


                    // Column : Side Top Support
                    if (currentLayer.ColumnList.Count > 0)
                    {
                        StructureColumnModel sideColumn = currentLayer.ColumnList[0];
                        StructureRafterModel eachRafter = structureService.GetShortRafterInLayer(currentLayer.RafterList);

                        double nCenterColumnPlateThk = valueService.GetDoubleValue(currentColumnSideSupport.A1);


                        // Girder : Top
                        Point3D columnSideGirderPoint = GetSumPoint(bottomUppderRadiusPoint, 0, sideColumn.Height + columnSideGirderHeight);
                        girderList.AddRange(GetGirderFrontAssembly(GetSumPoint(columnSideGirderPoint, 0, 0), currentGirderHBeam, scaleValue));

                        // Column : Top
                        Point3D columnSideTopPoint = GetSumPoint(bottomUppderRadiusPoint, 0, sideColumn.Height);
                        columnList.AddRange(drawShareService.DrawColumnSideTopSupport(columnSideTopPoint, singleModel, scaleValue, currentColumnSideSupport, currentColumnPipe).GetDrawEntity());

                        // Column : Base
                        Point3D columnSideBasePoint = GetSumPoint(bottomUppderRadiusPoint, 0, 0);
                        columnList.AddRange(drawShareService.DrawColumnBaseSupport_Side_WideView(columnSideBasePoint, singleModel, scaleValue, sideColumn.Height,
                            bottomThk, nCenterColumnPlateThk, currentColumnBaseSupport, currentColumnBaseAngle, currentColumnPipe, bottomSlope, false).GetDrawEntity());

                        // Column : CenterLine
                        columnCenterLineList.AddRange(editingService.GetCenterLine(GetSumPoint(bottomUppderRadiusPoint, 0, 0),
                                                                                   GetSumPoint(bottomUppderRadiusPoint, 0, sideColumn.Height), exLength, scaleValue));

                        // Current : Column Point
                        currentColumnSideGirderPoint = GetSumPoint(columnSideGirderPoint, 0, 0);
                        //columnList.Add(new Line(GetSumPoint(columnSideBasePoint, 0, 0), GetSumPoint(columnSideBasePoint, 100, 1000)));
                    }

                    // Rafter : Current -> Inner
                    if (currentLayer.RafterList.Count > 0)
                    {

                        // Rafter
                        StructureRafterModel eachRafter = structureService.GetShortRafterInLayer(currentLayer.RafterList);
                        if (layerIndex == 1)
                            eachRafter = structureService.GetLongRafterInLayer(currentLayer.RafterList);

                        NColumnRafterModel nColumnRafter = GetRafterModel(eachRafter.Size);
                        double sideRafterStartX = -eachRafter.InnerRealRadius;
                        double sideRafterStartY = -valueService.GetOppositeByAdjacent(assemblyComData.RoofSlopeRadian, eachRafter.InnerRealRadius);

                        // Rafter : Hole 관련
                        bool rafterRightToCenter = false;
                        if (layerIndex == 1)
                            rafterRightToCenter = true;

                        double rafterLeftHoleQty = valueService.GetDoubleValue(nColumnRafter.BoltHoleQtyOnColumnSide);
                        double rafterRightHoleQty = valueService.GetDoubleValue(nColumnRafter.BoltHoleQtyOnColumnSide);
                        if (layerIndex == 1)
                            rafterRightHoleQty = valueService.GetDoubleValue(nColumnRafter.BoltHoleQtyOnCenterSide);
                        if (layerIndex == structureCRTColumnModel.LayerList.Count - 1)
                            rafterLeftHoleQty = valueService.GetDoubleValue(nColumnRafter.BoltHoleQtyOnShellSide);

                        rafterList.AddRange(GetRafterAssembly(out rafterOutputPointList,
                                                GetSumPoint(roofCenterLowerPoint, sideRafterStartX, sideRafterStartY),
                                                eachRafter.Length, assemblyComData.RoofSlopeRadian, 1, 1, null, nColumnRafter,
                                                rafterLeftHoleQty, rafterRightHoleQty, shellLastCourseThk, rafterRightToCenter,
                                                scaleValue));



                        // Rafter : Inner Clip
                        StructureLayerModel innerLayer = structureCRTColumnModel.LayerList[layerIndex - 1];
                        StructureGirderModel innerGirder = structureService.GetGirderByRafterAngle(innerLayer.GirderList, eachRafter.AngleFromCenter);
                        StructureGirderModel currentGirder = structureService.GetGirderByRafterAngle(currentLayer.GirderList, eachRafter.AngleFromCenter);
                        StructureClipModel innerGirderClip = structureService.GetClipByRafterAngle(innerGirder.ClipList, eachRafter.AngleFromCenter, 1);
                        StructureClipModel currentGirderClip = structureService.GetClipByRafterAngle(currentGirder.ClipList, eachRafter.AngleFromCenter, 0);


                        if (layerIndex == 1)
                        {
                            // Outer Clip 만
                            Point3D outerClipPoint = new Point3D(roofCenterLowerPoint.X - eachRafter.OuterClipRadiusFromCenter, currentColumnSideGirderPoint.Y);
                            rafterClipList.AddRange(GetClipAssembly(GetSumPoint(outerClipPoint, 0, 0), currentGirderClip.ClipWidth, currentGirderClip.ClipHeight, scaleValue));
                        }
                        else if (layerIndex == structureCRTColumnModel.LayerList.Count - 1)
                        {
                            // Inner Clip 만

                            Point3D innerClipPoint = new Point3D(roofCenterLowerPoint.X - eachRafter.InnerClipRadiusFromCenter, innerColumnSideGirderPoint.Y);
                            rafterClipList.AddRange(GetClipAssembly(GetSumPoint(innerClipPoint, 0, 0), innerGirderClip.ClipWidth, innerGirderClip.ClipHeight, scaleValue));
                        }
                        else
                        {
                            // Outer , Inner Clip
                            // Outer Clip 만
                            Point3D innerClipPoint = new Point3D(roofCenterLowerPoint.X - eachRafter.InnerClipRadiusFromCenter, innerColumnSideGirderPoint.Y);
                            rafterClipList.AddRange(GetClipAssembly(GetSumPoint(innerClipPoint, 0, 0), innerGirderClip.ClipWidth, innerGirderClip.ClipHeight, scaleValue));
                            Point3D outerClipPoint = new Point3D(roofCenterLowerPoint.X - eachRafter.OuterClipRadiusFromCenter, currentColumnSideGirderPoint.Y);
                            rafterClipList.AddRange(GetClipAssembly(GetSumPoint(outerClipPoint, 0, 0), currentGirderClip.ClipWidth, currentGirderClip.ClipHeight, scaleValue));
                        }

                        //rafterList.Add(new Line(GetSumPoint(roofCenterLowerPoint, sideRafterStartX, 0), GetSumPoint(roofCenterLowerPoint, sideRafterStartX, -1000))); // 검증 라인
                    }

                    // Inner : Column Point
                    innerColumnSideGirderPoint = GetSumPoint(currentColumnSideGirderPoint, 0, 0);

                }

                // Column : Base
                //DrawEntityModel structureBaseDraw = DDSServiceShare.DrawColumnBaseSupport(refPoint, singleModel, scaleValue, baseModel);

            }

            // 
            // 1
            //etcList.AddRange(drawComService.GetColumnCenterTopSupport_TopView(GetSumPoint(referencePoint, 0, 0), scaleValue));
            // 2
            //etcList.AddRange(drawComService.GetColumnCenterTopSupport(GetSumPoint(referencePoint, 1000, 0), scaleValue));
            // 3





            //etcList.AddRange(drawShareService.Rafter(GetSumPoint(referencePoint, 0, tankHeight), scaleValue,));


            styleService.SetLayerListEntity(ref columnCenterLineList, layerService.LayerCenterLine);
            drawList.centerlineList.AddRange(columnCenterLineList);

            drawList.outlineList.AddRange(columnList);

            //styleService.SetLayerListEntity(ref rafterList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(rafterList);

            //styleService.SetLayerListEntity(ref clipList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(clipList);

            //styleService.SetLayerListEntity(ref girderList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(girderList);

            drawList.outlineList.AddRange(rafterClipList);

            styleService.SetLayerListEntity(ref topAngleList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(topAngleList);

            styleService.SetLayerListEntity(ref topAngleList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(topAngleList);

            styleService.SetLayerListEntity(ref shellList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(shellList);

            styleService.SetLayerListEntity(ref bottomList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(bottomList);

            styleService.SetLayerListEntity(ref roofList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(roofList);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            drawList.centerlineList.AddRange(centerLineList);

            drawList.outlineList.AddRange(etcList);

            return drawList;
        }

        public List<Entity> GetTopAngleAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> leftList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);


            Point3D anglePoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);

            string selAngleType = assemblyData.RoofCompressionRing[0].CompressionRingType;
            string selAngleSize = assemblyData.RoofCompressionRing[0].AngleSize;
            AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(selAngleSize);

            int maxCourse = assemblyData.ShellOutput.Count - 1;
            double lastShellCourseThk = -valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);

            TopAngle_Type currentAngle = drawCommon.GetCurrentTopAngleType();
            switch (currentAngle)
            {
                case TopAngle_Type.b:
                case TopAngle_Type.d:
                case TopAngle_Type.e:
                    leftList.AddRange(refBlockService.DrawReference_Angle(GetSumCDPoint(anglePoint, 0, 0), selAngleModel));
                    break;
                case TopAngle_Type.i:
                    anglePoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftShellTop, referencePoint);
                    leftList.AddRange(refBlockService.DrawReference_CompressionRingI(GetSumCDPoint(anglePoint,lastShellCourseThk,0)));
                    break;
                case TopAngle_Type.k:
                    anglePoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftShellTop, referencePoint);
                    leftList.AddRange(refBlockService.DrawReference_CompressionRingK(GetSumCDPoint(anglePoint, 0, 0)));
                    break;
                

            }


            


            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            Point3D mirrorPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

            // 좌측 하단이 기준

            newList.AddRange(leftList);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, leftList, GetSumPoint(mirrorPoint, 0, 0), true));

            return newList;
        }


        public List<Entity> GetRoofAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> leftList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;

            Point3D mirrorPoint = GetSumPoint(referencePoint, tankIDHalf, 0);


            Point3D leftRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);
            Point3D leftRoofUp = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofUp, referencePoint);
            Point3D centerRoofUp = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopUp, referencePoint);
            Point3D centerRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopDown, referencePoint);


            Line plateLowerLine = new Line(GetSumPoint(leftRoofDown, 0, 0), GetSumPoint(centerRoofDown, 0, 0));
            Line plateLeftLine = new Line(GetSumPoint(leftRoofDown, 0, 0), GetSumPoint(leftRoofUp, 0, 0));
            Line plateUpperLine = new Line(GetSumPoint(leftRoofUp, 0, 0), GetSumPoint(centerRoofUp, 0, 0));


            leftList.Add(plateLowerLine);
            leftList.Add(plateLeftLine);
            leftList.Add(plateUpperLine);



            newList.AddRange(leftList);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, leftList, GetSumPoint(mirrorPoint, 0, 0), true));

            return newList;
        }

        public List<Entity> GetBottomAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> bottomPlateLeft = new List<Entity>();
            List<Entity> annularPlateLeft= new List<Entity>();


            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            Point3D tankCenterPoint = GetSumPoint(refPoint, tankIDHalf, 0);




            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);
            double bottomOD = valueService.GetDoubleValue(assemblyData.BottomInput[0].OD);
            double bottomODHalf = bottomOD / 2;
            double bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            double bottomThicknessHyp = valueService.GetHypotenuseByWidth(bottomSlope, bottomThickness);

            double bottomLeftAdj = 0;
            double bottomCenterOpposite = 0;
            Point3D bottomLeftStartPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftBottomDown, referencePoint);
            Point3D bottomCenterPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterBottomDown, referencePoint);
            Point3D bottomCenterUpperPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterBottomUp, referencePoint);


            List<Point3D> outPointList = new List<Point3D>();
            //if (drawCommon.isAnnular())
            //{
            //    double annularOD = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateOD);
            //    double annularID = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateID);
            //    double annularThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateThickness);
            //    double annularWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateWidthWidth);
            //    double annularOutsideProjection = (annularOD - tankID) / 2;

            //    Point3D annularPoint = GetSumPoint(referencePoint, -annularOutsideProjection, 0);
            //    annularPlateLeft.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(annularPoint, 0, 0), annularWidth, annularThickness, 0, 0, 0));


            //    bottomLeftAdj = valueService.GetAdjacentByHypotenuse(bottomSlope, bottomODHalf);
            //    bottomCenterOpposite = valueService.GetOppositeByHypotenuse(bottomSlope, bottomODHalf);
            //    bottomLeftStartPoint = GetSumPoint(tankCenterPoint, -bottomLeftAdj, 0);

            //    //bottomCenterPoint = GetSumPoint(tankCenterPoint, 0, bottomCenterOpposite);
            //}
            //else
            //{

            //    double bottomCenterOppOfTankID = valueService.GetOppositeByAdjacent(bottomSlope, bottomODHalf);
            //    //bottomCenterPoint = GetSumPoint(tankCenterPoint, 0, bottomCenterOppOfTankID);

            //    bottomLeftAdj = valueService.GetAdjacentByHypotenuse(bottomSlope, bottomODHalf);
            //    bottomCenterOpposite = valueService.GetOppositeByHypotenuse(bottomSlope, bottomODHalf);
            //    bottomLeftStartPoint = GetSumPoint(bottomCenterUpperPoint, -bottomLeftAdj, -bottomCenterOpposite);


            //}

            //bottomCenterUpperPoint = GetSumPoint(bottomCenterPoint, 0, bottomThicknessHyp);

            Line plateLowerLine = new Line(GetSumPoint(bottomLeftStartPoint, 0, 0), GetSumPoint(bottomCenterPoint, 0,0));
            Line plateLeftLine = new Line(GetSumPoint(bottomLeftStartPoint, 0, 0), GetSumPoint(bottomLeftStartPoint, 0, bottomThickness));
            plateLeftLine.Rotate(bottomSlope, Vector3D.AxisZ, GetSumPoint(bottomLeftStartPoint, 0, 0));
            Line plateUpperLine = new Line(GetSumPoint(plateLeftLine.EndPoint, 0, 0), GetSumPoint(bottomCenterUpperPoint, 0, 0));



            bottomPlateLeft.Add(plateLowerLine);
            bottomPlateLeft.Add(plateUpperLine);
            bottomPlateLeft.Add(plateLeftLine);

            newList.AddRange(annularPlateLeft);
            newList.AddRange(bottomPlateLeft);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, annularPlateLeft, GetSumPoint(tankCenterPoint, 0, 0), true));
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, bottomPlateLeft, GetSumPoint(tankCenterPoint, 0, 0), true));

            return newList;
        }

        public List<Entity> GetShellAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> leftList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID= valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;

            Point3D mirrorPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

            // 좌측 하단이 기준


            List<double> shellWidth=drawCommon.GetShellCourseWidthForDrawing();
            List<double> shellThickness = drawCommon.GetShellCourseThickneeForDrawing();

            List<Point3D> outPointList = new List<Point3D>();
            Point3D shellCurrentPoint = GetSumPoint(referencePoint, 0, 0);
            for(int i=0;i<shellWidth.Count;i++)
            {
                double eachWidth = shellWidth[i];
                double eachThickness = shellThickness[i];
                leftList.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(shellCurrentPoint, 0, 0), eachThickness, eachWidth, 0, 0, 2));
                shellCurrentPoint = GetSumPoint(outPointList[1],0,0);
            }

            //Line leftID = new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(refPoint, 0, tankHeight));
            //leftList.Add(leftID);


            newList.AddRange(leftList);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, leftList, GetSumPoint(mirrorPoint, 0, 0), true));

            return newList;
        }

        public List<Entity> GetStructureRafterAssembly(Point3D refPoint,double scaleValue)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> leftList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;

            Point3D mirrorPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

            NColumnRafterModel selRafter = new NColumnRafterModel();
            //leftList.AddRange(drawShareService.Rafter(GetSumPoint(referencePoint, 0, tankHeight), scaleValue,selRa));


            Point3D leftRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);
            Point3D leftRoofUp = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofUp, referencePoint);
            Point3D centerRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopUp, referencePoint);
            Point3D centerRoofUp = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopDown, referencePoint);


            Line plateLowerLine = new Line(GetSumPoint(leftRoofDown, 0, 0), GetSumPoint(centerRoofDown, 0, 0));
            Line plateLeftLine = new Line(GetSumPoint(leftRoofDown, 0, 0), GetSumPoint(leftRoofUp, 0, 0));
            Line plateUpperLine = new Line(GetSumPoint(leftRoofUp, 0, 0), GetSumPoint(centerRoofUp, 0, 0));


            leftList.Add(plateLowerLine);
            leftList.Add(plateLeftLine);
            leftList.Add(plateUpperLine);



            newList.AddRange(leftList);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, leftList, GetSumPoint(mirrorPoint, 0, 0), true));

            return newList;
        }


        public List<Entity> GetClipAssembly(Point3D refPoint,double selWidth, double selHeight, double scaleValue)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();
            // Cetner Lower
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            double width = selWidth;
            double widthHalf = width / 2;
            double height = selHeight;

            Line topLine = new Line(GetSumPoint(referencePoint, -widthHalf, height), GetSumPoint(referencePoint, widthHalf, height));
            Line leftLine = new Line(GetSumPoint(referencePoint, -widthHalf, height), GetSumPoint(referencePoint, -widthHalf, 0));
            Line rightLine = new Line(GetSumPoint(referencePoint, widthHalf, height), GetSumPoint(referencePoint, widthHalf, 0));

            newList.Add(topLine);
            newList.Add(leftLine);
            newList.Add(rightLine);
            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            double exCenterLength = 2;
            centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(topLine.MidPoint, 0, 0),
                                                                GetSumPoint(topLine.MidPoint,0 , -height), exCenterLength, scaleValue));
            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            newList.AddRange(centerLineList);

            return newList;
        }



        // Model Point
        public Point3D GetClipShellSidePoint(Point3D refPoint)
        {
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double shellReduce = GetShellReduceByTopAngle();
            Point3D returnPoint = GetSumPoint(refPoint, shellReduce,tankHeight);
            return returnPoint;
        }
        public Point3D GetRafterShellSidePoint(Point3D refPoint)
        {
            double shellGap = 70;// 차후 값을 받아서 변환 해야 함
            double roofSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            Point3D leftRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, refPoint);
            leftRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.AdjLeftRoofDown,shellGap, refPoint);
            Point3D returnPoint = GetSumPoint(GetClipShellSidePoint(refPoint), shellGap, valueService.GetOppositeByAdjacent(roofSlope, shellGap));
            return returnPoint;
        }



        public DrawEntityModel DrawDetailRoofStructureOrientation(Point3D refPoint, object selModel, double scaleValue, PaperAreaModel selPaperModel)
        {
            // List
            DrawEntityModel drawList = new DrawEntityModel();

            // Reference Point
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            Point3D shellBottomPoint = GetSumPoint(referencePoint, 0, 0);

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            double roofSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);
            double shellLastCourseThk = assemblyComData.ShellLastCourseThk;


            List<Entity> orientationList = new List<Entity>();
            List<Entity> columnList = new List<Entity>();
            List<Entity> girderList = new List<Entity>();
            List<Entity> girderClipList = new List<Entity>();
            List<Entity> rafterList = new List<Entity>();
            List<Entity> shellClipList = new List<Entity>();

            List<Entity> centerLineList = new List<Entity>();


            // Center
            double maxRadius = 0;

            for (int layerIndex = 0; layerIndex < structureCRTColumnModel.LayerList.Count; layerIndex++)
            {
                StructureLayerModel currentLayer = structureCRTColumnModel.LayerList[layerIndex];
                StructureCRTRafterInputModel currentRafterModel = structureCRTColumnModel.strData.newRafterInputList[layerIndex];
                double columnRadius = currentLayer.Radius;

                NColumnGirderModel currentGirder = structureCRTColumnModel.strData.newColumnGirderList[layerIndex];
                NColumnSideTopSupportModel currentSideTopSupport = structureCRTColumnModel.strData.newColumnSideTopSupportList[layerIndex];
                NColumnCenterTopSupportModel currentTopSupport = structureCRTColumnModel.strData.newColumnCenterTopSupportList[layerIndex];
                HBeamModel currentGirderHBeam = structureCRTColumnModel.strData.newGirderHBeamList[layerIndex];
                // Center
                if (layerIndex == 0)
                {

                    // Center Column Support + Center Clip
                    if (structureCRTColumnModel.strData.newColumnCenterTopSupportList.Count > 0)
                    {
                        StructureGirderModel eachGirder = new StructureGirderModel();
                        if (currentLayer.GirderList.Count > 0)
                            eachGirder = currentLayer.GirderList[0];

                        columnList.AddRange(GetColumnCenterSideTopview(referencePoint, currentTopSupport, eachGirder.ClipList));
                    }

                }

                // Side
                else
                {
                    
                    // Side Column Support
                    Point3D columnPoint = GetSumPoint(referencePoint, 0, columnRadius);
                    if (currentLayer.GirderList.Count > 0)
                    {
                        // Column
                        StructureGirderModel firstGirder = currentLayer.GirderList[0];
                        double girderLength = firstGirder.Length;
                        double girderHeight = firstGirder.Height;
                        double girderDegree = 180- firstGirder.AngleOne;
                        double girderRoateDegree = 90- girderDegree/2;
                        double girderWidth = firstGirder.Width;
                        List<Entity> sideTopSupportEntities = drawShareService.ColumnSideTopPlate(GetSumPoint(columnPoint, 0, 0), girderHeight, girderDegree, 3, scaleValue, currentGirder, currentSideTopSupport).GetDrawEntity();



                        // Girder
                        List<Point3D> girderOutputPointList = new List<Point3D>();
                        double girderDistanceFromColumnPoint = valueService.GetDoubleValue(currentSideTopSupport.D);
                        Point3D girderStartPoint = GetSumPoint(columnPoint, girderDistanceFromColumnPoint, girderWidth / 2);
                        List<Entity> girderEntities = GetGirderAssembly(out girderOutputPointList, GetSumPoint(girderStartPoint,0,0),
                                                        girderLength, 0, 0, 0, null, currentGirder, currentSideTopSupport, currentGirderHBeam, scaleValue) ;
                        List<Entity> girderRotateEntities= editingService.GetRotate(girderEntities, columnPoint, Utility.DegToRad(-girderRoateDegree));

                        // Girder : Clip
                        Point3D girderClipStartPoint = GetSumPoint(columnPoint, 0,0);
                        List<Entity> girderClipEntities = GetRafterClipOnGirderAssembly(GetSumPoint(girderClipStartPoint, 0, 0), layerIndex, firstGirder.ClipList, currentTopSupport);
                        List<Entity> girderClipRotateEntities = editingService.GetRotate(girderClipEntities, columnPoint, Utility.DegToRad(-girderRoateDegree));

                        for (int columnIndex = 0; columnIndex < currentLayer.ColumnList.Count; columnIndex++)
                        {
                            double columnRadian = Utility.DegToRad(currentLayer.ColumnList[columnIndex].AngleFromCenter);
                            columnList.AddRange(editingService.GetRotate(sideTopSupportEntities, GetSumPoint(referencePoint, 0, 0), columnRadian));
                            girderList.AddRange(editingService.GetRotate(girderRotateEntities, GetSumPoint(referencePoint, 0, 0), columnRadian));
                            girderClipList.AddRange(editingService.GetRotate(girderClipRotateEntities, GetSumPoint(referencePoint, 0, 0), columnRadian));

                        }


                    }

                    // Rafter : Current -> internal
                    // Channel 만 고려 됨


                    if(layerIndex< structureCRTColumnModel.LayerList.Count )
                    {
                        StructureLayerModel innerLayer = structureCRTColumnModel.LayerList[layerIndex-1];
                        List<Point3D> girderOutputPointList = new List<Point3D>();
                        for (int columnIndex = 0; columnIndex < innerLayer.ColumnList.Count; columnIndex++)
                        {
                            StructureColumnModel eachColumn = innerLayer.ColumnList[columnIndex];
                            double columnStartAngle = eachColumn.AngleFromCenter;
                            double columnEndAngle = columnStartAngle + eachColumn.AngleOne;
                            List<StructureRafterModel> columnRafterList = structureService.GetRafterByAngleRange(currentLayer.RafterList, columnStartAngle, columnEndAngle);


                            // Clip
                            Point3D clipShellSidePoint = GetSumPoint(referencePoint, 0, columnRadius);
                            NRafterSupportClipShellSideModel rafterClipShellSide = GetNewRafterSupportClipShellSideModel(currentRafterModel.Size);
                            List<Entity> shellClipLeftEntities=drawShareService.RafterSideClipDetail(clipShellSidePoint, clipShellSidePoint, singleModel, scaleValue, rafterClipShellSide, shellLastCourseThk, 2).GetDrawEntity();
                            List<Entity> shellClipLeftRotateEntities = editingService.GetRotate(shellClipLeftEntities, clipShellSidePoint, Utility.DegToRad(-90));
                            List<Entity> shellClipRightEntities = drawShareService.RafterSideClipDetail(clipShellSidePoint, clipShellSidePoint, singleModel, scaleValue, rafterClipShellSide, shellLastCourseThk, 2,false).GetDrawEntity();
                            List<Entity> shellClipRightRotateEntities = editingService.GetRotate(shellClipRightEntities, clipShellSidePoint, Utility.DegToRad(-90));

                            int startRafterIndex = columnRafterList.Count-1;
                            for (int rafterIndex = 0; rafterIndex < columnRafterList.Count; rafterIndex++)
                            {
                                StructureRafterModel eachRafter = columnRafterList[rafterIndex];

                                // Rafter Position
                                bool leftLine = true;
                                if (layerIndex != 1)
                                    if (rafterIndex == startRafterIndex )
                                        leftLine = false;

                                double rafterPosition = 3;
                                if (!leftLine)
                                    rafterPosition = 0;
                                
                                // Rafter
                                NColumnRafterModel nColumnRafter = GetRafterModel(eachRafter.Size);
                                ChannelModel eachChannelModel = GetChannelModel(eachRafter.Size);
                                Point3D rafterInnertStartPoint = GetSumPoint(referencePoint, 0, eachRafter.InnerTopViewRadius);
                                List<Entity> tempRafterList = GetChannelTopAssembly(out girderOutputPointList,
                                                                            rafterInnertStartPoint,
                                                                            eachRafter.LengthTopView, Utility.DegToRad(90), rafterPosition, rafterPosition, null,
                                                                            nColumnRafter, eachChannelModel, leftLine,scaleValue);

                                double rafterRadian = Utility.DegToRad(eachRafter.AngleFromCenter);
                                rafterList.AddRange(editingService.GetRotate(tempRafterList, GetSumPoint(referencePoint, 0, 0), rafterRadian));


                                // Shell Clip : Top
                                if(layerIndex== structureCRTColumnModel.LayerList.Count - 1)
                                {
                                    if (leftLine)
                                        shellClipList.AddRange(editingService.GetRotate(shellClipLeftRotateEntities, GetSumPoint(referencePoint, 0, 0), rafterRadian));
                                    else
                                        shellClipList.AddRange(editingService.GetRotate(shellClipRightRotateEntities, GetSumPoint(referencePoint, 0, 0), rafterRadian));

                                }

                            }

                        }
                        

                      
                        //foreach (StructureRafterModel eachRafter in currentLayer.RafterList)
                        //{
                        //    NColumnRafterModel nColumnRafter = GetRafterModel(eachRafter.Size);
                        //    ChannelModel eachChannelModel = GetChannelModel(eachRafter.Size);

                        //    Point3D rafterInnertStartPoint = GetSumPoint(referencePoint, 0, eachRafter.InnerRadiusFromCenter);
                        //    List<Entity> tempRafterList=GetChannelTopAssembly(out girderOutputPointList,
                        //        rafterInnertStartPoint,
                        //        eachRafter.Length, Utility.DegToRad(90), rafterPosition, rafterPosition, null,nColumnRafter, eachChannelModel, scaleValue);


                        //    double rafterRadian = Utility.DegToRad(eachRafter.AngleFromCenter);    
                        //    rafterList.AddRange(editingService.GetRotate(tempRafterList, GetSumPoint(referencePoint, 0, 0), rafterRadian));     
                        //}

                    }


                }


                // Center Circle Line
                if (layerIndex > 0)
                {
                    Circle centerCircle = new Circle(GetSumPoint(referencePoint, 0, 0), columnRadius);
                    if(layerIndex== structureCRTColumnModel.LayerList.Count - 1) 
                        styleService.SetLayer(ref centerCircle, layerService.LayerVirtualLine);
                    else
                        styleService.SetLayer(ref centerCircle, layerService.LayerCenterLine);
                    centerLineList.Add(centerCircle);

                    if (currentLayer.ColumnList.Count > 0)
                    {
                        Point3D startColumnPoint = null;
                        Point3D beforeColumnPoint = null;
                        Point3D currentColumnPoint = null;
                        foreach(StructureColumnModel eachColumn in currentLayer.ColumnList)
                        {
                            Line eachColumnLine =new Line( GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, 0, columnRadius));
                            double columnStartAngle = Utility.DegToRad(eachColumn.AngleFromCenter);
                            eachColumnLine.Rotate(columnStartAngle, Vector3D.AxisZ, GetSumPoint(referencePoint, 0, 0));

                            if (startColumnPoint == null)
                            {
                                startColumnPoint = GetSumPoint(eachColumnLine.EndPoint, 0, 0);
                                beforeColumnPoint = GetSumPoint(startColumnPoint, 0, 0);
                            }
                            else
                            {
                                currentColumnPoint= GetSumPoint(eachColumnLine.EndPoint, 0, 0);
                                Line columnCenterLine = new Line(GetSumPoint(beforeColumnPoint, 0, 0), GetSumPoint(currentColumnPoint, 0, 0));
                                styleService.SetLayer(ref columnCenterLine, layerService.LayerCenterLine);
                                centerLineList.Add(columnCenterLine);

                                beforeColumnPoint = GetSumPoint(currentColumnPoint, 0, 0);

                            }
                        }
                        if (startColumnPoint != null)
                        {
                            Line columnCenterLine = new Line(GetSumPoint(startColumnPoint, 0, 0), GetSumPoint(currentColumnPoint, 0, 0));
                            styleService.SetLayer(ref columnCenterLine, layerService.LayerCenterLine);
                            centerLineList.Add(columnCenterLine);
                        }
                    }
                }


                // maxRadius
                if (maxRadius < columnRadius)
                    maxRadius = columnRadius;
            }



            // Center Line
            double extMainLength = 4;
            List<Entity> centerVerticalLine = editingService.GetCenterLine(GetSumPoint(referencePoint, 0, maxRadius), GetSumPoint(referencePoint, 0, -maxRadius), extMainLength, scaleValue);
            List<Entity> centerHorizontalLine = editingService.GetCenterLine(GetSumPoint(referencePoint, -maxRadius, 0), GetSumPoint(referencePoint, maxRadius, 0), extMainLength, scaleValue);

            styleService.SetLayerListEntity(ref centerVerticalLine, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref centerHorizontalLine, layerService.LayerCenterLine);
            centerLineList.AddRange(centerVerticalLine);
            centerLineList.AddRange(centerHorizontalLine);







            //styleService.SetLayerListEntity(ref orientationList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(columnList);
            drawList.outlineList.AddRange(girderList);
            drawList.outlineList.AddRange(girderClipList);
            drawList.outlineList.AddRange(rafterList);
            drawList.outlineList.AddRange(shellClipList);

            drawList.outlineList.AddRange(centerLineList);

            return drawList;



        }

        public List<Entity> GetColumnCenterSideTopview(Point3D refPoint, NColumnCenterTopSupportModel centerModel,List<StructureClipModel> clipList)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> newClipList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            double B = valueService.GetDoubleValue(centerModel.B);
            double C = valueService.GetDoubleValue(centerModel.C);
            double G = valueService.GetDoubleValue(centerModel.G);
            double A1 = valueService.GetDoubleValue(centerModel.A1);
            double G1 = valueService.GetDoubleValue(centerModel.G1);

            PipeModel pipeModel = GetPipeModel(centerModel.ColumnSize);
            double pipeRadius = valueService.GetDoubleValue(pipeModel.OD) / 2;

            double centerSupportRadius = pipeRadius + G + A1;
            double clipRadius = B;
            double clipWidth = C;
            double clipThk = G1;



            Circle pipeCir = new Circle(GetSumPoint(referencePoint, 0, 0), pipeRadius);
            Circle clipCir = new Circle(GetSumPoint(referencePoint, 0, 0), clipRadius);
            Circle supportCir = new Circle(GetSumPoint(referencePoint, 0, 0), centerSupportRadius);

            List<Point3D> outPointList = new List<Point3D>();
            double clipShapeRadius = clipRadius + clipWidth / 2;
            List<Entity> clipEntities = shapeService.GetRectangle(out outPointList, GetSumPoint(referencePoint, 0, clipShapeRadius), clipThk, clipWidth, 0, 0, 0);

            foreach (StructureClipModel eachClip in clipList)
            {
                double eachRadian = Utility.DegToRad( eachClip.AngleFromCenter);
                newClipList.AddRange(editingService.GetRotate(clipEntities, GetSumPoint(referencePoint, 0, 0), eachRadian));

            }




            styleService.SetLayer(ref pipeCir, layerService.LayerOutLine);
            styleService.SetLayer(ref clipCir, layerService.LayerCenterLine);
            styleService.SetLayer(ref supportCir, layerService.LayerOutLine);
            newList.Add(pipeCir);
            newList.Add(clipCir);
            newList.Add(supportCir);

            styleService.SetLayerListEntity(ref newClipList, layerService.LayerOutLine);
            newList.AddRange(newClipList);

            return newList;
        }
        



        public List<Entity> GetRafterAssembly(out List<Point3D> selOutputPointList, Point3D refPoint, double selLength, double selRotate, double selRotateCenter, double selTranslateNumber, bool[] selVisibleLine = null,
                            NColumnRafterModel rafterModel = null, double leftHoleQty = 0, double rightHoleQty = 0, double padThk = 0, bool rightCenterPosition = false, double scaleValue=1)
        {
            selOutputPointList = new List<Point3D>();
            List<Entity> newList = new List<Entity>();
            List<Entity> slotHoleList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();

            // Model Data
            double A = valueService.GetDoubleValue(rafterModel.A);    //rafter Lenth
            double A1 = valueService.GetDoubleValue(rafterModel.A1);   //woking Point to rafter : NOT USED
            double B = valueService.GetDoubleValue(rafterModel.B);    //rafter Width
            double B1 = valueService.GetDoubleValue(rafterModel.B1);   //workingPoint부터 rafter까지의 x축 거리
            double C = valueService.GetDoubleValue(rafterModel.C);    //centering Point to rafter : NOT USED  //rafter to hole : NOT USED
            double C1 = valueService.GetDoubleValue(rafterModel.C1);   //hole to hole length gap
            double D = valueService.GetDoubleValue(rafterModel.D);    //hole to hole width gap
            double E = valueService.GetDoubleValue(rafterModel.E);    //SHELL ID/4 : NOT USED
            double F = valueService.GetDoubleValue(rafterModel.F);    
            double G = valueService.GetDoubleValue(rafterModel.G);    
            double holeDia = valueService.GetDoubleValue(rafterModel.BoltHoleDia);

            double selHeight = A;

            double t = 6;   //Thk of doubleLine

            // Reference Point : Top Left
            Point3D pointA = GetSumPoint(refPoint, 0, 0);
            Point3D pointB = GetSumPoint(refPoint, selLength, 0);
            Point3D pointC = GetSumPoint(refPoint, selLength, -selHeight);
            Point3D pointD = GetSumPoint(refPoint, 0, -selHeight);

            // Line
            Line lineA = new Line(GetSumPoint(pointA, 0, 0), GetSumPoint(pointB, 0, 0));
            Line lineB = new Line(GetSumPoint(pointB, 0, 0), GetSumPoint(pointC, 0, 0));
            Line lineC = new Line(GetSumPoint(pointC, 0, 0), GetSumPoint(pointD, 0, 0));
            Line lineD = new Line(GetSumPoint(pointD, 0, 0), GetSumPoint(pointA, 0, 0));

            // Inner Line : HBeam,Channel Tpye에 따라서 달라짐
            Line lineAinner = (Line)lineA.Offset(t, Vector3D.AxisZ);
            Line lineCinner = (Line)lineC.Offset(t, Vector3D.AxisZ);

            newList.AddRange(new Line[] { lineA, lineB, lineC, lineD, lineAinner, lineCinner });
            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // CenterLine
            double exCenterLength = 2;
            centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(lineB.MidPoint, 0, 0),
                                                                GetSumPoint(lineD.MidPoint, 0, 0), exCenterLength, scaleValue));
            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            newList.AddRange(centerLineList);

            // Slot Hole : Center Line 처리 필요함
            // Slot Hole : 
            double holeCenterX = B1 + C1 / 2;
            double holeCenterLeftY = 0;
            double holeCenterRightY = 0;
            Point3D leftHoleCenterPoint = GetSumPoint(pointA, 0, 0);
            Point3D rightHoleCenterPoint = GetSumPoint(pointB, 0, 0);
            if (rafterModel.Type.ToLower().Contains("top"))
            {
                if (leftHoleQty == 2)
                    holeCenterLeftY = B;
                else if (leftHoleQty == 4)
                    holeCenterLeftY = D + E / 2;

                if (rightHoleQty == 2)
                    holeCenterRightY = B;
                else if (rightHoleQty == 4)
                    holeCenterRightY = D + E / 2;

                leftHoleCenterPoint = GetSumPoint(pointA, +holeCenterX, -holeCenterLeftY); ;
                rightHoleCenterPoint = GetSumPoint(pointB, -holeCenterX, -holeCenterRightY);

            }

            if (rafterModel.Type.ToLower().Contains("com"))
            {
                if (leftHoleQty == 2)
                    leftHoleCenterPoint = GetSumPoint(pointA, +holeCenterX, D);
                else if (leftHoleQty == 4)
                    leftHoleCenterPoint = GetSumPoint(pointA, +holeCenterX, -D - E / 2);
            }

            double leftHoleDistanceX = C1;
            double leftHoleDistanceY = E;
            double rightHoleDistanceX = C1;
            double rightHoleDistanceY = E;

            // 오른쪽이 Center 이면
            if (rightCenterPosition)
                if (rightCenterPosition)
                    if (rightHoleQty == 4)
                    {
                        rightHoleDistanceY = G;
                        rightHoleCenterPoint = GetSumPoint(pointB, -holeCenterX, -F - G / 2);
                    }


            // Left Hole
            slotHoleList.AddRange(GetHolesNew(GetSumPoint(leftHoleCenterPoint, 0, 0), leftHoleQty, holeDia, scaleValue, leftHoleDistanceX, leftHoleDistanceY));
            // Right Hole
            slotHoleList.AddRange(GetHolesNew(GetSumPoint(rightHoleCenterPoint, 0, 0), rightHoleQty, holeDia, scaleValue, rightHoleDistanceX, rightHoleDistanceY));

            newList.AddRange(slotHoleList);

            if (selRotate != 0)
            {
                Point3D WPRotate = GetSumPoint(pointA, 0, 0);
                if (selRotateCenter == 1)
                    WPRotate = GetSumPoint(pointB, 0, 0);
                else if (selRotateCenter == 2)
                    WPRotate = GetSumPoint(pointC, 0, 0);
                else if (selRotateCenter == 3)
                    WPRotate = GetSumPoint(pointD, 0, 0);

                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // 나중에 맨 앞으로 옮겨야 함
            if (selTranslateNumber > 0)
            {
                Point3D WPTranslate = new Point3D();
                if (selTranslateNumber == 1)
                    WPTranslate = GetSumPoint(pointB, 0, 0);
                else if (selTranslateNumber == 2)
                    WPTranslate = GetSumPoint(pointC, 0, 0);
                else if (selTranslateNumber == 3)
                    WPTranslate = GetSumPoint(pointD, 0, 0);
                editingService.SetTranslate(ref newList, GetSumPoint(refPoint, 0, 0), WPTranslate);

            }
            if (selVisibleLine != null)
            {
                if (selVisibleLine[0] == false)
                    newList.Remove(lineA);
                if (selVisibleLine[1] == false)
                    newList.Remove(lineB);
                if (selVisibleLine[2] == false)
                    newList.Remove(lineC);
                if (selVisibleLine[3] == false)
                    newList.Remove(lineD);
            }

            selOutputPointList.Add(lineA.StartPoint);
            selOutputPointList.Add(lineA.EndPoint);
            selOutputPointList.Add(lineC.StartPoint);
            selOutputPointList.Add(lineC.EndPoint);



            return newList;
        }

        public List<Entity> GetGirderAssembly(out List<Point3D> selOutputPointList, Point3D refPoint, double selLength, double selRotate, double selRotateCenter, double selTranslateNumber, bool[] selVisibleLine = null,
                            NColumnGirderModel padModel = null,NColumnSideTopSupportModel padModelTop=null,HBeamModel Hbeam=null, double scaleValue = 1)
        {
            selOutputPointList = new List<Point3D>();
            List<Entity> newList = new List<Entity>();
            List<Entity> slotHoleList = new List<Entity>();

            // Model Data
            double A = valueService.GetDoubleValue(padModel.A);    //rafter Lenth
            //double A1 = valueService.GetDoubleValue(padModel.A1);   //woking Point to rafter : NOT USED
            //double B = valueService.GetDoubleValue(padModel.B);    //rafter Width
            //double B1 = valueService.GetDoubleValue(padModel.B1);   //workingPoint부터 rafter까지의 x축 거리
            //double C = valueService.GetDoubleValue(padModel.C);    //centering Point to rafter : NOT USED  //rafter to hole : NOT USED
            //double C1 = valueService.GetDoubleValue(padModel.C1);   //hole to hole length gap
            //double D = valueService.GetDoubleValue(padModel.D);    //hole to hole width gap
            //double E = valueService.GetDoubleValue(padModel.E);    //SHELL ID/4 : NOT USED
            //double holeDia = valueService.GetDoubleValue(padModel.BoltHoleDia);


            double Hbeamt1 = valueService.GetDoubleValue(Hbeam.t1);    //rafter Lenth
            double selHeight = A;
            
            double t = selHeight/2- (Hbeamt1/2);   //Thk of doubleLine

            // Reference Point : Top Left
            Point3D pointA = GetSumPoint(refPoint, 0, 0);
            Point3D pointB = GetSumPoint(refPoint, selLength, 0);
            Point3D pointC = GetSumPoint(refPoint, selLength, -selHeight);
            Point3D pointD = GetSumPoint(refPoint, 0, -selHeight);

            // Line
            Line lineA = new Line(GetSumPoint(pointA, 0, 0), GetSumPoint(pointB, 0, 0));
            Line lineB = new Line(GetSumPoint(pointB, 0, 0), GetSumPoint(pointC, 0, 0));
            Line lineC = new Line(GetSumPoint(pointC, 0, 0), GetSumPoint(pointD, 0, 0));
            Line lineD = new Line(GetSumPoint(pointD, 0, 0), GetSumPoint(pointA, 0, 0));

            // Inner Line : HBeam,Channel Tpye에 따라서 달라짐
            Line lineAinner = (Line)lineA.Offset(t, Vector3D.AxisZ);
            Line lineCinner = (Line)lineC.Offset(t, Vector3D.AxisZ);
            styleService.SetLayer(ref lineAinner, layerService.LayerHiddenLine);
            styleService.SetLayer(ref lineCinner, layerService.LayerHiddenLine);

            newList.AddRange(new Line[] { lineA, lineB, lineC, lineD });
            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
            newList.AddRange(new Line[] { lineAinner, lineCinner });



            // Slot Hole : Center Line 처리 필요함
            double holeQty = 2;
            // hole Plan

            //double holePositionX = B1 + C1 / 2;

            //slotHoleList.AddRange(GetHolesQty(GetSumPoint(lineD.MidPoint, holePositionX, 0), holeDia, C1, holeQty, D));
            //slotHoleList.AddRange(GetHolesQty(GetSumPoint(lineB.MidPoint, -holePositionX, 0), holeDia, C1, holeQty, D));
            newList.AddRange(slotHoleList);

            if (selRotate != 0)
            {
                Point3D WPRotate = GetSumPoint(pointA, 0, 0);
                if (selRotateCenter == 1)
                    WPRotate = GetSumPoint(pointB, 0, 0);
                else if (selRotateCenter == 2)
                    WPRotate = GetSumPoint(pointC, 0, 0);
                else if (selRotateCenter == 3)
                    WPRotate = GetSumPoint(pointD, 0, 0);

                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // 나중에 맨 앞으로 옮겨야 함
            if (selTranslateNumber > 0)
            {
                Point3D WPTranslate = new Point3D();
                if (selTranslateNumber == 1)
                    WPTranslate = GetSumPoint(pointB, 0, 0);
                else if (selTranslateNumber == 2)
                    WPTranslate = GetSumPoint(pointC, 0, 0);
                else if (selTranslateNumber == 3)
                    WPTranslate = GetSumPoint(pointD, 0, 0);
                editingService.SetTranslate(ref newList, GetSumPoint(refPoint, 0, 0), WPTranslate);

            }
            if (selVisibleLine != null)
            {
                if (selVisibleLine[0] == false)
                    newList.Remove(lineA);
                if (selVisibleLine[1] == false)
                    newList.Remove(lineB);
                if (selVisibleLine[2] == false)
                    newList.Remove(lineC);
                if (selVisibleLine[3] == false)
                    newList.Remove(lineD);
            }

            selOutputPointList.Add(lineA.StartPoint);
            selOutputPointList.Add(lineA.EndPoint);
            selOutputPointList.Add(lineC.StartPoint);
            selOutputPointList.Add(lineC.EndPoint);

            return newList;
        }


        public List<Entity> GetChannelTopAssembly(out List<Point3D> selOutputPointList, Point3D refPoint, double selLength, double selRotate, double selRotateCenter, double selTranslateNumber, bool[] selVisibleLine = null,
                            NColumnRafterModel padModel = null,ChannelModel selChannel=null,bool leftLine=true, double scaleValue = 1)
        {
            selOutputPointList = new List<Point3D>();
            List<Entity> newList = new List<Entity>();
            List<Entity> slotHoleList = new List<Entity>();


            // Model Data
            double A = valueService.GetDoubleValue(padModel.A);    //rafter Lenth
            double A1 = valueService.GetDoubleValue(padModel.A1);   //woking Point to rafter : NOT USED
            double B = valueService.GetDoubleValue(padModel.B);    //rafter Width
            double B1 = valueService.GetDoubleValue(padModel.B1);   //workingPoint부터 rafter까지의 x축 거리
            double C = valueService.GetDoubleValue(padModel.C);    //centering Point to rafter : NOT USED  //rafter to hole : NOT USED
            double C1 = valueService.GetDoubleValue(padModel.C1);   //hole to hole length gap
            double D = valueService.GetDoubleValue(padModel.D);    //hole to hole width gap
            double E = valueService.GetDoubleValue(padModel.E);    //SHELL ID/4 : NOT USED
            double holeDia = valueService.GetDoubleValue(padModel.BoltHoleDia);

            double channelB = valueService.GetDoubleValue(selChannel.B);
            double channelt1 = valueService.GetDoubleValue(selChannel.t1);

            // Top View
            double selHeight = channelB;

            // 위쪽 기준
            double t = channelt1;   //Thk of doubleLine

            // Reference Point : Top Left
            Point3D pointA = GetSumPoint(refPoint, 0, 0);
            Point3D pointB = GetSumPoint(refPoint, selLength, 0);
            Point3D pointC = GetSumPoint(refPoint, selLength, -selHeight);
            Point3D pointD = GetSumPoint(refPoint, 0, -selHeight);

            // Line
            Line lineA = new Line(GetSumPoint(pointA, 0, 0), GetSumPoint(pointB, 0, 0));
            Line lineB = new Line(GetSumPoint(pointB, 0, 0), GetSumPoint(pointC, 0, 0));
            Line lineC = new Line(GetSumPoint(pointC, 0, 0), GetSumPoint(pointD, 0, 0));
            Line lineD = new Line(GetSumPoint(pointD, 0, 0), GetSumPoint(pointA, 0, 0));

            // Inner Line : HBeam,Channel Tpye에 따라서 달라짐
            Line lineAinner = (Line)lineA.Offset(t, Vector3D.AxisZ);
            Line lineCinner = (Line)lineC.Offset(t, Vector3D.AxisZ);
            styleService.SetLayer(ref lineAinner, layerService.LayerHiddenLine);
            styleService.SetLayer(ref lineCinner, layerService.LayerHiddenLine);

            newList.AddRange(new Line[] { lineA, lineB, lineC, lineD });
            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            if (leftLine)
            {
                newList.AddRange(new Line[] { lineCinner });
            }
            else
            {
                newList.AddRange(new Line[] { lineAinner });
            }
            // Slot Hole : Center Line 처리 필요함
            double holeQty = 2;
            // hole Plan

            double holePositionX = B1 + C1 / 2;

            //slotHoleList.AddRange(GetHolesQty(GetSumPoint(lineD.MidPoint, holePositionX, 0), holeDia, C1, holeQty, D));
            //slotHoleList.AddRange(GetHolesQty(GetSumPoint(lineB.MidPoint, -holePositionX, 0), holeDia, C1, holeQty, D));
            newList.AddRange(slotHoleList);

            if (selRotate != 0)
            {
                Point3D WPRotate = GetSumPoint(pointA, 0, 0);
                if (selRotateCenter == 1)
                    WPRotate = GetSumPoint(pointB, 0, 0);
                else if (selRotateCenter == 2)
                    WPRotate = GetSumPoint(pointC, 0, 0);
                else if (selRotateCenter == 3)
                    WPRotate = GetSumPoint(pointD, 0, 0);

                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // 나중에 맨 앞으로 옮겨야 함
            if (selTranslateNumber > 0)
            {
                Point3D WPTranslate = new Point3D();
                if (selTranslateNumber == 1)
                    WPTranslate = GetSumPoint(pointB, 0, 0);
                else if (selTranslateNumber == 2)
                    WPTranslate = GetSumPoint(pointC, 0, 0);
                else if (selTranslateNumber == 3)
                    WPTranslate = GetSumPoint(pointD, 0, 0);
                editingService.SetTranslate(ref newList, GetSumPoint(refPoint, 0, 0), WPTranslate);

            }
            if (selVisibleLine != null)
            {
                if (selVisibleLine[0] == false)
                    newList.Remove(lineA);
                if (selVisibleLine[1] == false)
                    newList.Remove(lineB);
                if (selVisibleLine[2] == false)
                    newList.Remove(lineC);
                if (selVisibleLine[3] == false)
                    newList.Remove(lineD);
            }

            selOutputPointList.Add(lineA.StartPoint);
            selOutputPointList.Add(lineA.EndPoint);
            selOutputPointList.Add(lineC.StartPoint);
            selOutputPointList.Add(lineC.EndPoint);

            return newList;
        }


        public List<Entity> GetRafterClipOnGirderAssembly(Point3D refPoint,double layerIndex, List<StructureClipModel> clipList, NColumnCenterTopSupportModel centerSupportModel)
        {
            List<Entity> newList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double C = valueService.GetDoubleValue(centerSupportModel.C);
            double G1 = valueService.GetDoubleValue(centerSupportModel.G1);

            double girderWidth = C;
            double girderThk = G1;

            bool clipLeft = true;
            
            List<Point3D> outPointList = new List<Point3D>();
            for(int clipIndex = 0; clipIndex < clipList.Count; clipIndex++) 
            {
                // 첫번째 두번재 Right로
                if (clipIndex == 0 || clipIndex == 1)
                    clipLeft = false;
                else
                    clipLeft = true;

                // 1 번째 Layer 에서는 첫번째 변환
                if (layerIndex == 1 && clipIndex==1)
                    clipLeft = true;

                double lengthAdj = 0;
                if (!clipLeft)
                    lengthAdj = -girderThk;

                

                StructureClipModel eachClip = clipList[clipIndex];
                Point3D eachClipPoint = GetSumPoint(referencePoint, eachClip.PointLengthFormColumn + lengthAdj, girderWidth/2);
                Point3D eachClipPointCenter = GetSumPoint(referencePoint, eachClip.PointLengthFormColumn , 0);
                List<Entity> clipEntities = shapeService.GetRectangle(out outPointList, GetSumPoint(eachClipPoint, 0, 0), girderThk, girderWidth, 0, 0, 0);
                List<Entity> clipRotateEntities = editingService.GetRotate(clipEntities, GetSumPoint(eachClipPointCenter, 0, 0), Utility.DegToRad(eachClip.GirderClipAngle));
                newList.AddRange(clipRotateEntities);
            }

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            return newList;
        }



        public List<Entity> GetGirderFrontAssembly(Point3D refPoint, HBeamModel selHBeam, double scaleValue)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> dimList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            newList.AddRange(refBlockService.DrawReference_HBeam(GetSumCDPoint(referencePoint, 0, 0), selHBeam));

            double columnSideGirderHeight = valueService.GetDoubleValue(selHBeam.A);
            double columnSideGirderWidth = valueService.GetDoubleValue(selHBeam.B);
            double columnSideGirderT2 = valueService.GetDoubleValue(selHBeam.t2);
            double girderExtLength = 800;

            Point3D girderSideViewPoint = GetSumPoint(referencePoint, columnSideGirderWidth / 2, 0);
            Line upperLine = new Line(GetSumPoint(girderSideViewPoint, 0, 0), GetSumPoint(girderSideViewPoint, girderExtLength, 0));
            Line lowerLine = new Line(GetSumPoint(girderSideViewPoint, 0, -columnSideGirderHeight), GetSumPoint(girderSideViewPoint, girderExtLength, -columnSideGirderHeight));
            Line upperInnerLine = (Line)upperLine.Offset(columnSideGirderT2, Vector3D.AxisZ);
            Line lowerInnerLine = (Line)lowerLine.Offset(-columnSideGirderT2, Vector3D.AxisZ);

            newList.Add(upperLine);
            newList.Add(lowerLine);

            newList.Add(upperInnerLine);
            newList.Add(lowerInnerLine);


            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            double tempHeight = 4;
            List<Entity> breakAllList = new List<Entity>();
            dimList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(girderSideViewPoint, girderExtLength, 0),
                                                           GetSumPoint(girderSideViewPoint, girderExtLength, -columnSideGirderHeight),
                                                            scaleValue, true, true));

            styleService.SetLayerListEntity(ref dimList, layerService.LayerDimension);

            newList.AddRange(dimList);
            return newList;
        }

        private List<Entity> GetHolesNew(Point3D refPoint, double holeQty, double holeDiameter, double scaleValue, double xGap = 0, double yGap = 0)
        {
            //refPoint는 홀과 홀 사이의 중앙인 점을 입력해주시면됩니다
            //lengthGap=0인경우는 2개, lengthGap에 값이 입력된 경우는 홀이 4개로 그려집니다 :)
            List<Entity> newList = new List<Entity>();
            List<Entity> holeList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();

            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);
            double exCenterLength = 2;

            double holeRadius = holeDiameter / 2;
            double xGapHalf = xGap / 2;
            double yGapHalf = yGap / 2;


            // Hole
            if (holeQty == 1)
            {
                holeList.Add(new Circle(GetSumPoint(workingPoint, 0, 0), holeRadius));
            }
            else if (holeQty == 2)
            {
                holeList.Add(new Circle(GetSumPoint(workingPoint, -xGapHalf, 0), holeRadius));
                holeList.Add(new Circle(GetSumPoint(workingPoint, xGapHalf, 0), holeRadius));
            }
            else if (holeQty == 4)
            {
                holeList.Add(new Circle(GetSumPoint(workingPoint, -xGapHalf, yGapHalf), holeRadius));
                holeList.Add(new Circle(GetSumPoint(workingPoint, xGapHalf, yGapHalf), holeRadius));
                holeList.Add(new Circle(GetSumPoint(workingPoint, -xGapHalf, -yGapHalf), holeRadius));
                holeList.Add(new Circle(GetSumPoint(workingPoint, xGapHalf, -yGapHalf), holeRadius));
            }

            // Center Line
            foreach (Circle eachCircle in holeList)
            {
                centerLineList.AddRange(
                    editingService.GetCenterLine(
                        GetSumPoint(eachCircle.Center, -holeRadius, 0),
                        GetSumPoint(eachCircle.Center, holeRadius, 0), exCenterLength, scaleValue));

                centerLineList.AddRange(
                    editingService.GetCenterLine(
                        GetSumPoint(eachCircle.Center, 0, -holeRadius),
                        GetSumPoint(eachCircle.Center, 0, holeRadius), exCenterLength, scaleValue));

            }

            styleService.SetLayerListEntity(ref holeList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);

            newList.AddRange(holeList);
            newList.AddRange(centerLineList);

            return newList;


        }



        #region Assembly Model : Structure
        private NColumnRafterModel GetRafterModel(string rafterSize)
    {
        NColumnRafterModel newModel = new NColumnRafterModel();
        foreach (NColumnRafterModel eachModel in assemblyData.NColumnRafterList)
        {
            if (eachModel.Size == rafterSize)
            {
                newModel = eachModel;
                break;
            }
        }
        return newModel;
    }

        private NColumnCenterTopSupportModel GetNewColumnCenterTopSupportModel(string selSize,string selInch)
        {
            NColumnCenterTopSupportModel newModel = new NColumnCenterTopSupportModel();
            foreach (NColumnCenterTopSupportModel eachModel in assemblyData.NColumnCenterTopSupportList)
            {
                if(eachModel.ColumnSize== selInch)
                    if (eachModel.RafterSize == selSize)
                    {
                        newModel = eachModel;
                        break;
                    }
            }
            return newModel;
        }

        private NColumnSideTopSupportModel GetNewColumnSideTopSupportModel(string selSize)
        {
            NColumnSideTopSupportModel newModel = new NColumnSideTopSupportModel();
            foreach (NColumnSideTopSupportModel eachModel in assemblyData.NColumnSideTopSupportList)
            {
                if (eachModel.ColumnSize == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }


        private NRafterSupportClipShellSideModel GetNewRafterSupportClipShellSideModel(string selSize)
        {
            NRafterSupportClipShellSideModel newModel = new NRafterSupportClipShellSideModel();
            foreach (NRafterSupportClipShellSideModel eachModel in assemblyData.NRafterSupportClipShellSideList)
            {
                if (eachModel.RafterSize == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }
        private NColumnGirderModel GetNewColumnGirderModel(string selSize)
        {
            NColumnGirderModel newModel = new NColumnGirderModel();
            foreach (NColumnGirderModel eachModel in assemblyData.NColumnGirderList)
            {
                if (eachModel.ColumnSize == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }

        private HBeamModel GetHBeamModel(string hBeamSize)
        {
            HBeamModel newModel = new HBeamModel();
            foreach (HBeamModel eachModel in assemblyData.HBeamList)
            {
                if (eachModel.SIZE == hBeamSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }

        private PipeModel GetPipeModel(string selSize)
        {
            PipeModel newModel = new PipeModel();
            foreach (PipeModel eachModel in assemblyData.PipeList)
            {
                if (eachModel.NPS == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }

        private NColumnBaseSupportModel GetColumnBaseSupportModel(string selSize)
        {
            NColumnBaseSupportModel newModel = new NColumnBaseSupportModel();
            foreach (NColumnBaseSupportModel eachModel in assemblyData.NColumnBaseSupportList)
            {
                if (eachModel.ColumnSize == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }

        private ChannelModel GetChannelModel(string selSize)
        {
            ChannelModel newModel = new ChannelModel();
            foreach (ChannelModel eachModel in assemblyData.ChannelList)
            {
                if (eachModel.SIZE == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }

        private AngleSizeModel GetAngleModel(string selSize)
        {
            AngleSizeModel newModel = new AngleSizeModel();
            foreach (AngleSizeModel eachModel in assemblyData.AngleIList)
            {
                if (eachModel.SIZE == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }


        #endregion

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private CDPoint GetSumCDPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }





        #region Lee

        #endregion



    }
}
