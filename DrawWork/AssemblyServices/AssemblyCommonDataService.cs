using AssemblyLib.AssemblyModels;
using DrawWork.Commons;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.AssemblyServices
{
    public class AssemblyCommonDataService
    {
        private AssemblyModel assemblyData;
        private ValueService valueService;

        // Tank
        public TANK_TYPE TankType { get; set; }

        // Basic
        public double TankHeight { get; set; }
        public double TankID { get; set; }
        public double TankInRadius { get; set; }

        // Roof
        public double RoofSlopeRadian { get; set; }

        public TopAngle_Type TopAngleType { get; set; }


        // Bottom
        public double BottomSlopeRadian { get; set; }

        public double BottomOD { get; set; }
        public double AnnularOD { get; set; }
        public double AnnularID { get; set; }
        public bool IsAnnular { get; set; }

        // Shell
        public double ShellMaxCourse { get; set; }

        public double ShellLastCourseThk { get; set; }


        // Structure 
        public StructureSupporting_Type StructureSupprtType {get;set;}

        // Etc : Fix : Value
        public double TopAngleTypeLastCourseAdjHeight { get; set; }

        public AssemblyCommonDataService(AssemblyModel assembly)
        {
            valueService=new ValueService();
            assemblyData = assembly;

            SetCommonValue();
            CreateFixValue();

            CalculationCommonValue();
        }
        public void SetCommonValue()
        {
            // Tank
            TankType = TANK_TYPE.NotSet;

            // Baisc
            TankHeight = 0;
            TankID = 0;
            TankInRadius = 0;


            // Roof
            RoofSlopeRadian = 0;
            TopAngleType = TopAngle_Type.NotSet;

            // Bottom
            BottomSlopeRadian = 0;
            BottomOD = 0;
            AnnularID = 0;
            AnnularOD = 0;
            IsAnnular = false;

            // Shell
            ShellMaxCourse = 0;
            ShellLastCourseThk = 0;

            // StructureSupprtType

            StructureSupprtType = StructureSupporting_Type.NotSet;

        }

        public void CalculationCommonValue()
        {
            // Tank
            TankType = GetTankType();

            // Baisc
            TankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            TankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            TankInRadius = TankID/2;

            // Roof
            RoofSlopeRadian = GetRoofSlope();
            TopAngleType = GetTopAngleType();

            // Bottom
            string annularYes = assemblyData.BottomInput[0].AnnularPlate;
            if (annularYes.ToLower().Contains("yes"))
                IsAnnular = true; ;
            
            // Shell
            if (assemblyData.ShellOutput.Count > 0)
                ShellMaxCourse = assemblyData.ShellOutput.Count - 1;
            ShellLastCourseThk = GetShellLastCourseThk();

            // Structure
            StructureSupprtType = GetStructureSupportingType();


        }

        public void CreateFixValue()
        {
            TopAngleTypeLastCourseAdjHeight = 10;
        }



        #region Calculation Function
        private TANK_TYPE GetTankType()
        {
            TANK_TYPE returnValue = TANK_TYPE.NotSet;
            switch (assemblyData.GeneralDesignData[0].RoofType.ToLower())
            {
                case "ccrt":
                case "crt":
                    returnValue = TANK_TYPE.CRT;
                    break;
                case "drt":
                    returnValue = TANK_TYPE.DRT;
                    break;
                case "ifrt":
                    returnValue = TANK_TYPE.IFRT;
                    break;
                case "efrt_singledeck":
                    returnValue = TANK_TYPE.EFRTSingle;
                    break;
                case "efrt_doubledeck":
                    returnValue = TANK_TYPE.EFRTDouble;
                    break;
            }

            return returnValue;
        }

        private double GetRoofSlope()
        {
            double returnValue = 0;
            switch (TankType)
            {
                case TANK_TYPE.CRT:
                    returnValue = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
                    break;
                case TANK_TYPE.DRT:

                    break;
            }
            return returnValue;
        }
        private TopAngle_Type GetTopAngleType()
        {
            TopAngle_Type retunValue = TopAngle_Type.NotSet;
            string topAngleType = assemblyData.RoofCompressionRing[0].CompressionRingType;

            switch (topAngleType)
            {
                case "Detail b":
                    retunValue = TopAngle_Type.b;
                    break;
                case "Detail d":
                    retunValue = TopAngle_Type.d;
                    break;
                case "Detail e":
                    retunValue = TopAngle_Type.e;
                    break;
                case "Detail i":
                    retunValue = TopAngle_Type.i;
                    break;
                case "Detail k":
                    retunValue = TopAngle_Type.k;
                    break;
            }


            return retunValue;
        }

        private double GetShellLastCourseThk()
        {
            double returnValue = 0;
            switch (TopAngleType)
            {
                case TopAngle_Type.b:
                case TopAngle_Type.d:
                case TopAngle_Type.e:
                case TopAngle_Type.i:
                    if (assemblyData.ShellOutput.Count > 1)
                    {
                        returnValue = valueService.GetDoubleValue(assemblyData.ShellOutput[assemblyData.ShellOutput.Count - 1].Thickness);
                    }
                    break;
                case TopAngle_Type.k:
                    if (assemblyData.ShellOutput.Count > 1)
                    {
                        returnValue = valueService.GetDoubleValue(assemblyData.ShellOutput[assemblyData.ShellOutput.Count - 2].Thickness);
                    }
                    break;
            }

            return returnValue;
        }

        private StructureSupporting_Type GetStructureSupportingType()
        {
            StructureSupporting_Type returnValue = StructureSupporting_Type.SelfSupporting;

            string structureSupport = "";
            switch (SingletonData.TankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.IFRT:
                    structureSupport = assemblyData.StructureCRTInput[0].SupportingType.ToLower();
                    break;
                case TANK_TYPE.DRT:
                    structureSupport = assemblyData.StructureDRTInput[0].SupportingType.ToLower();
                    break;
                case TANK_TYPE.EFRTSingle:
                    break;
                case TANK_TYPE.EFRTDouble:
                    break;
            }

            // Structure Type

            switch (structureSupport)
            {
                case "self-supporting":
                    returnValue = StructureSupporting_Type.SelfSupporting;
                    break;
                case "rafter only (internal)":
                    returnValue = StructureSupporting_Type.RafterOnlyInternal;
                    break;
                case "rafter only (external)":
                    returnValue = StructureSupporting_Type.RafterOnlyExternal;
                    break;
                case "rafter w/column":
                    returnValue = StructureSupporting_Type.RafterWColumn;
                    break;
                case "rafter w/column & girder":
                    returnValue = StructureSupporting_Type.RafterWColumnGirder;
                    break;
            }


            return returnValue;
        }
        #endregion




        #region Shell Plate Course
        public List<double> GetShellCourseWidthForDrawing()
        {
            List<double> newList = new List<double>();

            foreach (ShellOutputModel eachCourse in assemblyData.ShellOutput)
            {
                newList.Add(valueService.GetDoubleValue(eachCourse.PlateWidth));
            }

            TopAngle_Type topAngle = TopAngleType;
            if (topAngle == TopAngle_Type.b)
            {
                int maxCount = newList.Count - 1;
                newList[maxCount] -= TopAngleTypeLastCourseAdjHeight;
            }
            if (topAngle == TopAngle_Type.k)
            {
                if (newList.Count > 0)
                    newList.RemoveAt(newList.Count - 1);
            }

            return newList;
        }
        public List<double> GetShellCourseThickneeForDrawing()
        {
            List<double> newList = new List<double>();

            foreach (ShellOutputModel eachCourse in assemblyData.ShellOutput)
            {
                newList.Add(valueService.GetDoubleValue(eachCourse.Thickness));
            }
            TopAngle_Type topAngle = TopAngleType;
            if (topAngle == TopAngle_Type.k)
            {
                if (newList.Count > 0)
                    newList.RemoveAt(newList.Count - 1);
            }
            return newList;
        }
        #endregion

    }
}
