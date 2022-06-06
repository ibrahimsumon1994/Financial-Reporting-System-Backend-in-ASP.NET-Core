using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    public static class ActionConst
    {
        public const string Authorize = "A",
            Pending = "P",
            Cancel = "C";
    }
    public static class MessageConst
    {
        public const string Insert = "Record has been saved.",
        Update = "Record has been updated.",
        Cancel = "Record has been canceled!",
        SystemError = "System error!",
        IsExist = "Already exist!",
        IsExistCancel = "Already exist with cancel status!",
        IsExistUnknown = "Already exist with unknown status!",
        Decline = "Record has been declined!",
        Verify = "Record has been verified.",
        Deactive = "Record has been deactivated!",
        Active = "Record has been activated.",
        NotFound = "No Record found!",
        Found = "Record found.",
        Failed = "Failed!",
        AlreadyCancel = "Already cancelled!",
        InvalidUser = "Invalid userId or password!",
        InvalidUserId = "Invalid userId!",
        InvalidPassword = "Wrong password!",
        SuccussLogin = "Login successful.",
        InvalidRefreshToken = "Invalid refresh token!",
        ChangePassword = "Password has been changed.",
        StopSession = "Session has been stopped!";
    }
    public static class HeaderLayerConst
    {
        public const int FirstLayer = 1,
            SecondLayer = 2,
            ThirdLayer = 3;
    }
}
