using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSalesAPI.Converters
{
    public class ByteCompare
    {
        public static bool ByteArrayCompare(byte[] b1, byte[] b2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(b1, b2);
        }
    }
}