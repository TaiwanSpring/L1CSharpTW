//----------------------------------------------------------------------------------------
//	Copyright © 2007 - 2021 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class includes methods to convert Java rectangular arrays (jagged arrays
//	with inner arrays of the same length).
//----------------------------------------------------------------------------------------
internal static class RectangularArrays
{
    public static bool[][] RectangularBoolArray(int size1, int size2)
    {
        bool[][] newArray = new bool[size1][];
        for (int array1 = 0; array1 < size1; array1++)
        {
            newArray[array1] = new bool[size2];
        }

        return newArray;
    }

    public static sbyte[][] RectangularSbyteArray(int size1, int size2)
    {
        sbyte[][] newArray = new sbyte[size1][];
        for (int array1 = 0; array1 < size1; array1++)
        {
            newArray[array1] = new sbyte[size2];
        }

        return newArray;
    }
}