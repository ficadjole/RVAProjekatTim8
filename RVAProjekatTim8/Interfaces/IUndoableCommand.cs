namespace RVAProjekatTim8.Interfaces
{
    public interface IUndoableCommand
    {
        void Execute();
        void Undo();

        string GetDescription();
    }
}
