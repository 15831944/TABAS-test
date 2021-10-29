using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructureCRTColumnModel
    {
        public StructureCRTColumnModel()
        {

            Radius = 0;
            LayerList = new List<StructureLayerModel>();
            strData = new StructureDataInputModel();
        }

        public double Radius { get; set; }

        public List<StructureLayerModel> LayerList { get; set; }

        public StructureDataInputModel strData { get; set; }
    }
}
