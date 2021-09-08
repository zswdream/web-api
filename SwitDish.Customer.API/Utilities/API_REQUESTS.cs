using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Utilities
{
    public static class API_REQUESTS
    {
        public const string LOGIN = "auth/login";
        public const string REQUEST_PASSWORD_RESET = "auth/forgot-password-email";
        public const string RESET_PASSWORD = "auth/reset-password";
        public const string UPDATE_PASSWORD = "auth/update-password";
        public const string ADD_DELIVERY_ADDRESS = "customer/delivery-address";
        public const string UPDATE_DELIVERY_ADDRESS = "customer/delivery-address";
        public const string BOOK_CUSTOMER_TABLE = "customer/book-table";
        public const string DELETE_DELIVERY_ADDRESS = "customer/delivery-address";
        public const string GET_DELIVERY_ADDRESSES = "customer/delivery-address";
        public const string GET_FAVOURITE_VENDORS = "customer/favourites";
        public const string GET_ORDERS = "customer/orders";
        public const string GET_ORDER_DETAILS = "customer/order";
        public const string ADD_ORDER = "customer/order";
        public const string UPDATE_ORDER = "customer/order";
        public const string CANCEL_ORDER = "customer/cancel-order";
        public const string ADD_ORDER_FEEDBACK = "customer/order-feedback";
        public const string GET_PROFILE_DETAILS = "customer/profile";
        public const string GET_CUSTOMER_SECURITY_QUESTIONS = "customer/security-question";
        public const string CUSTOMER_SIGNUP = "customer/signup";
        public const string ADD_FAVOURITE_VENDOR = "customer/favourites";
        public const string DELETE_FAVOURITE_VENDOR = "customer/favourites";
        public const string UPDATE_PROFILE_DETAILS = "customer/profile";
        public const string UPDATE_CUSTOMER_PROFILE_IMAGE = "customer/profile-image";
        public const string UPDATE_CUSTOMER_SECURITY_QUESTION = "customer/security-question";
        public const string ADD_CUSTOMER_SECURITY_QUESTION = "customer/security-question";
        public const string ADD_VENDOR_FEEDBACK_REACTION = "customer/vendor-feedback-reaction";
        public const string DELETE_VENDOR_FEEDBACK_REACTION = "customer/vendor-feedback-reaction";
        public const string ADD_SECURITY_QUESTION = "security-question/add";
        public const string GET_SECURITY_QUESTION = "security-question/get";
        public const string GET_SECURITY_QUESTIONS = "security-question/get-all";
        public const string GET_VENDOR = "vendor/get";
        public const string GET_ALL_VENDORS = "vendor/get-list";
        public const string GET_PRODUCT_CATEGORIES = "vendor/product-categories";
        public const string GET_VENDOR_GALLERY = "vendor/gallery";
        public const string GET_VENDOR_OFFERS = "vendor/offers";
        public const string GET_VENDOR_FEEDBACKS = "vendor/feedbacks";
        public const string GET_VENDOR_FILTERS = "vendor/filters";
    }
}
