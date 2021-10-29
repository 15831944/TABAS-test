using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructurePurlinModel
    {
        public StructurePurlinModel()
        {
            Radius = 0;

            AngleFromCenter = 0;
            AngleOne = 0;

            Width = 0;
            Length = 0;


            PurlinSideAngle = 0;
            TopViewOuterRadius = 0;
            TopViewInerRadius = 0;
            TopViewOuterLength = 0;
            TopViewInnerLength = 0;

            Size = ""; // Angle
        }

        public double Radius { get; set; }

        public double AngleFromCenter { get; set; }

        public double AngleOne { get; set; }

        public double Width { get; set; }
        public double Length { get; set; }




        public double PurlinSideAngle { get; set; }

        public double TopViewOuterRadius { get; set; }
        public double TopViewInerRadius { get; set; }

        public double TopViewOuterLength { get; set; }
        public double TopViewInnerLength { get; set; }

        // 형상 정보
        public string Size { get; set; }
    }
}
