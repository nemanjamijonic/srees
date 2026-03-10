using SREES.Common.Constants;
using SREES.Common.Models;

namespace SREES.DAL.Models
{
    public class Notification : BaseModel
    {
        /// <summary>
        /// ID korisnika koji prima obaveštenje
        /// </summary>
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        /// <summary>
        /// ID kupca koji prima obaveštenje (opciono, ako je povezan sa korisnikom)
        /// </summary>
        public int? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        /// <summary>
        /// ID kvara na koji se odnosi obaveštenje
        /// </summary>
        public int? OutageId { get; set; }
        public virtual Outage? Outage { get; set; }

        /// <summary>
        /// Naslov obaveštenja
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Sadržaj obaveštenja
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Tip obaveštenja
        /// </summary>
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Da li je obaveštenje pro?itano
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// Datum kada je obaveštenje pro?itano
        /// </summary>
        public DateTime? ReadAt { get; set; }
    }
}
