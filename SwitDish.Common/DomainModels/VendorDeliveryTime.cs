using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class VendorDeliveryTime
    {
        public int VendorDeliveryTimeId { get; set; }
        public int VendorId { get; set; }
        public TimeSpan Minimum { get; set; }
        public TimeSpan Maximum { get; set; }
        public string MinimumLabel {
            get
            {
                var hourLabel = Minimum.Hours > 0 ? Minimum.Hours > 1 ? $"{Minimum.Hours} hrs" : $"{Minimum.Hours} hr" : string.Empty;
                var minuteLabel = Minimum.Minutes > 0 ? Minimum.Minutes > 1 ? $"{Minimum.Minutes} mins" : $"{Minimum.Minutes} min" : string.Empty;
                return hourLabel.Equals(string.Empty) ? minuteLabel : $"{hourLabel} {minuteLabel}";
            }
        } 
        public string MaximumLabel
        {
            get
            {
                var hourLabel = Maximum.Hours > 0 ? Maximum.Hours > 1 ? $"{Maximum.Hours} hrs" : $"{Maximum.Hours} hr" : string.Empty;
                var minuteLabel = Maximum.Minutes > 0 ? Maximum.Minutes > 1 ? $"{Maximum.Minutes} mins" : $"{Maximum.Minutes} min" : string.Empty;
                return hourLabel.Equals(string.Empty) ? minuteLabel : $"{hourLabel} {minuteLabel}";
            }
        }

        public override string ToString()
        {
            return $"{MinimumLabel} - {MaximumLabel}";
        }
    }
}
