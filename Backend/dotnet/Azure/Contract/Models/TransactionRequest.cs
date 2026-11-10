using System;
using System.Collections.Generic;
using System.Text;

namespace Contract.Models
{
    public record TransactionRequest(
        string AccountId,
        string TransactionType,
        decimal Amount,
        string ReferenceId
    );
}
