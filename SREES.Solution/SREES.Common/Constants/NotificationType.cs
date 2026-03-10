namespace SREES.Common.Constants
{
    public enum NotificationType
    {
        OutageReported = 1,      // Kvar prijavljen
        OutageStatusChanged = 2, // Promena statusa kvara
        OutageResolved = 3,      // Kvar rešen
        OutageAssigned = 4,      // Kvar dodeljen
        SystemAlert = 5,         // Sistemsko obaveštenje
        MaintenancePlanned = 6   // Planirano održavanje
    }
}
