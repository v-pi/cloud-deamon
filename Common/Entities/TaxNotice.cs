using CloudDaemon.Common.Interfaces;
using System;

namespace CloudDaemon.Common.Entities
{
    public class TaxNotice : IHasSubject, IHasMessage
    {
        private const string MessageFormat = @"
Un nouvel avis d'imposition est à payer sur http://www.impots.gouv.fr/
Id : {0}
Montant : {1}
Date limite de payement : {2:dd/MM/yyyy}";

        public long Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Message { get { return String.Format(MessageFormat, Id, Amount, PaymentDate); } }

        public string Subject { get { return "Nouvel avis d'imposition"; } }
    }
}
