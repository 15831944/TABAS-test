﻿using DrawSettingLib.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class PaperModel
    {
        public PaperModel()
        {
            DWGName = PAPERMAIN_TYPE.NotSet;

            Page = 1;
            ColumnDef = 1;
            RowDef = 1;

            IsFloating = false;

            // Ref
            //PaperSheet = null;
        }



        private PAPERMAIN_TYPE _DWGName;
        public PAPERMAIN_TYPE DWGName
        {
            get { return _DWGName; }
            set
            {
                _DWGName = value;
                //OnPropertyChanged(nameof(DWGName));
            }
        }

        private double _Page;
        public double Page
        {
            get { return _Page; }
            set
            {
                _Page = value;
                //OnPropertyChanged(nameof(Page));
            }
        }


        private double _ColumnDef;
        public double ColumnDef
        {
            get { return _ColumnDef; }
            set
            {
                _ColumnDef = value;
                //OnPropertyChanged(nameof(ColumnDef));
            }
        }

        private double _RowDef;
        public double RowDef
        {
            get { return _RowDef; }
            set
            {
                _RowDef = value;
                //OnPropertyChanged(nameof(RowDef));
            }
        }

        private bool _IsFloating;
        public bool IsFloating
        {
            get { return _IsFloating; }
            set
            {
                _IsFloating = value;
                //OnPropertyChanged(nameof(IsFloating));
            }
        }

    }
}
