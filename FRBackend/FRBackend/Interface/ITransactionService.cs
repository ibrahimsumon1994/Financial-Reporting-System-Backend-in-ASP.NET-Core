using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Interface
{
    public interface ITransactionService
    {
        bool AddOrUpdate(List<Transaction> transactions, string loginUser, out string message);
        bool AddOrUpdateFromExcel(List<TransactionExcelModel> transactions, string loginUser, out string message);
        List<Transaction> GetItemsWithDataBySecondHeader(Transaction transaction);
        List<TransactionExcelModel> GetAllHeaderItemsWithDataByHeaderType(Transaction transaction);
    }
}
