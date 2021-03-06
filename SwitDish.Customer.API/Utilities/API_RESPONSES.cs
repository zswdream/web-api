using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Utilities
{
    public static class API_RESPONSES
    {
        public const string USER_NOT_FOUND = "USER_NOT_FOUND";
        public const string CUSTOMER_NOT_FOUND = "CUSTOMER_NOT_FOUND";
        public const string OLD_PASSWORD_INVALID = "OLD_PASSWORD_INVALID";
        public const string NEW_PASSWORDS_DONT_MATCH = "NEW_PASSWORDS_DONT_MATCH";
        public const string OLD_AND_NEW_PASSWORDS_SAME = "OLD_AND_NEW_PASSWORDS_SAME";
        public const string INVALID_CUSTOMER_DETAILS = "INVALID_CUSTOMER_DETAILS";
        public const string EMAIL_ALREADY_REGISTERED = "EMAIL_ALREADY_REGISTERED";
        public const string PHONE_ALREADY_REGISTERED = "PHONE_ALREADY_REGISTERED";
        public const string CUSTOMER_ACCOUNT_CREATED = "CUSTOMER_ACCOUNT_CREATED";
        public const string BOOKING_CREATED = "BOOKING_CREATED";
        public const string CUSTOMER_DELIVERY_ADDRESS_ADDED = "CUSTOMER_DELIVERY_ADDRESS_ADDED";
        public const string CUSTOMER_DELIVERY_ADDRESS_ALREADY_EXISTS = "CUSTOMER_DELIVERY_ADDRESS_ALREADY_EXISTS";
        public const string DELIVERY_ADDRESS_ADDED = "DELIVERY_ADDRESS_ADDED";
        public const string DELIVERY_ADDRESS_UPDATED = "DELIVERY_ADDRESS_UPDATED";
        public const string DELIVERY_ADDRESS_DELETED = "DELIVERY_ADDRESS_DELETED";
        public const string DELIVERY_ADDRESS_NOT_FOUND = "DELIVERY_ADDRESS_NOT_FOUND";
        public const string FAVOURITE_VENDOR_NOT_FOUND = "FAVOURITE_VENDOR_NOT_FOUND";
        public const string FAVOURITE_VENDOR_ADDED = "FAVOURITE_VENDOR_ADDED";
        public const string FAVOURITE_VENDOR_REMOVED = "FAVOURITE_VENDOR_REMOVED";
        public const string FAVOURITE_VENDOR_ALREADY_EXISTS = "FAVOURITE_VENDOR_ALREADY_EXISTS";
        public const string PROFILE_UPDATED = "PROFILE_UPDATED";
        public const string SECURITY_QUESTION_NOT_FOUND = "SECURITY_QUESTION_NOT_FOUND";
        public const string SECURITY_QUESTION_ADDED = "SECURITY_QUESTION_ADDED";
        public const string SECURITY_QUESTION_UPDATED = "SECURITY_QUESTION_UPDATED";
        public const string VENDOR_NOT_FOUND = "VENDOR_NOT_FOUND";
        public const string NO_VENDORS_FOUND = "NO_VENDORS_FOUND";
        public const string CUSTOMER_ORDER_NOT_FOUND = "CUSTOMER_ORDER_NOT_FOUND";
        public const string CUSTOMER_ORDER_NOT_FROM_THIS_CUSTOMER = "CUSTOMER_ORDER_NOT_FROM_THIS_CUSTOMER";
        public const string CANCELLED_ORDER_CANNOT_BE_REVIEWED = "CANCELLED_ORDER_CANNOT_BE_REVIEWED";
        public const string CUSTOMER_ORDER_CANCELLED = "CUSTOMER_ORDER_CANCELLED";
        public const string CUSTOMER_ORDER_EDIT_TIME_LIMIT_EXCEEDED = "CUSTOMER_ORDER_EDIT_TIME_LIMIT_EXCEEDED";
        public const string FEEDBACK_NOT_FOUND = "FEEDBACK_NOT_FOUND";
        public const string FEEDBACK_REACTION_NOT_FOUND = "FEEDBACK_REACTION_NOT_FOUND";
        public const string FEEDBACK_REACTION_DELETED = "FEEDBACK_REACTION_DELETED";
        public const string FEEDBACK_REACTION_ALREADY_EXISTS = "FEEDBACK_REACTION_ALREADY_EXISTS";
        public const string NO_PRODUCT_CATEGORIES_FOUND = "NO_PRODUCT_CATEGORIES_FOUND";
    }
}
