using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// This interface groups operations common to all elements of the Rishennya project tree
/// (the main document, appendices, items and sub-items)
/// </summary>
public interface IRishProjectNode
{
    int ID { get; }

    int ParentID { get; set; }

    string DisplayNodeType { get; }

    string DisplayNumber { get; }

    string DisplayText { get; }

    int OrdinalPos { get; set; }

    /// <summary>
    /// A callback invoked by RishProjectDataSource before the node is removed from the
    /// datasource. The node must dispose of its data members (e.g. if backed up by a
    /// database entry, specific instructions must be submitted to the database context
    /// to delete affected database entries upon commit).
    /// </summary>
    void BeforeRemove();
}
