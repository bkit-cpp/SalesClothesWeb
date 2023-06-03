using EShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;


namespace EShop.BackendApi.Utils
{
    public static class Globals
    {
        //Encode Base64
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        //Decode Base64
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }


        public static string HMACSHA256(string text, string key)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] textBytes = encoding.GetBytes(text);
            byte[] keyBytes = encoding.GetBytes(key);
            byte[] hashBytes;
            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public static DataTable LoopPriority(DataTable dtTaxConfig, int Data, int Priority, int index)
        {
            string ApplyTop = Convert.ToString(Data, 2).PadLeft(6, '0');
            int priTemp = int.Parse(dtTaxConfig.Rows[index]["Priority"].ToString());
            dtTaxConfig.Rows[index]["Priority"] = priTemp + Priority + 1;
            if (ApplyTop[4] == '1' && index != 0)
                return LoopPriority(dtTaxConfig, int.Parse(dtTaxConfig.Rows[0]["Data"].ToString()), int.Parse(dtTaxConfig.Rows[0]["Priority"].ToString()), 0);
            if (ApplyTop[3] == '1' && index != 1)
                return LoopPriority(dtTaxConfig, int.Parse(dtTaxConfig.Rows[1]["Data"].ToString()), int.Parse(dtTaxConfig.Rows[1]["Priority"].ToString()), 1);
            if (ApplyTop[2] == '1' && index != 2)
                return LoopPriority(dtTaxConfig, int.Parse(dtTaxConfig.Rows[2]["Data"].ToString()), int.Parse(dtTaxConfig.Rows[2]["Priority"].ToString()), 2);
            if (ApplyTop[1] == '1' && index != 3)
                return LoopPriority(dtTaxConfig, int.Parse(dtTaxConfig.Rows[3]["Data"].ToString()), int.Parse(dtTaxConfig.Rows[3]["Priority"].ToString()), 3);
            if (ApplyTop[0] == '1' && index != 4)
                return LoopPriority(dtTaxConfig, int.Parse(dtTaxConfig.Rows[4]["Data"].ToString()), int.Parse(dtTaxConfig.Rows[4]["Priority"].ToString()), 4);
            return dtTaxConfig;
        }

        public static string VARCHARNULL(string strText)
        {
            return strText != "" ? "'" + strText + "'" : "NULL";
        }

        public static string ToHexValue(Color color)
        {
            return "#" + color.R.ToString("X2") +
                         color.G.ToString("X2") +
                         color.B.ToString("X2");
        }

        public static string ConvertToUnsign(string str)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty)
                        .Replace('\u0111', 'd').Replace('\u0110', 'D').Replace("'", "''");
        }

        public static string CUTSTRING(string strText, int MaxLength)
        {
            return strText.Length <= MaxLength ? strText : strText.Substring(0, MaxLength);
        }

        public static List<MessageModel> GetStatusCode()
        {
            List<MessageModel> rs = new List<MessageModel>();

            //Any
            rs.Add(new MessageModel { Status = 200, Function = "Any", Message = "OK", Description = "Xử lí thành công" });
            rs.Add(new MessageModel { Status = 201, Function = "Any", Message = "There is no access to this feature", Description = "Không có quyền truy cập vào tính năng này (Gửi yêu cầu quyền)" });
            rs.Add(new MessageModel { Status = 202, Function = "Any", Message = "Invalid token code", Description = "Mã xác thực không hợp lệ (Kiểm tra Token ở Header)" });
            rs.Add(new MessageModel { Status = 203, Function = "Any", Message = "Token code not found", Description = "Không tìm thấy mã xác thực (Kiểm tra Token ở Header)" });
            rs.Add(new MessageModel { Status = 204, Function = "Any", Message = "The input parameter is incomplete", Description = "Tham số đầu vào không đầy đủ (Kiểm tra các tham số truyền lên)" });
            rs.Add(new MessageModel { Status = 205, Function = "Any", Message = "An exception error occurred", Description = "Xảy ra lỗi ngoại lệ (Kiểm tra Exception trả về)" });

            return rs;
        }
    }
}
