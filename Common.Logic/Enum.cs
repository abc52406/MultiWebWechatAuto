using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logic
{
    /// <summary>
    /// 商品发布状态
    /// </summary>
    [Description("商品发布状态")]
    public enum ItemPublishStatus
    {
        /// <summary>
        /// 过期
        /// </summary>
        [Description("过期")]
        Overdue,
        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        Published,
        /// <summary>
        /// 待发布
        /// </summary>
        [Description("待发布")]
        Unpublish,

        /// <summary>
        /// 发布待审核
        /// </summary>
        [Description("发布待审核")]
        PublishAuditing,

        /// <summary>
        /// 发布审核未通过
        /// </summary>
        [Description("发布审核未通过")]
        PublishAuditNotPass,
    }

    /// <summary>
    /// 订单付款类型
    /// </summary>
    [Description("订单付款类型")]
    public enum OrderPayType
    {
        /// <summary>
        /// 货到付款Cash on delivery
        /// </summary>
        [Description("货到付款")]
        COD,
        /// <summary>
        /// 自提
        /// </summary>
        [Description("自提")]
        ZT,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        ZFB,
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    [Description("订单状态")]
    public enum OrderState
    {
        /// <summary>
        /// 待支付
        /// </summary>
        [Description("待支付")]
        WaitForPay,
        /// <summary>
        /// 待配送
        /// </summary>
        [Description("待配送")]
        WaitForSend,
        /// <summary>
        /// 可提货
        /// </summary>
        [Description("可提货")]
        WaitForCarry,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("已取消")]
        Closed,
        /// <summary>
        /// 配送中
        /// </summary>
        [Description("配送中")]
        Sending,
        /// <summary>
        /// 已提货
        /// </summary>
        [Description("已提货")]
        Carried,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Completed,
    }

    /// <summary>
    /// 退单状态
    /// </summary>
    [Description("退单状态")]
    public enum ReturnOrderState
    {
        /// <summary>
        /// 退单取消
        /// </summary>
        [Description("退单取消")]
        ReturnCancel,
        /// <summary>
        /// 退单审核
        /// </summary>
        [Description("退单审核")]
        ReturnAuditing,
        /// <summary>
        /// 退货中
        /// </summary>
        [Description("退货中")]
        GoodsReturning,
        /// <summary>
        /// 退款中
        /// </summary>
        [Description("退款中")]
        MoneyReturning,
        /// <summary>
        /// 退单完成
        /// </summary>
        [Description("退单完成")]
        ReturnCompleted,
    }

    /// <summary>
    /// 退货类型
    /// </summary>
    [Description("退货类型")]
    public enum ReturnOrderType
    {
        /// <summary>
        /// 退货
        /// </summary>
        [Description("退货")]
        Goods,
        /// <summary>
        /// 退钱
        /// </summary>
        [Description("退钱")]
        Money,
    }

    /// <summary>
    /// 卖家退货意见
    /// </summary>
    [Description("卖家退货意见")]
    public enum ReturnOrderSellerOpinion
    {
        /// <summary>
        /// 同意
        /// </summary>
        [Description("同意")]
        Agree,
        /// <summary>
        /// 拒绝
        /// </summary>
        [Description("拒绝")]
        Disagree,
    }

    /// <summary>
    /// 发票类型
    /// </summary>
    [Description("发票类型")]
    public enum InvoiceType
    {
        /// <summary>
        /// 不开发票
        /// </summary>
        [Description("不开发票")]
        NI,
        /// <summary>
        /// 个人
        /// </summary>
        [Description("个人")]
        SI,
        /// <summary>
        /// 单位
        /// </summary>
        [Description("单位")]
        CI,
    }

    /// <summary>
    /// 发票明细
    /// </summary>
    [Description("发票明细")]
    public enum InvoiceDetail
    {
        /// <summary>
        /// 明细
        /// </summary>
        [Description("明细")]
        Detail,
        /// <summary>
        /// 办公用品
        /// </summary>
        [Description("办公用品")]
        OG,
        /// <summary>
        /// 耗材
        /// </summary>
        [Description("耗材")]
        HC,
    }

    /// <summary>
    /// 订单配送方式
    /// </summary>
    [Description("订单配送方式")]
    public enum OrderSendType
    {
        /// <summary>
        /// 快递
        /// </summary>
        [Description("快递")]
        KD,
        /// <summary>
        /// 自提
        /// </summary>
        [Description("自提")]
        SelfCarray,

        /// <summary>
        /// 专配物流
        /// </summary>
        [Description("专配物流")]
        PS,
        
    }

    /// <summary>
    /// 物流公司
    /// </summary>
    [Description("物流公司")]
    public enum DistributionCompany
    {
        /// <summary>
        /// 申通
        /// </summary>
        [Description("申通")]
        shentong,
        /// <summary>
        /// 圆通
        /// </summary>
        [Description("圆通")]
        yuantong,
        /// <summary>
        /// 中通
        /// </summary>
        [Description("中通")]
        zhongtong,
        /// <summary>
        /// 汇通
        /// </summary>
        [Description("汇通")]
        huitong,
        /// <summary>
        /// 韵达
        /// </summary>
        [Description("韵达")]
        yunda,
        /// <summary>
        /// 顺丰
        /// </summary>
        [Description("顺丰")]
        shunfeng,
        /// <summary>
        /// EMS
        /// </summary>
        [Description("EMS")]
        EMS,
        /// <summary>
        /// 天天
        /// </summary>
        [Description("天天")]
        tiantian,
        /// <summary>
        /// CCES
        /// </summary>
        [Description("CCES")]
        cces,
        /// <summary>
        /// 宅急送
        /// </summary>
        [Description("宅急送")]
        zhaijisong,
    }

    /// <summary>
    /// 水印对齐方式
    /// </summary>
    [Description("水印对齐方式")]
    public enum ImageAlign : byte
    {
        /// <summary>
        /// 居中
        /// </summary>
        [Description("居中")]
        Center = 4,
        /// <summary>
        /// 向下居中
        /// </summary>
        [Description("向下居中")]
        CenterBottom = 5,
        /// <summary>
        /// 向上居中
        /// </summary>
        [Description("向上居中")]
        CenterTop = 6,
        /// <summary>
        /// 左下
        /// </summary>
        [Description("左下")]
        LeftBottom = 1,
        /// <summary>
        /// 左上
        /// </summary>
        [Description("左上")]
        LeftTop = 0,
        /// <summary>
        /// 右下
        /// </summary>
        [Description("右下")]
        RightBottom = 3,
        /// <summary>
        /// 右上
        /// </summary>
        [Description("右上")]
        RightTop = 2
    }

    /// <summary>
    /// 收藏类型
    /// </summary>
    [Description("收藏类型")]
    public enum FavoriteType
    {
        /// <summary>
        /// 商场
        /// </summary>
        [Description("商场")]
        Market,
        /// <summary>
        /// 专题/活动
        /// </summary>
        [Description("专题/活动")]
        Activity,
    }

    [Description("用户从属")]
    public enum UserBelongType
    {
        /// <summary>
        /// 商场
        /// </summary>
        [Description("商场")]
        Market,
        /// <summary>
        /// 专题/活动
        /// </summary>
        [Description("专题/活动")]
        Activity,
        /// <summary>
        /// 商铺
        /// </summary>
        [Description("商铺")]
        Shop,
    }

}
