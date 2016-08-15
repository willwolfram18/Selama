using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Common.Utility.Constants
{
    public static class ForumsConstants
    {
        public const int ThreadTitleMinLen = 4;
        public const int ThreadTitleMaxLen = 60;
        public const string ThreadTitleLenErrorMsg = "A title must be between {1} and {2} characters in length";

        public const int ThreadContentMinLen = 25;
        public const string ThreadContentLenErrorMsg = "A thread's content must contain at least {1} characters";

        public const int ThreadReplyContentMinLen = 1;
        public const string ThreadReplyContentLenErrorMsg = "A reply to a thread must contain at least {1} characters";
    }
}