using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public enum FrmType
    {

        Write, Modify, Reply

    }

    public static class FrmTypeExtensions
    {
        public static string ToFriendlyString(this FrmType fType)
        {
            string r = "";

            switch (fType)
            {
                case FrmType.Write:
                    r = "글 쓰기 페이지";
                    break;
                case FrmType.Modify:
                    r = "글 수정 페이지";
                    break;
                case FrmType.Reply:
                    r = "글 답변 페이지";
                    break;
                default:
                    r = "글 쓰기 페이지";
                    break;
            }

            return r;

        }
    }
}
