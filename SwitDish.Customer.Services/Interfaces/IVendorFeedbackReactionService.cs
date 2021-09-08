using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface IVendorFeedbackReactionService
    {
        Task<VendorFeedbackReaction> InsertVendorFeedbackReactionAsync(VendorFeedbackReaction vendorFeedbackReaction);
        Task<bool> DeleteVendorFeedbackReactionAsync(VendorFeedbackReaction vendorFeedbackReaction);
        Task<IEnumerable<VendorFeedbackReaction>> GetVendorFeedbackReactionsAsync(int vendorFeedbackId);
    }
}