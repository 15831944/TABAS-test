using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructureRafterModel
    {
        public StructureRafterModel()
        {
            AngleFromCenter = 0;

            AngleOne = 0;
            LengthTopView = 0;
            Length = 0;
            Height = 0;

            InnerRealRadius = 0;
            OuterRealRadius = 0;
            InnerTopViewRadius = 0;

            InnerClipRadiusFromCenter = 0;
            OuterClipRadiusFromCenter = 0;

            CenteringReduceRadius = 0;

             Size = "";
        }

        public double AngleFromCenter { get; set; }

        public double AngleOne { get; set; }
        public double Length { get; set; }
        public double LengthTopView { get; set; }

        public double Height { get; set; }


        // Rafter Upper : Inner
        public double InnerRealRadius { get; set; }
        // Rafter Upper : Outer
        public double OuterRealRadius { get; set; }
        // Rafter Lower : Inner
        public double InnerTopViewRadius{ get; set; }


        public double InnerClipRadiusFromCenter { get; set; }
        public double OuterClipRadiusFromCenter { get; set; }


        public double CenteringReduceRadius { get; set; }

        // 형상정보
        public string Size { get; set; }
    }
}
