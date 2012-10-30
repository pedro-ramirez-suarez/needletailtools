using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Needletail.DataAccess.Engines
{
    public class Converters
    {


        public static DbType GetDBTypeFor(object value)
        {
            if (value == null)
                return DbType.String;
            Type t = value.GetType();
            if (t == typeof(int) )
                return DbType.Int32;
            else if (t == typeof(decimal) || t == typeof(float) || t == typeof(Int64))
                return DbType.Decimal;
            else if (t == typeof(DateTime))
                return DbType.DateTime;
            else if (t == typeof(bool))
                return DbType.Boolean;
            else if (t == typeof(Guid))
                return DbType.Guid;
            return DbType.String;

        }
    }
}
