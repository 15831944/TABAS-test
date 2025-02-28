﻿using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailVisibleService
    {
        private Model singleModel;
        private AssemblyModel assemblyData;
        public DrawDetailVisibleService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;
        }


        public void SetDetailVisible(List<PaperAreaModel> selPaperAreaModelList)
        {
            string roofType = assemblyData.GeneralDesignData[0].RoofType;

            string compressionRingType = assemblyData.RoofCompressionRing[0].CompressionRingType;
            string anchorChair = assemblyData.AnchorageInput[0].AnchorChairBlot;
            string windGrider = assemblyData.WindGirderInput[0].WindGirderRequired;

            string annularType = assemblyData.BottomInput[0].AnnularPlate;

            string namePlate = assemblyData.AppurtenancesInput[0].NamePlate;
            string earthLug = assemblyData.AppurtenancesInput[0].EarthLug;
            string settlementCheckPiece = assemblyData.AppurtenancesInput[0].SettlementCheckPiece;

            // Default
            foreach (PaperAreaModel eachArea in selPaperAreaModelList)
            {
                if (eachArea.Name == PAPERMAIN_TYPE.GA1)
                {
                    eachArea.visible = true;
                }
                else if (eachArea.Name == PAPERMAIN_TYPE.GA1)
                {
                    eachArea.visible = true;
                }
                else if (eachArea.Name == PAPERMAIN_TYPE.ORIENTATION)
                {
                    eachArea.visible = true;
                }
                else if (eachArea.Name == PAPERMAIN_TYPE.DETAIL)
                {
                    switch (eachArea.SubName)
                    {

                        case PAPERSUB_TYPE.HORIZONTALJOINT:
                        case PAPERSUB_TYPE.ShellPlateArrangement:
                        case PAPERSUB_TYPE.DimensionsForCutting:
                        case PAPERSUB_TYPE.ToleranceLimit:
                        case PAPERSUB_TYPE.ShellPlateChordLength:
                        case PAPERSUB_TYPE.ONECOURSESHELLPLATE:
                            eachArea.visible = true;
                            break;

                        case PAPERSUB_TYPE.ComRing:
                            if (compressionRingType.Contains("Detail k"))
                                eachArea.visible = true;
                            break;
                        case PAPERSUB_TYPE.TopRingCuttingPlan:
                            if (compressionRingType.Contains("Detail k"))
                                eachArea.visible = true;
                            break;
                        case PAPERSUB_TYPE.ComRingCuttingPlan:
                            if (compressionRingType.Contains("Detail k"))
                                eachArea.visible = true;
                            break;

                        case PAPERSUB_TYPE.SectionDD:
                            if (compressionRingType.Contains("Detail b") ||
                                compressionRingType.Contains("Detail d") ||
                                compressionRingType.Contains("Detail e"))
                                eachArea.visible = true;
                            break;
                        case PAPERSUB_TYPE.TopAngleJoint:
                            if (compressionRingType.Contains("Detail b") ||
                                compressionRingType.Contains("Detail d") ||
                                compressionRingType.Contains("Detail e"))
                                eachArea.visible = true;
                            break;
                        case PAPERSUB_TYPE.WindGirderJoint:
                            if (windGrider.Contains("Yes"))
                                eachArea.visible = true;
                            break;
                        case PAPERSUB_TYPE.VertJointDetail:
                            if (windGrider.Contains("Yes"))
                                eachArea.visible = true;
                            break;



                        case PAPERSUB_TYPE.AnchorChair:
                            if (anchorChair.Contains("Yes"))
                                eachArea.visible = true;
                            break;

                        case PAPERSUB_TYPE.AnchorDetail:
                            if (anchorChair.Contains("Yes"))
                                eachArea.visible = true;
                            break;



                        case PAPERSUB_TYPE.NamePlateBracket:
                            if (namePlate.Contains("Yes"))
                                eachArea.visible = true;
                            break;
                        case PAPERSUB_TYPE.EarthLug:
                            if (earthLug.Contains("Yes"))
                                eachArea.visible = true;
                            break;
                        case PAPERSUB_TYPE.SettlementCheckPiece:
                            if (settlementCheckPiece.Contains("Yes"))
                                eachArea.visible = true;
                            break;


                        // Roof
                        case PAPERSUB_TYPE.RoofCompressionWeldingDetail:
                            if (compressionRingType.Contains("Detail i"))
                                eachArea.visible = true;
                            break;



                        // Bottom
                        case PAPERSUB_TYPE.BackingStripWeldingDetail:
                            if (annularType.Contains("Yes"))
                                eachArea.visible = true;
                            break;
                        case PAPERSUB_TYPE.BackingStrip:
                            if (annularType.Contains("Yes"))
                                eachArea.visible = true;
                            break;

                        case PAPERSUB_TYPE.BottomPlateJointAnnularDetail:
                            if (annularType.Contains("Yes"))
                                eachArea.visible = true;
                            break;
                        case PAPERSUB_TYPE.BottomPlateJointDetail:
                            if (annularType.Contains("No"))
                                eachArea.visible = true;
                            break;



                        // Structure 
                        case PAPERSUB_TYPE.RoofStructureAssembly:
                        case PAPERSUB_TYPE.RoofStructureOrientation:
                        case PAPERSUB_TYPE.RafterSideClipDetail:
                        case PAPERSUB_TYPE.CenterRingDetail:
                        case PAPERSUB_TYPE.RafterCenterClipDetail:
                        case PAPERSUB_TYPE.PurlinDetail:
                        case PAPERSUB_TYPE.SectionAA:
                        case PAPERSUB_TYPE.RibPlateDetail:
                        
                            eachArea.visible = true;
                            break;

                        // Structure Ext
                        case PAPERSUB_TYPE.CenterRingRafterDetail:
                        case PAPERSUB_TYPE.DetailB:
                        case PAPERSUB_TYPE.SectionCC:
                        case PAPERSUB_TYPE.ViewC:
                        case PAPERSUB_TYPE.RafterAndReinfPadCrossDetail:

                            eachArea.visible = true;
                            break;

                        // Structure Column
                        case PAPERSUB_TYPE.CenterColumnDetail:
                        case PAPERSUB_TYPE.C2SideColumnDetail:
                        case PAPERSUB_TYPE.C3SideColumnDetail:
                        case PAPERSUB_TYPE.DetailG:
                        case PAPERSUB_TYPE.DrainDetail:

                        case PAPERSUB_TYPE.SectionBB:
                        case PAPERSUB_TYPE.DetailC:
                        case PAPERSUB_TYPE.DetailF:
                        case PAPERSUB_TYPE.DetailE:
                        case PAPERSUB_TYPE.STRSectionDD:

                        case PAPERSUB_TYPE.SectionEE:
                        case PAPERSUB_TYPE.DetailD:
                        case PAPERSUB_TYPE.GirderBracketDetail:
                        case PAPERSUB_TYPE.Rafter:

                        case PAPERSUB_TYPE.Girder:
                        case PAPERSUB_TYPE.DetailA:
                        case PAPERSUB_TYPE.BracketDetail:

                        case PAPERSUB_TYPE.Table1:
                        case PAPERSUB_TYPE.Table2:
                        case PAPERSUB_TYPE.Table3:
                        case PAPERSUB_TYPE.Table4:
                            eachArea.visible = true;
                            break;

                    }
                }

            }

            if (roofType.Contains("CRT"))
            {
                foreach (PaperAreaModel eachArea in selPaperAreaModelList)
                {
                    switch (eachArea.SubName)
                    {
                        case PAPERSUB_TYPE.BottomPlateArrangement:
                        case PAPERSUB_TYPE.BottomPlateWeldingDetailC:
                        case PAPERSUB_TYPE.BottomPlateWeldingDetailD:
                        case PAPERSUB_TYPE.BottomPlateWeldingDetailBB:


                        //case PAPERSUB_TYPE.BottomPlateShellJointDetail:           // 2.5D

                        case PAPERSUB_TYPE.BottomPlateCuttingPlan:                 
                        case PAPERSUB_TYPE.AnnularPlateCuttingPlan:           
                        

                        case PAPERSUB_TYPE.RoofPlateArrangement:
                        case PAPERSUB_TYPE.RoofPlateWeldingDetailD:
                        case PAPERSUB_TYPE.RoofPlateWeldingDetailC:
                        case PAPERSUB_TYPE.RoofPlateWeldingDetailDD:
                        case PAPERSUB_TYPE.RoofCompressionRingJointDetail:

                        case PAPERSUB_TYPE.RoofPlateCuttingPlan:              
                        case PAPERSUB_TYPE.RoofCompressionRingCuttingPlan:    

                            eachArea.visible = true;
                            break;



                    }
                }
            }
            else if (roofType.Contains("DRT"))
            {
                foreach (PaperAreaModel eachArea in selPaperAreaModelList)
                {
                    switch (eachArea.SubName)
                    {

                        case PAPERSUB_TYPE.BottomPlateArrangement:
                        case PAPERSUB_TYPE.BottomPlateWeldingDetailC:
                        case PAPERSUB_TYPE.BottomPlateWeldingDetailD:
                        case PAPERSUB_TYPE.BottomPlateWeldingDetailBB:


                        //case PAPERSUB_TYPE.BottomPlateShellJointDetail:           // 2.5D

                        case PAPERSUB_TYPE.BottomPlateCuttingPlan:
                        case PAPERSUB_TYPE.AnnularPlateCuttingPlan:


                        case PAPERSUB_TYPE.RoofPlateArrangement:
                        case PAPERSUB_TYPE.RoofPlateWeldingDetailD:
                        case PAPERSUB_TYPE.RoofPlateWeldingDetailC:
                        case PAPERSUB_TYPE.RoofPlateWeldingDetailDD:
                        case PAPERSUB_TYPE.RoofCompressionRingJointDetail:

                        case PAPERSUB_TYPE.RoofPlateCuttingPlan:
                        case PAPERSUB_TYPE.RoofCompressionRingCuttingPlan:


                            eachArea.visible = true;
                            break;




                    }
                }
            }


                
        }

        public void SetDetailVisibleALL(List<PaperAreaModel> selPaperAreaModelList)
        {
            foreach (PaperAreaModel eachArea in selPaperAreaModelList)
            {
                if (eachArea.Name == PAPERMAIN_TYPE.GA1)
                {
                    eachArea.visible = true;
                }
                else if (eachArea.Name == PAPERMAIN_TYPE.GA1)
                {
                    eachArea.visible = true;
                }
                else if (eachArea.Name == PAPERMAIN_TYPE.ORIENTATION)
                {
                    eachArea.visible = true;
                }
                else if (eachArea.Name == PAPERMAIN_TYPE.DETAIL)
                {
                    eachArea.visible = true;
                }
            }
        }
    }
}
