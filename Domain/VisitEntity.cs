using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class VisitEntity
    {
        public Guid Id { get; private set; }
        public Guid PersonId { get; private set; }
        public DateTime EntryTime { get; private set; }
        public DateTime? ExitTime { get; private set; }
        public PersonEntity? Person { get; private set; }


        public bool isActive => ExitTime == null;
        public TimeSpan? Duration => ExitTime.HasValue ? ExitTime.Value - EntryTime : null;



        public VisitEntity(Guid personId, DateTime? entryTime = null)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentException("El ID de la persona no puede estar vacío", nameof(personId));
            }

            Id = Guid.NewGuid();
            PersonId = personId;
            EntryTime = entryTime ?? DateTime.UtcNow;
            ExitTime = null;
        }


        public void RegisterExit(DateTime? exitTime = null)
        {
            var exit = exitTime ?? DateTime.UtcNow;

            if (ExitTime.HasValue)
            {
                throw new InvalidOperationException("La salida ya fué registrada con anterioridad.");
            }

            if (exit <= EntryTime)
            {
                throw new ArgumentException("La hora de salida debe ser posterior a la hora de entrada", nameof(exitTime));
            }

            ExitTime = exit;
        }
    }
}
