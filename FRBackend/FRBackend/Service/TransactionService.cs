using Common.Helpers;
using DotNetCoreScaffoldingSqlServer.Entities;
using FRBackend.Helpers;
using FRBackend.Interface;
using FRBackend.Model;
using FRBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FRBackend.Service
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private IUnitRepository _unitRepository;
        private ICommonCodeRepository _commonCodeRepository;
        private IHeaderRepository _headerRepository;
        private readonly IHeaderService _headerService;
        public TransactionService(ITransactionRepository transactionRepository, IHeaderService headerService, IUnitRepository unitRepository, ICommonCodeRepository commonCodeRepository, IHeaderRepository headerRepository)
        {
            _transactionRepository = transactionRepository;
            _headerService = headerService;
            _unitRepository = unitRepository;
            _commonCodeRepository = commonCodeRepository;
            _headerRepository = headerRepository;
        }

        public bool AddOrUpdate(List<Transaction> transactions, string loginUser, out string message)
        {
            foreach (var transaction in transactions)
            {
                transaction.ThirdHeader = null;
                var existingData = _transactionRepository.AsQueryable().FirstOrDefault(x => x.TransactionDate.Value.Month == transaction.TransactionDate.Value.Month 
                                  && x.TransactionDate.Value.Year == transaction.TransactionDate.Value.Year && x.ThirdHeaderId == transaction.ThirdHeaderId 
                                  && x.TransactionTypeId == transaction.TransactionTypeId && x.UnitId == transaction.UnitId
                                  && x.Recstatus == ActionConst.Authorize);
                if (existingData != null)
                {
                    if (existingData.Value != transaction.Value)
                    {
                        if (existingData.NumberOfUpdate == null || existingData.NumberOfUpdate < 2)
                        {
                            existingData.Value = transaction.Value;
                            existingData.Remarks = transaction.Remarks;
                            existingData.NumberOfUpdate = existingData.NumberOfUpdate != null ? existingData.NumberOfUpdate + 1 : 1;
                            existingData.VersionId = transaction.VersionId;
                            existingData.UpdatedBy = Convert.ToInt32(loginUser);
                            existingData.UpdatedDate = DateTime.Now;
                            _transactionRepository.Update(existingData);
                        }
                    }
                    else
                    {
                        if (existingData.VersionId != transaction.VersionId)
                        {
                            existingData.Remarks = transaction.Remarks;
                            existingData.VersionId = transaction.VersionId;
                            existingData.UpdatedBy = Convert.ToInt32(loginUser);
                            existingData.UpdatedDate = DateTime.Now;
                            _transactionRepository.Update(existingData);
                        }
                    }
                }
                else
                {
                    if (transaction.Value != null)
                    {
                        var unit = _unitRepository.AsQueryable().FirstOrDefault(x => x.UnitId == transaction.UnitId);
                        if (unit != null)
                        {
                            transaction.GroupId = unit.GroupId;
                        }
                        transaction.CreatedBy = Convert.ToInt32(loginUser);
                        transaction.CreatedDate = DateTime.Now;
                        transaction.Recstatus = ActionConst.Authorize;
                        _transactionRepository.Add(transaction);
                    }
                }
            }
            message = MessageConst.Insert;
            return true;
        }

        public bool AddOrUpdateFromExcel(List<TransactionExcelModel> transactions, string loginUser, out string message)
        {
            foreach (var transaction in transactions)
            {
                long unitId = 0, thirdHeaderId = 0, transactionTypeId = 0;
                long? versionId = null, headerTypeId = null, firstHeaderId = null, secondHeaderId = null;
                var unit = _unitRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == transaction.Unit.Trim().ToLower());
                if (unit != null)
                {
                    unitId = unit.UnitId;
                }
                var transactionType = _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.NameEnglish.Trim().ToLower() == transaction.Type.Trim().ToLower() && x.Recstatus == ActionConst.Authorize);
                if (transactionType != null)
                {
                    transactionTypeId = transactionType.CommonCodeId;
                }
                var thirdHeader = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderName.Trim().ToLower() == transaction.ItemName.Trim().ToLower() && x.Recstatus == ActionConst.Authorize);
                if (thirdHeader != null)
                {
                    thirdHeaderId = thirdHeader.HeaderId;
                }
                var firstHeader = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderName.Trim().ToLower() == transaction.Header.Trim().ToLower() && x.Recstatus == ActionConst.Authorize);
                if (firstHeader != null)
                {
                    firstHeaderId = firstHeader.HeaderId;
                }
                var secondHeader = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderName.Trim().ToLower() == transaction.SubHeader.Trim().ToLower() && x.Recstatus == ActionConst.Authorize);
                if (secondHeader != null)
                {
                    secondHeaderId = secondHeader.HeaderId;
                }
                if (unitId > 0 && transactionTypeId > 0 && thirdHeaderId > 0)
                {
                    DateTime? transactionDate = !string.IsNullOrEmpty(transaction.MonthYear) ? Convert.ToDateTime(transaction.MonthYear) : null;
                    var existingData = _transactionRepository.AsQueryable().FirstOrDefault(x => x.TransactionDate.Value.Month == transactionDate.Value.Month
                                      && x.TransactionDate.Value.Year == transactionDate.Value.Year && x.ThirdHeaderId == thirdHeaderId
                                      && x.TransactionTypeId == transactionTypeId && x.UnitId == unitId
                                      && x.Recstatus == ActionConst.Authorize);
                    if (existingData != null)
                    {
                        if (existingData.Value != transaction.Value)
                        {
                            if (existingData.NumberOfUpdate == null || existingData.NumberOfUpdate < 2)
                            {
                                existingData.Value = transaction.Value;
                                existingData.NumberOfUpdate = existingData.NumberOfUpdate != null ? existingData.NumberOfUpdate + 1 : 1;
                                existingData.VersionId = versionId;
                                existingData.UpdatedBy = Convert.ToInt32(loginUser);
                                existingData.UpdatedDate = DateTime.Now;
                                _transactionRepository.Update(existingData);
                            }
                        }
                        else
                        {
                            if (existingData.VersionId != versionId)
                            {
                                existingData.VersionId = versionId;
                                existingData.UpdatedBy = Convert.ToInt32(loginUser);
                                existingData.UpdatedDate = DateTime.Now;
                                _transactionRepository.Update(existingData);
                            }
                        }
                    }
                    else
                    {
                        if (transaction.Value != null)
                        {
                            var saveObj = new Transaction();
                            saveObj.TransactionDate = !string.IsNullOrEmpty(transaction.MonthYear) ? Convert.ToDateTime(transaction.MonthYear) : null;
                            saveObj.TransactionTypeId = transactionTypeId;
                            saveObj.VersionId = versionId;
                            saveObj.UnitId = unitId;
                            var unitData = _unitRepository.AsQueryable().FirstOrDefault(x => x.UnitId == unitId);
                            if (unit != null)
                            {
                                saveObj.GroupId = unitData.GroupId;
                            }
                            saveObj.HeaderTypeId = headerTypeId;
                            saveObj.FirstHeaderId = firstHeaderId;
                            saveObj.SecondHeaderId = secondHeaderId;
                            saveObj.ThirdHeaderId = thirdHeaderId;
                            saveObj.Value = transaction.Value;
                            saveObj.CreatedBy = Convert.ToInt32(loginUser);
                            saveObj.CreatedDate = DateTime.Now;
                            saveObj.Recstatus = ActionConst.Authorize;
                            _transactionRepository.Add(saveObj);
                        }
                    }
                }
            }
            message = MessageConst.Insert;
            return true;
        }

        public List<Transaction> GetItemsWithDataBySecondHeader(Transaction transaction)
        {
            var data = new List<Transaction>();
            var thirdHeaderData = _headerService.GetItemsBySecondHeader(transaction.SecondHeaderId);
            foreach (var headerData in thirdHeaderData)
            {
                var obj = new Transaction();
                obj.TransactionDate = transaction.TransactionDate;
                obj.TransactionTypeId = transaction.TransactionTypeId;
                obj.UnitId = transaction.UnitId;
                obj.HeaderTypeId = transaction.HeaderTypeId;
                obj.FirstHeaderId = transaction.FirstHeaderId;
                obj.SecondHeaderId = transaction.SecondHeaderId;
                obj.ThirdHeaderId = headerData.HeaderId;
                if (!string.IsNullOrEmpty(headerData.HeaderCode))
                {
                    var thirdHeader = new Header();
                    thirdHeader.HeaderCode = headerData.HeaderCode;
                    thirdHeader.HeaderName = headerData.HeaderName;
                    obj.ThirdHeader = thirdHeader;
                }
                var checkTransactionData = _transactionRepository.AsQueryable().FirstOrDefault(x => x.TransactionDate.Value.Month == transaction.TransactionDate.Value.Month 
                                           && x.TransactionDate.Value.Year == transaction.TransactionDate.Value.Year && x.ThirdHeaderId == headerData.HeaderId 
                                           && x.TransactionTypeId == transaction.TransactionTypeId && x.UnitId == transaction.UnitId
                                           && x.Recstatus == ActionConst.Authorize);
                if (checkTransactionData != null)
                {
                    obj.VersionId = checkTransactionData.VersionId;
                    obj.Value = checkTransactionData.Value;
                    obj.Remarks = checkTransactionData.Remarks;
                    obj.NumberOfUpdate = checkTransactionData.NumberOfUpdate;
                }
                else
                {
                    obj.VersionId = transaction.VersionId;
                }
                data.Add(obj);
            }
            return data;
        }

        public List<TransactionExcelModel> GetAllHeaderItemsWithDataByHeaderType(Transaction transaction)
        {
            var data = new List<TransactionExcelModel>();
            var thirdHeaderData = _headerService.GetAllHeaderItemByHeaderType(transaction.HeaderTypeId);
            foreach (var headerData in thirdHeaderData)
            {
                var obj = new TransactionExcelModel();
                obj.MonthYear = transaction.TransactionDate != null ? Convert.ToDateTime(transaction.TransactionDate).ToString("dd MMMM yyyy") : null;
                var transactionType = _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.CommonCodeId == transaction.TransactionTypeId);
                if (transactionType != null)
                {
                    obj.Type = transactionType.NameEnglish;
                }
                var version = _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.CommonCodeId == transaction.VersionId);
                if (version != null)
                {
                    obj.Version = version.NameEnglish;
                }
                var unit = _unitRepository.AsQueryable().FirstOrDefault(x => x.UnitId == transaction.UnitId);
                if (unit != null)
                {
                    obj.Unit = unit.NameEnglish;
                }
                var headerType = _commonCodeRepository.AsQueryable().FirstOrDefault(x => x.CommonCodeId == transaction.HeaderTypeId);
                if (headerType != null)
                {
                    obj.EntryType = headerType.NameEnglish;
                }
                obj.ItemName = headerData.HeaderName;
                var secondHeader = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderId == headerData.ParentHeaderId);
                if (secondHeader != null)
                {
                    obj.SubHeader = secondHeader.HeaderName;
                    var firstHeader = _headerRepository.AsQueryable().FirstOrDefault(x => x.HeaderId == secondHeader.ParentHeaderId);
                    if (firstHeader != null)
                    {
                        obj.Header = firstHeader.HeaderName;
                    }
                }
                var checkTransactionData = _transactionRepository.AsQueryable().FirstOrDefault(x => x.TransactionDate.Value.Month == transaction.TransactionDate.Value.Month
                                           && x.TransactionDate.Value.Year == transaction.TransactionDate.Value.Year && x.ThirdHeaderId == headerData.HeaderId
                                           && x.TransactionTypeId == transaction.TransactionTypeId && x.UnitId == transaction.UnitId
                                           && x.Recstatus == ActionConst.Authorize);
                if (checkTransactionData != null)
                {
                    obj.Value = checkTransactionData.Value;
                    obj.NumberOfUpdate = checkTransactionData.NumberOfUpdate;
                }
                data.Add(obj);
            }
            return data;
        }
    }
}
