namespace SREES.Common.Constants
{
    public enum OutageLevel
    {
        Building = 1,      // Kvar u zgradi/ku?noj instalaciji
        Pole = 2,          // Kvar na stubu
        Feeder = 3,        // Kvar na fideru
        Substation = 4,    // Kvar na trafostanici
        Region = 5         // Kvar na nivou regiona
    }
}
