namespace SPO.Enums
{
    public enum Grade
    {
        Deset = 10,
        Devet = 9,
        Osum = 8,
        Sedum = 7,
        Shest = 6,
        Pet = 5
    }

    public static class GradeExtension
    {
        public static int ToNumber(this Grade grade)
        {
            switch (grade) {
                case Grade.Deset:
                    return 10;
                case Grade.Devet:
                    return 9;
                case Grade.Osum:
                    return 8;
                case Grade.Sedum:
                    return 7;
                case Grade.Shest:
                    return 6;
                default:
                    return 5;
            }
        }
    }
}