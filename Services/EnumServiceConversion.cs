using Dapper;
using System;
using System.Data;

public class EnumTypeHandler<T> : SqlMapper.TypeHandler<T> where T : struct, Enum
{
    public override void SetValue(IDbDataParameter parameter, T value)
    {
        parameter.Value = value.ToString(); // Convert enum to string
    }

    public override T Parse(object value)
    {
        return Enum.Parse<T>(value.ToString());
    }
}