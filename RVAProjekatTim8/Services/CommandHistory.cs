using RVAProjekatTim8.Enums;
using RVAProjekatTim8.Interfaces;
using System;
using System.Collections.Generic;

namespace RVAProjekatTim8.Services
{
    public class CommandHistoryChangedEventArgs : EventArgs
    {
        public IUndoableCommand Command { get; }
        public CommandAction Action { get; }

        public CommandHistoryChangedEventArgs(IUndoableCommand command, CommandAction action)
        {
            Command = command;
            Action = action;
        }
    }

    public class CommandHistory
    {
        private readonly Stack<IUndoableCommand> _undoStack = new();
        private readonly Stack<IUndoableCommand> _redoStack = new();

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        /// <summary>
        /// Izaziva se nakon svake promene stanja (Execute/Undo/Redo) da bi
        /// ViewModel mogao da ažurira CanExecute status svojih komandi.
        /// </summary>
        public event EventHandler HistoryChanged;

        public event EventHandler<CommandHistoryChangedEventArgs> CommandStateChanged;

        public void ExecuteCommand(IUndoableCommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
            RaiseEvents(command, CommandAction.Executed);
        }

        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
            RaiseEvents(command, CommandAction.Executed);
        }

        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
            RaiseEvents(command, CommandAction.Executed);
        }

        private void RaiseEvents(IUndoableCommand command, CommandAction action)
        {
            HistoryChanged?.Invoke(this, EventArgs.Empty);
            CommandStateChanged?.Invoke(this, new CommandHistoryChangedEventArgs(command, action));
        }
    }
}
