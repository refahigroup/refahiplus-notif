
namespace Refahi.Notif.Infrastructure.Messaging.Sms.Mediana;

public enum ErrorTypes
{
    No_active_plan_found = 1032,
    Plan_does_not_have_API_facility = 1033,
    Plan_does_not_have_pattern_facility = 1034,
    Plan_does_not_have_a_dedicated_line_facility = 1035,
    Invalid_receiver_in_API_request = 1041,
    Insufficient_wallet_balance = 1042,
    Maximum_number_of_receivers_exceeded = 1043,
    Invalid_SMS_ID = 1044,
    Invalid_request_code = 1045,
    Invalid_input_parameters = 1046,
    Phone_number_is_blacklisted = 1047,
    WebEngage_is_not_enabled = 1048,
    Campaign_has_expired = 1051,
    No_active_line_found = 1061,
    Line_is_not_usable_at_this_time_of_day = 1062,
    Pattern_URL_detected = 1071,
    Pattern_rejected_by_admin = 1072,
    Pattern_belongs_to_another_sending_number = 1073,
    Message_text_is_empty = 1074,
    Message_request_not_found = 1075,
    Pattern_is_empty = 1076,
    Postal_code_not_verified = 1081,
    National_code_not_verified = 1082,
    Mobile_number_not_verified = 1083,
    Profile_not_completed = 1084,
    Receivers_not_found = 1093,
    Sending_number_not_found = 1101,
    Sending_number_has_expired = 1102,
    Unknown_error_occurred = 1021

}
