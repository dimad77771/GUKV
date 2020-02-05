using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// DisposableScope class may be used to implement commit/rollback semantics of regular
/// database transactions applicable to .NET objects.
/// </summary>
public sealed class DisposableScope : IDisposable
{
    public enum DefaultDisposeAction
    {
        Rollback,
        Commit,
    }

    private readonly List<Action> _commitHandlers = new List<Action>();
    public event Action OnCommit
    { 
        add
        {
            _commitHandlers.Add(value);
        }
        remove
        {
            _commitHandlers.Remove(value);
        }
    }

    private readonly List<Action> _rollbackHandlers = new List<Action>();
    public event Action OnRollback
    {
        add
        {
            _rollbackHandlers.Add(value);
        }
        remove
        {
            _rollbackHandlers.Remove(value);
        }
    }

    public DefaultDisposeAction DefaultAction { get; set; }

    public void Commit()
    {
        _commitHandlers.ForEach(x => x());
        _commitHandlers.Clear();
        _rollbackHandlers.Clear();
    }

    public void Rollback()
    {
        _rollbackHandlers.ForEach(x => x());
        _rollbackHandlers.Clear();
        _commitHandlers.Clear();
    }

    #region IDisposable Members

    public void Dispose()
    {
        switch (DefaultAction)
        {
            case DefaultDisposeAction.Commit:
                Commit();
                break;
            case DefaultDisposeAction.Rollback:
                Rollback();
                break;
        }
    }

    #endregion
}