using System;
using System.Collections.Generic;
using System.Linq;
using Currency;
using Factories;
using Shop;
using TerminalCommands;
using TerminalCommands.Implementations;
using UnityEngine;


namespace StaticData.Data
{
    public class TerminalCommandsService
    {
        private readonly TerminalCommandsFactory terminalCommandsFactory;
        private readonly Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();
        private bool isInitialized;


        public TerminalCommandsService(TerminalCommandsFactory terminalCommandsFactory)
        {
            this.terminalCommandsFactory = terminalCommandsFactory;
        }


        public void Init()
        {
            if (!isInitialized)
            {
                RegisterCommand(terminalCommandsFactory.CreateBuyCommand());
                RegisterCommand(terminalCommandsFactory.CreateClearCommand());
                
                isInitialized = true;
            }
        }


        public CommandResult TryParseCommand(string input)
        {
            if (input == null)
            {
                CommandResult noCommandDetectedResult = new CommandResult(false, "> No command was detected");
                return noCommandDetectedResult;
            }

            string[] splitInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (splitInput.Length == 0)
            {
                CommandResult noCommandDetectedResult = new CommandResult(false, "> No command was detected");
                return noCommandDetectedResult;
            }

            string commandName = splitInput[0];

            string[] args = splitInput.Skip(1).ToArray();

            foreach (KeyValuePair<string, ICommand> commandPair in commands)
            {
                if (commandPair.Key == commandName)
                {
                    return commandPair.Value.Execute(args);
                }
            }

            CommandResult commandWasNotFoundResult = new CommandResult(false, $"> Command '{commandName}' not found");
            return commandWasNotFoundResult;
        }


        private void RegisterCommand(ICommand command)
        {
            bool addedSuccessfully = commands.TryAdd(command.Name, command);

            if (!addedSuccessfully)
            {
                Debug.LogError($"Command with name {command.Name} already exists");
                return;
            }

            foreach (string alias in command.Aliases)
            {
                addedSuccessfully = commands.TryAdd(alias, command);

                if (!addedSuccessfully)
                {
                    Debug.LogError($"Command with name {alias} already exists");
                    return;
                }
            }
        }
    }
}
