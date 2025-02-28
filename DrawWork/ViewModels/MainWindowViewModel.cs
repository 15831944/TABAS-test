﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using DrawWork.CommandModels;
using DrawWork.CommandServices;
using DrawWork.DrawBuilders;

using AssemblyLib.AssemblyModels;

namespace DrawWork.ViewModels
{
    public class MainWindowViewModel
    {

        public AssemblyModel TankData;
        public BasicCommandModel commandData;
        private CommandBasicService commandService;

        public MainWindowViewModel()
        {
            TankData = new AssemblyModel();
            commandData = new BasicCommandModel();


            // Mapping Data
            TankData.CreateMappingAssembly();

            // Sample Data
            //TankData.CreateSampleAssembly();
            //commandData.CreateSampleCommandModel();

        }

        public void CreateCommandService(Object selModel)
        {
            commandService = new CommandBasicService(commandData.commandList, TankData, selModel);
        }

        public LogicBuilder GetLogicBuilder(double selScale)
        {
            // Scale 1:200
            //commandService.SetScaleData(selScale);

            commandService.SetCommandData(commandData.commandList);

            // Create Entiti
            commandService.ExecuteCommand("GA");
            LogicBuilder logicB = new LogicBuilder(commandService.commandEntities);

            return logicB;
        }
    }
}
