﻿using AssemblyLib.AssemblyModels;
using DrawCalculationLib.FunctionServices;
using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingServices
{
    public class StructureService
    {
        public GeometryService geoService { get; set; }

        public StructureDataInputModel strCRTColumnData { get; set; }
        public StructureDataInputModel strCRTCenteringData { get; set; }

        public StructureService()
        {
            geoService = new GeometryService();

            strCRTColumnData = new StructureDataInputModel();
            strCRTCenteringData = new StructureDataInputModel();
        }


        public StructureCRTColumnModel CreateStructureCRTColumn(
                                                List<StructureCRTRafterInputModel> rafterInputList,
                                                 List<StructureCRTColumnInputModel> columnInputList,
                                                 List<StructureCRTGirderInputModel> girderInputList,


                                                 List<HBeamModel> girderHbeamList,
                                                 List<NColumnCenterTopSupportModel> columnCenterTopSupportList,
                                                 List<NColumnSideTopSupportModel> columnSideTopSupportList,
                                                 List<NColumnGirderModel> columnGirderList,
                                                 List<NColumnBaseSupportModel> columnBaseSupportList,
                                                 List<PipeModel> columnPipeList,


                                                 List<NColumnRafterModel> rafterOutputList,

                                                 double selTankID, double selTankHeight, double selAnnularInnerWidth, double selRoofOD, double selBottomThk,
                                                 double selRoofSlope, double selBottoSlope,
                                                 double selShellReduce = 0
                                                 )
        {

            // Layer 에 맞게끔 수정 작업

            // Input
            strCRTColumnData = new StructureDataInputModel();
            strCRTColumnData.newRafterInputList.Add(new StructureCRTRafterInputModel());
            strCRTColumnData.newRafterInputList.AddRange(rafterInputList);


            strCRTColumnData.newColumnInputList.AddRange(columnInputList);
            strCRTColumnData.newColumnInputList.Add(new StructureCRTColumnInputModel());


            strCRTColumnData.newGirderInputList.Add(new StructureCRTGirderInputModel());
            strCRTColumnData.newGirderInputList.AddRange(girderInputList);
            strCRTColumnData.newGirderInputList.Add(new StructureCRTGirderInputModel());

            strCRTColumnData.newGirderHBeamList.Add(new HBeamModel());
            strCRTColumnData.newGirderHBeamList.AddRange(girderHbeamList);
            strCRTColumnData.newGirderHBeamList.Add(new HBeamModel());



            // Output
            strCRTColumnData.newRafterOutputList.Add(new NColumnRafterModel());
            strCRTColumnData.newRafterOutputList.AddRange(rafterOutputList);

            strCRTColumnData.newColumnCenterTopSupportList.AddRange(columnCenterTopSupportList);
            strCRTColumnData.newColumnCenterTopSupportList.Add(new NColumnCenterTopSupportModel());

            strCRTColumnData.newColumnSideTopSupportList.AddRange(columnSideTopSupportList);
            strCRTColumnData.newColumnSideTopSupportList.Add(new NColumnSideTopSupportModel());

            strCRTColumnData.newColumnGirderList.Add(new NColumnGirderModel());
            strCRTColumnData.newColumnGirderList.AddRange(columnGirderList);
            strCRTColumnData.newColumnGirderList.Add(new NColumnGirderModel());

            strCRTColumnData.newColumnBaseSupportList.AddRange(columnBaseSupportList);
            strCRTColumnData.newColumnBaseSupportList.Add(new NColumnBaseSupportModel());

            strCRTColumnData.newColumnPipeList.AddRange(columnPipeList);
            strCRTColumnData.newColumnPipeList.Add(new PipeModel());

            // Column
            //NColumnCenterTopSupportModel columnCenterTopSupport = new NColumnCenterTopSupportModel();
            //if (newColumnCenterTopSupportList.Count > 0)
            //    columnCenterTopSupport = newColumnCenterTopSupportList[0];




            //기울기, 밑변 길이
            double TankID = selTankID;
            double TankH = selTankHeight;
            double TankHalfID = TankID / 2;
            double AnnularInnerWidth = selAnnularInnerWidth;
            double RoofOD = selRoofOD;
            double BottomThickness = selBottomThk;

            // double RoofSlopeDegree = RadianToDegree(selRoofSlope);
            // double BottomSlopDegree = RadianToDegree(selBottoSlope);
            double RoofSlopeDegree = selRoofSlope;
            double BottomSlopDegree = selBottoSlope;


            // 고정 값
            double columnSpace = 25;
            double RafterOffset = 85; //Clip Point로부터 늘어나는 길이
            double RafterOffsetTopView = geoService.GetAdjacentByHypotenuse(RoofSlopeDegree, RafterOffset);
            double ShellReduce = 70 + selShellReduce; // Shell ID로부터 안쪽으로 줄어드는 길이 -> Angle Type 따라서 달라짐 : k Type에서 조금 안쪽으로 들어감
            double GirderReduce = 200; //Column Point로부터 줄어드는 길이

            StructureCRTColumnModel newStrModel = new StructureCRTColumnModel();

            // Data
            newStrModel.strData = strCRTColumnData;

            // 1. Create Layer : Column, Rafter, Girder
            for (int layerIndex = 0; layerIndex < strCRTColumnData.newColumnInputList.Count; layerIndex++)
            {
                // Layer
                StructureLayerModel newLayer = new StructureLayerModel();
                newLayer.Number = 0;

                newLayer.StartAngle = 0;

                // Column
                StructureCRTColumnInputModel eachColumn = strCRTColumnData.newColumnInputList[layerIndex];
                double columnQty = GetDoubleValue(eachColumn.Qty);
                double columnRadius = GetDoubleValue(eachColumn.Radius);
                string columnSize = eachColumn.Size;
                double columnAngleOne = 0;
                if (columnQty > 0)
                    columnAngleOne = 360 / columnQty;
                for (int columnIndex = 0; columnIndex < columnQty; columnIndex++)
                {
                    StructureColumnModel newColumn = new StructureColumnModel();
                    newColumn.Radius = columnRadius;
                    newColumn.AngleOne = columnAngleOne;
                    newColumn.Height = 0;   // 나중에 구함
                    newColumn.Size = columnSize;

                    newLayer.ColumnList.Add(newColumn);
                }

                // Layer : Radius
                newLayer.Radius = columnRadius;
                if (layerIndex == strCRTColumnData.newColumnInputList.Count - 1)
                    newLayer.Radius = TankHalfID;


                // Rafter
                NColumnRafterModel eachOutputRafter = strCRTColumnData.newRafterOutputList[layerIndex];
                double rafterHeight = GetDoubleValue(eachOutputRafter.A);

                StructureCRTRafterInputModel eachRafter = strCRTColumnData.newRafterInputList[layerIndex];
                double rafterQty = GetDoubleValue(eachRafter.Qty);
                double rafterRadius = GetDoubleValue(eachRafter.Radius);
                string rafterSize = eachRafter.Size;
                double rafterAngleOne = 0;
                if (rafterQty > 0)
                    rafterAngleOne = 360 / rafterQty;
                for (int rafterIndex = 0; rafterIndex < rafterQty; rafterIndex++)
                {
                    StructureRafterModel newRafter = new StructureRafterModel();
                    newRafter.AngleOne = rafterAngleOne;
                    newRafter.Length = 0; // 나중에 구함
                    newRafter.Height = rafterHeight;
                    newRafter.Size = rafterSize;
                    newLayer.RafterList.Add(newRafter);
                }


                // Girder + Clip
                HBeamModel eachGriderHBeam = strCRTColumnData.newGirderHBeamList[layerIndex];
                double girderWidth = GetDoubleValue(eachGriderHBeam.B);
                double girderHeight = GetDoubleValue(eachGriderHBeam.A);

                StructureCRTGirderInputModel eachGirder = strCRTColumnData.newGirderInputList[layerIndex];
                double girderQty = GetDoubleValue(eachGirder.Qty);
                double girderRadius = GetDoubleValue(eachGirder.Radius);
                string girderSize = eachGirder.Size;
                double girderAngleOne = 0;
                if (girderQty > 0)
                    girderAngleOne = 360 / girderQty;
                for (int girderIndex = 0; girderIndex < girderQty; girderIndex++)
                {
                    StructureGirderModel newGirder = new StructureGirderModel();
                    newGirder.Radius = girderRadius;
                    newGirder.AngleOne = girderAngleOne;
                    newGirder.Size = girderSize;


                    newGirder.Length = 0; // 나중에 구함
                    newGirder.Width = girderWidth;
                    newGirder.Height = girderHeight;

                    newLayer.GirderList.Add(newGirder);
                }



                newStrModel.LayerList.Add(newLayer);
            }

            // 2. Grider : Length
            for (int layerIndex = 0; layerIndex < newStrModel.LayerList.Count; layerIndex++)
            {
                StructureLayerModel eachLayer = newStrModel.LayerList[layerIndex];
                double eachLayerRadius = eachLayer.Radius;
                NColumnSideTopSupportModel eachColumnSideTop = strCRTColumnData.newColumnSideTopSupportList[layerIndex];
                GirderReduce = GetDoubleValue(eachColumnSideTop.D);
                for (int girderIndex = 0; girderIndex < eachLayer.GirderList.Count; girderIndex++)
                {
                    StructureColumnModel eachColumn = eachLayer.ColumnList[girderIndex];

                    StructureGirderModel eachGirder = eachLayer.GirderList[girderIndex];
                    eachGirder.Length = geoService.GetStringLengthByArcAngle(eachLayerRadius, eachColumn.AngleOne) - (GirderReduce * 2); //Radius와 Angle로 현의 길이 구한 후 - (200 * 2)

                }
            }

            // 4. AngleFormCenter
            foreach (StructureLayerModel eachLayer in newStrModel.LayerList)
            {
                double layerStartAngle = eachLayer.StartAngle;

                double girderStartAngle = layerStartAngle;
                for (int girderIndex = 0; girderIndex < eachLayer.GirderList.Count; girderIndex++)
                {
                    StructureGirderModel eachGirder = eachLayer.GirderList[girderIndex];
                    eachGirder.AngleFromCenter = girderStartAngle;
                    girderStartAngle += eachGirder.AngleOne;
                }

                double columnStartAngle = layerStartAngle;
                for (int columnIndex = 0; columnIndex < eachLayer.ColumnList.Count; columnIndex++)
                {
                    StructureColumnModel eachColumn = eachLayer.ColumnList[columnIndex];
                    eachColumn.AngleFromCenter = columnStartAngle;
                    columnStartAngle += eachColumn.AngleOne;
                }

                double rafterStartAngle = layerStartAngle;
                // 절반으로 시작
                if (eachLayer.RafterList.Count > 0)
                    rafterStartAngle += eachLayer.RafterList[0].AngleOne / 2;
                for (int rafterIndex = 0; rafterIndex < eachLayer.RafterList.Count; rafterIndex++)
                {
                    StructureRafterModel eachRafter = eachLayer.RafterList[rafterIndex];
                    eachRafter.AngleFromCenter = rafterStartAngle;
                    rafterStartAngle += eachRafter.AngleOne;
                }
            }

            // 5. Girder : Clip
            for (int layerIndex = 0; layerIndex < newStrModel.LayerList.Count - 1; layerIndex++)
            {
                StructureLayerModel innerLayer = newStrModel.LayerList[layerIndex];
                StructureLayerModel outerLayer = newStrModel.LayerList[layerIndex + 1];

                double innerLayerRadius = innerLayer.Radius;
                for (int girderIndex = 0; girderIndex < innerLayer.GirderList.Count; girderIndex++)
                {
                    StructureGirderModel eachGirder = innerLayer.GirderList[girderIndex];

                    List<StructureRafterModel> innerRafterList = GetRafterByAngleRange(innerLayer.RafterList, eachGirder.AngleFromCenter, eachGirder.AngleFromCenter + eachGirder.AngleOne);
                    List<StructureRafterModel> outerRafterList = GetRafterByAngleRange(outerLayer.RafterList, eachGirder.AngleFromCenter, eachGirder.AngleFromCenter + eachGirder.AngleOne);

                    //Girder Point는 InnerRafter, OuterRafter 두가지가 반영되어야 함 -> Inner Rafter각도와 outer Rafter 각도를 고려하여 Clip의 포인트가 필요요
                    for (int innerRafterIndex = 0; innerRafterIndex < innerRafterList.Count; innerRafterIndex++)
                    {
                        StructureRafterModel eachRafter = innerRafterList[innerRafterIndex];

                        StructureClipModel newClipModel = new StructureClipModel();
                        newClipModel.InOut = 0;// In = 0
                        newClipModel.Number = innerRafterIndex; // 0부터
                        newClipModel.AngleFromCenter = eachRafter.AngleFromCenter;

                        newClipModel.ColumnSideAngle = (180 - eachGirder.AngleOne) / 2; // 각도 B;
                        newClipModel.CenterSideAngle = eachRafter.AngleFromCenter - eachGirder.AngleFromCenter; // 각도 C
                        newClipModel.ClipSideAngle = 180 - newClipModel.ColumnSideAngle - newClipModel.CenterSideAngle; // 각도 A
                        newClipModel.GirderClipAngle = newClipModel.ClipSideAngle - 90; //Girder의 수직과 Clip의 각도;
                        newClipModel.GirderClipAngleABS = Math.Abs(newClipModel.GirderClipAngle);

                        //Girder Point는 InnerRafter, OuterRafter 두가지가 반영되어야 함 -> Inner Rafter각도와 outer Rafter 각도를 고려하여 Clip의 포인트가 필요요
                        //newClipModel.PointLengthFormColumn = (innerLayerRadius * Math.Sin(DegreeToRadian( newClipModel.CenterSideAngle))) / Math.Sin(DegreeToRadian(newClipModel.ColumnSideAngle));
                        //newClipModel.HorizontalLengthFromCenter = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.ColumnSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));
                        newClipModel.PointLengthFormColumn = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.CenterSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));
                        newClipModel.HorizontalLengthFromCenter = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.ColumnSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));

                        eachGirder.ClipList.Add(newClipModel);
                    }
                    for (int outerRafterIndex = 0; outerRafterIndex < outerRafterList.Count; outerRafterIndex++)
                    {
                        StructureRafterModel eachRafter = outerRafterList[outerRafterIndex];

                        StructureClipModel newClipModel = new StructureClipModel();
                        newClipModel.InOut = 1;// Out = 1
                        newClipModel.Number = outerRafterIndex; // 0부터
                        newClipModel.AngleFromCenter = eachRafter.AngleFromCenter;

                        newClipModel.ColumnSideAngle = (180 - eachGirder.AngleOne) / 2; // 각도 B;
                        newClipModel.CenterSideAngle = eachRafter.AngleFromCenter - eachGirder.AngleFromCenter; // 각도 C
                        newClipModel.ClipSideAngle = 180 - newClipModel.ColumnSideAngle - newClipModel.CenterSideAngle; // 각도 A
                        newClipModel.GirderClipAngle = newClipModel.ClipSideAngle - 90; //Girder의 수직과 Clip의 각도;
                        newClipModel.GirderClipAngleABS = Math.Abs(newClipModel.GirderClipAngle);

                        //Girder Point는 InnerRafter, OuterRafter 두가지가 반영되어야 함 -> Inner Rafter각도와 outer Rafter 각도를 고려하여 Clip의 포인트가 필요요
                        //newClipModel.PointLengthFormColumn = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.CenterSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ColumnSideAngle));
                        //newClipModel.HorizontalLengthFromCenter = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.ColumnSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));
                        newClipModel.PointLengthFormColumn = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.CenterSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));
                        newClipModel.HorizontalLengthFromCenter = innerLayerRadius * Math.Sin(DegreeToRadian(newClipModel.ColumnSideAngle)) / Math.Sin(DegreeToRadian(newClipModel.ClipSideAngle));

                        eachGirder.ClipList.Add(newClipModel);
                    }

                    // 작은 각도 기준으로 정렬
                    eachGirder.ClipList = eachGirder.ClipList.OrderBy(x => x.AngleFromCenter).ToList();

                }
            }

            // 6. Column : Height
            for (int layerIndex = 0; layerIndex < newStrModel.LayerList.Count - 1; layerIndex++)
            {
                StructureLayerModel innerLayer = newStrModel.LayerList[layerIndex];
                StructureLayerModel outerLayer = newStrModel.LayerList[layerIndex + 1];
                StructureRafterModel OuterRafterOne = outerLayer.RafterList[0];
                NColumnCenterTopSupportModel eachColumnCenterTopSupport = strCRTColumnData.newColumnCenterTopSupportList[layerIndex];

                PipeModel eachColumnPipe = strCRTColumnData.newColumnPipeList[layerIndex];

                double eachLayerRadius = innerLayer.Radius;
                for (int columnIndex = 0; columnIndex < innerLayer.ColumnList.Count; columnIndex++)
                {
                    StructureColumnModel innerColumn = innerLayer.ColumnList[columnIndex];
                    if (layerIndex == 0)
                    {
                        double centerColumnTopRadius = (GetDoubleValue(eachColumnPipe.OD) / 2) +
                        GetDoubleValue(eachColumnCenterTopSupport.G) +
                        GetDoubleValue(eachColumnCenterTopSupport.A1);//B1->A1 으로 수정 2021-10-24

                        // Center : Height
                        double RoofCenterHeight = geoService.GetOppositeByAdjacent(RoofSlopeDegree, RoofOD / 2);
                        double BottomHalfID = TankHalfID - AnnularInnerWidth;
                        double BottomCenterHeight = geoService.GetOppositeByAdjacent(BottomSlopDegree, BottomHalfID);
                        double BottomThicknessSlopeH = geoService.GetHypotenuseByWidth(BottomSlopDegree, BottomThickness);
                        double CenterColumnHalfOD = centerColumnTopRadius;
                        double CenterElevationPointLgH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, CenterColumnHalfOD);

                        double columnSpaceHeight = geoService.GetHypotenuseByWidth(RoofSlopeDegree, columnSpace);
                        double RafterAA = geoService.GetHypotenuseByWidth(RoofSlopeDegree, OuterRafterOne.Height);// 수정 완료

                        // annular 처리
                        if (AnnularInnerWidth == 0)
                            BottomThicknessSlopeH = 0;

                        innerColumn.Height = RoofCenterHeight + (TankH - BottomCenterHeight - BottomThicknessSlopeH) - CenterElevationPointLgH - RafterAA - columnSpaceHeight;
                        innerColumn.PipeOD = GetDoubleValue(eachColumnPipe.OD);
                    }
                    else
                    {
                        StructureGirderModel innerGirder = innerLayer.GirderList[columnIndex];
                        StructureClipModel innerGirderClip = new StructureClipModel();
                        if (innerGirder.ClipList.Count > 0)
                            innerGirderClip = innerGirder.ClipList[0];

                        // Side : Height
                        double ColumnPointHLg = TankHalfID - eachLayerRadius;
                        double BottomHalfID = TankHalfID - AnnularInnerWidth;
                        double BottomThicknessSlopeH = geoService.GetHypotenuseByWidth(BottomSlopDegree, BottomThickness);
                        double ColumnPointRoofH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, ColumnPointHLg);
                        double ColumnPointBottomLg = BottomHalfID - eachLayerRadius;
                        double ColumnPointBottomHeight = geoService.GetOppositeByAdjacent(BottomSlopDegree, ColumnPointBottomLg);

                        double ElavationPointToClipPointLength = geoService.GetHypotenuseByWidth(DegreeToRadian(innerGirderClip.GirderClipAngleABS), innerGirder.Width / 2);


                        StructureClipModel firstClip = new StructureClipModel();
                        if (innerGirder.ClipList.Count > 0)
                            firstClip = innerGirder.ClipList[0];

                        double ElavationPointToColumnRadiusHLg = innerColumn.Radius - firstClip.HorizontalLengthFromCenter - ElavationPointToClipPointLength;
                        double ElevationPointLgH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, ElavationPointToColumnRadiusHLg);

                        double columnSpaceHeight = geoService.GetHypotenuseByWidth(RoofSlopeDegree, columnSpace);
                        double RafterAA = geoService.GetHypotenuseByWidth(RoofSlopeDegree, OuterRafterOne.Height); // 수정 완료


                        // annular 처리
                        if (AnnularInnerWidth == 0)
                            BottomThicknessSlopeH = 0;



                        innerColumn.Height = ColumnPointRoofH + (TankH - ColumnPointBottomHeight - BottomThicknessSlopeH) - ElevationPointLgH - RafterAA - innerGirder.Height - columnSpaceHeight;
                        innerColumn.PipeOD = GetDoubleValue(eachColumnPipe.OD);



                    }





                }
            }



            // 7. Rafter : Length
            for (int layerIndex = 0; layerIndex < newStrModel.LayerList.Count - 1; layerIndex++)
            {
                StructureLayerModel innerLayer = newStrModel.LayerList[layerIndex];
                StructureLayerModel outerLayer = newStrModel.LayerList[layerIndex + 1];

                NColumnCenterTopSupportModel eachColumnCenterTopSupport = strCRTColumnData.newColumnCenterTopSupportList[layerIndex];

                double eachLayerRadius = innerLayer.Radius;
                StructureRafterModel firstRafter = new StructureRafterModel();
                if (outerLayer.RafterList.Count > 0)
                    firstRafter = outerLayer.RafterList[0];

                if (newStrModel.LayerList.Count == 2) //CenterColumn만 있을 경우 계산
                {

                    double centerColumnB = GetDoubleValue(eachColumnCenterTopSupport.B);
                    double RafterHLg = (TankHalfID - centerColumnB - ShellReduce);
                    double RafterLg = geoService.GetHypotenuseByWidth(RoofSlopeDegree, RafterHLg) - geoService.GetOppositeByAdjacent(RoofSlopeDegree, firstRafter.Height / 2);
                    double RafterSlopeOffset = geoService.GetOppositeByHypotenuse(RoofSlopeDegree, firstRafter.Height);
                    double RafterSlopeOffsetHalf = RafterSlopeOffset / 2;

                    // 1개 구해서 전체 배정
                    foreach (StructureRafterModel eachRafter in outerLayer.RafterList)
                    {
                        eachRafter.Length = RafterLg + RafterOffset;
                        eachRafter.OuterRealRadius = TankHalfID - ShellReduce;
                        eachRafter.InnerRealRadius = centerColumnB - RafterOffsetTopView + RafterSlopeOffsetHalf;
                        eachRafter.InnerTopViewRadius = centerColumnB - RafterOffsetTopView - RafterSlopeOffsetHalf;
                        eachRafter.InnerClipRadiusFromCenter = centerColumnB;
                        eachRafter.LengthTopView = eachRafter.OuterRealRadius - eachRafter.InnerTopViewRadius;
                    }
                }
                else if (newStrModel.LayerList.Count > 1) // Side Column이 있을 경우
                {
                    if (layerIndex == 0) // 첫단일 경우 계산
                    {
                        double centerColumnB = GetDoubleValue(eachColumnCenterTopSupport.B);
                        foreach (StructureRafterModel eachRafter in outerLayer.RafterList)
                        {
                            StructureGirderModel eachGirder = GetGirderByRafterAngle(outerLayer.GirderList, eachRafter.AngleFromCenter);
                            StructureClipModel eachClip = GetClipByRafterAngle(eachGirder.ClipList, eachRafter.AngleFromCenter, 0); // Inner Clip

                            double RafterSlopeOffset = geoService.GetOppositeByHypotenuse(RoofSlopeDegree, eachRafter.Height);
                            double RafterSlopeOffsetHalf = RafterSlopeOffset / 2;
                            double Layer1RafterHLg = eachClip.HorizontalLengthFromCenter - centerColumnB;

                            eachRafter.Length = geoService.GetHypotenuseByWidth(RoofSlopeDegree, Layer1RafterHLg) + RafterOffset * 2;
                            eachRafter.OuterRealRadius = eachClip.HorizontalLengthFromCenter + RafterOffsetTopView + RafterSlopeOffsetHalf;
                            eachRafter.InnerRealRadius = centerColumnB - RafterOffsetTopView + RafterSlopeOffsetHalf;
                            eachRafter.InnerTopViewRadius = centerColumnB - RafterOffsetTopView - RafterSlopeOffsetHalf;
                            eachRafter.OuterClipRadiusFromCenter = eachClip.HorizontalLengthFromCenter;
                            eachRafter.InnerClipRadiusFromCenter = centerColumnB;
                            eachRafter.LengthTopView = eachRafter.OuterRealRadius - eachRafter.InnerTopViewRadius;



                        }
                    }
                    else if (layerIndex == newStrModel.LayerList.Count - 2) // 끝단일 경우 계산
                    {
                        foreach (StructureRafterModel eachRafter in outerLayer.RafterList)
                        {
                            StructureGirderModel eachGirder = GetGirderByRafterAngle(innerLayer.GirderList, eachRafter.AngleFromCenter);
                            StructureClipModel eachClip = GetClipByRafterAngle(eachGirder.ClipList, eachRafter.AngleFromCenter, 1); // Outer Clip

                            double RafterSlopeOffset = geoService.GetOppositeByHypotenuse(RoofSlopeDegree, eachRafter.Height);
                            double RafterSlopeOffsetHalf = RafterSlopeOffset / 2;
                            double tempEndHLg = TankHalfID - eachClip.HorizontalLengthFromCenter - ShellReduce;

                            eachRafter.Length = geoService.GetHypotenuseByWidth(RoofSlopeDegree, tempEndHLg) - geoService.GetOppositeByAdjacent(RoofSlopeDegree, firstRafter.Height / 2) + RafterOffset;
                            eachRafter.OuterRealRadius = TankHalfID - ShellReduce;
                            eachRafter.InnerRealRadius = eachClip.HorizontalLengthFromCenter - RafterOffsetTopView + RafterSlopeOffsetHalf;
                            eachRafter.InnerTopViewRadius = eachClip.HorizontalLengthFromCenter - RafterOffsetTopView - RafterSlopeOffsetHalf;
                            eachRafter.OuterClipRadiusFromCenter = TankHalfID - ShellReduce;
                            eachRafter.InnerClipRadiusFromCenter = eachClip.HorizontalLengthFromCenter;
                            eachRafter.LengthTopView = eachRafter.OuterRealRadius - eachRafter.InnerTopViewRadius;
                        }




                    }
                    else //ColumnLayer < ColumnLayerCount) // 중간 Rafter Length
                    {
                        foreach (StructureRafterModel eachRafter in outerLayer.RafterList)
                        {
                            StructureGirderModel eachOuterGirder = GetGirderByRafterAngle(outerLayer.GirderList, eachRafter.AngleFromCenter);
                            StructureClipModel eachInnerClip = GetClipByRafterAngle(eachOuterGirder.ClipList, eachRafter.AngleFromCenter, 0); // Inner Clip

                            StructureGirderModel eachInnerGirder = GetGirderByRafterAngle(innerLayer.GirderList, eachRafter.AngleFromCenter);
                            StructureClipModel eachOuterClip = GetClipByRafterAngle(eachInnerGirder.ClipList, eachRafter.AngleFromCenter, 1); // Outer Clip

                            double RafterSlopeOffset = geoService.GetOppositeByHypotenuse(RoofSlopeDegree, eachRafter.Height);
                            double RafterSlopeOffsetHalf = RafterSlopeOffset / 2;
                            double RafterHLg = eachInnerClip.HorizontalLengthFromCenter - eachOuterClip.HorizontalLengthFromCenter;

                            eachRafter.Length = geoService.GetHypotenuseByWidth(RoofSlopeDegree, RafterHLg) + RafterOffset * 2;
                            eachRafter.OuterRealRadius = eachInnerClip.HorizontalLengthFromCenter + RafterOffsetTopView + RafterSlopeOffsetHalf;
                            eachRafter.InnerRealRadius = eachOuterClip.HorizontalLengthFromCenter - RafterOffsetTopView + RafterSlopeOffsetHalf;
                            eachRafter.InnerTopViewRadius = eachOuterClip.HorizontalLengthFromCenter - RafterOffsetTopView - RafterSlopeOffsetHalf;
                            eachRafter.OuterClipRadiusFromCenter = eachInnerClip.HorizontalLengthFromCenter;
                            eachRafter.InnerClipRadiusFromCenter = eachOuterClip.HorizontalLengthFromCenter;
                            eachRafter.LengthTopView = eachRafter.OuterRealRadius - eachRafter.InnerTopViewRadius;



                        }





                    }




                }




            }



            // 8. Clip : Height
            for (int layerIndex = 0; layerIndex < newStrModel.LayerList.Count - 1; layerIndex++)
            {
                StructureLayerModel innerLayer = newStrModel.LayerList[layerIndex];
                StructureLayerModel outerLayer = newStrModel.LayerList[layerIndex + 1];

                double eachLayerRadius = innerLayer.Radius;
                StructureRafterModel outerRafter = new StructureRafterModel();
                if (outerLayer.RafterList.Count > 0)
                    outerRafter = outerLayer.RafterList[0];

                StructureRafterModel innerRafter = new StructureRafterModel();
                if (innerLayer.RafterList.Count > 0)
                    innerRafter = innerLayer.RafterList[0];

                PipeModel eachColumnPipe = strCRTColumnData.newColumnPipeList[layerIndex];

                NColumnCenterTopSupportModel eachColumnCenterTopSupport = strCRTColumnData.newColumnCenterTopSupportList[layerIndex];
                double clipWidth = GetDoubleValue(eachColumnCenterTopSupport.C);

                double centerColumnTopRadius = (GetDoubleValue(eachColumnPipe.OD) / 2) +
                GetDoubleValue(eachColumnCenterTopSupport.G) +
                GetDoubleValue(eachColumnCenterTopSupport.B1);
                double CenterClipE = GetDoubleValue(eachColumnCenterTopSupport.E); //ClipPoint에서 위로 길이
                double ClipPointHeight = geoService.GetOppositeByHypotenuse(RoofSlopeDegree, GetDoubleValue(eachColumnCenterTopSupport.D) / 2);
                double CenterDistance = GetDoubleValue(eachColumnCenterTopSupport.B);
                double RafterHalfAA = geoService.GetHypotenuseByWidth(RoofSlopeDegree, outerRafter.Height / 2);

                double CenterAddClipLg = centerColumnTopRadius - CenterDistance;
                double CenterAddClipH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, CenterAddClipLg);
                double columnSpaceHeight = geoService.GetHypotenuseByWidth(RoofSlopeDegree, columnSpace);

                // Rafter 추가
                NColumnRafterModel outerRafterModel = strCRTColumnData.newRafterOutputList[layerIndex + 1];
                double outerRafterG = GetDoubleValue(outerRafterModel.G) / 2;
                double outerRafterGLg = geoService.GetAdjacentByHypotenuse(RoofSlopeDegree, outerRafterG);
                double outerRafterE = GetDoubleValue(outerRafterModel.E) / 2;
                double outerRafterELg = geoService.GetAdjacentByHypotenuse(RoofSlopeDegree, outerRafterE);
                double outerRafterColumnHoleQty = GetDoubleValue(outerRafterModel.BoltHoleQtyOnColumnSide);
                double outerRafterCenterHoleQty = GetDoubleValue(outerRafterModel.BoltHoleQtyOnCenterSide);
                if (outerRafterCenterHoleQty == 2)
                    outerRafterGLg = 0;
                if (outerRafterColumnHoleQty == 2)
                    outerRafterELg = 0;



                // Inner Rafter
                double innerRafterHalfAA = geoService.GetHypotenuseByWidth(RoofSlopeDegree, innerRafter.Height / 2);

                NColumnRafterModel innerRafterModel = strCRTColumnData.newRafterOutputList[layerIndex];
                double innerRafterG = GetDoubleValue(innerRafterModel.G) / 2;
                double innerRafterGLg = geoService.GetAdjacentByHypotenuse(RoofSlopeDegree, innerRafterG);
                double innerRafterE = GetDoubleValue(innerRafterModel.E) / 2;
                double innerRafterELg = geoService.GetAdjacentByHypotenuse(RoofSlopeDegree, innerRafterE);
                double innerRafterCenterHoleQty = GetDoubleValue(innerRafterModel.BoltHoleQtyOnCenterSide);
                if (innerRafterCenterHoleQty == 2)
                    innerRafterGLg = 0;
                double innerRafterColumnHoleQty = GetDoubleValue(innerRafterModel.BoltHoleQtyOnColumnSide);
                if (innerRafterColumnHoleQty == 2)
                    innerRafterELg = 0;








                if (layerIndex == 0)
                {
                    double CenterClipHeight = CenterClipE + ClipPointHeight + RafterHalfAA + columnSpaceHeight + CenterAddClipH + outerRafterGLg; // Rafter G

                    StructureGirderModel newGirder = new StructureGirderModel();

                    for (int rafterIndex = 0; rafterIndex < outerLayer.RafterList.Count; rafterIndex++)
                    {
                        StructureClipModel newClip = new StructureClipModel();
                        newClip.AngleFromCenter = outerLayer.RafterList[rafterIndex].AngleFromCenter;
                        newClip.ClipHeight = CenterClipHeight;
                        newClip.ClipWidth = clipWidth; // Clip Width 추가 2021-10-25
                        newGirder.ClipList.Add(newClip);
                    }
                    innerLayer.GirderList.Add(newGirder);

                }
                else
                {


                    for (int girderIndex = 0; girderIndex < innerLayer.GirderList.Count; girderIndex++)
                    {
                        StructureGirderModel innerGirder = innerLayer.GirderList[girderIndex];
                        StructureClipModel firstClip = new StructureClipModel();
                        if (innerGirder.ClipList.Count > 0)
                            firstClip = innerGirder.ClipList[0];

                        double ElavationPointToClipPointLength = geoService.GetHypotenuseByWidth(DegreeToRadian(firstClip.GirderClipAngleABS), innerGirder.Width / 2); // 수정완료
                        double ElevationPointLgH = geoService.GetOppositeByAdjacent(RoofSlopeDegree, ElavationPointToClipPointLength);
                        double SideAddClipH = ElevationPointLgH;



                        for (int clipIndex = 0; clipIndex < innerGirder.ClipList.Count; clipIndex++)
                        {
                            StructureClipModel eachClip = innerGirder.ClipList[clipIndex];


                            if (eachClip.InOut == 0)
                            {
                                double EachAddHeight = geoService.GetOppositeByAdjacent(RoofSlopeDegree, firstClip.HorizontalLengthFromCenter - eachClip.HorizontalLengthFromCenter);

                                //Rafter Gap
                                double outerRafterGap = RafterHalfAA - innerRafterHalfAA;

                                //Side Clip Height
                                double SideClipHeight = CenterClipE + ClipPointHeight + RafterHalfAA + columnSpaceHeight + SideAddClipH + innerRafterELg; //SideColumn 첫번째 Clip의 높이 // Rafter G
                                double EachClipHeight = SideClipHeight + EachAddHeight + outerRafterGap; // 나머지 Clip의 높이 = 첫번째 클립의 Horizental Length와 각 클립의 Horizental Length
                                eachClip.ClipHeight = EachClipHeight;
                                eachClip.ClipWidth = clipWidth; // Clip Width 추가 2021-10-25
                            }
                            else
                            {
                                double EachAddHeight = geoService.GetOppositeByAdjacent(RoofSlopeDegree, firstClip.HorizontalLengthFromCenter - eachClip.HorizontalLengthFromCenter);

                                //Side Clip Height
                                double SideClipHeight = CenterClipE + ClipPointHeight + RafterHalfAA + columnSpaceHeight + SideAddClipH + outerRafterELg; //SideColumn 첫번째 Clip의 높이 // Rafter G
                                double EachClipHeight = SideClipHeight + EachAddHeight; // 나머지 Clip의 높이 = 첫번째 클립의 Horizental Length와 각 클립의 Horizental Length
                                eachClip.ClipHeight = EachClipHeight;
                                eachClip.ClipWidth = clipWidth; // Clip Width 추가 2021-10-25
                            }


                        }





                    }

                }
            }


            return newStrModel;
        }


        public StructureCRTCenteringModel CreateStructureCRTCentering(
                                                List<StructureCRTCenteringInputModel> centeringInput,
                                                List<StructureCRTRafterInputModel> rafterInputList,
                                                List<NCenteringInternalModel> centeringInternalInputList,
                                                List<NRafterCenteringInternalModel> rafterCenteringInternalInputList,
                                                List<NRafterSupportClipCenterSideModel> rafterSupportClipCenterList,

                                         AngleSizeModel purlinInput,
                                                List<object> rafterOutputList,

                                         double selTankID, double selTankHeight, double selAnnularInnerWidth, double selRoofOD, double selBottomThk,
                                         double selRoofSlope, double selBottoSlope, double selRoogThk,
                                         double selShellReduce = 0
                                         )
        {


            // Input
            strCRTCenteringData = new StructureDataInputModel();
            strCRTCenteringData.newRafterInputList.AddRange(rafterInputList);

            // Output
            strCRTCenteringData.newCenteringInternalInputList.AddRange(centeringInternalInputList);
            strCRTCenteringData.newRafterCenteringInternalOutputList.AddRange(rafterCenteringInternalInputList);

            strCRTCenteringData.newCenteringInputList.AddRange(centeringInput);
            strCRTCenteringData.newCenteringHBeamChannelOutputList.AddRange(rafterOutputList);

            strCRTCenteringData.newRafterSupportClipCenterList.AddRange(rafterSupportClipCenterList);

            strCRTCenteringData.newPurlinInput = purlinInput;

            StructureCRTCenteringInputModel centeringInputOne = strCRTCenteringData.newCenteringInputList[0];
            StructureCRTRafterInputModel rafterInputOne = strCRTCenteringData.newRafterInputList[0];
            NCenteringInternalModel centeringOne = strCRTCenteringData.newCenteringInternalInputList[0];
            NRafterCenteringInternalModel rafterCenteringOne = strCRTCenteringData.newRafterCenteringInternalOutputList[0];
            AngleSizeModel purlinOne = strCRTCenteringData.newPurlinInput;

            //기울기, 밑변 길이
            double TankID = selTankID;
            double TankH = selTankHeight;
            double TankHalfID = TankID / 2;
            double AnnularInnerWidth = selAnnularInnerWidth;
            double RoofOD = selRoofOD;
            double BottomThickness = selBottomThk;
            double roofThickness = selRoogThk;

            double RoofSlopeDegree = selRoofSlope;
            double BottomSlopDegree = selBottoSlope;


            // 고정 값
            double RafterOffset = 85; //Clip Point로부터 늘어나는 길이
            double ShellReduce = 70 + selShellReduce; // Shell ID로부터 안쪽으로 줄어드는 길이 -> Angle Type 따라서 달라짐 : k Type에서 조금 안쪽으로 들어감


            StructureCRTCenteringModel newStrModel = new StructureCRTCenteringModel();

            // Data
            newStrModel.strData = strCRTCenteringData;


            // Centering Model
            string centeringPosition = centeringInputOne.Position; // Internal, External

            double centeringOD = GetDoubleValue(centeringInputOne.CenteringOD);
            double flangeOD = GetDoubleValue(centeringInputOne.FlangeOD);
            double flangeID = GetDoubleValue(centeringInputOne.FlangeID);
            double centeringB = (flangeOD - centeringOD) / 2;
            double RoofThickness = roofThickness;
            double centeringT1 = GetDoubleValue(centeringInputOne.Thickness1);


            double centeringRafterQTY = GetDoubleValue(rafterInputOne.Qty);
            double centeringIntRafterAngle = 360 / centeringRafterQTY;
            double rafterThickness = 6; // Channel HBeam Thickness
            double rafterHeight = GetDoubleValue(rafterCenteringOne.A);
            double rafterHeightTopView = geoService.GetOppositeByHypotenuse(RoofSlopeDegree, rafterHeight);
            double RoofThicknessTopView = geoService.GetOppositeByHypotenuse(RoofSlopeDegree, RoofThickness);
            double RoofThicknessAddLength = geoService.GetOppositeByAdjacent(RoofSlopeDegree, RoofThickness);


            double centeringA = GetDoubleValue(centeringOne.A);// Centering Height

            // Channel Beam Thickness
            foreach (object eachRfter in rafterOutputList)
            {
                if (eachRfter is HBeamModel)
                {
                    rafterThickness = GetDoubleValue(((HBeamModel)eachRfter).t1);
                    rafterHeight = GetDoubleValue(((HBeamModel)eachRfter).A);
                }
                else if (eachRfter is ChannelModel)
                {
                    rafterThickness = GetDoubleValue(((ChannelModel)eachRfter).t1);
                    rafterHeight = GetDoubleValue(((ChannelModel)eachRfter).A);
                }
            }


            double centeringExternalReduce = 30; // A1
            double centeringInternalReduce = 30; // B1
            foreach (NRafterCenteringInternalModel eachRafter in strCRTCenteringData.newRafterCenteringInternalOutputList)
            {
                centeringExternalReduce = GetDoubleValue(eachRafter.A1);
                centeringInternalReduce = GetDoubleValue(eachRafter.B1);
            }

            // First Layer
            StructureLayerModel firstLayer = new StructureLayerModel();


            if (!centeringPosition.ToLower().Contains("ex"))
            {
                // Layer : Radius
                firstLayer.Radius = TankHalfID;
                firstLayer.StartAngle = 0;


                // Blank : Internal
                #region CRT CenterRing Internal Type 



                double PurlinA = GetDoubleValue(purlinOne.A); //Angle 탭 참조      
                double PurlinB = GetDoubleValue(purlinOne.B); //Angle 탭 참조      

                //double CenterRingIntRafterHorozontalLength = TankHalfID - centeringOD / 2 + centeringB - ShellReduce - centeringInternalReduce;
                double CenterRingIntRafterHorozontalLength = TankHalfID - flangeOD / 2 - ShellReduce - centeringInternalReduce; //2021-10-29 수정 완료
                double CneterRingIntRafterLength = geoService.GetHypotenuseByWidth(RoofSlopeDegree, CenterRingIntRafterHorozontalLength);


                double PurlinRadius = Math.Ceiling(TankID / 4 / 100) * 100; //Center에서 Purlin Point까지 길이
                double PurlinCalLength = PurlinRadius + geoService.GetAdjacentByHypotenuse(RoofSlopeDegree, PurlinA) + geoService.GetOppositeByHypotenuse(RoofSlopeDegree, PurlinB / 2);
                //Purlin의 가장 긴 point 길이를 구하기 위해 purlin의 Horizontal Length값 더해줌

                double tempPurlinLength = geoService.GetStringLengthByArcAngle(PurlinCalLength, centeringIntRafterAngle); //String 길이
                double RafterAngleThickness = geoService.GetHypotenuseByWidth(DegreeToRadian(centeringIntRafterAngle / 2), rafterThickness); //Rafter 두께가 더해진 값
                double PurlinLength = tempPurlinLength - RafterAngleThickness; //String 길이에서 Rafter의 각도를 반영한 두께를 빼준 값

                double purlinSideAngle = (180 - centeringIntRafterAngle) / 2;
                double purlinTopViewA = geoService.GetHypotenuseByWidth(DegreeToRadian(90 - purlinSideAngle), PurlinA);


                for (int rafterIndex = 0; rafterIndex < centeringRafterQTY; rafterIndex++)
                {
                    StructureRafterModel newRafter = new StructureRafterModel();
                    newRafter.AngleOne = centeringIntRafterAngle;
                    newRafter.AngleFromCenter = firstLayer.StartAngle + newRafter.AngleOne * rafterIndex;
                    newRafter.Length = CneterRingIntRafterLength;
                    newRafter.Height = rafterHeight;

                    newRafter.Size = rafterInputOne.Size;

                    newRafter.OuterRealRadius = TankHalfID - ShellReduce;
                    newRafter.InnerRealRadius = centeringOD / 2 + centeringB + centeringInternalReduce;
                    newRafter.InnerTopViewRadius = newRafter.InnerRealRadius - rafterHeightTopView;
                    newRafter.LengthTopView = newRafter.OuterRealRadius - newRafter.InnerTopViewRadius; ;



                    StructurePurlinModel newPurlin = new StructurePurlinModel();
                    newPurlin.AngleOne = newRafter.AngleOne;
                    newPurlin.AngleOne = newRafter.AngleFromCenter;
                    newPurlin.Radius = PurlinRadius;
                    newPurlin.Length = PurlinLength;

                    newPurlin.Size = purlinInput.SIZE;

                    newPurlin.PurlinSideAngle = purlinSideAngle;
                    newPurlin.TopViewOuterRadius = PurlinRadius + purlinTopViewA;
                    newPurlin.TopViewInerRadius = PurlinRadius;
                    newPurlin.TopViewOuterLength = geoService.GetStringLengthByArcAngle(newPurlin.TopViewOuterRadius, newPurlin.AngleOne) - RafterAngleThickness;
                    newPurlin.TopViewInnerLength = geoService.GetStringLengthByArcAngle(newPurlin.TopViewInerRadius, newPurlin.AngleOne) - RafterAngleThickness; ;




                    firstLayer.RafterList.Add(newRafter);
                    firstLayer.PurlinList.Add(newPurlin);
                }

                #endregion
            }
            else
            {
                // Layer
                firstLayer.Radius = RoofOD / 2;
                firstLayer.TopViewRadius = RoofOD / 2 + rafterHeightTopView;
                firstLayer.StartAngle = 0;


                #region CRT CenterRing External Type

                double roofHorizontalRadius = RoofOD / 2;
                double centeringHeightSpace = GetDoubleValue(rafterCenteringOne.B1);
                double Centeringt1 = centeringT1; // Centering t1
                double CenteringA = centeringA; // 값 불러와야 함

                double RafterSlopeHeight = geoService.GetHypotenuseByWidth(RoofSlopeDegree, rafterHeight);


                double tempCenteringExtRafterLength = roofHorizontalRadius - (centeringOD / 2); //Roof HorizontalLength 값
                double CenteringExtRafterLength0 = geoService.GetHypotenuseByWidth(RoofSlopeDegree, tempCenteringExtRafterLength); //Roof의 길이


                //double tempRafterAddLength1 = geoService.GetHypotenuseByWidth(RoofSlopeDegree, rafterHeight)
                double RafterCenterTopPointLength = CenteringA - Centeringt1 - centeringHeightSpace;
                if (RafterSlopeHeight <= RafterCenterTopPointLength)
                {
                    RafterCenterTopPointLength = RafterSlopeHeight;
                }


                double RafterAddLength1 = geoService.GetOppositeByHypotenuse(RoofSlopeDegree, RafterCenterTopPointLength); //CenteringExtRafterLength0 에 더해주면 Rafter의 윗변 길이 나옴
                double RafterAddLength2 = geoService.GetOppositeByAdjacent(RoofSlopeDegree, centeringHeightSpace + Centeringt1); //CenteringExtRafterLength0 에 더해주면 Rafter 아랫변 길이 나옴
                double CenterRingExtRafterLength1 = CenteringExtRafterLength0 + RafterAddLength1 + RoofThicknessAddLength;
                double CenterRingExtRafterLength2 = CenteringExtRafterLength0 + RafterAddLength2 + RoofThicknessAddLength;

                //double UnderCutHeight = geoService.GetHypotenuseByWidth(RoofSlopeDegree, centeringHeightSpace + Centeringt1);
                //double CenteringBetweenRafterHeight = tempRafterAddLength1-UnderCutHeight;

                for (int rafterIndex = 0; rafterIndex < centeringRafterQTY; rafterIndex++)
                {
                    StructureRafterModel newRafter = new StructureRafterModel();
                    newRafter.AngleOne = centeringIntRafterAngle;
                    newRafter.AngleFromCenter = firstLayer.StartAngle + newRafter.AngleOne * rafterIndex;
                    newRafter.Length = CenterRingExtRafterLength1;
                    newRafter.Height = rafterHeight;



                    newRafter.Size = rafterInputOne.Size;



                    newRafter.OuterRealRadius = (RoofOD / 2) + rafterHeightTopView + RoofThicknessTopView;



                    newRafter.InnerRealRadius = centeringOD / 2;
                    newRafter.InnerTopViewRadius = flangeOD / 2 + centeringExternalReduce;
                    newRafter.LengthTopView = newRafter.OuterRealRadius - newRafter.InnerTopViewRadius; ;



                    // 추가함
                    firstLayer.TopViewRadius = newRafter.OuterRealRadius;



                    firstLayer.RafterList.Add(newRafter);
                }





                #endregion
            }





            newStrModel.LayerList.Add(firstLayer);

            return newStrModel;
        }


        public List<StructureRafterModel> GetRafterByAngleRange(List<StructureRafterModel> selRafterList, double startAngle, double endAngle)
        {
            List<StructureRafterModel> newList = new List<StructureRafterModel>();
            foreach (StructureRafterModel eachRafter in selRafterList)
            {
                if (startAngle <= eachRafter.AngleFromCenter && eachRafter.AngleFromCenter <= endAngle)
                {
                    newList.Add(eachRafter);
                }
            }

            return newList;
        }

        public StructureGirderModel GetGirderByRafterAngle(List<StructureGirderModel> selGirderList, double rafterAngle)
        {
            StructureGirderModel returnModel = new StructureGirderModel();
            foreach (StructureGirderModel eachGirder in selGirderList)
            {
                double startAngle = eachGirder.AngleFromCenter;
                double endAngle = eachGirder.AngleFromCenter + eachGirder.AngleOne;
                if (startAngle <= rafterAngle && rafterAngle <= endAngle)
                {
                    returnModel = eachGirder;
                    break;
                }
            }

            return returnModel;
        }

        public StructureClipModel GetClipByRafterAngle(List<StructureClipModel> selClipList, double rafterAngle, double inOut = 0)
        {
            // in =0
            // out =1
            StructureClipModel returnModel = new StructureClipModel();
            foreach (StructureClipModel eachClip in selClipList)
            {
                if (eachClip.InOut == inOut)
                {
                    if (eachClip.AngleFromCenter == rafterAngle)
                    {
                        returnModel = eachClip;
                        break;
                    }
                }
            }
            return returnModel;
        }



        public StructureRafterModel GetLongRafterInLayer(List<StructureRafterModel> selRafterList)
        {
            StructureRafterModel returnModel = new StructureRafterModel();
            double beforeLength = 0;
            foreach (StructureRafterModel eachModel in selRafterList)
            {
                if (eachModel.Length > beforeLength)
                {
                    returnModel = eachModel;
                    beforeLength = eachModel.Length;
                }
            }

            return returnModel;
        }
        public StructureRafterModel GetShortRafterInLayer(List<StructureRafterModel> selRafterList)
        {
            StructureRafterModel returnModel = new StructureRafterModel();
            double beforeLength = 99999999999;
            foreach (StructureRafterModel eachModel in selRafterList)
            {
                if (eachModel.Length < beforeLength)
                {
                    returnModel = eachModel;
                    beforeLength = eachModel.Length;
                }
            }

            return returnModel;
        }




        public double GetDoubleValue(string selValue)
        {
            // Default Value
            double doubleValue = 0;

            if (!double.TryParse(selValue, out doubleValue))
                doubleValue = 0;
            return doubleValue;
        }

        private double DegreeToRadian(double angle) { return Math.PI * angle / 180.0; }


        private double RadianToDegree(double angle) { return angle * (180.0 / Math.PI); }

    }
}