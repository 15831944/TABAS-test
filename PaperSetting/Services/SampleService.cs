﻿using PaperSetting.Commons;
using PaperSetting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PaperSetting.Services
{
    public static class SampleService
    {
        public static ObservableCollection<PaperDwgModel> CreatePaperSample()
        {
            ObservableCollection<PaperDwgModel> newModel = new ObservableCollection<PaperDwgModel>();
            List<string> dwgList = GetDWGList();

            Random r = new Random();
            double tempX = 0;

            int paperNo = 0;
            foreach(string eachDwg in dwgList)
            {
                paperNo++;
                PaperDwgModel newPaper = new PaperDwgModel();
                // SheetSize
                newPaper.SheetSize.Width = 841;
                newPaper.SheetSize.Height = 594;

                


                // Basic
                newPaper.Basic.No = paperNo.ToString();
                newPaper.Basic.Title = eachDwg;
                newPaper.Basic.DwgNo = "VP-210220-MF-" + paperNo.ToString("000");
                newPaper.Basic.StampName = "AS BUILT";


                int subCount = 2;
                for(int i = 1; i <= subCount; i++)
                {
                    // Revision
                    PaperRevisionModel newRevision = new PaperRevisionModel();
                    newRevision.RevString = i.ToString();
                    newRevision.DateString = "JAN." + i.ToString("00") + ".2021";
                    newRevision.Description = "FOR INFORMATION";
                    newRevision.REVD = "J.Y.HAM";
                    newRevision.CHKD = "S.Y.PARK";
                    newRevision.APVD = "M.G.JEONG";
                    newPaper.Revisions.Add(newRevision);

                    // Table
                    PaperTableModel newTable = new PaperTableModel();
                    newTable.No = i.ToString();
                    newTable.Name = "Table" + i.ToString("00");
                    newTable.TableSelection = "CustomTable" + i.ToString("00");
                    TableCustom(newTable, i);
                    newPaper.Tables.Add(newTable);



                    // Note 
                    PaperNoteModel newNote = new PaperNoteModel();
                    newNote.No = i.ToString();
                    newNote.Name = "Note" + i.ToString("00");
                    newNote.Note = "CustomNote" + i.ToString("00");
                    NoteCustom(newNote, i);
                    newPaper.Notes.Add(newNote);
                }

                subCount = 4;
                for (int i = 1; i <= subCount; i++)
                {
                    // Viewport
                    PaperViewportModel newViewport = new PaperViewportModel();
                    newViewport.No = i.ToString();
                    newViewport.Name = "ViewPort" + i.ToString("00");
                    newViewport.AssemblySelection = "CustomAssembly" + i.ToString("00");

                    tempX = r.Next(0, 140);
                    tempX += 80;
                    Debug.WriteLine(tempX.ToString());

                    if (paperNo == 1)
                    {
                        ViewPortModel cuView = new ViewPortModel();
                        cuView.ScaleStr = "90";
                        cuView.TargetX = "17000";
                        cuView.TargetY = "14000";
                        cuView.LocationX = "330";
                        cuView.LocationY = "360";
                        cuView.SizeX = "600";
                        cuView.SizeY = "400";

                        newViewport.ViewPort = cuView;
                        newPaper.ViewPorts.Add(newViewport);
                    }
                    else
                    {
                        ViewPortCustom(newViewport, i, tempX);
                        newPaper.ViewPorts.Add(newViewport);
                    }
                }


                newModel.Add(newPaper);
            }
            return newModel;
        }
        public static void ViewPortCustom(PaperViewportModel newViewPort,int selCount,double tempX)
        {

            switch (selCount)
            {
                case 0:
                    newViewPort.ViewPort.Location.X = 15 + 15 + 110;
                    newViewPort.ViewPort.Location.Y = 297 - 15 - 15 - 80;
                    newViewPort.ViewPort.Size.Width = 190;
                    newViewPort.ViewPort.Size.Height = 140;
                    break;
                case 1:
                    newViewPort.ViewPort.Location.X = tempX;
                    newViewPort.ViewPort.Location.Y = 100;
                    newViewPort.ViewPort.Size.Width = 30;
                    newViewPort.ViewPort.Size.Height = 30;
                    break;
                case 2:
                    newViewPort.ViewPort.Location.X = tempX;
                    newViewPort.ViewPort.Location.Y = 140;
                    newViewPort.ViewPort.Size.Width = 30;
                    newViewPort.ViewPort.Size.Height = 30;

                    //newViewPort.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                    //newViewPort.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                    //newViewPort.Dock.DockPriority = 3;
                    break;
                case 3:
                    newViewPort.ViewPort.Location.X = tempX;
                    newViewPort.ViewPort.Location.Y = 180;
                    newViewPort.ViewPort.Size.Width = 30;
                    newViewPort.ViewPort.Size.Height = 30;
                    break;
                case 4:
                    newViewPort.ViewPort.Location.X = tempX;
                    newViewPort.ViewPort.Location.Y = 240;
                    newViewPort.ViewPort.Size.Width = 30;
                    newViewPort.ViewPort.Size.Height = 30;
                    break;
            }
        }
        public static void NoteCustom(PaperNoteModel newNote,int selCount)
        {
            switch (selCount)
            {
                case 1:
                    newNote.Location.X = 40;
                    newNote.Location.Y = 40;
                    newNote.Size.Width = 60;
                    newNote.Size.Height = 30;

                    newNote.Dock.DockPosition = DOCKPOSITION_TYPE.BOTTOM;
                    newNote.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.LEFT;
                    newNote.Dock.DockPriority = 2;
                    break;
                case 2:
                    newNote.Location.X = 80;
                    newNote.Location.Y = 80;
                    newNote.Size.Width = 60;
                    newNote.Size.Height = 30;

                    newNote.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                    newNote.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                    newNote.Dock.DockPriority = 2;
                    break;
                case 3:
                    newNote.Location.X = 120;
                    newNote.Location.Y = 120;
                    newNote.Size.Width = 60;
                    newNote.Size.Height = 30;
                    break;
                case 4:
                    newNote.Location.X = 160;
                    newNote.Location.Y = 160;
                    newNote.Size.Width = 60;
                    newNote.Size.Height = 30;
                    break;
            }

            newNote.Note = "NOTE " + selCount +"." + "\n";
            newNote.Note += "1. ALL DIMENSIONS ARE IN MM UNLESS OTHERWISE NOTED." +"\n";
            newNote.Note += "2. DIMENSION \"H\" FOR SHELL NOZZLES IS MEASURED FROM UNDERSIDE OF BASE TO CENTER LINE OF NOZZLE." + "\n";
            newNote.Note += "3. DESIGN PRESSURE : FULL OF WATER + 200 / -50 mmH20" + "\n";
            newNote.Note += "4. PAINTING" + "\n";
            newNote.Note += "   - EXTERNAL (SHELL, ROOF)" + "\n";
            newNote.Note += "     SURFACE PREPARATION : SSPC-SP-10" + "\n";
            newNote.Note += "   - INTERNAL : NONE" + "\n";
            newNote.Note += "20. GASKET" + "\n";
            newNote.Note += "   1) FOR STANDARD FLANGED" + "\n";
            newNote.Note += "      - SPIRAL WOUND GASKET" + "\n";
            newNote.Note += "      - FILLER : GRAPHITE" + "\n";
            newNote.Note += "      - OUTER RING : C.S" + "\n";
        }
        public static void TableCustom(PaperTableModel newTable, int selCount)
        {

            for(int i = 0; i < 10; i++)
            {
                string[] newRow = new string[4];
                for (int j = 0; j < newRow.Length; j++)
                    newRow[j] = "Table R:" + i + " C:" + j;
                newTable.TableList.Add(newRow);
            }
            switch (selCount)
            {
                case 1:
                    newTable.Location.X = 40;
                    newTable.Location.Y = 40;
                    newTable.Size.Width = 80;
                    newTable.Size.Height = 30;

                    newTable.Dock.DockPosition = DOCKPOSITION_TYPE.BOTTOM;
                    newTable.Dock.HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.LEFT;
                    newTable.Dock.DockPriority = 1;
                    break;
                case 2:
                    newTable.Location.X = 80;
                    newTable.Location.Y = 80;
                    newTable.Size.Width = 140;
                    newTable.Size.Height = 30;

                    newTable.Dock.DockPosition = DOCKPOSITION_TYPE.RIGHT;
                    newTable.Dock.VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
                    newTable.Dock.DockPriority = 1;
                    break;
                case 3:
                    newTable.Location.X = 120;
                    newTable.Location.Y = 120;
                    newTable.Size.Width = 100;
                    newTable.Size.Height = 30;
                    break;
                case 4:
                    newTable.Location.X = 160;
                    newTable.Location.Y = 160;
                    newTable.Size.Width = 60;
                    newTable.Size.Height = 30;
                    break;
            }


        }
        public static List<string> GetDWGList()
        {

            List<string> newList = new List<string>();
            newList.Add("GENERAL ASSEMBLY");
            newList.Add("ORIENTATION");
            newList.Add("SHELL PLATE ARRANGE");
            newList.Add("1st COURSE SHELL PLATE");
            newList.Add("BOTTOM PLATE ARRANGE");

            newList.Add("ROOF PLATE ARRANGE");
            newList.Add("ROOF STRUCTURE");
            newList.Add("SHELL MANHOLE");
            newList.Add("ROOF MANHOLE");
            newList.Add("NOZZLE&NOZZLE ACCESSORY");

            newList.Add("LOADING DATA");
            newList.Add("SHELL PLATE DEVELOPMENT");
            newList.Add("CLEANOUT DOOR");
            newList.Add("WATER SPRAY SYSTEM");
            newList.Add("AIRFOAM CHAMBER CONN.");

            newList.Add("AIRFOAM CHAMBER PLATFORM");
            newList.Add("STAIRWAY");
            newList.Add("LADDER");
            newList.Add("ROOF HANDRAIL");
            newList.Add("INSULATION SUPPORT");

            newList.Add("EXE. PIPE SUPPORT CLIP");
            newList.Add("NAME PLATE");

            return newList;
        }
    }
}
