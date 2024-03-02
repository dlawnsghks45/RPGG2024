// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("tJKRTJmATZr5Pf4r9XVcNAmPnIK5nvWbUO0qbQuPhxkAGJFW20payRLC8u2zirAUN9CF0NibMtcQlvIzB2Ub90k6QzwhJRVcpCDP2fg1yUy/DY6tv4KJhqUJxwl4go6OjoqPjMSuSKwIZS4LQ9qax1QBrBpMeopqPfycEzcMAfDtCSJ9aD9YuCg0uEUdXkDmEb67x4zhCzd+wljryAQtp8QVUkh5N5o9Zgf5SWTNk1moqcigDY6Aj78NjoWNDY6Oj15oH06u8iYaY935Mav3wWBk7fSbHOpIGmtEN6It0NpGddXULkcgB9ZcS3usqIQ/K5/6XZgus1AriMnEWYq00Lck6iXly4RhD+7FgbE/KAHZ6czC619rKsBUfABy8B8UVI2Mjo+O");
        private static int[] order = new int[] { 11,10,5,13,11,9,13,9,11,10,10,13,13,13,14 };
        private static int key = 143;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
