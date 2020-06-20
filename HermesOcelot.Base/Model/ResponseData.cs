using System;
using System.Collections.Generic;
using System.Text;

namespace HermesOcelot.Base
{
    /// <summary>
    /// 返回数据
    /// </summary>
    public class ResponseData
    {
        #region Property
        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回内容
        /// </summary>
        public string Result { get; set; }
        #endregion //Property
    }
}
