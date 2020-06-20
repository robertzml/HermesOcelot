using System;
using System.Collections.Generic;
using System.Text;

namespace HermesOcelot.Base
{
    /// <summary>
    /// JWT 状态
    /// </summary>
    public class JwtState
    {
        #region Property
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 验证成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool IsExpire { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }
        #endregion //Property
    }
}
