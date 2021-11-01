using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.Commons
{
    public enum PAPERMAIN_TYPE
    {
        NotSet = 0,
        GA1,
        GA2,
        ORIENTATION,
        ShellPlateArrangement,
        CourseShellPlate,
        BottomPlateArrangement,
        BottomPlateCuttingPlan,
        RoofPlateArrangement,
        RoofPlateCuttingPlan,
        DETAIL,
        DetailOfRoofStructure
    }

    public enum PAPERSUB_TYPE
    {
        NotSet = 0,

        EmptyArea,

        HORIZONTALJOINT,

        ComRing,
        TopRingCuttingPlan,
        ComRingCuttingPlan,


        AnchorChair,
        AnchorDetail,

        TopAngleJoint,
        WindGirderJoint,

        SectionDD,

        VertJointDetail,

        DimensionsForCutting,
        ToleranceLimit,
        ShellPlateChordLength,


        ONECOURSESHELLPLATE,
        BOTTOMPLATEJOINT,

        ShellPlateArrangement,

        RoofArrange,

        NamePlateBracket,
        EarthLug,
        SettlementCheckPiece,

        // Bottom
        BottomPlateArrangement,
        BottomPlateJointAnnularDetail,
        BottomPlateJointDetail,
        BottomPlateWeldingDetailC,
        BottomPlateWeldingDetailD,
        BottomPlateWeldingDetailBB,
        BackingStripWeldingDetail,
        BottomPlateShellJointDetail,

        BottomPlateCuttingPlan,
        AnnularPlateCuttingPlan,
        BackingStrip,

        // Roof
        RoofPlateArrangement,
        RoofCompressionRingJointDetail,
        RoofCompressionWeldingDetail,
        RoofPlateWeldingDetailD,
        RoofPlateWeldingDetailC,
        RoofPlateWeldingDetailDD,

        RoofPlateCuttingPlan,
        RoofCompressionRingCuttingPlan,


        //Structure 공통
        RoofStructureOrientation,
        RoofStructureAssembly,

        //Strcuture Int
        CenteringIntRafterSideClipDetail,
        CenteringIntRafter,
        CenteringIntCenterRingDetail,
        CenteringIntRafterCenterClipDetail,
        CenteringIntPurlinDetail,
        CenteringIntPurlinSectionAA,
        CenteringIntRibPlateDetail,

        //Structure Ext
        CenteringExtCenterRingRafterDetail,
        CenteringExtCenterRingDetail,
        CenteringExtDetailB,
        CenteringExtSectionCC,
        CenteringExtViewC,
        CenteringExtRafterAndReinfPadCrossDetail,


        //Structure Column
        ColumnGirder,
        ColumnGirderDetailA,
        ColumnGirderBracketDetail,
        ColumnGirderTable1,
        ColumnGirderTable2,

        ColumnRafter,
        ColumnRafterSideClipDetail,
        ColumnRafterTable2,
        ColumnRafterRafterSideClipDetail,
        ColumnRafterTable3,
        ColumnRafterTable4,


        //Column Center
        CenterColumnDetail,

        ColumnCenterTopViewAA,
        ColumnCenterTopSectionBB,
        ColumnCenterTopDetailG,
        
        ColumnCenterBaseSectionCC,
        ColumnCenterBaseDrainDetail,
        ColumnCenterBaseSectionDD,
        ColumnCenterBaseDetailF,
        ColumnCenterBaseDetailE,


        //Clumn Side
        C2SideColumnDetail,
        C3SideColumnDetail,

        ColumnSideTopSectionAA,
        ColumnSideTopjViewAA,
        
        ColumnSideBaseDrainDetail,
        ColumnSideBaseSectionCC,
        ColumnSideBaseDetailF,
        ColumnSideBaseSectionDD,
        ColumnSideBaseDetailE,







    }


}