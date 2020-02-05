using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;
using System.IO;

[SqlUserDefinedAggregate(Format.UserDefined, MaxByteSize = -1)]
public struct Concatenate : IBinarySerialize
{
    public const string ItemDelimiter = ",";

    private StringBuilder values;

    public void Init()
    {
        values = new StringBuilder();
    }

    public void Accumulate(SqlString Value)
    {
        if (values.Length > 0)
            values.Append(ItemDelimiter);
        values.Append(Value.Value);
    }

    public void Merge(Concatenate Group)
    {
        if (Group.values.Length > 0)
        {
            if (values.Length > 0)
                values.Append(ItemDelimiter);
            values.Append(Group.values);
        }
    }

    public SqlString Terminate()
    {
        return values.ToString();
    }

    public void Read(BinaryReader r)
    {
        if (!r.ReadBoolean())
            values = new StringBuilder(r.ReadString());
    }

    public void Write(BinaryWriter w)
    {
        if (values == null)
        {
            w.Write(true);
        }
        else
        {
            w.Write(false);
            w.Write(values.ToString());
        }
    }
}
