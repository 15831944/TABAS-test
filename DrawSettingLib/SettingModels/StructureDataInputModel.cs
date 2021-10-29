using AssemblyLib.AssemblyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructureDataInputModel
    {
        public StructureDataInputModel()
        {
            newRafterInputList = new List<StructureCRTRafterInputModel>();
            newColumnInputList = new List<StructureCRTColumnInputModel>();
            newGirderInputList = new List<StructureCRTGirderInputModel>();

            newGirderHBeamList = new List<HBeamModel>();
            newRafterOutputList = new List<NColumnRafterModel>();
            newColumnCenterTopSupportList = new List<NColumnCenterTopSupportModel>();
            newColumnSideTopSupportList = new List<NColumnSideTopSupportModel>();
            newColumnGirderList = new List<NColumnGirderModel>();
            newColumnBaseSupportList = new List<NColumnBaseSupportModel>();

            newColumnPipeList = new List<PipeModel>();


            // CRT Centering
            newCenteringInputList = new List<StructureCRTCenteringInputModel>();
            newCenteringInternalInputList = new List<NCenteringInternalModel>();
            newRafterCenteringInternalInputList = new List<NRafterCenteringInternalModel>();
            newPurlinInput = new AngleSizeModel();
        }
        
        // CRT Column
        public List<StructureCRTRafterInputModel> newRafterInputList { get; set; }

        public List<StructureCRTColumnInputModel> newColumnInputList { get; set; }
        public List<StructureCRTGirderInputModel> newGirderInputList { get; set; }
        public List<HBeamModel> newGirderHBeamList { get; set; }
        public List<NColumnRafterModel> newRafterOutputList {get;set;}

        public List<NColumnCenterTopSupportModel> newColumnCenterTopSupportList { get; set; }
        
        public List<NColumnSideTopSupportModel> newColumnSideTopSupportList { get; set; }

        public List<NColumnGirderModel> newColumnGirderList { get; set; }

        public List<NColumnBaseSupportModel> newColumnBaseSupportList { get; set; }

        public List<PipeModel> newColumnPipeList { get; set; }


        // CRT Centering
        public List<StructureCRTCenteringInputModel> newCenteringInputList { get; set; }
        public List<NCenteringInternalModel> newCenteringInternalInputList{ get;set; }
        public List<NRafterCenteringInternalModel> newRafterCenteringInternalInputList { get; set; }

        public AngleSizeModel newPurlinInput { get; set; }
    }
}
