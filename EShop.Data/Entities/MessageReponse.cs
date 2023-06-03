using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Data.Entities
{
    public class MessageResponseConstants
    {
        private static readonly Dictionary<MessageKey, MessageModel> messageModels = new Dictionary<MessageKey, MessageModel>
        {
            //Any
            {MessageKey.SUCCESS, new MessageModel(200, "OK", "Xử lí thành công", "Any") },
            {MessageKey.FORBIDDEN, new MessageModel(201, "There is no access to this feature",
                                                                            "Không có quyền truy cập vào tính năng này (Gửi yêu cầu quyền)", "Any")},
            {MessageKey.INVALID_TOKEN, new MessageModel(202, "Invalid token code", "Mã xác thực không hợp lệ (Kiểm tra Token ở Header)", "Any")},
            {MessageKey.TOKEN_NOT_FOUND, new MessageModel(203, "Token code not found", "Không tìm thấy mã xác thực (Kiểm tra Token ở Header)", "Any")},
            {MessageKey.BAD_PARAM, new MessageModel(204, "The input parameter is incomplete", "Tham số đầu vào không đầy đủ (Kiểm tra các tham số truyền lên)", "Any")},
            {MessageKey.BAD_REQESUT, new MessageModel(205, "An exception error occurred", "Xảy ra lỗi ngoại lệ (Kiểm tra Exception trả về)", "Any")},
            {MessageKey.PARTNERKEY_NOT_FOUND, new MessageModel(206, "PartnerKey not found", "Không tìm thấy mã đối tác (Kiểm tra PartnerKey ở Header)", "Any")},

            //Define any model here
        };

        public static string GetMessage(MessageKey key)
        {
            if (messageModels.TryGetValue(key, out var messageModel))
            {
                return messageModel.Message;
            }

            throw new InvalidOperationException("Key not found");
        }
    }

    public enum MessageKey
    {
        SUCCESS,
        FORBIDDEN,
        INVALID_TOKEN,
        TOKEN_NOT_FOUND,
        BAD_PARAM,
        BAD_REQESUT,
        PARTNERKEY_NOT_FOUND,
        PAYMENT_METHOD_NOT_FOUND,
        EOI_CLOSED,
        UNAUTHORIZED,
        TABLE_NUM_NOT_FOUND,
        SECTION_NUM_NOT_FOUND,
        REV_TYPE_NUMBER_NOT_FOUND,
        EMP_NUM_NOT_FOUND,
        TABLE_RESERVED,
        REV_NUM_NOT_FOUND,
        TABLE_MAP_ID_NOT_FOUND,
        REFUND_REASON_NOT_FOUND,
        INCORRECT_TABLE_ID,
        STATION_ID_NOT_FOUND,
        NOT_START_DAY,
        MEM_ADDRESS_NOT_FOUND,
        SALE_TYPE_NOT_FOUND,
        PROD_COMBO_NOT_FOUND,
        QUESTION_NOT_FOUND,
        PROD_SOLDOUT,
        PROD_NUM_NOT_FOUND,
        PROMO_NOT_FOUND,
        TRANSACT_NOT_EXIST,
        TRANSACT_CLOSED,
        TRANSACT_BLOCKED,
        UNIQUED_ID_EXIST,
        UNIQUE_ID_NOT_FOUND,
        MERGE_FAILED,
        TABLE_USED_BY_POS,
        MERGE_CLOSED_CHECK,
        MERGE_CHECK_PAYING,
        NOT_ENOUGH_CHECK
    }
}
